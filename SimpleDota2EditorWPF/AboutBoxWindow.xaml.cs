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
    /// Логика взаимодействия для AboutBoxWindow.xaml
    /// </summary>
    public partial class AboutBoxWindow : Window
    {
        public AboutBoxWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void label1_Clicked(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Oplkill");
        }

        private void label2_Clicked(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Oplkill/Simple-Dota-2-Editor");
        }

        private void label3_Clicked(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Oplkill/Simple-Dota-2-Editor/blob/master/ChangeLog.txt");
        }
    }
}
