using System.CodeDom;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;

namespace SimpleDota2Editor
{
    public class Settings
    {
        public string DotaPath;
        public readonly string NpcPath;
        public readonly string AddonsPath;

        public readonly string AbilitiesPath;
        public readonly string AbilitiesOverridePath;
        public readonly string HeroesPath;
        public readonly string UnitsPath;
        public readonly string ItemsPath;

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

            AbilitiesPath = "npc_abilities_custom.txt";
            AbilitiesOverridePath = "npc_abilities_override.txt";
            HeroesPath = "npc_heroes_custom.txt";
            UnitsPath = "npc_units_custom.txt";
            ItemsPath = "npc_items_custom.txt";

            WriteHeadLinkOnSave = true;

            Lang = Language.English;

            HighSetts = new HighlightingSettings()
            {
                MarginWidth = 25,
                Font = "Consolas",
                FontSize = 10,
                Bold = false,
                Italic = false,
                Underline = false,
                DefaultWordColor = ColorTranslator.ToHtml(Color.Black),
                CommentColor = ColorTranslator.ToHtml(Color.Gray),
                KeyColor = ColorTranslator.ToHtml(Color.Blue),
                KVBlockColor = ColorTranslator.ToHtml(Color.Brown),
                ValueNumberColor = ColorTranslator.ToHtml(Color.Purple),
                ValueStringColor = ColorTranslator.ToHtml(Color.BurlyWood),
            };
        }
    }
}