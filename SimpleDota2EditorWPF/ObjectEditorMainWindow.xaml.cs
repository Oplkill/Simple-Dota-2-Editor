using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SimpleDota2EditorWPF.Panels;
using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Layout;
using Xceed.Wpf.Toolkit;
using Xceed.Wpf.DataGrid;

namespace SimpleDota2EditorWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class ObjectEditorMainWindow : Window
    {
        

        public ObjectEditorMainWindow()
        {
            InitializeComponent();

            ToolbarKVTextEditor.Visibility = Visibility.Collapsed;
            ToolbarLuaTextEditor.Visibility = Visibility.Collapsed;

            AllPanels.PrimaryDocking = DockingManager;
            AllPanels.LayoutDocumentPane = DocumentsPane;
            AllPanels.LayoutAnchorablePane = MenuPane;

            InitTabs();

            DataBase.InitProgramm();
        }

        private void InitTabs()
        {
            AllPanels.ObjectEditorForm = this;

            ObjectsViewPanel objPanel;

            objPanel = new ObjectsViewPanel();
            objPanel.ObjectsType = ObjectsViewPanel.ObjectTypePanel.Heroes;
            AllPanels.HeroesView = new LayoutAnchorable();
            AllPanels.HeroesView.Content = objPanel;
            AllPanels.HeroesView.Title = "Heroes";
            MenuPane.Children.Add(AllPanels.HeroesView);

            objPanel = new ObjectsViewPanel();
            objPanel.ObjectsType = ObjectsViewPanel.ObjectTypePanel.Units;
            AllPanels.UnitsView = new LayoutAnchorable();
            AllPanels.UnitsView.Content = objPanel;
            AllPanels.UnitsView.Title = "Units";
            MenuPane.Children.Add(AllPanels.UnitsView);


            objPanel = new ObjectsViewPanel();
            objPanel.ObjectsType = ObjectsViewPanel.ObjectTypePanel.Items;
            AllPanels.ItemsView = new LayoutAnchorable();
            AllPanels.ItemsView.Content = objPanel;
            AllPanels.ItemsView.Title = "Items";
            MenuPane.Children.Add(AllPanels.ItemsView);


            objPanel = new ObjectsViewPanel();
            objPanel.ObjectsType = ObjectsViewPanel.ObjectTypePanel.Abilities;
            AllPanels.AbilityView = new LayoutAnchorable();
            AllPanels.AbilityView.Content = objPanel;
            AllPanels.AbilityView.Title = "Abilities";
            MenuPane.Children.Add(AllPanels.AbilityView);

            InitIcons();
        }

        private void InitIcons()
        {
            BitmapImage bmpImg = new BitmapImage();
            bmpImg.BeginInit();
            bmpImg.UriSource = new Uri("Images\\Abilities.ico", UriKind.Relative);
            bmpImg.DecodePixelWidth = 16;
            bmpImg.EndInit();
            AllPanels.AbilityView.IconSource = bmpImg;

            bmpImg = new BitmapImage();
            bmpImg.BeginInit();
            bmpImg.UriSource = new Uri("Images\\Units.ico", UriKind.Relative);
            bmpImg.DecodePixelWidth = 16;
            bmpImg.EndInit();
            AllPanels.UnitsView.IconSource = bmpImg;

            bmpImg = new BitmapImage();
            bmpImg.BeginInit();
            bmpImg.UriSource = new Uri("Images\\Heroes.ico", UriKind.Relative);
            bmpImg.DecodePixelWidth = 16;
            bmpImg.EndInit();
            AllPanels.HeroesView.IconSource = bmpImg;

            bmpImg = new BitmapImage();
            bmpImg.BeginInit();
            bmpImg.UriSource = new Uri("Images\\Items.ico", UriKind.Relative);
            bmpImg.DecodePixelWidth = 16;
            bmpImg.EndInit();
            AllPanels.ItemsView.IconSource = bmpImg;
        }

        private void RibbonWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DataBase.CloseAddon())
            {
                Settings.SaveSttings();
                App.Current.Shutdown(0);
                return;
            }

            e.Cancel = true;
        }

        #region Menu

        public void MenuItem_Open_Click(object sender, RoutedEventArgs e)
        {
            bool? res = DataBase.OpenFolderDialog.ShowDialog();
            if (res != true) return;

            DataBase.LoadAddon(DataBase.OpenFolderDialog.FileName + "\\");
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

        private void OpenLuaEditor_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_WindowObjectsView_Abilities_Click(object sender, RoutedEventArgs e)
        {
            if (((MenuItem)sender).IsChecked)
                AllPanels.AbilityView.Show();
            else
                AllPanels.AbilityView.Hide();
        }

        private void MenuItem_WindowObjectsView_Heroes_Click(object sender, RoutedEventArgs e)
        {
            if (((MenuItem)sender).IsChecked)
                AllPanels.HeroesView.Show();
            else
                AllPanels.HeroesView.Hide();
        }

        private void MenuItem_WindowObjectsView_Units_Click(object sender, RoutedEventArgs e)
        {
            if (((MenuItem)sender).IsChecked)
                AllPanels.UnitsView.Show();
            else
                AllPanels.UnitsView.Hide();
        }

        private void MenuItem_WindowObjectsView_Items_Click(object sender, RoutedEventArgs e)
        {
            if (((MenuItem)sender).IsChecked)
                AllPanels.ItemsView.Show();
            else
                AllPanels.ItemsView.Hide();
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

        public void ShowEditorsMenu(bool showKVEditor, bool showLUAEditor)
        {
            if (showKVEditor)
                ToolbarKVTextEditor.Visibility = Visibility.Visible;
            else
                ToolbarKVTextEditor.Visibility = Visibility.Collapsed;

            if (showLUAEditor)
                ToolbarLuaTextEditor.Visibility = Visibility.Visible;
            else
                ToolbarLuaTextEditor.Visibility = Visibility.Collapsed;
        }

        #region Menu - KV Text Editor

        private void TextEditorMenu_Undo_Click(object sender, RoutedEventArgs e)
        {
            var selectedContent = AllPanels.LayoutDocumentPane.SelectedContent?.Content;

            if (selectedContent is TextEditorKVPanel)
                ((TextEditorKVPanel)selectedContent).ButtonUndo_Click();
            else if (selectedContent is EditorsCollectionPanel)
                ((TextEditorKVPanel)((EditorsCollectionPanel)selectedContent).DocumentsPane.SelectedContent?.Content)?.ButtonUndo_Click();
        }

        private void TextEditorMenu_Redo_Click(object sender, RoutedEventArgs e)
        {
            var selectedContent = AllPanels.LayoutDocumentPane.SelectedContent?.Content;

            if (selectedContent is TextEditorKVPanel)
                ((TextEditorKVPanel)selectedContent).ButtonRedo_Click();
            else if (selectedContent is EditorsCollectionPanel)
                ((TextEditorKVPanel)((EditorsCollectionPanel)selectedContent).DocumentsPane.SelectedContent?.Content)?.ButtonRedo_Click();
        }

        private void TextEditorMenu_Comment_Click(object sender, RoutedEventArgs e)
        {
            var selectedContent = AllPanels.LayoutDocumentPane.SelectedContent?.Content;

            if (selectedContent is TextEditorKVPanel)
                ((TextEditorKVPanel)selectedContent).ButtonCommentIt_Click();
            else if (selectedContent is EditorsCollectionPanel)
                ((TextEditorKVPanel)((EditorsCollectionPanel)selectedContent).DocumentsPane.SelectedContent?.Content)?.ButtonCommentIt_Click();
        }

        private void TextEditorMenu_UnComment_Click(object sender, RoutedEventArgs e)
        {
            var selectedContent = AllPanels.LayoutDocumentPane.SelectedContent?.Content;

            if (selectedContent is TextEditorKVPanel)
                ((TextEditorKVPanel)selectedContent).ButtonUnCommentIt_Click();
            else if (selectedContent is EditorsCollectionPanel)
                ((TextEditorKVPanel)((EditorsCollectionPanel)selectedContent).DocumentsPane.SelectedContent?.Content)?.ButtonUnCommentIt_Click();
        }


        #endregion


        #region Menu - LUA Text Editor

        private void TextEditorLuaMenu_Undo_Click(object sender, RoutedEventArgs e)
        {
            var selectedContent = AllPanels.LayoutDocumentPane.SelectedContent?.Content;

            if (selectedContent is TextEditorLUAPanel)
                ((TextEditorLUAPanel)selectedContent).ButtonUndo_Click();
            else if (selectedContent is EditorsCollectionPanel)
                ((TextEditorLUAPanel)((EditorsCollectionPanel)selectedContent).DocumentsPane.SelectedContent?.Content)?.ButtonUndo_Click();
        }

        private void TextEditorLuaMenu_Redo_Click(object sender, RoutedEventArgs e)
        {
            var selectedContent = AllPanels.LayoutDocumentPane.SelectedContent?.Content;

            if (selectedContent is TextEditorLUAPanel)
                ((TextEditorLUAPanel)selectedContent).ButtonRedo_Click();
            else if (selectedContent is EditorsCollectionPanel)
                ((TextEditorLUAPanel)((EditorsCollectionPanel)selectedContent).DocumentsPane.SelectedContent?.Content)?.ButtonRedo_Click();
        }

        #endregion
    }
}
