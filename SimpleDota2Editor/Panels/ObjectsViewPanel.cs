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
        private FileKV ObjectKV;

        public void LoadMe(FileKV fileKv)
        {
            ObjectKV = fileKv;
            treeView1.Nodes.Clear();

            int i = 0;
            foreach (var obj in ObjectKV.ObjectList)
            {
                var kv = obj.SystemComment.FindKV("Folder");
                if (kv == null)
                    treeView1.Nodes.Add(i.ToString(), obj.Name);
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
                    node.Nodes.Add(i.ToString(), obj.Name);
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
            var obj = CreateObjectForm.ShowAndGet();
            if (obj == null) return;

            if (treeView1.SelectedNode?.Parent == null)
            {
                ObjectKV.ObjectList.Add(obj);
                treeView1.Nodes.Add((ObjectKV.ObjectList.Count - 1).ToString(), obj.Name);
                return;
            }

            TreeNode node = treeView1.SelectedNode.Parent;
            if (treeView1.SelectedNode.Name.Contains("#"))
                node = treeView1.SelectedNode;

            string path = node.GetNodePath("");
            obj.SystemComment.KVList.Add(new KV() {Key = "Folder", Value = path});
            ObjectKV.ObjectList.Add(obj);
            node.Nodes.Add((ObjectKV.ObjectList.Count - 1).ToString(), obj.Name);

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
                ObjectKV.RemoveObject(treeView1.SelectedNode.Text);
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

            if (e.Node.Name.Contains("#"))
            {
                e.Node.Text = e.Label;
                e.Node.Name = "#" + e.Label;
                e.Node.RenameChildsFolders(ObjectKV, e.Node.GetNodePath(""));
            }
            else
            {
                var obj = ObjectKV.FindObject(e.Node.Text);
                obj.Name = e.Label;
            }

            DataBase.Edited = true;
        }
    }
}
