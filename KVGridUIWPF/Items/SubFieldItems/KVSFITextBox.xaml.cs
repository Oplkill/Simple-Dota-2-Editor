using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace KVGridUIWPF.Items.SubFieldItems
{
    /// <summary>
    /// Логика взаимодействия для KVSFITextBox.xaml
    /// </summary>
    public partial class KVSFITextBox : UserControl, KVGridSubFieldItemInterface
    {
        public KVSFITextBox()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 1);
            timer.Tick += timerExperied;
        }

        public EventHandler OnActivateClick;

        public delegate void TextChangedFunc(string oldText, string newText);
        public new TextChangedFunc OnTextChanged;

        public string Text
        {
            get { return textBox1.Text; }
            set
            {
                loading = true;
                textBox1.Text = value;
            }
        }

        private string oldText;
        private DispatcherTimer timer;
        private bool loading;

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

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (loading)
            {
                loading = false;
                return;
            }

            if (timer.IsEnabled)
                timer.Stop();

            timer.Start();
        }

        private void textBox1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OnActivateClick?.Invoke(this, e);
        }

        private void TextBox1_OnGotFocus(object sender, RoutedEventArgs e)
        {
            OnActivateClick?.Invoke(this, e);
        }
    }
}
