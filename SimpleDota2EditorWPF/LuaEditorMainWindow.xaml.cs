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

namespace SimpleDota2EditorWPF
{
    /// <summary>
    /// Логика взаимодействия для LuaEditorMainWindow.xaml
    /// </summary>
    public partial class LuaEditorMainWindow : Window
    {
        public LuaEditorMainWindow()
        {
            InitializeComponent();
        }

        #region Menu

        public void MenuItem_Open_Click(object sender, RoutedEventArgs e)
        {
            //bool? res = DataBase.openDialogView.Show();
            //if (res != true) return;

            //DataBase.LoadAddon(DataBase.openDialogView.SelectedFilePath + "\\");
        }

        private void MenuItem_Save_Click(object sender, RoutedEventArgs e)
        {
            DataBase.SaveAddon();
        }

        private void MenuItem_Close_Click(object sender, RoutedEventArgs e)
        {
            DataBase.CloseAddon();
        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MenuItem_Settings_Click(object sender, RoutedEventArgs e)
        {
            AllPanels.settingForm.ShowDialog();
        }

        private void OpenObjectEditor_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_SendBug_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(Settings.GithubIssuesLink);
        }

        private void MenuItem_About_Click(object sender, RoutedEventArgs e)
        {
            AboutBoxWindow aboutBox = new AboutBoxWindow();
            aboutBox.ShowDialog();
        }

        #endregion
    }
}
