using System.Drawing;
using System.Windows.Forms;
using TempLoaderKVfiles;
using WeifenLuo.WinFormsUI.Docking;

namespace SimpleDota2Editor.Panels
{
    public partial class ObjectsViewPanel : DockContent
    {
        public ObjectsViewPanel()
        {
            InitializeComponent();
        }

        public ObjectTypePanel ObjectsType;

        public void UpdateIcon()
        {
            switch (ObjectsType)
            {
                case ObjectTypePanel.Abilities:
                    this.Icon = new Icon("Resources\\Abilities.ico");
                    break;

                case ObjectTypePanel.AbilitiesOverride:
                    this.Icon = new Icon("Resources\\AbilitiesOverride.ico");
                    break;

                case ObjectTypePanel.Heroes:
                    this.Icon = new Icon("Resources\\Heroes.ico");
                    break;

                case ObjectTypePanel.Units:
                    this.Icon = new Icon("Resources\\Units.ico");
                    break;

                case ObjectTypePanel.Items:
                    this.Icon = new Icon("Resources\\Items.ico");
                    break;
            }
        }

        /// <summary>
        /// Сылка на загруженный файл объектов
        /// </summary>
        private FileKV ObjectKV;

        public void LoadMe(FileKV fileKv)
        {
            ObjectKV = fileKv;
            treeView1.Nodes.Clear();

            int i = 0;
            foreach (var obj in ObjectKV.ObjectList)
            {
                treeView1.Nodes.Add(i.ToString(), obj.Name);
                i++;
            }
        }

        public void CloseMe()
        {
            ObjectKV = null;
            treeView1.Nodes.Clear();
        }

        private void treeView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (treeView1.SelectedNode == null)
                return;

            var textPanel = AllPanels.FindPanel(treeView1.SelectedNode.Text);
            if (textPanel == null)
            {
                var panel = new TextEditorPanel();
                panel.PanelName = treeView1.SelectedNode.Text;
                panel.SetText(ObjectKV.ObjectList[int.Parse(treeView1.SelectedNode.Name)].Text);
                panel.ObjectRef = ObjectKV.ObjectList[int.Parse(treeView1.SelectedNode.Name)];
                panel.Show(AllPanels.PrimaryDocking, DockState.Document);
            }
            else
            {
                textPanel.Activate();
            }
            
        }

        public enum ObjectTypePanel
        {
            Abilities,
            AbilitiesOverride,
            Heroes,
            Units,
            Items,
        }

        private void ObjectsViewPanel_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            AllPanels.Form1.SomeObjectWindowHided(ObjectsType);
        }
    }
}
