using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SimpleDota2Editor.Properties;
using WeifenLuo.WinFormsUI.Docking;

namespace SimpleDota2Editor.Panels
{
    public partial class StartPagePanel : DockContent
    {
        public StartPagePanel()
        {
            InitializeComponent();
            this.Text = Resources.StartPage;
        }
    }
}
