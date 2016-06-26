using System.Windows.Forms;

namespace KVGridUI
{
    public class KVGridItemAbstract : UserControl
    {
        public delegate void TextChangedFunc(KVGridItemInterface item, string oldText, string newText, KVType type);

        public new TextChangedFunc OnTextChanged;


        public string[] comments; //todo удалить и изменить, временное решение оформления
    }
}