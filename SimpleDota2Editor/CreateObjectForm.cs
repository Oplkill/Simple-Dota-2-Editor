using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using KV_reloaded;
using SimpleDota2Editor.Properties;

namespace SimpleDota2Editor
{
    public partial class CreateObjectForm : Form
    {
        public KVToken obj;
        public KVToken parent;

        public CreateObjectForm(KVToken _parent)
        {
            InitializeComponent();
            parent = _parent;
        }

        private void buttonCancel_Click(object sender, System.EventArgs e)
        {
            obj = null;
            this.Hide();
        }

        private void buttonOk_Click(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show(Resources.CreateObjectNameEmpty);
                return;
            }

            obj = new KVToken()
            {
                Key = textBoxName.Text,
                Children = new List<KVToken>(),
                Type = KVTokenType.KVblock,
                Parent = parent,
            };
            this.Hide();
        }

        private void CreateObjectForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        public static KVToken ShowAndGet(KVToken parent)
        {
            var form = new CreateObjectForm(parent);
            form.ShowDialog();
            var obj = form.obj;
            form.Close();

            return obj;
        }
    }
}
