using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace SimpleDota2EditorWPF.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для FindWindowDialog.xaml
    /// </summary>
    public partial class FindWindowDialog : Window
    {
        public FindWindowDialog()
        {
            InitializeComponent();
        }

        private FindStruct GetFindSettingsStruct()
        {
            return new FindStruct()
            {
                text = textBoxFind.Text,
                loop = checkBoxLoop.IsChecked != null && checkBoxLoop.IsChecked.Value,
                registr = checkBoxRegister.IsChecked != null && checkBoxRegister.IsChecked.Value,
            };
        }

        private void buttonPrev_Click(object sender, RoutedEventArgs e)
        {
            var selectedContent = AllPanels.LayoutDocumentPane.SelectedContent?.Content as IEditor;
            if (selectedContent == null) return;

            var reachedStart = selectedContent.FindPrev(GetFindSettingsStruct());
            if (reachedStart)
                this.Title = "Find" + " - " + "Reached start of document"; //todo move to resource
            else if (this.Title != "Find")
                this.Title = "Find"; //todo move to resource

        }

        private void buttonNext_Click(object sender, RoutedEventArgs e)
        {
            var selectedContent = AllPanels.LayoutDocumentPane.SelectedContent?.Content as IEditor;
            if (selectedContent == null) return;

            var reachedEnd = selectedContent.FindNext(GetFindSettingsStruct());
            if (reachedEnd)
                this.Title = "Find" + " - " + "Reached end of document"; //todo move to resource
            else if (this.Title != "Find")
                this.Title = "Find"; //todo move to resource
        }

        private void buttonCount_Click(object sender, RoutedEventArgs e)
        {
            var selectedContent = AllPanels.LayoutDocumentPane.SelectedContent?.Content as IEditor;
            if (selectedContent == null) return;

            int number = selectedContent.CountIt(GetFindSettingsStruct());
            this.Title = "Find" + " - " + "Number items " + number; //todo move to resource
        }

        private void buttonReplace_Click(object sender, RoutedEventArgs e)
        {
            var selectedContent = AllPanels.LayoutDocumentPane.SelectedContent?.Content as IEditor;
            if (selectedContent == null) return;

            var reachedEnd = selectedContent.Replace(GetFindSettingsStruct());
            if (reachedEnd)
                this.Title = "Find" + " - " + "Reached end of document"; //todo move to resource
            else if (this.Title != "Find")
                this.Title = "Find"; //todo move to resource
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.Title != "Find")
                this.Title = "Find"; //todo move to resource
        }
    }

    //todo rename it
    public struct FindStruct
    {
        public bool registr;
        public string text;
        public bool loop;
    }
}
