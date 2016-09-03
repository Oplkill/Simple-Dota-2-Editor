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
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;
using KV_reloaded;
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

            /// <summary>
            /// Getting first prev symbol thruegh comments
            /// </summary>
            private int GetPositionFirstPrevSymbol(string text, char symbol, int start)
            {
                while (start > 0)
                {
                    if (text[start] == symbol)
                    {
                        int symb = thisSymbolInCommentZone(text, start);
                        if (symb == -1)
                            break;
                        else
                            start = symb;
                    }

                    start--;
                }

                return start;
            }

            /// <summary>
            /// If it in comment zone - returns start of comment, else - "-1"
            /// </summary>
            private int thisSymbolInCommentZone(string text, int symbPos)
            {
                while (symbPos > 0)
                {
                    if (text[symbPos] == '\\')
                        return symbPos;
                    else if (text[symbPos] == '\n')
                        return -1;

                    symbPos--;
                }

                return -1;
            }
        }
    }
}
