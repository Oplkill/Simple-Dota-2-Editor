using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using KV_reloaded;
using SimpleDota2EditorWPF.Panels;
using SimpleDota2EditorWPF.Properties;
using SimpleDota2EditorWPF.ScriptsUtils.KV;
using WPFFolderBrowser;
using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Layout;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;

namespace SimpleDota2EditorWPF
{
    public static class DataBase
    {
        public static Settings Settings = new Settings();

        public static string AddonPath;
        public static string ProjectName = @"Simple Dota 2 Editor";

        public static KVToken Units;
        public static KVToken Heroes;
        public static KVToken Items;
        public static KVToken Abilities;

        public static bool Edited
        {
            get { return edited; }
            set
            {
                AllPanels.ObjectEditorForm.Title = ProjectName + ((value) ? Resources.ProjectEdited : "");
                edited = value;
            }
        }

        private static bool edited = false;
        private static bool inited = false;

        public static WPFFolderBrowser.WPFFolderBrowserDialog OpenFolderDialog;

        public static void InitProgramm()
        {
            if (inited) return;

            inited = true;

            BasicCompletionKV.Init();

            OpenFolderDialog = new WPFFolderBrowserDialog();
            OpenFolderDialog.InitialDirectory = Settings.DotaPath + Settings.AddonsPath;

            Settings.LoadSettings();
            DataBase.CreateMainPage();
            TurnOffOnProjectEditElements(false);

            if (Environment.GetCommandLineArgs().Length > 1)
            {
                LoadAddon(Environment.GetCommandLineArgs()[1]);
            }
            else
            {
#if DEBUG
                LoadAddon(
                    "C:\\Program Files (x86)\\Steam\\steamapps\\common\\dota 2 beta\\game\\dota_addons\\rpchacled\\");
#endif
            }
        }

        public static void LoadAddon(string path)
        {
            if (!IsDotaProjectFolder(path))
            {
                MessageBox.Show("Didnt finded Addoninfo.txt in " + path, "Error load", MessageBoxButton.OK);
                return;
            }

            CloseAddon();
            AllPanels.StartPage.Close();
            AddonPath = path;

            string text = "";

            try
            {
                text = AddonPath + Settings.NpcPath + Settings.UnitsPath;
                if (!File.Exists(text))
                    CreateKVFile(text, "DOTAUnits");
                Units = TokenAnalizer.AnaliseText(File.ReadAllText(text)).FirstOrDefault();
                ((ObjectsViewPanel)AllPanels.UnitsView.Content).LoadMe(Units);

                text = AddonPath + Settings.NpcPath + Settings.HeroesPath;
                if (!File.Exists(text))
                    CreateKVFile(text, "DOTAHeroes");
                Heroes = TokenAnalizer.AnaliseText(File.ReadAllText(text)).FirstOrDefault();
                ((ObjectsViewPanel)AllPanels.HeroesView.Content).LoadMe(Heroes);

                text = AddonPath + Settings.NpcPath + Settings.ItemsPath;
                if (!File.Exists(text))
                    CreateKVFile(text, "DOTAAbilities");
                Items = TokenAnalizer.AnaliseText(File.ReadAllText(text)).FirstOrDefault();
                ((ObjectsViewPanel)AllPanels.ItemsView.Content).LoadMe(Items);

                text = AddonPath + Settings.NpcPath + Settings.AbilitiesPath;
                if (!File.Exists(text))
                    CreateKVFile(text, "DOTAAbilities");
                Abilities = TokenAnalizer.AnaliseText(File.ReadAllText(text)).FirstOrDefault();
                ((ObjectsViewPanel)AllPanels.AbilityView.Content).LoadMe(Abilities);

                TurnOffOnProjectEditElements(true);
            }
            catch (Exception e)
            {
                if (e is ErrorParser)
                    MessageBox.Show(
                        "Finded error in file" + " \"" + text + "\"\n" +
                        "Line " + ((ErrorParser) e).Line + "\n" +
                        "Error text - " + ((ErrorParser) e).KvError.ToStringLang(),
                        "Error in openning project"); // todo Move to resource
                else
                    MessageBox.Show("Unregistered error!\n" + 
                        e.Message);
                CloseAddon(false);
            }
            

            string projectName = path.Substring(0, path.Length - 1);
            projectName = projectName.Substring(projectName.LastIndexOf("\\", StringComparison.Ordinal) + 1);
            AllPanels.ObjectEditorForm.Title = ProjectName = projectName;

            LoadStuffSettingsKv();
        }

        private static void LoadStuffSettingsKv()
        {
            try
            {
                if (!File.Exists(AddonPath + Settings.ProjectStuffSettings))
                    return;
                var stuffKv =
                    TokenAnalizer.AnaliseText(File.ReadAllText(AddonPath + Settings.ProjectStuffSettings)).FirstOrDefault();
                if (stuffKv == null)
                    return;

                LoadLastOpenedObjects(stuffKv.GetChild("Last_Opened_Objects"));
                LoadLastSelectedObjectView(stuffKv.GetChild("Last_focused_object_view"));
            }
            catch (Exception e)
            {
                return;
            }
        }

        private static void LoadLastSelectedObjectView(KVToken lastOpenedKv)
        {
            if (lastOpenedKv == null)
                return;

            try
            {
                var panelType = (ObjectsViewPanel.ObjectTypePanel)(int.Parse(lastOpenedKv.Value));

                switch (panelType)
                {
                    case ObjectsViewPanel.ObjectTypePanel.Units:
                        AllPanels.UnitsView.IsSelected = true;
                    break;

                    case ObjectsViewPanel.ObjectTypePanel.Heroes:
                        AllPanels.HeroesView.IsSelected = true;
                    break;

                    case ObjectsViewPanel.ObjectTypePanel.Abilities:
                        AllPanels.AbilityView.IsSelected = true;
                    break;

                    case ObjectsViewPanel.ObjectTypePanel.Items:
                        AllPanels.ItemsView.IsSelected = true;
                    break;
                }
            }
            catch (Exception e)
            {
                return;
            }
        }

        private static void LoadLastOpenedObjects(KVToken lastOpenedKv)
        {
            if (lastOpenedKv == null)
                return;

            if (!Settings.LoadSaveOpenedObjects)
                return;

            try
            {
                foreach (var kv in lastOpenedKv.Children)
                {
                    if (kv.Type != KVTokenType.KVblock)
                        continue;

                    string name = kv.Key;
                    var type = (ObjectsViewPanel.ObjectTypePanel)(int.Parse(kv.GetChild("ObjectType").Value));
                    KVToken objectsKv;
                    LayoutAnchorable objectView;

                    switch (type)
                    {
                        case ObjectsViewPanel.ObjectTypePanel.Abilities:
                            objectsKv = Abilities;
                            objectView = AllPanels.AbilityView;
                        break;

                        case ObjectsViewPanel.ObjectTypePanel.Heroes:
                            objectsKv = Heroes;
                            objectView = AllPanels.HeroesView;
                            break;

                        case ObjectsViewPanel.ObjectTypePanel.Units:
                            objectsKv = Units;
                            objectView = AllPanels.UnitsView;
                            break;

                        case ObjectsViewPanel.ObjectTypePanel.Items:
                            objectsKv = Items;
                            objectView = AllPanels.ItemsView;
                        break;

                        default:
                            continue;
                    }

                    if (objectsKv.GetChild(name) == null)
                        continue;

                    ((ObjectsViewPanel)(objectView.Content)).LoadObject(name);
                }
            }
            catch (Exception e)
            {
                return;
            }
        }

        /// <summary>
        /// Allow/Not elements for editing project, like create folder, save...
        /// </summary>
        private static void TurnOffOnProjectEditElements(bool turnOn)
        {
            ((ObjectsViewPanel)AllPanels.AbilityView.Content).GridMenu.IsEnabled = turnOn;
            ((ObjectsViewPanel)AllPanels.HeroesView.Content).GridMenu.IsEnabled = turnOn;
            ((ObjectsViewPanel)AllPanels.ItemsView.Content).GridMenu.IsEnabled = turnOn;
            ((ObjectsViewPanel)AllPanels.UnitsView.Content).GridMenu.IsEnabled = turnOn;
            ((MenuItem)((MenuItem)AllPanels.ObjectEditorForm.Menu.Items[0]).Items[1]).IsEnabled = turnOn;
            ((MenuItem)((MenuItem)AllPanels.ObjectEditorForm.Menu.Items[0]).Items[2]).IsEnabled = turnOn;
            AllPanels.ObjectEditorForm.ButtonSaveMenu.IsEnabled = turnOn;
        }

        private static void CreateKVFile(string pathName, string mainToken)
        {
            string text = "\"" + mainToken + "\"\n{\n\n}\n";

            var file = new StreamWriter(pathName);
            file.WriteLine(text);
            file.Close();
        }

        public static bool IsDotaProjectFolder(string folder)
        {
            return File.Exists(folder + "\\addoninfo.txt");
        }

        public static bool CloseAddon(bool saveChanges = true)
        {
            if (Edited && saveChanges)
            {
                var dialog = MessageBox.Show(Resources.NotSavedDialogText, Resources.NotSavedCapture, MessageBoxButton.YesNoCancel);
                switch (dialog)
                {
                    case MessageBoxResult.Yes:
                        if (!SaveAddon())
                        {
                            var dialog2 = MessageBox.Show(Properties.Resources.TextContainErrors, Properties.Resources.TextContainErrorsCapture, MessageBoxButton.YesNo);
                            switch (dialog2)
                            {
                                case MessageBoxResult.Yes:
                                    break;

                                case MessageBoxResult.No:
                                    return false;
                                    break;
                            }
                        }
                        break;

                    case MessageBoxResult.Cancel:
                        return false;
                        break;

                    case MessageBoxResult.No:
                        break;
                }
            }
            if (saveChanges)
                SaveSomeProjectStuff();

            AllPanels.LayoutDocumentPane.Children.Clear();
            CreateMainPage();

            ((ObjectsViewPanel) AllPanels.AbilityView.Content).CloseMe();
            ((ObjectsViewPanel)AllPanels.HeroesView.Content).CloseMe();
            ((ObjectsViewPanel)AllPanels.UnitsView.Content).CloseMe();
            ((ObjectsViewPanel)AllPanels.ItemsView.Content).CloseMe();

            Units = null;
            Heroes = null;
            Items = null;
            Abilities = null;

            AddonPath = "";

            Edited = false;
            AllPanels.ObjectEditorForm.Title = @"Simple Dota 2 Editor";
            TurnOffOnProjectEditElements(false);

            return true;
        }

        public static void CreateMainPage()
        {
            StartPagePanel startPagePanel = new StartPagePanel();
            AllPanels.StartPage = new LayoutDocument();
            AllPanels.StartPage.Content = startPagePanel;
            AllPanels.StartPage.Title = "Start page";
            AllPanels.LayoutDocumentPane.Children.Add(AllPanels.StartPage);
        }

        public static bool SaveAddon()
        {
            var panels = AllPanels.LayoutDocumentPane.Children.Where(doc => doc.Content is IEditor);
            foreach (var doc in panels)
            {
                var error = ((IEditor) doc.Content).SaveChanges();
                if (error != null)
                {
                    //todo добавить вывод ошибки
                    return  false;
                }
            }

            if (Units != null)
                saveFile(AddonPath + Settings.NpcPath + Settings.UnitsPath, Units.ToString());
            if (Heroes != null)
                saveFile(AddonPath + Settings.NpcPath + Settings.HeroesPath, Heroes.ToString());
            if (Abilities != null)
                saveFile(AddonPath + Settings.NpcPath + Settings.AbilitiesPath, Abilities.ToString());
            if (Items != null)
                saveFile(AddonPath + Settings.NpcPath + Settings.ItemsPath, Items.ToString());

            Edited = false;
            return true;
        }

        /// <summary>
        /// Saving: Opened objects, Places windows, Focused windows
        /// Saves in project folder
        /// </summary>
        private static void SaveSomeProjectStuff()
        {
            var stuffKv = new KVToken("Project_stuff_settings");

            //Last opened objects
            var openObjectsKv = new KVToken("Last_Opened_Objects");

            var panels = AllPanels.LayoutDocumentPane.Children.Where(doc => doc.Content is IEditor);
            foreach (var doc in panels)
            {
                var kv = new KVToken(((IEditor)doc.Content).PanelName);
                kv.Children.Add(new KVToken("ObjectType", ((int)((IEditor)doc.Content).ObjectType).ToString()));
                openObjectsKv.Children.Add(kv);
            }

            stuffKv.Children.Add(openObjectsKv);

            //--------------------------

            //Last focused object view item
            ObjectsViewPanel.ObjectTypePanel openedViewPanel = ObjectsViewPanel.ObjectTypePanel.Units;
            if (AllPanels.UnitsView.IsSelected)
                openedViewPanel = ObjectsViewPanel.ObjectTypePanel.Units;
            else if (AllPanels.AbilityView.IsSelected)
                openedViewPanel = ObjectsViewPanel.ObjectTypePanel.Abilities;
            else if (AllPanels.HeroesView.IsSelected)
                openedViewPanel = ObjectsViewPanel.ObjectTypePanel.Heroes;
            else if (AllPanels.ItemsView.IsSelected)
                openedViewPanel = ObjectsViewPanel.ObjectTypePanel.Items;

            var focusedObjectViewKv = new KVToken("Last_focused_object_view", ((int)openedViewPanel).ToString());

            stuffKv.Children.Add(focusedObjectViewKv);
            //-------------------------------------

            stuffKv.ForceSetStandartStyle();
            saveFile(AddonPath + Settings.ProjectStuffSettings, stuffKv.ToString());
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
        public static ObjectEditorMainWindow ObjectEditorForm;
        public static SettingsWindow settingForm = new SettingsWindow();

        public static DockingManager PrimaryDocking { get; set; }
        public static LayoutDocumentPane LayoutDocumentPane;
        public static LayoutAnchorablePane LayoutAnchorablePane;

        public static LayoutDocument StartPage;
        public static LayoutAnchorable AbilityView;
        public static LayoutAnchorable UnitsView;
        public static LayoutAnchorable HeroesView;
        public static LayoutAnchorable ItemsView;

        public static LayoutContent FindAnyEditorPanel(string name, ObjectsViewPanel.ObjectTypePanel objectsTypeTag)
        {
            var panels = LayoutDocumentPane.Children.Where(doc => doc.Content is IEditor);

            foreach (var doc in panels)
            {
                if (((IEditor)doc.Content).PanelName == name &&
                    ((IEditor) doc.Content).ObjectType == objectsTypeTag)
                    return doc;
            }

            return null;
        }

        public static LayoutContent[] GetAllEditorPanels()
        {
            return LayoutDocumentPane.Children.Where(doc => doc.Content is IEditor).ToArray();
        }
    }
}