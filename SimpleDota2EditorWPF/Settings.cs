using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Serialization;

namespace SimpleDota2EditorWPF
{
    [Serializable]
    public class Settings
    {
        public string DotaPath;
        public readonly string NpcPath;
        public readonly string AddonsPath;

        public readonly string AbilitiesPath;
        public readonly string HeroesPath;
        public readonly string UnitsPath;
        public readonly string ItemsPath;
        public readonly string VScriptPath;
        public readonly string Flash3Path;

        public readonly string DotaCachePath;

        public readonly string ProjectStuffSettings;

        public readonly string[] StandartsDota2Projects = { "addon_template", "adventure_example", "hero_demo", "holdout_example", "lua_ability_example", "overthrow", "rpg_example", "tutorial_assist_game", "tutorial_basics", "ui_example" };
        public bool HideStandartDota2Projects;

        public bool LoadSaveOpenedObjects;
        public bool OpenLastOpenedProject;
        public List<string> LastOpenedProjects;
        public readonly int MaximumNumberLastOpenedProjects = 10;

        public bool ShowFullLuaFileNames;

        public EditorType EditorPriority;

        public enum EditorType
        {
            TextEditor,
            GuiEditor,
            LuaEditor,
        }

        /// <summary>
        /// Добавлять в файлы шапку, что он сохранен с помощью этого редактора
        /// </summary>
        public bool WriteHeadLinkOnSave;

        public const string HeadLinkText = "//File edited with Simple Dota 2 Editor\n//https://github.com/Oplkill/Simple-Dota-2-Editor\n\n";
        public const string GithubIssuesLink = "https://github.com/Oplkill/Simple-Dota-2-Editor/issues";

        public Language Lang;

        public enum Language
        {
            English,
            Russian,
        }

        public HighlightingSettings HighSetts;
        public HighlightingLuaSettings HighSettsLua;

        [Serializable]
        public class HighlightingSettings
        {
            public int MarginWidth;
            public string Font;
            public int FontSize;
            public bool Bold;
            public bool Italic;
            public bool Underline;

            public string DefaultWordColor;
            public string CommentColor;
            public string KeyColor;
            public string KVBlockColor;
            public string ValueNumberColor;
            public string ValueStringColor;
        }

        [Serializable]
        public class HighlightingLuaSettings
        {
            public string Font;
            public int FontSize;
            public bool Bold;
            public bool Italic;

            public string DigitsColor;
            public string BlockCommentColor;
            public string LineCommentsColor;
            public string StringsColor;
            public string UserFunctionsColor;
            public string PunctuationsColor;
            public string KeyWordsColor;
            public string TablesColor;
            public string TodoColor;
            public string HackColor;
            public string CharColor;
            public string MultilineStringsColor;
        }

        public static void SetLanguage(Language lang)
        {
            switch (lang)
            {
                case Language.Russian:
                    CultureInfo.CurrentCulture = new CultureInfo("ru-RU");
                    CultureInfo.CurrentUICulture = new CultureInfo("ru-RU");
                    break;

                case Language.English:
                default:
                    CultureInfo.CurrentCulture = new CultureInfo("en-US");
                    CultureInfo.CurrentUICulture = new CultureInfo("en-US");
                    break;
            }
        }

        public static Language GetUserLanguage()
        {
            var currCultureName = CultureInfo.CurrentCulture.Name;

            switch (currCultureName)
            {
                case "ru-RU":
                    return Language.Russian;

                case "en-US":
                default:
                    return Language.English;
            }
        }

        public static void LoadSettings()
        {
            if (File.Exists("Settings.xml"))
            {
                Stream stream = new FileStream("Settings.xml", FileMode.Open);
                XmlSerializer xml = new XmlSerializer(DataBase.Settings.GetType());
                DataBase.Settings = xml.Deserialize(stream) as Settings;
                stream.Close();
                if (DataBase.Settings.Lang != GetUserLanguage())
                {
                    SetLanguage(DataBase.Settings.Lang);
                }
            }
            else
            {
                DataBase.Settings.Lang = GetUserLanguage();
            }
        }

        public static void SaveSttings()
        {
            Stream stream = new FileStream("Settings.xml", FileMode.OpenOrCreate);
            XmlSerializer xml = new XmlSerializer(DataBase.Settings.GetType());
            xml.Serialize(stream, DataBase.Settings);
            stream.Close();
        }

        public Settings()
        {
            DotaPath = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\dota 2 beta\\";
            AddonsPath = "game\\dota_addons\\";
            NpcPath = "scripts\\npc\\";
            VScriptPath = "scripts\\vscripts\\";
            Flash3Path = "resource\\flash3\\";

            AbilitiesPath = "npc_abilities_custom.txt";
            HeroesPath = "npc_heroes_custom.txt";
            UnitsPath = "npc_units_custom.txt";
            ItemsPath = "npc_items_custom.txt";

            DotaCachePath = "DotaCache\\";

            ProjectStuffSettings = "ProjectStuffSettings.kv";

            WriteHeadLinkOnSave = true;

            LoadSaveOpenedObjects = true;

            OpenLastOpenedProject = false;
            LastOpenedProjects = new List<string>(MaximumNumberLastOpenedProjects);

            ShowFullLuaFileNames = false;

            HideStandartDota2Projects = true;

            Lang = Language.English;

            HighSetts = new HighlightingSettings()
            {
                MarginWidth = 25,
                Font = "Consolas",
                FontSize = 10,
                Bold = false,
                Italic = false,
                Underline = false,
                DefaultWordColor = "#FF000000",
                CommentColor = "#FF808080",
                KeyColor = "#FF0094FF",
                KVBlockColor = "#FF7F3300",
                ValueNumberColor = "#FFB200FF",
                ValueStringColor = "#FFFFA366",
            };
            HighSettsLua = new HighlightingLuaSettings()
            {
                Font = "Arial",
                FontSize = 11,
                Bold = false,
                Italic = false,
                DigitsColor = "#00008b",
                BlockCommentColor = "#008000",
                LineCommentsColor = "#008000",
                CharColor = "#00008b",
                StringsColor = "#00008b",
                MultilineStringsColor = "#00008b",
                UserFunctionsColor = "#191970",
                PunctuationsColor = "#006400",
                KeyWordsColor = "#0000ff",
                TablesColor = "#00008b",
                TodoColor = "#ff0000",
                HackColor = "#EEE0E000",
            };

            EditorPriority = EditorType.GuiEditor;
        }
    }
}
