using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using KV_reloaded;
using SimpleDota2Editor.Properties;
using WeifenLuo.WinFormsUI.Docking;

namespace SimpleDota2Editor.Panels
{
    public partial class ObjectsViewPanel : DockContent
    {
        public ObjectsViewPanel()
        {
            undoRedoManager = new UndoRedoManager();
            InitializeComponent();

            UpdateUndoRedoButtons();
        }

        public ObjectTypePanel ObjectsType;
        private int lastFreeFolderNum = 0;
        private UndoRedoManager undoRedoManager;

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
            treeView1.TreeViewNodeSorter = new NodeSorter();
            ObjectKV = fileKv;
            treeView1.Nodes.Clear();

            int i = 0;
            foreach (var obj in ObjectKV.Children)
            {
                LoadObject(obj, treeView1, i);
                i++;
            }
            treeView1.Sort();
        }

        public static void LoadObject(KVToken obj, TreeView tree, int i)
        {
            if (obj.Type == KVTokenType.Comment || obj.Type == KVTokenType.KVsimple)
                return;

            var kv = obj.SystemComment?.FindKV("Folder");
            if (kv == null)
                tree.Nodes.Add(i.ToString(), obj.Key);
            else
            {
                string folderPath = kv.Value;
                int finded = folderPath.IndexOf('\\');
                TreeNodeCollection lastNodeCollection = tree.Nodes;
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

            if (treeView1.SelectedNode.IsFolder())
                return;

            var editorPanel = AllPanels.FindAnyEditorPanel(treeView1.SelectedNode.Text);
            if (editorPanel == null)
            {
                if (DataBase.Settings.EditorPriority == Settings.EditorType.TextEditor)
                {
                    var textPanel = new TextEditorPanel();
                    textPanel.PanelName = treeView1.SelectedNode.Text;
                    textPanel.ObjectRef = ObjectKV.GetChild(treeView1.SelectedNode.Text);
                    textPanel.SetText(ObjectKV.GetChild(treeView1.SelectedNode.Text).ChilderToString());
                    textPanel.Show(AllPanels.PrimaryDocking, DockState.Document);
                }
                else if (DataBase.Settings.EditorPriority == Settings.EditorType.GuiEditor)
                {
                    var guiPanel = new GuiEditorPanel();
                    guiPanel.PanelName = treeView1.SelectedNode.Text;
                    guiPanel.ObjectRef = ObjectKV.GetChild(treeView1.SelectedNode.Text);
                    guiPanel.InitGuiAndLoad();
                    guiPanel.Show(AllPanels.PrimaryDocking, DockState.Document);
                }
            }
            else
            {
                editorPanel.Activate();
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
        private void createObjectMenuItem_Click(object sender, System.EventArgs e)
        {
            var obj = CreateObjectForm.ShowAndGet(ObjectKV);
            if (obj == null) return;

            undoRedoManager.Execute(new CreateObjectCommand(treeView1, treeView1.SelectedNode, ObjectKV, obj));
            UpdateUndoRedoButtons();
        }

        /// <summary>
        /// Создание папки
        /// </summary>
        private void createFolderMenuItem_Click(object sender, System.EventArgs e)
        {
            undoRedoManager.Execute(new CreateFolderCommand(treeView1, treeView1.SelectedNode));
            UpdateUndoRedoButtons();
        }

        /// <summary>
        /// Переименовывание объекта/папки
        /// </summary>
        private void renameMenuItem_Click(object sender, System.EventArgs e)
        {
            if (treeView1.SelectedNode == null) return;

            string newText = RenameForm.ShowAndGet(treeView1.SelectedNode.Text);
            if (string.IsNullOrEmpty(newText))
                return;

            if (treeView1.SelectedNode.IsFolder())
            {
                undoRedoManager.Execute(new RenameFolderCommand(treeView1, treeView1.SelectedNode, ObjectKV, newText));
            }
            else
            {
                //todo сюда надо вставить проверку допустимых названий объектов в дотке
                undoRedoManager.Execute(new RenameObjectCommand(treeView1, treeView1.SelectedNode, ObjectKV, newText));
            }
            UpdateUndoRedoButtons();
        }

        /// <summary>
        /// Удаление объекта/папки(вместе с содержимым)
        /// </summary>
        private void deleteMenuItem_Click(object sender, System.EventArgs e)
        {
            if (treeView1.SelectedNode == null) return;

            if (treeView1.SelectedNode.IsFolder())
            {
                undoRedoManager.Execute(new DeleteFolderCommand(treeView1, treeView1.SelectedNode, ObjectKV));
            }
            else
            {
                undoRedoManager.Execute(new DeleteObjectCommand(treeView1, treeView1.SelectedNode, ObjectKV));
            }
            UpdateUndoRedoButtons();
        }

        private void undoMenuItem_Click(object sender, EventArgs e)
        {
            undoRedoManager.Undo();
            UpdateUndoRedoButtons();
        }

        private void redoMenuItem_Click(object sender, EventArgs e)
        {
            undoRedoManager.Redo();
            UpdateUndoRedoButtons();
        }

        private void UpdateUndoRedoButtons()
        {
            toolStripButtonUndo.Enabled = undoRedoManager.CanUndo();
            toolStripButtonRedo.Enabled = undoRedoManager.CanRedo();

            toolStripButtonUndo.ToolTipText = Resources.ObjViewPanelUndoDescription + @" """ + undoRedoManager.GetUndoActionName() + @"""";
            toolStripButtonRedo.ToolTipText = Resources.ObjViewPanelRedoDescription + @" """ + undoRedoManager.GetRedoActionName() + @"""";
        }

        #endregion

        private void treeView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void treeView1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
            treeView1.SelectedNode = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");
        }

        private void treeView1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false))
            {
                var movingNode = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");
                Point pt = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
                TreeNode destinationNode = ((TreeView)sender).GetNodeAt(pt);

                if ((destinationNode != null && destinationNode.TreeView != movingNode.TreeView) || treeView1 != movingNode.TreeView)
                    return;

                if (destinationNode != null && !destinationNode.IsFolder())
                    destinationNode = destinationNode.Parent;

                undoRedoManager.Execute(new MoveFolderObjectCommand(treeView1, ObjectKV, destinationNode, movingNode));
                UpdateUndoRedoButtons();
            }
        }

        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        {
            //treeView1.SelectedNode = treeView1.GetNodeAt(e.X, e.Y);
        }

        private void treeView1_MouseClick(object sender, MouseEventArgs e)
        {
            treeView1.SelectedNode = treeView1.GetNodeAt(e.X, e.Y);
        }

        



        #region UndoRedo


        private class CreateFolderCommand : ICommand
        {
            public string Name => Resources.CreateFolder;
            private readonly TreeView tree;
            private readonly TreeNode node;
            private TreeNode createdNode;

            public CreateFolderCommand(TreeView tree, TreeNode node)
            {
                this.tree = tree;
                this.node = node;
            }

            public void Execute()
            {
                int lastFreeFolderNum = 0; //todo сделать так чтобы идентификационные порядковые номера папок считывались при загрузки
                TreeNode fNode = tree.Nodes.FindNodeLike(node);

                if (fNode != null && fNode.IsFolder())
                {
                    createdNode = fNode.Nodes.Add("#" + lastFreeFolderNum, "New folder " + lastFreeFolderNum);
                }
                else
                {
                    createdNode = tree.Nodes.Add("#" + lastFreeFolderNum, "New folder " + lastFreeFolderNum);
                }
                fNode?.Expand();
                lastFreeFolderNum++;
                tree.Sort();
                DataBase.Edited = true;
            }

            public void UnExecute()
            {
                TreeNode fNode = tree.Nodes.FindNodeLike(createdNode);

                fNode.Remove();
                DataBase.Edited = true;
            }
        }

        private class CreateObjectCommand : ICommand
        {
            public string Name => Resources.CreateObject;
            private readonly TreeView tree;
            private readonly TreeNode node;
            private readonly KVToken objectKV;
            private readonly KVToken obj;
            private TreeNode createdNode;

            public CreateObjectCommand(TreeView tree, TreeNode node, KVToken objectKV, KVToken obj)
            {
                this.tree = tree;
                this.node = node;
                this.objectKV = objectKV;
                this.obj = obj;
            }

            public void Execute()
            {
                TreeNode fNode = tree.Nodes.FindNodeLike(node);

                if (fNode == null || (fNode.Parent == null && !fNode.IsFolder()))
                {
                    objectKV.Children.Add(obj);
                    createdNode = tree.Nodes.Add((objectKV.Children.Count - 1).ToString(), obj.Key);
                    tree.Sort();
                    DataBase.Edited = true;
                    return;
                }

                TreeNode parNode = fNode.Parent;

                if (fNode.IsFolder())
                    parNode = fNode;

                string path = parNode.GetNodePath("");
                if(obj.SystemComment == null)
                    obj.SystemComment = new SystemComment();
                obj.SystemComment.AddKV(new KV() { Key = "Folder", Value = path });
                objectKV.Children.Add(obj);
                createdNode = parNode.Nodes.Add((objectKV.Children.Count - 1).ToString(), obj.Key);

                tree.Sort();
                DataBase.Edited = true;
            }

            public void UnExecute()
            {
                TreeNode fNode = tree.Nodes.FindNodeLike(createdNode);

                fNode.Remove();
                var textPanel = AllPanels.FindEditorPanel(obj.Key);
                textPanel?.ForceClose();
                objectKV.RemoveChild(obj.Key);

                tree.Sort();
                DataBase.Edited = true;
            }
        }

        private class RenameFolderCommand : ICommand
        {
            public string Name => Resources.RenameFolder;
            private readonly TreeView tree;
            private readonly TreeNode node;
            private readonly KVToken objectKV;
            private readonly string newText;
            private readonly string oldText;

            public RenameFolderCommand(TreeView tree, TreeNode node, KVToken objectKV, string newText)
            {
                this.tree = tree;
                this.node = node;
                this.objectKV = objectKV;
                this.newText = newText;
                this.oldText = node.Text;
            }

            public void Execute()
            {
                TreeNode fNode = tree.Nodes.FindNodeLike(node);
                fNode.Text = newText;
                fNode.Name = "#" + newText;
                fNode.RenameChildsFolders(objectKV, fNode.GetNodePath(""));

                tree.Sort();
                DataBase.Edited = true;
            }

            public void UnExecute()
            {
                TreeNode fNode = tree.Nodes.FindNodeLike(node);
                fNode.Text = oldText;
                fNode.Name = "#" + oldText;
                fNode.RenameChildsFolders(objectKV, fNode.GetNodePath(""));

                tree.Sort();
                DataBase.Edited = true;
            }
        }

        private class RenameObjectCommand : ICommand
        {
            public string Name => Resources.RenameObject;
            private readonly TreeView tree;
            private readonly TreeNode node;
            private readonly KVToken objectKV;
            private readonly string newText;
            private readonly string oldText;

            public RenameObjectCommand(TreeView tree, TreeNode node, KVToken objectKV, string newText)
            {
                this.tree = tree;
                this.node = node;
                this.objectKV = objectKV;
                this.newText = newText;
                this.oldText = node.Text;
            }

            public void Execute()
            {
                var textPanel = AllPanels.FindEditorPanel(oldText);
                if (textPanel != null)
                    textPanel.PanelName = newText;
                var obj = objectKV.GetChild(oldText);
                obj.Key = newText;
                TreeNode fNode = tree.Nodes.FindNodeLike(node);
                fNode.Text = newText;
                fNode.Name = newText;

                tree.Sort();
                DataBase.Edited = true;
            }

            public void UnExecute()
            {
                var textPanel = AllPanels.FindEditorPanel(newText);
                if (textPanel != null)
                    textPanel.PanelName = oldText;
                var obj = objectKV.GetChild(newText);
                obj.Key = oldText;
                TreeNode fNode = tree.Nodes.FindNodeLike(node);
                fNode.Text = oldText;
                fNode.Name = oldText;

                tree.Sort();
                DataBase.Edited = true;
            }
        }

        private class DeleteFolderCommand : ICommand
        {
            public string Name => Resources.DeleteFolder;
            private readonly TreeView tree;
            private readonly TreeNode node;
            private readonly KVToken objectKV;
            private List<KVToken> deletedObjects; 

            public DeleteFolderCommand(TreeView tree, TreeNode node, KVToken objectKV)
            {
                this.tree = tree;
                this.node = node;
                this.objectKV = objectKV;
                deletedObjects = new List<KVToken>();
            }

            public void Execute()
            {
                TreeNode fNode = tree.Nodes.FindNodeLike(node);

                deletedObjects.Clear();
                fNode.DeleteChilds(objectKV, deletedObjects);
                fNode.Remove();

                foreach (var obj in deletedObjects)
                {
                    var textPanel = AllPanels.FindEditorPanel(obj.Key);
                    textPanel?.ForceClose();
                }

                DataBase.Edited = true;
            }

            public void UnExecute()
            {
                foreach (var obj in deletedObjects)
                {
                    ObjectsViewPanel.LoadObject(obj, tree, 0);
                    objectKV.Children.Add(obj);
                }

                DataBase.Edited = true;
            }
        }

        private class DeleteObjectCommand : ICommand
        {
            public string Name => Resources.DeleteObject;
            private readonly TreeView tree;
            private TreeNode node;
            private readonly KVToken objectKV;
            private KVToken deletedObject;
            private TreeNode deletedParentNode;

            public DeleteObjectCommand(TreeView tree, TreeNode node, KVToken objectKV)
            {
                this.tree = tree;
                this.node = node;
                this.objectKV = objectKV;
            }

            public void Execute()
            {
                TreeNode fNode = tree.Nodes.FindNodeLike(node);

                deletedObject = objectKV.GetChild(fNode.Text);
                objectKV.RemoveChild(fNode.Text);
                deletedParentNode = fNode.Parent;
                fNode.Remove();

                var textPanel = AllPanels.FindEditorPanel(deletedObject.Key);
                textPanel?.ForceClose();
                DataBase.Edited = true;
            }

            public void UnExecute()
            {
                objectKV.Children.Add(deletedObject);

                if (deletedObject != null)
                {
                    TreeNode fNode = tree.Nodes.FindNodeLike(deletedParentNode);

                    node = fNode.Nodes.Add(deletedObject.Key, deletedObject.Key);
                }
                else
                {
                    node = tree.Nodes.Add(deletedObject.Key, deletedObject.Key);
                }
                DataBase.Edited = true;
            }
        }

        private class MoveFolderObjectCommand : ICommand
        {
            public string Name => Resources.MoveFolderObject;
            private readonly TreeView tree;
            private readonly KVToken objectKV;
            private TreeNode destinationNode;
            private TreeNode movingNode;
            private TreeNode sourceNode;

            public MoveFolderObjectCommand(TreeView tree, KVToken objectKV, TreeNode destinationNode, TreeNode movingNode)
            {
                this.tree = tree;
                this.destinationNode = destinationNode;
                this.movingNode = movingNode;
                this.objectKV = objectKV;
            }

            public void Execute()
            {
                var mNode = tree.Nodes.FindNodeLike(movingNode);
                var dNode = tree.Nodes.FindNodeLike(destinationNode);

                sourceNode = mNode.Parent;
                var newNode = (TreeNode) mNode.Clone();
                if (dNode == null)
                {
                    tree.Nodes.Add(newNode);
                    mNode.Remove();

                    if (newNode.IsFolder())
                        newNode.RenameChildsFolders(objectKV, newNode.GetNodePath(""));
                    else
                    {
                        var obj = objectKV.GetChild(newNode.Text);
                        obj.SystemComment?.DeleteKV("Folder");
                    }
                }
                else
                {
                    dNode.Nodes.Add(newNode);
                    dNode.Expand();
                    mNode.Remove();

                    dNode.RenameChildsFolders(objectKV, dNode.GetNodePath(""));
                }
                movingNode = newNode;
                destinationNode = dNode;

                tree.Sort();
                DataBase.Edited = true;
            }

            public void UnExecute()
            {
                var sNode = tree.Nodes.FindNodeLike(sourceNode);
                var dNode = tree.Nodes.FindNodeLike(destinationNode);
                var mNode = tree.Nodes.FindNodeLike(movingNode);

                destinationNode = dNode;
                var newNode = (TreeNode) mNode.Clone();
                if (sNode == null)
                {
                    tree.Nodes.Add(newNode);
                    mNode.Remove();

                    if (newNode.IsFolder())
                        newNode.RenameChildsFolders(objectKV, newNode.GetNodePath(""));
                    else
                    {
                        var obj = objectKV.GetChild(newNode.Text);
                        obj.SystemComment?.DeleteKV("Folder");
                    }
                }
                else
                {
                    sNode.Nodes.Add(newNode);
                    sNode.Expand();
                    mNode.Remove();

                    sNode.RenameChildsFolders(objectKV, sNode.GetNodePath(""));
                }
                movingNode = newNode;
                sourceNode = sNode;

                tree.Sort();
                DataBase.Edited = true;
            }
        }

        #endregion





    }
}
