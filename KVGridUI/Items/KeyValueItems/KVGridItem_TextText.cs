using System.Drawing;
using System.Windows.Forms;

namespace KVGridUI
{
    public partial class KVGridItem_TextText : KVGridItemAbstract, KVGridItemInterface
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

        public int UpdateHeight()
        {
            return this.Size.Height;
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

        public bool Selected
        {
            get { return splitContainer1.BackColor != Control.DefaultBackColor; }
            set
            {
                if (this.Selected == value) return;

                if (value)
                {
                    splitContainer1.BackColor = Color.DarkBlue;
                }
                else
                {
                    splitContainer1.BackColor = Control.DefaultBackColor;
                }
            }
        }

        public int Id { get; set; }

        public int ItemHeight => this.Size.Height;
        public int ItemWidth { get { return this.Size.Width; } set { this.Width = value; } }

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
