using System.Windows;
using KVGridUIWPF.Items.KeyValueItems;

namespace KVGridUIWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            KvGrid1.MainBlock.AddItem(KvGrid1, new KVGridItem_TextText(), -1);
            KvGrid1.MainBlock.AddItem(KvGrid1, new KVGridItem_TextText(), -1);
            var block = KvGrid1.MainBlock.AddItem(KvGrid1, new KVGridBlock(), 0);
            ((KVGridBlock)block).AddItem(KvGrid1, new KVGridItem_TextText(), -1);
            for (int i = 0; i < 30; i++)
            {
                KvGrid1.MainBlock.AddItem(KvGrid1, new KVGridItem_TextText(), -1);
                ((KVGridBlock)block).AddItem(KvGrid1, new KVGridItem_TextText(), -1);
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            KvGrid1.SelectedItem?.ParentBlock.RemoveItem(KvGrid1.SelectedItem, true);
            
        }
    }
}
