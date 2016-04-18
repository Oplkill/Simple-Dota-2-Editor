using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using SimpleDota2Editor.Panels;
using SimpleDota2Editor.Properties;
using TempLoaderKVfiles;
using WeifenLuo.WinFormsUI.Docking;

namespace SimpleDota2Editor
{
    public static class DataBase
    {
        public static Settings Settings = new Settings();

        public static string AddonPath;

        public static FileKV Units;
        public static FileKV Heroes;
        public static FileKV Items;
        public static FileKV Abilities;
        public static FileKV AbilitiesOverrite;

        public static bool Edited = false;

        public static void LoadAddon(string path)
        {
            if (!File.Exists(path + "addoninfo.txt"))
            {
                MessageBox.Show(Resources.ErrorLoadAddonNoFindedAddoninfoTxt, Resources.InvalidFolder, MessageBoxButtons.OK);
                return;
            }

            CloseAddon();
            AllPanels.StartPage.Close();
            AddonPath = path;

            string text;

            text = AddonPath + Settings.NpcPath + Settings.UnitsPath;
            if (File.Exists(text))
            {
                Units = TempLoaderKV.LoadFile(File.ReadAllText(text));
                AllPanels.UnitsView.LoadMe(Units);
            }

            text = AddonPath + Settings.NpcPath + Settings.HeroesPath;
            if (File.Exists(text))
            {
                Heroes = TempLoaderKV.LoadFile(File.ReadAllText(text));
                AllPanels.HeroesView.LoadMe(Heroes);
            }

            text = AddonPath + Settings.NpcPath + Settings.ItemsPath;
            if (File.Exists(text))
            {
                Items = TempLoaderKV.LoadFile(File.ReadAllText(text));
                AllPanels.ItemsView.LoadMe(Items);
            }

            text = AddonPath + Settings.NpcPath + Settings.AbilitiesPath;
            if (File.Exists(text))
            {
                Abilities = TempLoaderKV.LoadFile(File.ReadAllText(text));
                AllPanels.AbilityView.LoadMe(Abilities);
            }

            text = AddonPath + Settings.NpcPath + Settings.AbilitiesOverridePath;
            if (File.Exists(text))
            {
                AbilitiesOverrite = TempLoaderKV.LoadFile(File.ReadAllText(text));
                AllPanels.AbilityOverrideView.LoadMe(AbilitiesOverrite);
            }
        }

        public static bool CloseAddon()
        {
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
            AllPanels.StartPage = new StartPagePanel();
            AllPanels.StartPage.Show(AllPanels.PrimaryDocking, DockState.Document);

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

            return true;
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
            if (Settings.WriteHeadLinkOnSave)
            {
                text = Settings.HeadLinkText + text;
            }

            StreamWriter vvod = new StreamWriter(path, false);
            vvod.WriteLine(text);
            vvod.Close();
        }
    }

    public class AllPanels
    {
        public static Form1 Form1;

        public static DockPanel PrimaryDocking;

        public static StartPagePanel StartPage;
        public static ObjectsViewPanel AbilityView;
        public static ObjectsViewPanel AbilityOverrideView;
        public static ObjectsViewPanel UnitsView;
        public static ObjectsViewPanel HeroesView;
        public static ObjectsViewPanel ItemsView;

        public static TextEditorPanel FindPanel(string name)
        {
            var textPanels = AllPanels.PrimaryDocking.Documents.ToArray();
            foreach (var doc in textPanels.Where(doc => doc.DockHandler.Form is TextEditorPanel))
            {
                if (((TextEditorPanel) doc.DockHandler.Form).PanelName == name)
                    return (TextEditorPanel) doc.DockHandler.Form;
            }

            return null;
        }
    }
}