using System.Linq;
using System.Windows.Forms;
using SimpleDota2Editor.Properties;
using TempLoaderKVfiles;

namespace SimpleDota2Editor
{
    public partial class CreateObjectForm : Form
    {
        public FileKV.ObjectStruct obj;

        public CreateObjectForm()
        {
            InitializeComponent();
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

            obj = new FileKV.ObjectStruct
            {
                Name = textBoxName.Text,
                SystemComment = new SystemComment(),
                Text = "\n\"" + textBoxName.Text + "\"\n{\n\n}"
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

        public static FileKV.ObjectStruct ShowAndGet()
        {
            var form = new CreateObjectForm();
            form.ShowDialog();
            var obj = form.obj;
            form.Close();

            return obj;
        }
    }
}
