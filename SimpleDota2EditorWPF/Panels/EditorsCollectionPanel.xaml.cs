using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using KV_reloaded;
using SimpleDota2EditorWPF.Dialogs;
using Xceed.Wpf.AvalonDock.Layout;

namespace SimpleDota2EditorWPF.Panels
{
    /// <summary>
    /// Логика взаимодействия для EditorsCollectionPanel.xaml
    /// </summary>
    public partial class EditorsCollectionPanel : UserControl, IEditor
    {
        public IEditor ParentEditor { get; set; }
        public bool? FindNext(FindStruct find)
        {
            throw new NotImplementedException();
        }

        public bool? FindPrev(FindStruct find)
        {
            throw new NotImplementedException();
        }

        public int CountIt(FindStruct find)
        {
            throw new NotImplementedException();
        }

        public bool? Replace(FindStruct find)
        {
            throw new NotImplementedException();
        }

        public bool Edited
        {
            get { return edited; }
            set
            {
                PanelDocument.Title = PanelName + (value ? @" *" : "");
                edited = value;
            }
        }
        private bool edited;
        public EditorsCollectionPanel()
        {
            InitializeComponent();
        }

        public string PanelName
        {
            get { return panelName; }
            set
            {
                PanelDocument.Title = value + (Edited ? @" *" : "");
                var panels = DocumentsPane.Children.Where(doc => doc.Content is IEditor);
                foreach (var panel in panels)
                {
                    if (((IEditor) panel.Content).PanelName == panelName)
                    {
                        ((IEditor) panel.Content).PanelName = value;
                    }
                }
                panelName = value;
            }
        }

        private string panelName;
        public KVToken ObjectRef { get; set; }
        public ObjectsViewPanel.ObjectTypePanel ObjectType { get; set; }
        public Settings.EditorType EditorType { get; }
        public void ForceClose()
        {
            var panels = DocumentsPane.Children.Where(doc => doc.Content is IEditor);

            foreach (var panel in panels)
            {
                ((IEditor)panel.Content).ForceClose();
            }
        }

        public ErrorParser SaveChanges()
        {
            ErrorParser errors = null;
            var panels = DocumentsPane.Children.Where(doc => doc.Content is IEditor);

            foreach (var panel in panels)
            {
                var error = ((IEditor) panel.Content).SaveChanges();
                if (error != null)
                    errors = error;
            }

            return errors;
        }

        public void Closing(object sender, CancelEventArgs e)
        {
            var errors = SaveChanges();

            if (errors == null)
                return;

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
        public void Update()
        {
            var panels = DocumentsPane.Children.Where(doc => doc.Content is IEditor);

            foreach (var panel in panels)
            {
                ((IEditor)panel.Content).Update();
            }
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
