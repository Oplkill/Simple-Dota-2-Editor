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

            timer = new Timer() {Enabled = false, Interval = 1000};
            timer.Tick += timerExperied;
        }

        private void KVSFITextBox_Click(object sender, EventArgs e)
        {
            OnActivateClick?.Invoke(sender, e);
        }

        public EventHandler OnActivateClick;

        public delegate void TextChangedFunc(string oldText, string newText);
        public new TextChangedFunc OnTextChanged;

        public override string Text
        {
            get { return textBox1.Text; }
            set
            {
                loading = true;
                textBox1.Text = value;
            }
        }

        private string oldText;
        private Timer timer;
        private bool loading;

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (loading)
            {
                loading = false;
                return;
            }

            if (timer.Enabled)
                timer.Stop();

            timer.Start();
        }

        private void timerExperied(object obj, EventArgs e)
        {
            if (oldText == null) return;

            var old = oldText;
            oldText = null;
            OnTextChanged?.Invoke(old, Text);

            timer.Stop();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (oldText == null)
                oldText = Text;
        }
    }
}
