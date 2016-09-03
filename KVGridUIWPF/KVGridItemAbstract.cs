using System.Windows.Controls;

namespace KVGridUIWPF
{
    public abstract class KVGridItemAbstract : UserControl
    {
        

        public new KVGrid.TextChangedFunc OnTextChanged;


        public string[] comments; //todo удалить и изменить, временное решение оформления 
    }
}