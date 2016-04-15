using System.CodeDom;
using System.Drawing;

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

        public HighlightingSettings HighSetts;

        public class HighlightingSettings
        {
            public int MarginWidth;
            public string Font;
            public int FontSize;
            public bool Bold;
            public bool Italic;
            public bool Underline;
            public Color DefaultWordColor;
            public Color CommentColor;
            public Color KeyColor;
            public Color KVBlockColor;
            public Color ValueNumberColor;
            public Color ValueStringColor;
        }

        /// <summary>
        /// Добавлять в файлы шапку, что он сохранен с помощью этого редактора
        /// </summary>
        public bool WriteHeadLinkOnSave;

        public const string HeadLinkText = "//File edited with Simple Dota 2 Editor\n//GITHUB.RU...\n\n"; //todo добавить сылку на редактор 

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

            HighSetts = new HighlightingSettings()
            {
                MarginWidth = 25,
                Font = "Consolas",
                FontSize = 10,
                Bold = false,
                Italic = false,
                Underline = false,
                DefaultWordColor = Color.Black,
                CommentColor = Color.Gray,
                KeyColor = Color.Blue,
                KVBlockColor = Color.Brown,
                ValueNumberColor = Color.Purple,
                ValueStringColor = Color.BurlyWood,
            };
        }
    }
}