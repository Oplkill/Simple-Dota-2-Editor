using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KV_reloaded
{
    public class KVreloaded
    {
        public List<Token> Tokens;

        public void LoadKVText(string text)
        {
            Tokens = new TokenAnalizer().AnaliseText(text);

        }
    }
}
