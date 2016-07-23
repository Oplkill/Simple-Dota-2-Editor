using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using KV_reloaded;
using SimpleDota2Editor.Panels;
using SimpleDota2Editor.Properties;
using WeifenLuo.WinFormsUI.Docking;

namespace SimpleDota2Editor
{
    public static class DataBase
    {
        public static Settings Settings = new Settings();

        public static string AddonPath;

        public static KVToken Units;
        public static KVToken Heroes;
        public static KVToken Items;
        public static KVToken Abilities;
        public static KVToken AbilitiesOverrite;

        public static bool Edited = false;

        public static void LoadAddon(string path)
        {
            if (!IsDotaProjectFolder(path))
            {
                MessageBox.Show(Resources.ErrorLoadAddonNoFindedAddoninfoTxt, Resources.InvalidFolder, MessageBoxButtons.OK);
                return;
            }

            CloseAddon();
            AllPanels.StartPage.Close();
            AddonPath = path;

            string text;

            text = AddonPath + Settings.NpcPath + Settings.UnitsPath;
            if (!File.Exists(text))
                CreateKVFile(path, "DOTAUnits");
            Units = TokenAnalizer.AnaliseText(File.ReadAllText(text)).FirstOrDefault();
            AllPanels.UnitsView.LoadMe(Units);

            text = AddonPath + Settings.NpcPath + Settings.HeroesPath;
            if (!File.Exists(text))
                CreateKVFile(path, "DOTAHeroes");
            Heroes = TokenAnalizer.AnaliseText(File.ReadAllText(text)).FirstOrDefault();
            AllPanels.HeroesView.LoadMe(Heroes);

            text = AddonPath + Settings.NpcPath + Settings.ItemsPath;
            if (!File.Exists(text))
                CreateKVFile(path, "DOTAAbilities");
            Items = TokenAnalizer.AnaliseText(File.ReadAllText(text)).FirstOrDefault();
            AllPanels.ItemsView.LoadMe(Items);

            text = AddonPath + Settings.NpcPath + Settings.AbilitiesPath;
            if (!File.Exists(text))
                CreateKVFile(path, "DOTAAbilities");
            Abilities = TokenAnalizer.AnaliseText(File.ReadAllText(text)).FirstOrDefault();
            AllPanels.AbilityView.LoadMe(Abilities);

            text = AddonPath + Settings.NpcPath + Settings.AbilitiesOverridePath;
            if (!File.Exists(text))
                CreateKVFile(path, "DOTAAbilities");
            AbilitiesOverrite = TokenAnalizer.AnaliseText(File.ReadAllText(text)).FirstOrDefault();
            AllPanels.AbilityOverrideView.LoadMe(AbilitiesOverrite);

            string projectName = path.Substring(0, path.Length - 1);
            projectName = projectName.Substring(projectName.LastIndexOf("\\", StringComparison.Ordinal) + 1);
            AllPanels.Form1.Text = projectName;
        }

        private static void CreateKVFile(string pathName, string mainToken)
        {
            string text = "\""+mainToken+"\"\n{\n\n}\n";

            var file = new StreamWriter(pathName);
            file.WriteLine(text);
            file.Close();
        }

        public static bool IsDotaProjectFolder(string folder)
        {
            return File.Exists(folder + "\\addoninfo.txt");
        }

        private static void SaveMainPanelsDocking()
        {
            KVToken token = new KVToken
            {
                Type = KVTokenType.KVblock,
                Key = "PanelSettings",
                Children = new List<KVToken>()
            };

            token.Children.Add(new KVToken() { Type = KVTokenType.KVsimple, Key = "HeroesPanel",    Value = ((int)AllPanels.DockHeroesView).ToString() });
            token.Children.Add(new KVToken() { Type = KVTokenType.KVsimple, Key = "UnitsPanel",     Value = ((int)AllPanels.DockUnitsView).ToString() });
            token.Children.Add(new KVToken() { Type = KVTokenType.KVsimple, Key = "ItemsPanel",     Value = ((int)AllPanels.DockItemsView).ToString() });
            token.Children.Add(new KVToken() { Type = KVTokenType.KVsimple, Key = "AbilityPanel",   Value = ((int)AllPanels.DockAbilityView).ToString() });
            token.Children.Add(new KVToken() { Type = KVTokenType.KVsimple, Key = "AbilityOverPanel", Value = ((int)AllPanels.DockAbilityOverrideView).ToString() });
            token.ForceSetStandartStyle();

            var file = new StreamWriter("PanelSettings.kv");
            file.WriteLine(token.ToString());
            file.Close();
        }

        public static bool CloseAddon()
        {
            SaveMainPanelsDocking();

            if (Edited)
            {
                var dialog = MessageBox.Show(Resources.NotSavedDialogText, Resources.NotSavedCapture, MessageBoxButtons.YesNoCancel);
                switch (dialog)
                {
                    case DialogResult.Yes:
                        SaveAddon();
                        break;

                    case DialogResult.Cancel:
                        return false;
                        break;

                    case DialogResult.No:
                        break;
                }
            }

            var panels = AllPanels.PrimaryDocking.Documents.ToArray();
            foreach (var doc in panels)
            {
                doc.DockHandler.Close();
            }
            CreateMainPage();

            AllPanels.AbilityOverrideView.CloseMe();
            AllPanels.AbilityView.CloseMe();
            AllPanels.HeroesView.CloseMe();
            AllPanels.ItemsView.CloseMe();
            AllPanels.UnitsView.CloseMe();

            Units = null;
            Heroes = null;
            Items = null;
            Abilities = null;
            AbilitiesOverrite = null;

            AddonPath = "";

            Edited = false;
            AllPanels.Form1.Text = @"Simple Dota 2 Editor";

            return true;
        }

        public static void CreateMainPage()
        {
            AllPanels.StartPage = new StartPagePanel();
            AllPanels.StartPage.Show(AllPanels.PrimaryDocking, AllPanels.DockStartPage);
        }

        public static void SaveAddon()
        {
            var panels = AllPanels.PrimaryDocking.Documents.ToArray();
            foreach (var doc in panels.Where(doc => doc.DockHandler.Form is TextEditorPanel))
            {
                ((TextEditorPanel)doc.DockHandler.Form).SaveChanges();
            }

            if(Units != null)
                saveFile(AddonPath + Settings.NpcPath + Settings.UnitsPath, Units.ToString());
            if (Heroes != null)
                saveFile(AddonPath + Settings.NpcPath + Settings.HeroesPath, Heroes.ToString());
            if (Abilities != null)
                saveFile(AddonPath + Settings.NpcPath + Settings.AbilitiesPath, Abilities.ToString());
            if (Items != null)
                saveFile(AddonPath + Settings.NpcPath + Settings.ItemsPath, Items.ToString());
            if (AbilitiesOverrite != null)
                saveFile(AddonPath + Settings.NpcPath + Settings.AbilitiesOverridePath, AbilitiesOverrite.ToString());

            Edited = false;
        }

        private static void saveFile(string path, string text)
        {
            if (text.Contains(Settings.HeadLinkText))
            {
                if (!Settings.WriteHeadLinkOnSave)
                {
                    text = text.Replace(Settings.HeadLinkText, "");
                }
            }
            else if (Settings.WriteHeadLinkOnSave)
            {
                text = Settings.HeadLinkText + text;
            }

            StreamWriter file = new StreamWriter(path, false);
            file.WriteLine(text);
            file.Close();
        }
    }

    public class AllPanels
    {
        public static Form1 Form1;

        public static DockPanel PrimaryDocking { get; set; }

        public static DockContent LastActiveDocumentEditor;

        public static StartPagePanel StartPage;
        public static ObjectsViewPanel AbilityView;
        public static ObjectsViewPanel AbilityOverrideView;
        public static ObjectsViewPanel UnitsView;
        public static ObjectsViewPanel HeroesView;
        public static ObjectsViewPanel ItemsView;

        public static DockState DockStartPage = DockState.Document;
        public static DockState DockAbilityView = DockState.DockLeft;
        public static DockState DockAbilityOverrideView = DockState.DockLeft;
        public static DockState DockUnitsView = DockState.DockLeft;
        public static DockState DockHeroesView = DockState.DockLeft;
        public static DockState DockItemsView = DockState.DockLeft;

        public static TextEditorPanel FindEditorPanel(string name, ObjectsViewPanel.ObjectTypePanel objectsTypeTag)
        {
            var panels = PrimaryDocking.Contents.Where(doc => doc.DockHandler.Form is TextEditorPanel);

            foreach (var doc in panels)
            {
                if (((TextEditorPanel)doc.DockHandler.Form).PanelName == name)
                    if ((ObjectsViewPanel.ObjectTypePanel)doc.DockHandler.Form.Tag == objectsTypeTag)
                        return (TextEditorPanel)doc.DockHandler.Form;
            }

            return null;
        }

        public static GuiEditorPanel FindGuiPanel(string name, ObjectsViewPanel.ObjectTypePanel objectsTypeTag)
        {
            var panels = PrimaryDocking.Contents.Where(doc => doc.DockHandler.Form is GuiEditorPanel);

            foreach (var doc in panels)
            {
                if (((GuiEditorPanel)doc.DockHandler.Form).PanelName == name)
                    if ((ObjectsViewPanel.ObjectTypePanel)doc.DockHandler.Form.Tag == objectsTypeTag)
                        return (GuiEditorPanel)doc.DockHandler.Form;
            }

            return null;
        }

        public static DockContent FindAnyEditorPanel(string name, ObjectsViewPanel.ObjectTypePanel objectsTypeTag)
        {
            var panels = PrimaryDocking.Contents.Where(doc => 
                    doc.DockHandler.Form is TextEditorPanel
                    || doc.DockHandler.Form is GuiEditorPanel);

            foreach (var doc in panels)
            {
                if ((doc.DockHandler.Form as TextEditorPanel)?.PanelName == name
                    || (doc.DockHandler.Form as GuiEditorPanel)?.PanelName == name)
                    if ((ObjectsViewPanel.ObjectTypePanel)doc.DockHandler.Form.Tag == objectsTypeTag)
                        return (DockContent)doc.DockHandler.Form;
            }

            return null;
        }
    }
}