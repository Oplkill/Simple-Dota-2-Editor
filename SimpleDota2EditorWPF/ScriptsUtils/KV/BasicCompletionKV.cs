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
        public static Dictionary<string, IList<ICompletionData>> DataKeys = new Dictionary<string, IList<ICompletionData>>();
        public static Dictionary<string, IList<ICompletionData>> DataValues = new Dictionary<string, IList<ICompletionData>>();

        public static void Init()
        {
            load("ScriptsUtils\\KV\\BasicCompletionKeys.kv", DataKeys, KVScriptResourcesKeys.ResourceManager);
            load("ScriptsUtils\\KV\\BasicCompletionValues.kv", DataValues, KVScriptResourcesValues.ResourceManager);
        }

        private static void load(string file, Dictionary<string, IList<ICompletionData>> data,
            System.Resources.ResourceManager resources)
        {
            data.Clear();

            if (!File.Exists(file))
                return;

            var kvToken = TokenAnalizer.AnaliseText(File.ReadAllText(file)).FirstOrDefault();
            if (kvToken == null)
                return;

            foreach (var kv in kvToken.Children)
            {
                if (kv.Children != null && kv.Type == KVTokenType.KVblock)
                {
                    // if value starting with '#', description will be getted from resources
                    var list = (from key in kv.Children let descr = (key.Value.FirstOrDefault() == '#') ? resources.GetString(key.Value.Substring(1)) : key.Value select new TextEditorKVPanel.MyCompletionData(key.Key, descr)).Cast<ICompletionData>().ToList();
                    data.Add((kv.Key == "ROOT") ? "" : kv.Key, list);
                }
            }
        }
    }
}
