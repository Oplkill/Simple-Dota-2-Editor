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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimpleDota2EditorWPF.Panels
{
    /// <summary>
    /// Логика взаимодействия для StartPagePanel.xaml
    /// </summary>
    public partial class StartPagePanel : UserControl
    {
        private List<string> projectsInDotaFolder;

        public StartPagePanel()
        {
            projectsInDotaFolder = new List<string>();

            InitializeComponent();
            LoadProjectsInDotaFolder();
        }

        private void ButtonLoadAddon_clicked(object sender, EventArgs e)
        {
            AllPanels.ObjectEditorForm.MenuItem_Open_Click(sender, null);
        }

        private void LoadProjectsInDotaFolder()
        {
            try
            {
                System.IO.DirectoryInfo info = new System.IO.DirectoryInfo(DataBase.Settings.DotaPath + DataBase.Settings.AddonsPath);
                System.IO.DirectoryInfo[] dirs = info.GetDirectories();

                projectsInDotaFolder.Clear();
                ListBoxProjectsInDota.Items.Clear();

                foreach (var dir in dirs)
                {
                    if (DataBase.IsDotaProjectFolder(dir.FullName))
                    {
                        projectsInDotaFolder.Add(dir.FullName);
                        ListBoxProjectsInDota.Items.Add(dir.Name);
                    }
                }
            }
            catch (Exception)
            {

                return;
            }
        }

        private void ListBoxProjectsInDota_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBoxProjectsInDota.SelectedItems.Count == 0) return;

            DataBase.LoadAddon(projectsInDotaFolder[ListBoxProjectsInDota.Items.IndexOf(ListBoxProjectsInDota.SelectedItems[0])] + "\\");
        }
    }
}
