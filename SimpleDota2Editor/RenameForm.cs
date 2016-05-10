using System;
using System.Windows.Forms;

namespace SimpleDota2Editor
{
    public partial class RenameForm : Form
    {
        public string RenameText;

        public RenameForm(string text)
        {
            InitializeComponent();
            textBox1.Text = RenameText = text;
        }

        private void RenameForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            
            if (e.CloseReason == CloseReason.UserClosing)
            {
                RenameText = null;
                e.Cancel = true;
                this.Hide();
            }
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            RenameText = textBox1.Text;
            this.Hide();
        }

        public static string ShowAndGet(string str = "")
        {
            var form = new RenameForm(str);
            form.ShowDialog();
            string text = form.RenameText;
            form.Close();

            return text;
        }

        
    }
}
