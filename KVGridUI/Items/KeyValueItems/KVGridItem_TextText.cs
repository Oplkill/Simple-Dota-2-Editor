using System.Drawing;
using System.Windows.Forms;

namespace KVGridUI
{
    public partial class KVGridItem_TextText : UserControl, KVGridItemInterface
    {
        public KVGridItem_TextText()
        {
            ItemType = ItemTypes.TextText;

            InitializeComponent();

            kvsfiTextBoxKey.OnActivateClick += select_Click;
            kvsfiTextBoxValue.OnActivateClick += select_Click;
        }

        public int UpdateHeight()
        {
            return this.Size.Height;
        }

        private void select_Click(object sender, System.EventArgs e)
        {
            GridOwner.SelectedItem = this;
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
