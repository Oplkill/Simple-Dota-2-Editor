using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using KV_reloaded;
using SimpleDota2EditorWPF.Panels.KV;
using SimpleDota2EditorWPF.ScriptsUtils.KV;
using SomeUtils;
using Xceed.Wpf.AvalonDock.Layout;

namespace SimpleDota2EditorWPF.Panels
{
    /// <summary>
    /// Логика взаимодействия для TextEditorPanel.xaml
    /// </summary>
    public partial class TextEditorKVPanel : UserControl, IEditor
    {
        public IEditor ParentEditor { get; set; }
        public bool Edited
        {
            get { return TextEditor.IsModified; }
            set
            {
                if (edited != value)
                {
                    PanelDocument.Title = PanelName + (value ? @" *" : "");
                    if (ParentEditor != null)
                        ParentEditor.Edited = value;
                }
                TextEditor.IsModified = edited = value;
            }
        }
        private bool edited;

        public string PanelName
        {
            get { return panelName; }
            set
            {
                panelName = value;
                PanelDocument.Title = PanelName + (Edited ? @" *" : "");
            }
        }

        private string panelName;
        public KVToken ObjectRef { get; set; }
        public ObjectsViewPanel.ObjectTypePanel ObjectType { get; set; }
        public Settings.EditorType EditorType { get; }
        private OffsetColorizer _offsetColorizer;

        private ToolTip toolTip = new ToolTip();
        private int startToolTipOffset, endToolTipOffset;
        private CompletionWindow completionWindow;


        public TextEditorKVPanel()
        {
            InitializeComponent();

            _offsetColorizer = new OffsetColorizer();
            TextEditor.TextChanged += TextChanged;
            TextEditor.TextArea.TextView.LineTransformers.Add(_offsetColorizer);
            TextEditor.TextArea.TextEntering += textEditor_TextArea_TextEntering;
            TextEditor.TextArea.TextEntered += textEditor_TextArea_TextEntered;
            TextEditor.TextArea.SelectionChanged += textEditor_TextArea_TextSelected;
            TextEditor.MouseHover += MouseHovered;
            TextEditor.MouseMove += TextEditor_MouseMove;
            toolTip.MouseMove += TextEditor_MouseMove;
            Update();
        }

        private void TextEditor_MouseMove(object sender, MouseEventArgs e)
        {
            if (!toolTip.IsOpen) return;

            var pos = TextEditor.GetPositionFromPoint(e.GetPosition(TextEditor));
            if (pos == null)
            {
                toolTip.IsOpen = false;
                return;
            }

            var offset = TextEditor.Document.GetOffset(pos.Value.Line, pos.Value.Column);
            if (offset < startToolTipOffset || offset > endToolTipOffset)
                toolTip.IsOpen = false;
        }

        private void MouseHovered(object sender, MouseEventArgs e)
        {
            var pos = TextEditor.GetPositionFromPoint(e.GetPosition(TextEditor));
            if (pos == null) return;

            var line = pos.Value.Line;
            var column = pos.Value.Column;
            string wordHover = "";
            var offset = TextEditor.Document.GetOffset(line, column);
            if (offset >= TextEditor.Text.Length)
                return;
            if (ParserUtils.thisSymbolInCommentZone(TextEditor.Text, offset) != -1)
                return;
            bool? key = ParserUtils.ItsKey(TextEditor.Text, offset);
            if (key == null)
                return;

            if (offset >= TextEditor.Document.TextLength)
                offset--;
            string textAtOffset = TextEditor.Document.GetText(offset, 1);
            while (!string.IsNullOrWhiteSpace(textAtOffset) && textAtOffset != "\"" && textAtOffset != "|")
            {
                startToolTipOffset = offset;
                wordHover = String.Concat(textAtOffset, wordHover);
                offset--;
                if (offset == -1)
                    break;
                textAtOffset = TextEditor.Document.GetText(offset, 1);
            }
            offset = TextEditor.Document.GetOffset(line, column) + 1;
            if (offset >= TextEditor.Document.TextLength)
                offset--;
            textAtOffset = TextEditor.Document.GetText(offset, 1);
            while (!string.IsNullOrWhiteSpace(textAtOffset) && textAtOffset != "\"" && textAtOffset != "|")
            {
                endToolTipOffset = offset;
                wordHover = String.Concat(wordHover, textAtOffset);
                offset++;
                if (offset == TextEditor.Document.TextLength)
                    break;
                textAtOffset = TextEditor.Document.GetText(offset, 1);
            }

            if (string.IsNullOrWhiteSpace(wordHover))
                return;

            string toolTipText = null;
            
            if (key == true)
            {
                //todo
            }
            else
            {
                string keyText = ParserUtils.GetKeyText(TextEditor.Text, offset);
                if (keyText == "") return;

                KVToken ownerTok = BasicCompletionKV.Values.GetChild(keyText);
                if (ownerTok == null)
                    return; //Key not founded

                var tok = ownerTok.GetChild(wordHover);
                if (tok == null)
                    return;

                toolTipText = KVScriptResourcesValues.ResourceManager.GetString(tok.Value.Substring(1));
            }
            if (String.IsNullOrEmpty(toolTipText))
                return;
            StackPanel toolTipPanel = new StackPanel();
            toolTipPanel.Children.Add(new TextBlock { Text = wordHover, FontSize = 16 });
            toolTipPanel.Children.Add(new TextBlock { Text = toolTipText });
            toolTip.Content = toolTipPanel;
            toolTip.IsOpen = true;
        }

        public void Update()
        {
            _offsetColorizer.Update();
            TextEditor.FontFamily = new FontFamily(DataBase.Settings.HighSetts.Font);
            TextEditor.FontSize = DataBase.Settings.HighSetts.FontSize;
            TextEditor.FontWeight = DataBase.Settings.HighSetts.Bold ? FontWeights.Bold : FontWeights.Normal;
            TextEditor.FontStyle = DataBase.Settings.HighSetts.Italic ? FontStyles.Italic : FontStyles.Normal;
        }

        public void IsActiveChanged(object sender, EventArgs e)
        {
            var selectedContent = AllPanels.LayoutDocumentPane.SelectedContent?.Content;

            bool showKv = selectedContent is TextEditorKVPanel;
            bool showLua = selectedContent is TextEditorLUAPanel;
            if (selectedContent is EditorsCollectionPanel)
            {
                var content = ((EditorsCollectionPanel) selectedContent).DocumentsPane.SelectedContent.Content;
                showKv = content is TextEditorKVPanel;
                showLua = content is TextEditorLUAPanel;
            }
            AllPanels.ObjectEditorForm.ShowEditorsMenu(showKv, showLua);
        }

        public void ForceClose()
        {
            Edited = false;
            PanelDocument.Close();
        }

        public ErrorParser SaveChanges()
        {
            if (!Edited)
                return null;

            try
            {
                ObjectRef.Children = TokenAnalizer.AnaliseText(TextEditor.Text);
            }
            catch (ErrorParser error)
            {
                return error;
            }
            Edited = false;

            return null;
        }

        public void Closing(object sender, CancelEventArgs e)
        {
            if (!Edited)
                return;

            var error = SaveChanges();
            if (error == null) return;

            var dialog = MessageBox.Show(Properties.Resources.TextContainErrors, Properties.Resources.TextContainErrorsCapture, MessageBoxButton.YesNo);
            switch (dialog)
            {
                case MessageBoxResult.Yes:
                    break;

                case MessageBoxResult.No:
                    e.Cancel = true;
                    break;
            }
        }

        public LayoutDocument PanelDocument { get; set; }

        void textEditor_TextArea_TextSelected(object sender, EventArgs e)
        {
            _offsetColorizer.selectedText = "";
            string selectedtext = TextEditor.TextArea.Selection.GetText();

            if (String.IsNullOrWhiteSpace(selectedtext))
                goto endFuncRedrawAll;

            //todo добавить это в настройки. Минимальная длина текста выделения для выделения повторов
            if (selectedtext.Length < 2)
                goto endFuncRedrawAll;

            _offsetColorizer.selectedText = selectedtext;

            endFuncRedrawAll:
            TextEditor.TextArea.TextView.Redraw();
        }

        void textEditor_TextArea_TextEntered(object sender, TextCompositionEventArgs e)
        {
            int offset = TextEditor.CaretOffset - 1;

            //skip comment zone
            if (ParserUtils.thisSymbolInCommentZone(TextEditor.Text, offset) != -1)
                return;

            if (e.Text == "\"") 
            {
                //skip if it end of key/value
                if (offset > 0 && Char.IsLetterOrDigit(TextEditor.Text[offset-1]))
                    return;

                bool? key = ParserUtils.ItsKey(TextEditor.Text, offset);
                if (key == null)
                    return;

                if (key == true)
                { //Its key
                    string ownerKey = ParserUtils.GetOwnerKeyBlockText(TextEditor.Text, offset);
                    if (ownerKey == "")
                        ownerKey = "ROOT";
                    KVToken ownerTok = BasicCompletionKV.Keys.GetChild(ownerKey);

                    if (ownerTok == null)
                        return; //Owner key not founded

                    completionWindow = new CompletionWindow(TextEditor.TextArea);
                    foreach (var tok in ownerTok.Children)
                    {
                        if (tok.Type == KVTokenType.Comment)
                            continue;

                        string descr = tok.GetChild("Description").Value;
                        descr = KVScriptResourcesKeys.ResourceManager.GetString(descr.Substring(1));
                        if (String.IsNullOrEmpty(descr))
                            descr = "";

                        completionWindow.CompletionList.CompletionData.Add(new MyCompletionData(tok.Key, descr));
                    }

                    completionWindow.Show();
                    completionWindow.Closed += delegate
                    {
                        completionWindow = null;
                    };
                }
                else
                { //Its value
                    string keyText = ParserUtils.GetKeyText(TextEditor.Text, offset);
                    if (keyText == "") return;

                    KVToken ownerTok = BasicCompletionKV.Values.GetChild(keyText);
                    if (ownerTok == null)
                        return; //Key not founded

                    completionWindow = new CompletionWindow(TextEditor.TextArea);
                    foreach (var tok in ownerTok.Children)
                    {
                        if (tok.Type == KVTokenType.Comment)
                            continue;

                        string descr = tok.Value;
                        descr = KVScriptResourcesValues.ResourceManager.GetString(descr.Substring(1));
                        if (String.IsNullOrEmpty(descr))
                            descr = "";

                        completionWindow.CompletionList.CompletionData.Add(new MyCompletionData(tok.Key, descr));
                    }
                    completionWindow.Show();
                    completionWindow.Closed += delegate
                    {
                        completionWindow = null;
                    };
                }

                return;
            }

            if (e.Text == "|")
            {
                bool? key = ParserUtils.ItsKey(TextEditor.Text, offset);
                if (key == null || key == true)
                    return;

                string keyText = ParserUtils.GetKeyText(TextEditor.Text, offset);
                if (keyText == "") return;

                KVToken ownerTok = BasicCompletionKV.Values.GetChild(keyText);
                if (ownerTok == null)
                    return; //Key not founded

                completionWindow = new CompletionWindow(TextEditor.TextArea);
                foreach (var tok in ownerTok.Children)
                {
                    completionWindow.CompletionList.CompletionData.Add(new MyCompletionData(tok.Key, tok.Value));
                }
                completionWindow.Show();
                completionWindow.Closed += delegate
                {
                    completionWindow = null;
                };
            }
        }

        void textEditor_TextArea_TextEntering(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Length > 0 && completionWindow != null)
            {
                if (!char.IsLetterOrDigit(e.Text[0]))
                {
                    // Whenever a non-letter is typed while the completion window is open,
                    // insert the currently selected element.
                    completionWindow.CompletionList.RequestInsertion(e);
                }
            }
            // Do not set e.Handled=true.
            // We still want to insert the character that was typed.
        }

        private void TextChanged(object sender, EventArgs e)
        {
            _offsetColorizer.tempText = TextEditor.Text;
            Edited = true;
            DataBase.Edited = true;
        }

        public void SetText(string text)
        {
            bool editedTemp = DataBase.Edited;
            TextEditor.Document = new TextDocument(text);
            Edited = false;
            DataBase.Edited = editedTemp;
        }

        public void ButtonUndo_Click()
        {
            TextEditor.Undo();
            Edited = true;
            DataBase.Edited = true;
        }

        public void ButtonRedo_Click()
        {
            TextEditor.Redo();
            Edited = true;
            DataBase.Edited = true;
        }

        public void ButtonCommentIt_Click()
        {
            if (String.IsNullOrEmpty(TextEditor.SelectedText))
                return;

            int selStart = TextEditor.SelectionStart;
            int selLen = TextEditor.SelectionLength;
            var startLine = TextEditor.Document.GetLineByOffset(selStart);
            int column = selStart - startLine.Offset;

            if (selLen > (startLine.TotalLength - column))
            {// Multipline selected
                int firstOffset = startLine.Offset;
                int lastOffset = 0;
                int numLines = 0;
                string tempDocText = TextEditor.Text;
                int remLen = selLen + column;
                var nextLine = startLine;
                while (nextLine != null)
                {
                    int placeComment = ParserUtils.SkipSpace(TextEditor.Text, nextLine.Offset);
                    tempDocText = tempDocText.Insert(placeComment + numLines * 2, "//");
                    lastOffset = nextLine.EndOffset;
                    numLines++;

                    if (nextLine.TotalLength > remLen)
                        break;

                    remLen -= nextLine.TotalLength;
                    nextLine = nextLine.NextLine;
                }

                TextEditor.Document.Text = tempDocText;
                TextEditor.Select(firstOffset, lastOffset - firstOffset + numLines*2);
            }
            else
            {//Single line selected
                TextEditor.Document.Text = TextEditor.Text.Insert(selStart, "//");
                TextEditor.Select(selStart, selLen + 2);
            }
            Edited = true;
            DataBase.Edited = true;
        }

        public void ButtonUnCommentIt_Click()
        {
            int selStart = TextEditor.SelectionStart;
            int selLen = TextEditor.SelectionLength;
            var startLine = TextEditor.Document.GetLineByOffset(selStart);
            int column = selStart - startLine.Offset;

            if (selLen > (startLine.TotalLength - column))
            {// Multipline selected
                int remLen = selLen + column;
                int lastOffset = 0;
                var nextLine = startLine;
                TextEditor.SelectionLength = 0;

                while (nextLine != null)
                {
                    int commentStart = GetCommentStart(TextEditor.Text, nextLine.Offset);
                    if (commentStart != -1)
                        TextEditor.Document.Text = TextEditor.Document.Text.Remove(commentStart, 2);

                    lastOffset = nextLine.EndOffset;
                    if (nextLine.TotalLength > remLen)
                        break;

                    remLen -= nextLine.TotalLength;
                    nextLine = nextLine.NextLine;
                }
                if (lastOffset >= TextEditor.Text.Length)
                    lastOffset = TextEditor.Text.Length;
                TextEditor.SelectionStart = lastOffset;
            }
            else
            {// single line selected
                int commentStart = GetCommentStart(TextEditor.Text, selStart);
                if (commentStart == -1)
                    return;
                TextEditor.Document.Text = TextEditor.Document.Text.Remove(commentStart, 2);
                TextEditor.SelectionLength = 0;
                TextEditor.SelectionStart = commentStart;
            }
            Edited = true;
            DataBase.Edited = true;
        }

        private int GetCommentStart(string text, int offset)
        {
            int commentStart = ParserUtils.thisSymbolInCommentZone(text, offset);
            if (commentStart == -1)
            {
                if (offset + 2 > text.Length)
                    return -1;
                commentStart = ParserUtils.SkipSpace(text, offset);
                if (!ParserUtils.ThisPositionStartOfComment(text, commentStart))
                    return -1;
            }

            return commentStart;
        }

        public void ButtonAutoTabIt_Click()
        {
            if (string.IsNullOrEmpty(TextEditor.SelectedText) || string.IsNullOrWhiteSpace(TextEditor.SelectedText))
                return;

            int start = ParserUtils.GetPositionFirstPrevSymbol(TextEditor.Text, '\n', TextEditor.SelectionStart) + 1;
            int end = ParserUtils.GetPositionFirstNextSymbol(TextEditor.Text, '\n', TextEditor.SelectionStart + TextEditor.SelectedText.Length);
            if (end == -1)
                end = TextEditor.Text.Length - 1;
            string selected = TextEditor.Text.Substring(start, end - start);
            //TextEditor.SelectedText = selected;

            List<string> lines = new List<string>();
            while (selected.Length > 0)
            {
                int pos = selected.IndexOf('\n');
                if (pos == -1)
                {
                    lines.Add(selected);
                    break;
                }
                string line = selected.Substring(0, pos + 1);
                lines.Add(line);
                selected = selected.Substring(pos + 1);
            }
            if (lines.Count == 0)
                return;

            AnalyseInThisLevel(ref lines, 0, lines.Count - 1);

            string strLines = lines.Aggregate("", (current, line) => current + line);
            TextEditor.Document.Text = string.Concat(TextEditor.Text.Substring(0, start), strLines, TextEditor.Text.Substring(end));
            //TextEditor.SelectedText = strLines;
            Edited = true;
            DataBase.Edited = true;
        }

        private void AnalyseInThisLevel(ref List<string> lines, int first, int last)
        {
            int index;
            //for (int i = first; i <= last; i++)
            //{
            //    if (OffsetColorizer.FindSymbol(lines[i], '{', 0) != -1)
            //    {
            //        todo
            //    }
            //}
            //А надо ли?!
            MakeTabsInLines(ref lines, first, last);
        }

        private void MakeTabsInLines(ref List<string> lines, int first, int last)
        {
            List<int> tabingIndex = new List<int>(last-first);
            List<int> posEndKey = new List<int>(last-first);

            for (int i = first; i <= last; i++)
            {
                int pos;
                pos = ParserUtils.FindSymbol(lines[i], '\"', 0);
                if (pos == -1) continue;
                int endKey = ParserUtils.FindSymbol(lines[i], '\"', pos + 1);
                pos = ParserUtils.FindSymbol(lines[i], '\"', endKey + 1);
                if (pos == -1) continue;

                posEndKey.Add(endKey);
                tabingIndex.Add(i);
                lines[i] = string.Concat(lines[i].Substring(0, endKey+1), lines[i].Substring(pos));
            }

            int maxEndKeyPos = posEndKey.Max();

            for (int i = 0; i < tabingIndex.Count; i++)
            {
                int maxSpaceNum = maxEndKeyPos - posEndKey[i] + 1;
                lines[tabingIndex[i]] = string.Concat(
                    lines[tabingIndex[i]].Substring(0, posEndKey[i] + 1), 
                    StringUtils.GetCharMultip(' ', maxSpaceNum),
                    StringUtils.GetCharMultip('\t', 2), //todo make this num settingable
                    lines[tabingIndex[i]].Substring(posEndKey[i] + 1));
            }
        }

        

        
    }
}
