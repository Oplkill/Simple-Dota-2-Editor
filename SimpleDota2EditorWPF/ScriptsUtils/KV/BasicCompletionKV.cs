using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.AvalonEdit.CodeCompletion;
using KV_reloaded;
using SimpleDota2EditorWPF.Panels;

namespace SimpleDota2EditorWPF.ScriptsUtils.KV
{
    public static class BasicCompletionKV
    {
        public static KVToken Keys = new KVToken("");
        public static KVToken Values = new KVToken("");

        public static void Init()
        {
            loadKv("ScriptsUtils\\KV\\BasicCompletionKeys.kv", ref Keys);
            loadKv("ScriptsUtils\\KV\\BasicCompletionValues.kv", ref Values);
        }

        private static void loadKv(string file, ref KVToken token)
        {
            if (!File.Exists(file))
                return;

            token = TokenAnalizer.AnaliseText(File.ReadAllText(file)).FirstOrDefault();
        }
    }
}
