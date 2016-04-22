using System.Drawing;
using System.Windows.Forms;
using KV_reloaded;
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
        private int lastFreeFolderNum = 0;

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
        private KVToken ObjectKV;

        public void LoadMe(KVToken fileKv)
        {
            ObjectKV = fileKv;
            treeView1.Nodes.Clear();

            int i = 0;
            foreach (var obj in ObjectKV.Children)
            {
                if(obj.Type == KVTokenType.Comment)
                    continue;

                var kv = obj.SystemComment?.FindKV("Folder");
                if (kv == null)
                    treeView1.Nodes.Add(i.ToString(), obj.Key);
                else
                {
                    string folderPath = kv.Value;
                    int finded = folderPath.IndexOf('\\');
                    TreeNodeCollection lastNodeCollection = treeView1.Nodes;
                    TreeNode node;
                    while (finded != -1)
                    {
                        string folder = folderPath.Substring(0, finded);
                        node = lastNodeCollection.FindNode(folder);
                        if (node == null)
                        {
                            node = lastNodeCollection.Add("#" + folder, folder);
                        }
                        lastNodeCollection = node.Nodes;
                        folderPath = folderPath.Substring(finded + 1); 

                        finded = folderPath.IndexOf('\\');
                    }

                    node = lastNodeCollection.FindNode(folderPath);
                    if (node == null)
                    {
                        node = lastNodeCollection.Add("#" + folderPath, folderPath);
                    }
                    node.Nodes.Add(i.ToString(), obj.Key);
                }
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

            if (treeView1.SelectedNode.Name.Contains("#"))
                return;

            var textPanel = AllPanels.FindPanel(treeView1.SelectedNode.Text);
            if (textPanel == null)
            {
                var panel = new TextEditorPanel();
                panel.PanelName = treeView1.SelectedNode.Text;
                panel.SetText(ObjectKV.GetChild(treeView1.SelectedNode.Text).ChilderToString());
                panel.ObjectRef = ObjectKV.GetChild(treeView1.SelectedNode.Text);
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

        #region SubMenu

        /// <summary>
        /// Меню открыто
        /// </summary>
        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (treeView1.SelectedNode == null)
            {
                renameToolStripMenuItem.Enabled = false;
                deleteToolStripMenuItem.Enabled = false;
                return;
            }
        }

        /// <summary>
        /// Меню закрыто
        /// </summary>
        private void contextMenuStrip1_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            createObjectToolStripMenuItem.Enabled = true;
            createFolderToolStripMenuItem.Enabled = true;
            renameToolStripMenuItem.Enabled = true;
            deleteToolStripMenuItem.Enabled = true;
        }

        /// <summary>
        /// Создание объекта
        /// </summary>
        private void createObjectToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            var obj = CreateObjectForm.ShowAndGet(ObjectKV);
            if (obj == null) return;

            if (treeView1.SelectedNode?.Parent == null)
            {
                ObjectKV.Children.Add(obj);
                treeView1.Nodes.Add((ObjectKV.Children.Count - 1).ToString(), obj.Key);
                DataBase.Edited = true;
                return;
            }

            TreeNode node = treeView1.SelectedNode.Parent;
            if (treeView1.SelectedNode.Name.Contains("#"))
                node = treeView1.SelectedNode;

            string path = node.GetNodePath("");
            obj.SystemComment.KVList.Add(new KV() {Key = "Folder", Value = path});
            ObjectKV.Children.Add(obj);
            node.Nodes.Add((ObjectKV.Children.Count - 1).ToString(), obj.Key);

            DataBase.Edited = true;
        }

        /// <summary>
        /// Создание папки
        /// </summary>
        private void createFolderToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (treeView1.SelectedNode == null)
            {
                treeView1.Nodes.Add("#" + lastFreeFolderNum, "New folder " + lastFreeFolderNum);
                lastFreeFolderNum++;
                return;
            }

            if (treeView1.SelectedNode.Name.Contains("#"))
            {
                var node = treeView1.SelectedNode.Nodes.Add("#" + lastFreeFolderNum, "New folder " + lastFreeFolderNum);
                node.Parent?.Expand();
                lastFreeFolderNum++;
            }
            else
            {
                treeView1.Nodes.Add("#" + lastFreeFolderNum, "New folder " + lastFreeFolderNum);
                lastFreeFolderNum++;
            }

            DataBase.Edited = true;
        }

        /// <summary>
        /// Переименовывание объекта/папки
        /// </summary>
        private void renameToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            treeView1.SelectedNode.BeginEdit();
        }

        /// <summary>
        /// Удаление объекта/папки(вместе с содержимым)
        /// </summary>
        private void deleteToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (treeView1.SelectedNode.Name.Contains("#"))
            {
                treeView1.SelectedNode.DeleteChilds(ObjectKV);
                if (treeView1.SelectedNode.Parent == null)
                    treeView1.Nodes.Remove(treeView1.SelectedNode);
                else
                    treeView1.SelectedNode.Parent.Nodes.Remove(treeView1.SelectedNode);
            }
            else
            {
                ObjectKV.RemoveChild(treeView1.SelectedNode.Text);
                treeView1.SelectedNode.Parent.Nodes.Remove(treeView1.SelectedNode);
            }

            DataBase.Edited = true;
        }


        #endregion

        

        private void treeView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                treeView1.SelectedNode = treeView1.GetNodeAt(e.X, e.Y);
        }

        /// <summary>
        /// TreeNode: Текст отредактирован
        /// </summary>
        private void treeView1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.CancelEdit) return;
            if (string.IsNullOrEmpty(e.Label))
            {
                e.CancelEdit = true;
                return;
            }

            if (e.Node.Name.Contains("#"))
            {
                e.Node.Text = e.Label;
                e.Node.Name = "#" + e.Label;
                e.Node.RenameChildsFolders(ObjectKV, e.Node.GetNodePath(""));
            }
            else
            {
                var textPanel = AllPanels.FindPanel(e.Node.Text);
                textPanel.PanelName = e.Label;
                var obj = ObjectKV.GetChild(e.Node.Text);
                obj.Key = e.Label;
            }

            DataBase.Edited = true;
        }
    }
}
