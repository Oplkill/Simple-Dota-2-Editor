using System;
using System.Windows.Forms;
using SimpleDota2Editor.Panels;
using WeifenLuo.WinFormsUI.Docking;

namespace SimpleDota2Editor
{
    public partial class Form1 : Form
    {
        private SettingForm settingForm;

        public Form1()
        {
            InitializeComponent();

            

            AllPanels.PrimaryDocking = dockPanel1; //Set a static accessor to our docking panel for all default controls to go to

            InitTabs();

            settingForm = new SettingForm();
            DEBUGLOAD();
        }

        private void DEBUGLOAD()
        {
            DataBase.LoadAddon("C:\\Program Files (x86)\\Steam\\steamapps\\common\\dota 2 beta\\game\\dota_addons\\testoverthrow\\");
        }

        private void InitTabs()
        {
            AllPanels.Form1 = this;

            AllPanels.HeroesView = new ObjectsViewPanel();
            AllPanels.HeroesView.Show(AllPanels.PrimaryDocking, DockState.DockLeft);
            AllPanels.HeroesView.Text = @"Heroes";
            AllPanels.HeroesView.ObjectsType = ObjectsViewPanel.ObjectTypePanel.Heroes;
            AllPanels.HeroesView.UpdateIcon();

            AllPanels.UnitsView = new ObjectsViewPanel();
            AllPanels.UnitsView.Show(AllPanels.PrimaryDocking, DockState.DockLeft);
            AllPanels.UnitsView.Text = @"Units";
            AllPanels.UnitsView.ObjectsType = ObjectsViewPanel.ObjectTypePanel.Units;
            AllPanels.UnitsView.UpdateIcon();

            AllPanels.ItemsView = new ObjectsViewPanel();
            AllPanels.ItemsView.Show(AllPanels.PrimaryDocking, DockState.DockLeft);
            AllPanels.ItemsView.Text = @"Items";
            AllPanels.ItemsView.ObjectsType = ObjectsViewPanel.ObjectTypePanel.Items;
            AllPanels.ItemsView.UpdateIcon();

            AllPanels.AbilityView = new ObjectsViewPanel();
            AllPanels.AbilityView.Show(AllPanels.PrimaryDocking, DockState.DockLeft);
            AllPanels.AbilityView.Text = @"Abils";
            AllPanels.AbilityView.ObjectsType = ObjectsViewPanel.ObjectTypePanel.Abilities;
            AllPanels.AbilityView.UpdateIcon();

            AllPanels.AbilityOverrideView = new ObjectsViewPanel();
            AllPanels.AbilityOverrideView.Show(AllPanels.PrimaryDocking, DockState.DockLeft);
            AllPanels.AbilityOverrideView.Text = @"AbilsOver";
            AllPanels.AbilityOverrideView.ObjectsType = ObjectsViewPanel.ObjectTypePanel.AbilitiesOverride;
            AllPanels.AbilityOverrideView.UpdateIcon();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !DataBase.CloseAddon();
            if (!e.Cancel)
            {
                Settings.SaveSttings();
            }
        }

        public void SomeObjectWindowHided(ObjectsViewPanel.ObjectTypePanel objType)
        {
            switch (objType)
            {
                case ObjectsViewPanel.ObjectTypePanel.Abilities:
                    abilitiesToolStripMenuItem.Checked = false;
                break;

                case ObjectsViewPanel.ObjectTypePanel.AbilitiesOverride:
                    abilitiesOverriteToolStripMenuItem.Checked = false;
                    break;

                case ObjectsViewPanel.ObjectTypePanel.Heroes:
                    heroesToolStripMenuItem.Checked = false;
                    break;

                case ObjectsViewPanel.ObjectTypePanel.Units:
                    unitsToolStripMenuItem.Checked = false;
                    break;

                case ObjectsViewPanel.ObjectTypePanel.Items:
                    itemsToolStripMenuItem.Checked = false;
                    break;
            }
        }

        #region Menu

        private void openAddonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = DataBase.Settings.DotaPath + DataBase.Settings.AddonsPath;
            var res = folderBrowserDialog1.ShowDialog();
            if (res != DialogResult.OK)
                return;

            DataBase.LoadAddon(folderBrowserDialog1.SelectedPath);
        }

        private void closeAddonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataBase.CloseAddon();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataBase.SaveAddon();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            settingForm.ShowDialog();
        }

        private void abilitiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            abilitiesToolStripMenuItem.Checked = !abilitiesToolStripMenuItem.Checked;
            if (abilitiesToolStripMenuItem.Checked)
                AllPanels.AbilityView.Show();
            else 
                AllPanels.AbilityView.Hide();
        }

        private void abilitiesOverriteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            abilitiesOverriteToolStripMenuItem.Checked = !abilitiesOverriteToolStripMenuItem.Checked;
            if (abilitiesOverriteToolStripMenuItem.Checked)
                AllPanels.AbilityOverrideView.Show();
            else
                AllPanels.AbilityOverrideView.Hide();
        }

        private void heroesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            heroesToolStripMenuItem.Checked = !heroesToolStripMenuItem.Checked;
            if (heroesToolStripMenuItem.Checked)
                AllPanels.HeroesView.Show();
            else
                AllPanels.HeroesView.Hide();
        }

        private void unitsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            unitsToolStripMenuItem.Checked = !unitsToolStripMenuItem.Checked;
            if (unitsToolStripMenuItem.Checked)
                AllPanels.UnitsView.Show();
            else
                AllPanels.UnitsView.Hide();
        }

        private void itemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            itemsToolStripMenuItem.Checked = !itemsToolStripMenuItem.Checked;
            if (itemsToolStripMenuItem.Checked)
                AllPanels.ItemsView.Show();
            else
                AllPanels.ItemsView.Hide();
        }

        #endregion

        #region SubMenu

        private void toolStripButtonUndo_Click(object sender, EventArgs e)
        {
            (dockPanel1.ActiveDocument?.DockHandler.Form as TextEditorPanel)?.ButtonUndo_Click();
        }

        private void toolStripButtonRedo_Click(object sender, EventArgs e)
        {
            (dockPanel1.ActiveDocument?.DockHandler.Form as TextEditorPanel)?.ButtonRedo_Click();
        }

        #endregion

        private void sendBugOrToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Settings.GithubIssuesLink);
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }
    }
}
