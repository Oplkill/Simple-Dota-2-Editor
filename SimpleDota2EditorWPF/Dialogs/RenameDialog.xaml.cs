using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SimpleDota2EditorWPF
{
    /// <summary>
    /// Логика взаимодействия для RenameDialog.xaml
    /// </summary>
    public partial class RenameDialog : Window
    {
        public RenameDialog()
        {
            InitializeComponent();
        }

        private string startText = "";
        private string[] lockedNames;

        public string ShowDialog(string oldName, string[] lockedNames)
        {
            this.lockedNames = lockedNames;
            textBox.Text = startText = oldName;
            this.ShowDialog();
            return textBox.Text;
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            textBox.Text = startText;
            this.Close();
        }

        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            if (textBox.Text == startText)
            { ButtonCancel_Click(sender, e); return; }

            if (lockedNames.FirstOrDefault(name => String.Equals(name, textBox.Text, StringComparison.CurrentCultureIgnoreCase)) == null)
                this.Close();
            else
            {
                label.Content = Properties.Resources.RenameThisNameAlreadyUsing;
            }
        }

        private void textBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            label.Content = "";
        }
    }
}
