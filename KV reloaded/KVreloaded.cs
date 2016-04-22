using System.Linq;

namespace KV_reloaded
{
    public class KVreloaded
    {
        public KVToken MainToken;

        public void LoadKVText(string text)
        {
            MainToken = TokenAnalizer.AnaliseText(text).FirstOrDefault();
        }
    }
}
