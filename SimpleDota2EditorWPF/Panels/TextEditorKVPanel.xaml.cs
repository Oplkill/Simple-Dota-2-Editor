using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Rendering;
using KV_reloaded;
using SimpleDota2EditorWPF.ScriptsUtils.KV;
using Xceed.Wpf.AvalonDock.Layout;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace SimpleDota2EditorWPF.Panels
{
    /// <summary>
    /// Логика взаимодействия для TextEditorPanel.xaml
    /// </summary>
    public partial class TextEditorKVPanel : UserControl, IEditor
    {
        public string PanelName
        {
            set
            {
                if (panelName == value) return;
                panelName = value;
                PanelDocument.Title = value + (TextEditor.IsModified ? @" *" : "");
            }
            get { return panelName; }
        }
        private string panelName;
        public KVToken ObjectRef { get; set; }
        public ObjectsViewPanel.ObjectTypePanel ObjectType { get; set; }
        public Settings.EditorType EditorType { get; }
        private OffsetColorizer _offsetColorizer;

        public TextEditorKVPanel()
        {
            InitializeComponent();

            _offsetColorizer = new OffsetColorizer();
            TextEditor.TextChanged += TextChanged;
            TextEditor.TextArea.TextView.LineTransformers.Add(_offsetColorizer);
            TextEditor.TextArea.TextEntering += textEditor_TextArea_TextEntering;
            TextEditor.TextArea.TextEntered += textEditor_TextArea_TextEntered;
            Update();
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
            TextEditor.IsModified = false;
            PanelDocument.Close();
        }

        public ErrorParser SaveChanges()
        {
            if (!TextEditor.IsModified)
                return null;

            try
            {
                ObjectRef.Children = TokenAnalizer.AnaliseText(TextEditor.Text);
            }
            catch (ErrorParser error)
            {
                return error;
            }
            TextEditor.IsModified = false;
            PanelName = panelName;

            return null;
        }

        public void Closing(object sender, CancelEventArgs e)
        {
            if (!TextEditor.IsModified)
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

        CompletionWindow completionWindow;

        void textEditor_TextArea_TextEntered(object sender, TextCompositionEventArgs e)
        {
            int offset = TextEditor.CaretOffset - 1;

            //skip comment zone
            if (OffsetColorizer.thisSymbolInCommentZone(TextEditor.Text, offset) != -1)
                return;

            if (e.Text == "\"") 
            {
                //skip if it end of key/value
                if (offset > 0 && Char.IsLetterOrDigit(TextEditor.Text[offset-1]))
                    return;

                bool? key = OffsetColorizer.ItsKey(TextEditor.Text, offset);
                if (key == null)
                    return;

                if (key == true)
                { //Its key
                    string ownerKey = OffsetColorizer.GetOwnerKeyBlockText(TextEditor.Text, offset);

                    if (!BasicCompletionKV.DataKeys.ContainsKey(ownerKey))
                        return; //Owner key not founded

                    var list = BasicCompletionKV.DataKeys[ownerKey];

                    completionWindow = new CompletionWindow(TextEditor.TextArea);
                    foreach (var item in list)
                    {
                        completionWindow.CompletionList.CompletionData.Add(item);
                    }
                    completionWindow.Show();
                    completionWindow.Closed += delegate
                    {
                        completionWindow = null;
                    };
                }
                else
                { //Its value
                    string keyText = OffsetColorizer.GetKeyText(TextEditor.Text, offset);

                    if (!BasicCompletionKV.DataValues.ContainsKey(keyText))
                        return; //Key not founded

                    var list = BasicCompletionKV.DataValues[keyText];

                    completionWindow = new CompletionWindow(TextEditor.TextArea);
                    foreach (var item in list)
                    {
                        completionWindow.CompletionList.CompletionData.Add(item);
                    }
                    completionWindow.Show();
                    completionWindow.Closed += delegate
                    {
                        completionWindow = null;
                    };
                }

                return;

                //Console.WriteLine(TextEditor.Document.GetLineByOffset(TextEditor.CaretOffset));
                //Console.WriteLine(TextEditor.Document.GetLocation(TextEditor.CaretOffset));
                //Console.WriteLine(GetOwnerKeyBlockText(TextEditor.Text, TextEditor.CaretOffset));
                //// Open code completion after the user has pressed dot:
                //completionWindow = new CompletionWindow(TextEditor.TextArea);
                //IList<ICompletionData> data = completionWindow.CompletionList.CompletionData;
                //data.Add(new MyCompletionData("Item1", "Great item1"));
                //data.Add(new MyCompletionData("Body1", "Thats just a body!"));
                //data.Add(new MyCompletionData("Bod2", "Hmmm another body"));
                //completionWindow.Show();
                //completionWindow.Closed += delegate {
                //    completionWindow = null;
                //};
            }

            if (e.Text == "|")
            {
                bool? key = OffsetColorizer.ItsKey(TextEditor.Text, offset);
                if (key == null || key == true)
                    return;

                string keyText = OffsetColorizer.GetKeyText(TextEditor.Text, offset);

                if (!BasicCompletionKV.DataValues.ContainsKey(keyText))
                    return; //Key not founded

                var list = BasicCompletionKV.DataValues[keyText];

                completionWindow = new CompletionWindow(TextEditor.TextArea);
                foreach (var item in list)
                {
                    completionWindow.CompletionList.CompletionData.Add(item);
                }
                completionWindow.Show();
                completionWindow.Closed += delegate
                {
                    completionWindow = null;
                };
            }
        }

        //todo вынести отсюда
        #region todo ВЫНЕСТИ отсюда


        

        

#endregion

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

        /// Implements AvalonEdit ICompletionData interface to provide the entries in the
        /// completion drop down.
        public class MyCompletionData : ICompletionData
        {
            public MyCompletionData(string text, string description)
            {
                this.Text = text;
                this.description = description;
            }

            public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
            {
                textArea.Document.Replace(completionSegment, this.Text);
            }

            public System.Windows.Media.ImageSource Image
            {
                get { return null; }
            }

            public string Text { get; private set; }

            // Use this property if you want to show a fancy UIElement in the list.
            public object Content
            {
                get { return this.Text; }
            }

            public object Description
            {
                get { return this.description; }
            }

            public double Priority { get; }
            public string description;
        }

        private void TextChanged(object sender, EventArgs e)
        {
            _offsetColorizer.tempText = TextEditor.Text;
            if (TextEditor.IsModified)
                PanelName = panelName;
            DataBase.Edited = true;
        }

        public void SetText(string text)
        {
            bool editedTemp = DataBase.Edited;
            TextEditor.Document = new TextDocument(text);
            TextEditor.IsModified = false;
            DataBase.Edited = editedTemp;
        }

        public void ButtonUndo_Click()
        {
            TextEditor.Undo();
            DataBase.Edited = true;
        }

        public void ButtonRedo_Click()
        {
            TextEditor.Redo();
            DataBase.Edited = true;
        }

        public void ButtonCommentIt_Click()
        {
            //todo
        }

        public void ButtonUnCommentIt_Click()
        {
            //todo
        }

        public void ButtonAutoTabIt_Click()
        {
            if (string.IsNullOrEmpty(TextEditor.SelectedText) || string.IsNullOrWhiteSpace(TextEditor.SelectedText))
                return;

            int start = OffsetColorizer.GetPositionFirstPrevSymbol(TextEditor.Text, '\n', TextEditor.SelectionStart) + 1;
            int end = OffsetColorizer.GetPositionFirstNextSymbol(TextEditor.Text, '\n', TextEditor.SelectionStart + TextEditor.SelectedText.Length);
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
            MakeTabsInLines(ref lines, first, last);
        }

        private void MakeTabsInLines(ref List<string> lines, int first, int last)
        {
            List<int> tabingIndex = new List<int>(last-first);
            List<int> posEndKey = new List<int>(last-first);

            for (int i = first; i <= last; i++)
            {
                int pos;
                pos = OffsetColorizer.FindSymbol(lines[i], '\"', 0);
                if (pos == -1) continue;
                int endKey = OffsetColorizer.FindSymbol(lines[i], '\"', pos + 1);
                pos = OffsetColorizer.FindSymbol(lines[i], '\"', endKey + 1);
                if (pos == -1) continue;

                posEndKey.Add(endKey);
                tabingIndex.Add(i);
                lines[i] = string.Concat(lines[i].Substring(0, endKey+1), lines[i].Substring(pos));
            }

            const int tabSpaces = 4;//todo сделать изменяемым в настройках
            int maxEndKey = posEndKey.Concat(new[] {0}).Max() + tabSpaces;

            for (int i = 0; i < tabingIndex.Count; i++)
            {
                int tabNum = (maxEndKey - posEndKey[i])/tabSpaces;
                lines[tabingIndex[i]] = string.Concat(
                    lines[tabingIndex[i]].Substring(0, posEndKey[i] + 1), 
                    GetCharMultip('\t', tabNum), 
                    lines[tabingIndex[i]].Substring(posEndKey[i] + 1));
            }
        }

        private string GetCharMultip(char ch, int num)
        {
            string str = "";

            for (int i = 0; i <= num; i++)
            {
                str += ch;
            }

            return str;
        }

        public class OffsetColorizer : DocumentColorizingTransformer
        {
            public string tempText;
            private SolidColorBrush[] brushes;

            public OffsetColorizer()
            {
                Update();
            }

            public void Update()
            {
                brushes = new SolidColorBrush[6];
                brushes[(int)KV_STYLES.STYLE_DEFAULT] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(DataBase.Settings.HighSetts.DefaultWordColor));
                brushes[(int)KV_STYLES.STYLE_COMMENT] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(DataBase.Settings.HighSetts.CommentColor));
                brushes[(int)KV_STYLES.STYLE_KVBLOCK] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(DataBase.Settings.HighSetts.KVBlockColor));
                brushes[(int)KV_STYLES.STYLE_KEY] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(DataBase.Settings.HighSetts.KeyColor));
                brushes[(int)KV_STYLES.STYLE_VALUE_STRING] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(DataBase.Settings.HighSetts.ValueStringColor));
                brushes[(int)KV_STYLES.STYLE_VALUE_NUMBER] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(DataBase.Settings.HighSetts.ValueNumberColor));
            }

            protected override void ColorizeLine(DocumentLine line)
            {
                if (line.Length == 0)
                    return;

                try
                {
                    var pos = line.Offset;
                    var endPos = line.EndOffset;
                    bool key = true; // Ожидается, что будет далее, ключ(true) или значение(false)

                    var ch = tempText[pos];
                    while (pos < endPos)
                    {
                        ch = tempText[pos];
                        if (!SomeUtils.StringUtils.IsSpaceOrTab(ch) || ch != '\n')
                            switch (ch)
                            {
                                case '\"':
                                    int end = nextCharThroughIs(tempText, pos, '\"');
                                    if (end == -1)
                                    {
                                        ChangeLinePart(pos, endPos + 1, element => element.TextRunProperties.SetForegroundBrush(
                                            brushes[key ? (int)KV_STYLES.STYLE_KEY : (int)KV_STYLES.STYLE_VALUE_STRING]));
                                        return;
                                    }

                                    int isBlock = nextCharThroughIs(tempText, end + 1, '{');
                                    if (isBlock != -1)
                                    {
                                        key = true;
                                        if (end > endPos) end = endPos;
                                        ChangeLinePart(pos, end + 1, element => element.TextRunProperties.SetForegroundBrush(
                                            brushes[(int)KV_STYLES.STYLE_KVBLOCK]));
                                        pos = end;
                                    }
                                    else
                                    {
                                        string str = tempText.Substring(pos + 1, end - (pos + 1));
                                        int style = 0;
                                        if (SomeUtils.StringUtils.isDigit(str))
                                            style = (int)KV_STYLES.STYLE_VALUE_NUMBER;
                                        else
                                            style = key ? (int)KV_STYLES.STYLE_KEY : (int)KV_STYLES.STYLE_VALUE_STRING;

                                        if (end > endPos) end = endPos;
                                        ChangeLinePart(pos, end + 1, element => element.TextRunProperties.SetForegroundBrush(
                                            brushes[style]));
                                        pos = end;
                                        key = !key;
                                    }
                                    break;

                                case '/':
                                    if (pos + 1 >= tempText.Length)
                                        return;
                                    if (tempText[pos + 1] != '/')
                                        break;

                                    ChangeLinePart(pos, endPos, element => element.TextRunProperties.SetForegroundBrush(
                                            brushes[(int)KV_STYLES.STYLE_COMMENT]));
                                    return;
                                    break;

                                case '{':
                                case '}':
                                    ChangeLinePart(pos, pos + 1, element => element.TextRunProperties.SetForegroundBrush(
                                            brushes[(int)KV_STYLES.STYLE_DEFAULT]));
                                    break;
                            }

                        pos++;
                    }
                }
                catch (Exception ex)
                {
                    //Console.WriteLine(@"Error - " + ex.Message);

                }
            }

            [SuppressMessage("ReSharper", "InconsistentNaming")]
            public enum KV_STYLES
            {
                STYLE_DEFAULT,
                STYLE_KEY,
                STYLE_KVBLOCK,
                STYLE_VALUE_NUMBER,
                STYLE_VALUE_STRING,
                STYLE_COMMENT,
            }

            private int nextCharThroughIs(string str, int pos, char target)
            {
                int n = pos;
                while (n < str.Length)
                {
                    switch (str[n])
                    {
                        case '{':
                            if (target == '{')
                                return n;
                            break;

                        case '/':
                            while (str[n] != '\n')
                            {
                                n++;
                            }
                            break;

                        case '\"':
                            if (target == '{')
                                return -1;
                            n++;
                            if (n >= str.Length)
                                return -1;

                            while (str[n] != '\"')
                            {
                                n++;
                                if (n >= str.Length)
                                    return -1;
                            }
                            if (target == '\"')
                                return n;
                            break;
                    }

                    n++;
                }

                return -1;
            }

            public static int FindSymbol(string text, char symbol, int start)
            {
                int len = text.Length;
                while (start < len)
                {
                    if (text[start] == symbol)
                    {
                        int symb = thisSymbolInCommentZone(text, start);
                        if (symb == -1)
                            return start;
                        else
                            start = symb;
                    }

                    start++;
                }

                return -1;
            }

            /// <summary>
            /// Getting first prev symbol thruegh comments
            /// </summary>
            public static int GetPositionFirstPrevSymbol(string text, char symbol, int start)
            {
                while (start > 0)
                {
                    if (text[start] == symbol)
                    {
                        int symb = thisSymbolInCommentZone(text, start);
                        if (symb == -1)
                            return start;
                        else
                            start = symb;
                    }

                    start--;
                }

                return -1;
            }

            public static int GetPositionFirstNextSymbol(string text, char symbol, int start)
            {
                int len = text.Length;
                while (start < len)
                {
                    if (text[start] == symbol)
                    {
                        int symb = thisSymbolInCommentZone(text, start);
                        if (symb == -1)
                            break;
                        else
                            start = symb;
                    }

                    start++;
                }

                return start;
            }

            /// <summary>
            /// If it in comment zone - returns start of comment, else - "-1"
            /// </summary>
            public static int thisSymbolInCommentZone(string text, int symbPos)
            {
                while (symbPos > 0 && symbPos < text.Length)
                {
                    if (text[symbPos] == '\\')
                        return symbPos;
                    else if (text[symbPos] == '\n')
                        return -1;

                    symbPos--;
                }

                return -1;
            }

            public static string GetKeyText(string text, int offset)
            {
                int pos = OffsetColorizer.GetPositionFirstPrevSymbol(text, '\n', offset);
                string line = text.Substring(pos + 1, offset - pos);

                int posStart = OffsetColorizer.GetPositionFirstNextSymbol(line, '\"', 0);
                pos = OffsetColorizer.GetPositionFirstNextSymbol(line, '\"', posStart + 1);

                return line.Substring(posStart + 1, pos - posStart - 1);
            }

            /// <summary>
            /// true - значит это ключ
            /// false - это значение
            /// null - это ни ключ, ни значение. Неопределенно
            /// </summary>
            public static bool? ItsKey(string text, int offset)
            {
                int pos = OffsetColorizer.GetPositionFirstPrevSymbol(text, '\n', offset);
                int localOfsset = offset - pos;
                string line = text.Substring(pos + 1, localOfsset);

                pos = OffsetColorizer.GetPositionFirstNextSymbol(line, '\"', 0);
                if (pos == -1)
                    return null;
                if (pos + 1 >= line.Length)
                    return true;
                pos = OffsetColorizer.GetPositionFirstNextSymbol(line, '\"', pos + 1);
                if (pos == -1)
                    return true;

                pos = OffsetColorizer.GetPositionFirstNextSymbol(line, '\"', pos + 1);
                if (pos == -1 || pos == localOfsset)
                    return null;
                pos = OffsetColorizer.GetPositionFirstNextSymbol(line, '\"', pos + 1);
                if (pos == -1)
                    return false;
                if (pos < localOfsset)
                    return null;
                else
                    return false;

                return null;
            }

            /// <summary>
            /// Возвращает текст ключа блока, который содержит этот offset
            /// Если это корень, то возвращается пустая строка
            /// Null если есть ошибка
            /// </summary>
            public static string GetOwnerKeyBlockText(string text, int offset)
            {
                int pos = OffsetColorizer.GetPositionFirstPrevSymbol(text, '{', offset);
                if (pos == -1) return "";
                pos = OffsetColorizer.GetPositionFirstPrevSymbol(text, '\"', pos);
                if (pos == -1) return null;
                int posStart = OffsetColorizer.GetPositionFirstPrevSymbol(text, '\"', pos - 1);
                if (posStart == -1) return null;
                if (posStart + 1 >= pos) return null;

                return text.Substring(posStart + 1, pos - posStart - 1);
            }
        }
    }
}
