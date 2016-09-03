using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace KVGridUIWPF.Items.KeyValueItems
{
    /// <summary>
    /// Логика взаимодействия для KVGridItem_TextText.xaml
    /// </summary>
    public partial class KVGridItem_TextText : UserControl, KVGridItemInterface
    {
        public KVGridItem_TextText(int id = -1)
        {
            this.Id = id;
            ItemType = ItemTypes.TextText;

            InitializeComponent();

            kvsfiTextBoxKey.OnActivateClick += select_Click;
            kvsfiTextBoxValue.OnActivateClick += select_Click;

            kvsfiTextBoxKey.OnTextChanged += KeyTextChanged;
            kvsfiTextBoxValue.OnTextChanged += ValueTextChanged;
        }

        private void select_Click(object sender, System.EventArgs e)
        {
            GridOwner.SelectedItem = this;
        }

        private void KeyTextChanged(string oldText, string newText)
        {
            OnTextChanged?.Invoke(this, oldText, newText, KVType.Key);
        }

        private void ValueTextChanged(string oldText, string newText)
        {
            OnTextChanged?.Invoke(this, oldText, newText, KVType.Value);
        }

        #region variables

        private static Brush selectedBrush = new SolidColorBrush(Colors.Blue);
        private static Brush normalBrush = new SolidColorBrush(SystemColors.ControlColor);
        public bool Selected
        {
            get
            {
                return Equals(GridSplitter.Background, selectedBrush);
            }
            set
            {
                if (this.Selected == value) return;

                if (value)
                {
                    GridSplitter.Background = selectedBrush;
                }
                else
                {
                    GridSplitter.Background = normalBrush;
                }
            }
        }

        public int Id { get; set; }
        public KVGrid.TextChangedFunc OnTextChanged { get; set; }

        public int ItemHeight => (int)this.Height;

        public ItemTypes ItemType { get; }
        public KVGridBlock ParentBlock { get; set; }
        public KVGrid GridOwner { get; set; }

        public string KeyText
        {
            get { return kvsfiTextBoxKey.Text; }
            set { kvsfiTextBoxKey.Text = value; }
        }

        public string ValueText
        {
            get { return kvsfiTextBoxValue.Text; }
            set { kvsfiTextBoxValue.Text = value; }
        }

        #endregion
    }
}
