using System;
using System.IO;
using System.Windows.Forms;
using KV_reloaded;
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

            ShowEditorMenu(EditorType.None);


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

            DataBase.LoadAddon(folderBrowserDialog1.SelectedPath + "\\");
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

        private void sendBugOrToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Settings.GithubIssuesLink);
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }

        #endregion

        #region EditorSubMenu

        private void toolStripButtonEditorUndo_Click(object sender, EventArgs e)
        {
            (AllPanels.LastActiveDocumentEditor?.DockHandler.Form as TextEditorPanel)?.ButtonUndo_Click();
        }

        private void toolStripButtonEditorRedo_Click(object sender, EventArgs e)
        {
            (AllPanels.LastActiveDocumentEditor?.DockHandler.Form as TextEditorPanel)?.ButtonRedo_Click();
        }

        private void toolStripButtonCommentIt_Click(object sender, EventArgs e)
        {
            (AllPanels.LastActiveDocumentEditor?.DockHandler.Form as TextEditorPanel)?.ButtonCommentIt_Click();
        }

        private void toolStripButtonUnCommentIt_Click(object sender, EventArgs e)
        {
            (AllPanels.LastActiveDocumentEditor?.DockHandler.Form as TextEditorPanel)?.ButtonUnCommentIt_Click();
        }

        private void toolStripButtonToGuiEditor_Click(object sender, EventArgs e)
        {
            if (!(AllPanels.LastActiveDocumentEditor?.DockHandler.Form is TextEditorPanel))
                return;

            var canClose = (AllPanels.LastActiveDocumentEditor?.DockHandler.Form as TextEditorPanel).CloseMe();

            if (canClose)
            {
                var objRef = ((TextEditorPanel)AllPanels.LastActiveDocumentEditor?.DockHandler.Form).ObjectRef;
                var tag = AllPanels.LastActiveDocumentEditor?.DockHandler.Form.Tag;
                AllPanels.LastActiveDocumentEditor?.DockHandler.Form.Close();
                ShowEditorMenu(EditorType.Gui);

                var guiPanel = new GuiEditorPanel();
                guiPanel.PanelName = objRef.Key;
                guiPanel.ObjectRef = objRef;
                guiPanel.Tag = tag;
                guiPanel.InitGuiAndLoad();
                guiPanel.Show(AllPanels.PrimaryDocking, DockState.Document);
            }
        }


        #endregion

        public enum EditorType
        {
            Text, Gui, None,
        }

        public EditorType MenuShowed { get; private set; }

        public void ShowEditorMenu(EditorType type)
        {
            if (type == MenuShowed) return;

            MenuShowed = type;
            if (type == EditorType.Text)
            {
                ShowCloseMenu(toolStripGui, false);
                ShowCloseMenu(toolStripEditor, true);
            }
            else if (type == EditorType.Gui)
            {
                ShowCloseMenu(toolStripEditor, false);
                ShowCloseMenu(toolStripGui, true);
            }
            else if (type == EditorType.None)
            {
                ShowCloseMenu(toolStripEditor, false);
                ShowCloseMenu(toolStripGui, false);
            }
        }

        private void ShowCloseMenu(ToolStrip menu, bool show)
        {
            if (show)
            {
                menu.Enabled = true;
                menu.Show();
            }
            else
            {
                menu.Enabled = false;
                menu.Hide();
            }
        }

        #region GuiSubMenu


        private void toolStripButtonGuiUndo_Click(object sender, EventArgs e)
        {
            (AllPanels.LastActiveDocumentEditor?.DockHandler.Form as GuiEditorPanel)?.UndoButton_Click();
        }

        private void toolStripButtonGuiRedo_Click(object sender, EventArgs e)
        {
            (AllPanels.LastActiveDocumentEditor?.DockHandler.Form as GuiEditorPanel)?.RedoButton_Click();
        }

        private void toolStripButtonGuiCreateKVItem_Click(object sender, EventArgs e)
        {
            (AllPanels.LastActiveDocumentEditor?.DockHandler.Form as GuiEditorPanel)?.CreateKVButton_Click();
        }

        private void toolStripButtonGuiCreateKVBlockItem_Click(object sender, EventArgs e)
        {
            (AllPanels.LastActiveDocumentEditor?.DockHandler.Form as GuiEditorPanel)?.CreateKVBlockButton_Click();
        }

        private void toolStripButtonGuiMoveDown_Click(object sender, EventArgs e)
        {
            (AllPanels.LastActiveDocumentEditor?.DockHandler.Form as GuiEditorPanel)?.MoveDownButton_Click();
        }

        private void toolStripButtonGuiMoveUp_Click(object sender, EventArgs e)
        {
            (AllPanels.LastActiveDocumentEditor?.DockHandler.Form as GuiEditorPanel)?.MoveUpButton_Click();
        }

        private void toolStripButtonGuiDelete_Click(object sender, EventArgs e)
        {
            (AllPanels.LastActiveDocumentEditor?.DockHandler.Form as GuiEditorPanel)?.DeleteButton_Click();
        }

        private void toolStripButtonToTextEditor_Click(object sender, EventArgs e)
        {
            if (!(AllPanels.LastActiveDocumentEditor?.DockHandler.Form is GuiEditorPanel))
                return;

            var objRef = ((GuiEditorPanel)AllPanels.LastActiveDocumentEditor?.DockHandler.Form).ObjectRef;
            var tag = AllPanels.LastActiveDocumentEditor?.DockHandler.Form.Tag;
            var objectType = (AllPanels.LastActiveDocumentEditor?.DockHandler.Form as GuiEditorPanel).ObjectType;

            AllPanels.LastActiveDocumentEditor?.DockHandler.Form.Close();
            ShowEditorMenu(EditorType.Text);

            var textPanel = new TextEditorPanel();
            textPanel.PanelName = objRef.Key;
            textPanel.ObjectRef = objRef;
            textPanel.Tag = tag;
            textPanel.SetText(objRef.ChilderToString());
            textPanel.Show(AllPanels.PrimaryDocking, DockState.Document);
            textPanel.ObjectType = objectType;
        }



        #endregion

        
    }
}
