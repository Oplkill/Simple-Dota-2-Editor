using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
