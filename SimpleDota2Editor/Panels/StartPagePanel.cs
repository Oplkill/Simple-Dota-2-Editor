using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SimpleDota2Editor.Properties;
using WeifenLuo.WinFormsUI.Docking;

namespace SimpleDota2Editor.Panels
{
    public partial class StartPagePanel : DockContent
    {
        private List<string> projectsInDotaFolder; 

        public StartPagePanel()
        {
            projectsInDotaFolder = new List<string>();

            InitializeComponent();
            this.Text = Resources.StartPage;
        }

        private void StartPagePanel_Load(object sender, EventArgs e)
        {
            LoadProjectsInDotaFolder();
        }

        private void linkLabelLoadAddon_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            folderBrowserDialog1.SelectedPath = DataBase.Settings.DotaPath + DataBase.Settings.AddonsPath;
            var res = folderBrowserDialog1.ShowDialog();
            if (res != DialogResult.OK)
                return;

            DataBase.LoadAddon(folderBrowserDialog1.SelectedPath + "\\");
        }

        private void LoadProjectsInDotaFolder()
        {
            try
            {
                System.IO.DirectoryInfo info = new System.IO.DirectoryInfo(DataBase.Settings.DotaPath + DataBase.Settings.AddonsPath);
                System.IO.DirectoryInfo[] dirs = info.GetDirectories();

                projectsInDotaFolder.Clear();
                listViewProjectsInFolder.Items.Clear();

                foreach (var dir in dirs)
                {
                    if (DataBase.IsDotaProjectFolder(dir.FullName))
                    {
                        projectsInDotaFolder.Add(dir.FullName);
                        listViewProjectsInFolder.Items.Add(dir.Name);
                    }
                }
            }
            catch (Exception)
            {
                
                return;
            }
        }

        
    }
}
