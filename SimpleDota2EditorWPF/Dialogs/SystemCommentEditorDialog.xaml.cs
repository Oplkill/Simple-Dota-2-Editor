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
using KV_reloaded;
using Microsoft.Win32;

namespace SimpleDota2EditorWPF.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для SystemCommentEditorDialog.xaml
    /// </summary>
    public partial class SystemCommentEditorDialog : Window
    {
        public SystemCommentEditorDialog(SystemComment SystemComment)
        {
            this.SystemComment = SystemComment;

            InitializeComponent();

            foreach (var kv in SystemComment.KVList)
            {
                if (kv.Key.Contains("lua"))
                    ListBox.Items.Add(kv.Value);
            }
        }

        public SystemComment SystemComment;
        private bool Okayed;

        public static SystemComment ShowDialog(SystemComment sysComment)
        {
            var dialog = new SystemCommentEditorDialog(sysComment);
            dialog.ShowDialog();
            sysComment = dialog.SystemComment;

            return sysComment;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.DefaultExt = ".lua";
            dialog.Filter = "Lua codes (.lua)|*.lua";
            dialog.InitialDirectory = DataBase.AddonPath + DataBase.Settings.VScriptPath;
            dialog.Multiselect = true;

            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                var paths = dialog.FileNames;
                foreach (var path in paths)
                {
                    if (!path.Contains(DataBase.AddonPath + DataBase.Settings.VScriptPath)) return; //todo make messagebox
                    int len = string.Concat(DataBase.AddonPath, DataBase.Settings.VScriptPath).Length;

                    ListBox.Items.Add(path.Substring(len));
                }
            }
        }

        private void buttonRemove_Click(object sender, RoutedEventArgs e)
        {
            if (ListBox.SelectedItems == null || ListBox.SelectedItems?.Count == 0)
                return;

            ListBox.Items.Remove(ListBox.SelectedItems[0]);
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            SystemComment = null;
            this.Close();
        }

        private void buttonOk_Click(object sender, RoutedEventArgs e)
        {
            Okayed = true;
            List<KV> deleteKvTokens = new List<KV>();

            foreach (var kv in SystemComment.KVList)
            {
                if(kv.Key.Contains("lua"))
                    deleteKvTokens.Add(kv);
            }
            foreach (var kv in deleteKvTokens)
            {
                SystemComment.DeleteKV(kv.Key);
            }

            int i = 0;
            foreach (var item in ListBox.Items)
            {
                SystemComment.AddKV(string.Concat("lua", i++), (string)item);
            }

            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!Okayed)
                SystemComment = null;
        }
    }
}
