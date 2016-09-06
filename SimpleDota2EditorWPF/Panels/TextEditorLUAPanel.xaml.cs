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
using Xceed.Wpf.AvalonDock.Layout;

namespace SimpleDota2EditorWPF.Panels
{
    /// <summary>
    /// Логика взаимодействия для TextEditorLUAPanel.xaml
    /// </summary>
    public partial class TextEditorLUAPanel : UserControl, IEditor
    {
        public TextEditorLUAPanel()
        {
            InitializeComponent();

            IHighlightingDefinition customHighlighting;

            using (XmlReader reader = new XmlTextReader(@"Lua.xshd"))
            {
                customHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
            }

            HighlightingManager.Instance.RegisterHighlighting("lua", new string[] { ".lua" }, customHighlighting);
            TextEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("lua");

            TextEditor.TextChanged += TextChanged;
        }

        public void LoadTextFromFile(string path)
        {
            bool editedTemp = DataBase.Edited;
            FilePath = path;
            TextEditor.Document = new TextDocument(File.ReadAllText(path));
            TextEditor.IsModified = false;
            DataBase.Edited = editedTemp;
        }

        private void TextChanged(object sender, EventArgs e)
        {
            if (TextEditor.IsModified)
                PanelName = panelName;
            DataBase.Edited = true;
        }

        public string FilePath;
        public string PanelName {
            set
            {
                if (panelName == value) return;
                panelName = value;
                PanelDocument.Title = value + (TextEditor.IsModified ? @" *" : "");
            }
            get { return panelName; }
        }
        public KVToken ObjectRef { get; set; }
        public ObjectsViewPanel.ObjectTypePanel ObjectType { get; set; }
        public Settings.EditorType EditorType { get; }
        public void ForceClose()
        {
            TextEditor.IsModified = false;
            PanelDocument.Close();
        }

        private string panelName;

        public ErrorParser SaveChanges()
        {
            if (!TextEditor.IsModified)
                return null;


            StreamWriter file = new StreamWriter(FilePath, false);
            file.WriteLine(TextEditor.Document.Text);
            file.Close();

            TextEditor.IsModified = false;
            PanelName = panelName;

            return null;
        }

        public void Closing(object sender, CancelEventArgs e)
        {
            if (!TextEditor.IsModified)
                return;

            SaveChanges();
        }

        public LayoutDocument PanelDocument { get; set; }
        public void Update()
        {
            //throw new NotImplementedException(); todo
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
