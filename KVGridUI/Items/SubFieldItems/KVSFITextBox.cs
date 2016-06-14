using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KVGridUI.Items.SubFieldItems
{
    public partial class KVSFITextBox : UserControl, KVGridSubFieldItemInterface
    {
        public KVSFITextBox()
        {
            InitializeComponent();
        }

        private void KVSFITextBox_Click(object sender, EventArgs e)
        {
            OnActivateClick?.Invoke(sender, e);
        }

        public EventHandler OnActivateClick;

        public override string Text
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }
    }
}
