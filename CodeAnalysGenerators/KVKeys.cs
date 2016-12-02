using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KV_reloaded;

namespace CodeAnalysGenerators
{
    public static class KVKeys
    {
        public static void Start()
        {
            KVToken parsedKVs = new KVToken("Keys");
            parsedKVs.Children.Add(new KVToken("ROOT"));

            AnalysFile("Files\\npc_units_custom.txt", ref parsedKVs);
            AnalysFile("Files\\npc_abilities_custom.txt", ref parsedKVs);
            AnalysFile("Files\\npc_heroes_custom.txt", ref parsedKVs);
            AnalysFile("Files\\npc_items_custom.txt", ref parsedKVs);

            parsedKVs.ForceSetStandartStyle();
            StreamWriter file = new StreamWriter("parsedKVs.txt", false);
            file.WriteLine(parsedKVs.ToString());
            file.Close();
        }

        private static void AnalysFile(string filePath, ref KVToken parsedKVs)
        {
            string unitsFileCode = File.ReadAllText(filePath);

            KVToken kvToken;
            kvToken = TokenAnalizer.AnaliseText(unitsFileCode).FirstOrDefault();

            foreach (var tok in kvToken.Children)
            {
                if (tok.Type == KVTokenType.KVblock)
                    Parse(ref parsedKVs, tok, "ROOT");
            }
        }

        private static void Parse(ref KVToken parsedKv, KVToken token, string parentName)
        {
            foreach (var tok in token.Children)
            {
                if (string.IsNullOrEmpty(tok.Key))
                    continue;
                if (parentName == "AbilitySpecial")
                    continue;
                if (parentName == "RunScript" && (tok.Key != "ScriptFile" || tok.Key != "Function"))
                    continue;

                if (parentName == "Modifiers")
                {
                    if (parsedKv.GetChild(parentName) == null)
                        parsedKv.Children.Add(new KVToken(parentName));
                    if (parsedKv.GetChild("MODIFIERS") == null)
                        parsedKv.Children.Add(new KVToken("MODIFIERS"));
                    if (tok.Type == KVTokenType.KVblock)
                        Parse(ref parsedKv, tok, "MODIFIERS");
                    continue;
                }

                if (parsedKv.GetChild(parentName) == null || parsedKv.GetChild(parentName).GetChild(tok.Key) == null)
                {
                    KVToken kv = new KVToken(tok.Key);
                    kv.Children.Add(new KVToken("Description", String.Concat("#", kv.Key)));
                    kv.Children.Add(new KVToken("SimpleKey", (tok.Type != KVTokenType.KVblock).ToString()));
                    if (parsedKv.GetChild(parentName) == null)
                        parsedKv.Children.Add(new KVToken(parentName));

                    parsedKv.GetChild(parentName).Children.Add(kv);
                }

                if (tok.Type == KVTokenType.KVblock)
                    Parse(ref parsedKv, tok, tok.Key);
            }
        }
    }
}