using System.Windows.Forms;

namespace KVGridUI
{
    public abstract class KVGridItemAbstract : UserControl
    {
        public delegate void TextChangedFunc(KVGridItemInterface item, string oldText, string newText, KVType type);

        public new TextChangedFunc OnTextChanged;
    }
}