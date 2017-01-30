using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using System.Xml;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using KV_reloaded;
using SimpleDota2EditorWPF.Dialogs;
using Xceed.Wpf.AvalonDock.Layout;

namespace SimpleDota2EditorWPF.Panels
{
    /// <summary>
    /// Логика взаимодействия для TextEditorLUAPanel.xaml
    /// </summary>
    public partial class TextEditorLUAPanel : UserControl, IEditor
    {
        public IEditor ParentEditor { get; set; }
        public bool FindNext(FindStruct find)
        {
            throw new NotImplementedException(); //todo
        }

        public bool FindPrev(FindStruct find)
        {
            throw new NotImplementedException();
        }

        public int CountIt(FindStruct find)
        {
            throw new NotImplementedException();
        }

        public bool Replace(FindStruct find)
        {
            throw new NotImplementedException();
        }

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
        public TextEditorLUAPanel()
        {
            if (customHighlighting == null)
                Load();

            InitializeComponent();

            TextEditor.TextChanged += TextChanged;
            TextEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("lua");
        }

        private static IHighlightingDefinition customHighlighting = null;
        private static void Load()
        {
            string luaxml = File.ReadAllText(@"Lua.xshd");

            luaxml = luaxml.Replace(@"#Digits", DataBase.Settings.HighSettsLua.DigitsColor);
            luaxml = luaxml.Replace(@"#Hack", DataBase.Settings.HighSettsLua.HackColor);
            luaxml = luaxml.Replace(@"#KeyWords", DataBase.Settings.HighSettsLua.KeyWordsColor);
            luaxml = luaxml.Replace(@"#LineComment", DataBase.Settings.HighSettsLua.LineCommentsColor);
            luaxml = luaxml.Replace(@"#MultiLineString", DataBase.Settings.HighSettsLua.MultilineStringsColor);
            luaxml = luaxml.Replace(@"#Punctuation", DataBase.Settings.HighSettsLua.PunctuationsColor);
            luaxml = luaxml.Replace(@"#Tables", DataBase.Settings.HighSettsLua.TablesColor);
            luaxml = luaxml.Replace(@"#Todo", DataBase.Settings.HighSettsLua.TodoColor);
            luaxml = luaxml.Replace(@"#FuncNames", DataBase.Settings.HighSettsLua.UserFunctionsColor);
            luaxml = luaxml.Replace(@"#BlockComment", DataBase.Settings.HighSettsLua.BlockCommentColor);
            luaxml = luaxml.Replace(@"#Char", DataBase.Settings.HighSettsLua.CharColor);
            luaxml = luaxml.Replace(@"#Strings", DataBase.Settings.HighSettsLua.StringsColor);

            StreamWriter file = new StreamWriter(@"Lua.xshd.temp", false);
            file.WriteLine(luaxml);
            file.Close();

            using (XmlReader reader = new XmlTextReader(@"Lua.xshd.temp"))
            {
                customHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
            }

            HighlightingManager.Instance.RegisterHighlighting("lua", new string[] { ".lua" }, customHighlighting);
        }

        public void LoadTextFromFile(string path)
        {
            bool editedTemp = DataBase.Edited;
            FilePath = path;
            TextEditor.Document = new TextDocument(File.ReadAllText(path));
            Edited = false;
            DataBase.Edited = editedTemp;
        }

        private void TextChanged(object sender, EventArgs e)
        {
            Edited = true;
            DataBase.Edited = true;
        }

        public string FilePath;
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
        public void ForceClose()
        {
            Edited = false;
            PanelDocument.Close();
        }

        public ErrorParser SaveChanges()
        {
            if (!Edited)
                return null;


            StreamWriter file = new StreamWriter(FilePath, false);
            file.WriteLine(TextEditor.Document.Text);
            file.Close();

            Edited = false;

            return null;
        }

        public void Closing(object sender, CancelEventArgs e)
        {
            if (!Edited)
                return;

            SaveChanges();
        }

        public LayoutDocument PanelDocument { get; set; }
        public void Update()
        {
            Load();
            TextEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("lua");
            TextEditor.FontFamily = new FontFamily(DataBase.Settings.HighSettsLua.Font);
            TextEditor.FontSize = DataBase.Settings.HighSettsLua.FontSize;
            TextEditor.FontWeight = DataBase.Settings.HighSettsLua.Bold ? FontWeights.Bold : FontWeights.Normal;
            TextEditor.FontStyle = DataBase.Settings.HighSettsLua.Italic ? FontStyles.Italic : FontStyles.Normal;
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

        public void IsActiveChanged(object sender, EventArgs e)
        {
            var selectedContent = AllPanels.LayoutDocumentPane.SelectedContent?.Content;

            bool showKv = selectedContent is TextEditorKVPanel;
            bool showLua = selectedContent is TextEditorLUAPanel;
            if (selectedContent is EditorsCollectionPanel)
            {
                var content = ((EditorsCollectionPanel)selectedContent).DocumentsPane.SelectedContent.Content;
                showKv = content is TextEditorKVPanel;
                showLua = content is TextEditorLUAPanel;
            }
            AllPanels.ObjectEditorForm.ShowEditorsMenu(showKv, showLua);
        }
    }
}
