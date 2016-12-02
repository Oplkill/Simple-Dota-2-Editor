using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using KV_reloaded;
using SimpleDota2EditorWPF.Dialogs;
using SomeUtils;
using Xceed.Wpf.AvalonDock.Layout;
using ICommand = SomeUtils.ICommand;

namespace SimpleDota2EditorWPF.Panels
{
    public partial class ObjectsViewPanel : UserControl
    {
        public ObjectsViewPanel()
        {
            undoRedoManager = new UndoRedoManager();
            InitializeComponent();

            
            //UpdateUndoRedoButtons();
        }

        public ObjectTypePanel ObjectsType;
        private int lastFreeFolderNum = 0;
        private UndoRedoManager undoRedoManager;
        private TreeViewItem movingItem;
        private TreeViewItem maybeMovingItem; //Этот предмет возможно скоро будет двигаемым, но пока не ясно будет ли
        private Point startPoint;

        /// <summary>
        /// Сылка на загруженный файл объектов
        /// </summary>
        private KVToken ObjectKV;

        public void LoadMe(KVToken fileKv)
        {
            //TreeView.TreeViewNodeSorter = new NodeSorter();
            ObjectKV = fileKv;
            TreeView1.Items.Clear();

            long i = (long)DateTime.Now.TimeOfDay.TotalMilliseconds;
            foreach (var obj in ObjectKV.Children)
            {
                LoadObject(obj, TreeView1, ref i);
                i--;
            }
            TreeView1.Items.Sort();
        }

        public static void LoadObject(KVToken obj, TreeView tree, ref long i)
        {
            if (obj.Type == KVTokenType.Comment || obj.Type == KVTokenType.KVsimple)
                return;

            var kv = obj.SystemComment?.FindKV("Folder");
            if (kv == null)
            {
                tree.Items.Add(new TreeViewItem() { Header = obj.Key, Uid = i.ToString() });
            }
            else
            {
                string folderPath = kv.Value;
                int finded = folderPath.IndexOf('\\');
                ItemCollection lastNodeCollection = tree.Items;
                TreeViewItem node;
                while (finded != -1)
                {
                    string folder = folderPath.Substring(0, finded);
                    node = lastNodeCollection.FindItem(folder);
                    if (node == null)
                    {
                        node = new TreeViewItem() { Header = folder, Tag = String.Concat("#", folder), Uid = (i--).ToString() };
                        lastNodeCollection.Add(node);
                    }
                    lastNodeCollection = node.Items;
                    folderPath = folderPath.Substring(finded + 1);

                    finded = folderPath.IndexOf('\\');
                }

                node = lastNodeCollection.FindItem(folderPath);
                if (node == null)
                {
                    node = new TreeViewItem() { Header = folderPath, Tag = String.Concat("#", folderPath) };
                    lastNodeCollection.Add(node);
                }
                node.Items.Add(new TreeViewItem() { Header = obj.Key, Uid = i.ToString() });
            }
        }

        public void CloseMe()
        {
            ObjectKV = null;
            TreeView1.Items.Clear();
        }

        private void TreeView1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left) return;
            if (TreeView1.SelectedItem == null) return;
            if (((TreeViewItem)TreeView1.SelectedItem).IsFolder()) return;

            var selectedItem = (TreeViewItem)TreeView1.SelectedItem;
            var objectName = (string) selectedItem.Header;
            LoadObject(objectName);
        }

        public void LoadObject(string objectName)
        {
            var editorPanel = AllPanels.FindAnyEditorPanel(objectName, ObjectsType);
            if (editorPanel == null)
            {
                var panel = new LayoutDocument();
                KVToken kvItem = ObjectKV.GetChild(objectName);
                if (kvItem.SystemComment != null && kvItem.SystemComment.HasAnyKeyWhichContainingString("lua"))
                {
                    var collEditors = new EditorsCollectionPanel();

                    collEditors.PanelDocument = panel;
                    collEditors.ObjectType = ObjectsType;
                    collEditors.ObjectRef = ObjectKV;
                    collEditors.PanelName = objectName;

                    //--
                    var KVPanel = new LayoutDocument();
                    var KVPanelEditor = new TextEditorKVPanel();
                    KVPanelEditor.PanelDocument = KVPanel;
                    KVPanelEditor.ObjectRef = kvItem;
                    KVPanelEditor.SetText(kvItem.ChilderToString());
                    KVPanelEditor.ObjectType = ObjectsType;
                    KVPanelEditor.PanelName = objectName;
                    KVPanel.Content = KVPanelEditor;
                    KVPanel.Closing += KVPanelEditor.Closing;
                    KVPanel.IsActiveChanged += KVPanelEditor.IsActiveChanged;
                    collEditors.DocumentsPane.Children.Add(KVPanel);

                    foreach (var kv in kvItem.SystemComment.KVList.Where(kv => kv.Key.Contains("lua")))
                    {
                        var panelT = new LayoutDocument();
                        var LuaPanelEditor = new TextEditorLUAPanel();
                        LuaPanelEditor.PanelDocument = panelT;
                        string fileName = kv.Value;
                        LuaPanelEditor.PanelName = fileName;
                        LuaPanelEditor.LoadTextFromFile(DataBase.AddonPath + DataBase.Settings.VScriptPath + kv.Value);
                        panelT.Content = LuaPanelEditor;
                        panelT.Closing += LuaPanelEditor.Closing;
                        panelT.IsActiveChanged += LuaPanelEditor.IsActiveChanged;
                        collEditors.DocumentsPane.Children.Add(panelT);
                    }
                    //--

                    panel.Content = collEditors;
                    panel.Closing += collEditors.Closing;
                    panel.IsActiveChanged += ((IEditor)collEditors).IsActiveChanged;
                }
                else
                {
                    var textPanel = new TextEditorKVPanel();

                    textPanel.PanelDocument = panel;
                    textPanel.ObjectRef = kvItem;
                    textPanel.SetText(kvItem.ChilderToString());
                    textPanel.ObjectType = ObjectsType;
                    textPanel.PanelName = objectName;

                    panel.Content = textPanel;
                    panel.Closing += textPanel.Closing;
                    panel.IsActiveChanged += ((IEditor)textPanel).IsActiveChanged;
                }


                AllPanels.LayoutDocumentPane.Children.Add(panel);

                panel.IsActive = true;
                //AllPanels.Form1.ShowEditorMenu(Form1.EditorType.Text);
            }
            else
            {
                editorPanel.IsActive = true;
            }
        }

        public enum ObjectTypePanel
        {
            Abilities,
            Heroes,
            Units,
            Items,
        }
        
        private void List_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Store the mouse position
            startPoint = e.GetPosition(null);
            //maybeMovingItem = (TreeViewItem)TreeView1.SelectedItem;
            maybeMovingItem = MyTreeViewHelper._currentItem;
        }

        private void treeView1_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && movingItem == null)
            {
                Point mousePos = e.GetPosition(null);
                Vector diff = startPoint - mousePos;

                //TreeViewItem treeViewItem =
                //    TreeViewUtils.FindAnchestor<TreeViewItem>((DependencyObject)e.OriginalSource);

                //if (treeViewItem == null) return;
                // ищем родительский элемент
                //ItemsControl parent = TreeViewUtils.FindParent<ItemsControl>(treeViewItem);

                //object item = parent.ItemContainerGenerator.ItemFromContainer(treeViewItem);

                //maybeMovingItem = item as TreeViewItem;

                if (!(Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance) ||
                    !(Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
                    return;

                if (maybeMovingItem != null)
                {
                    

                    movingItem = maybeMovingItem;
                    maybeMovingItem = null;
                    this.Cursor = Cursors.Hand;
                }
            }
            else if (e.LeftButton != MouseButtonState.Pressed && movingItem != null)
            {
                maybeMovingItem = null;
                movingItem = null;
                this.Cursor = Cursors.Arrow;
            }
        }

        private void TreeView1_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (movingItem == null) return;

            this.Cursor = Cursors.Arrow;

            TreeViewItem targetItem = MyTreeViewHelper._currentItem;
            if (!TreeViewUtils.CanMoveItemToItem(movingItem, targetItem)) return;

            Console.WriteLine("Moving = " + movingItem.Header.ToString() + "   /nTarget" + targetItem);

            undoRedoManager.Execute(new MoveCommand(TreeView1, ObjectKV, movingItem, targetItem));

            movingItem = null;
            UpdateUndoRedoButtons();
            DataBase.Edited = true;
        }

        #region Menu

        private void ButtonUndo_Click(object sender, RoutedEventArgs e)
        {
            undoRedoManager.Undo();
            DataBase.Edited = true;
            UpdateUndoRedoButtons();
        }

        private void ButtonRedo_Click(object sender, RoutedEventArgs e)
        {
            undoRedoManager.Redo();
            DataBase.Edited = true;
            UpdateUndoRedoButtons();
        }

        private void UpdateUndoRedoButtons()
        {
            ButtonUndo.IsEnabled = undoRedoManager.CanUndo();
            ButtonRedo.IsEnabled = undoRedoManager.CanRedo();

            //ButtonUndo.ToolTipText = Resources.ObjViewPanelUndoDescription + @" """ + undoRedoManager.GetUndoActionName() + @"""";
            //ButtonRedo.ToolTipText = Resources.ObjViewPanelRedoDescription + @" """ + undoRedoManager.GetRedoActionName() + @"""";
        }

        private void ButtonNewFolder_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem selectedItem = (TreeViewItem)TreeView1.SelectedItem;

            long lastFreeFolderNum = (long)DateTime.Now.TimeOfDay.TotalMilliseconds;
            undoRedoManager.Execute(new CreateFolderCommand(TreeView1, selectedItem, lastFreeFolderNum));
            UpdateUndoRedoButtons();
            DataBase.Edited = true;
        }

        private void ButtonNewObject_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem selectedItem = (TreeViewItem)TreeView1.SelectedItem;

            long lastFreeFolderNum = (long)DateTime.Now.TimeOfDay.TotalMilliseconds;
            KVToken obj = new KVToken()
            {
                Key = String.Concat("New_object_", lastFreeFolderNum),
                Type = KVTokenType.KVblock,
                Parent = ObjectKV,
            };
            undoRedoManager.Execute(new CreateObjectCommand(TreeView1, selectedItem, obj, lastFreeFolderNum, ObjectsType));
            UpdateUndoRedoButtons();
            DataBase.Edited = true;
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem selectedItem = (TreeViewItem)TreeView1.SelectedItem;

            if (selectedItem == null)
                return;

            undoRedoManager.Execute(new DeleteCommand(TreeView1, selectedItem, ObjectKV, ObjectsType));

            UpdateUndoRedoButtons();
            DataBase.Edited = true;
        }

        private void ButtonRename_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem selectedItem = (TreeViewItem)TreeView1.SelectedItem;

            if (selectedItem == null) return;
            var oldName = (string) selectedItem.Header;
            var editingDialog = new RenameDialog();
            editingDialog.Owner = AllPanels.ObjectEditorForm;
            editingDialog.Visibility = Visibility.Visible;

            List<string> lockNamesList = new List<string>();
            if (!selectedItem.IsFolder())
            {
                lockNamesList.AddRange(DataBase.Abilities.Children.Select(kv => kv.Key));
                lockNamesList.AddRange(DataBase.Heroes.Children.Select(kv => kv.Key));
                lockNamesList.AddRange(DataBase.Units.Children.Select(kv => kv.Key));
                lockNamesList.AddRange(DataBase.Items.Children.Select(kv => kv.Key));
            }

            string newName = editingDialog.ShowDialog(oldName, lockNamesList.ToArray());
            if (newName == oldName)
                return;

            undoRedoManager.Execute(new RenameCommand(TreeView1, selectedItem, ObjectKV, newName, ObjectsType));

            DataBase.Edited = true;
        }

        private void ButtonEditSystemComments_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem selectedItem = (TreeViewItem)TreeView1.SelectedItem;

            if (selectedItem == null) return;
            if (selectedItem.IsFolder()) return;

            var kvItem = ObjectKV.GetChild((string) selectedItem.Header);
            var sysComment = SystemCommentEditorDialog.ShowDialog(kvItem.SystemComment ?? new SystemComment());
            if (sysComment == null) return;

            undoRedoManager.Execute(new EditSystemComments(kvItem, sysComment, ObjectKV));
        }

        private void ButtonShowHideFindPanel_Click(object sender, RoutedEventArgs e)
        {
            if (GridFindPanel.Visibility == Visibility.Collapsed)
            {
                GridFindPanel.Visibility = Visibility.Visible;
                ButtonShowHideFindPanel.Content = this.FindResource("FindHide");
            }
            else
            {
                GridFindPanel.Visibility = Visibility.Collapsed;
                ButtonShowHideFindPanel.Content = this.FindResource("FindShow");
            }
        }

        private int finderGlobalNumber = 0; //todo move

        private void ButtonFindNext_Click(object sender, RoutedEventArgs e)
        {
            var text = TextBoxFind.Text;
            int i = finderGlobalNumber;

            if (string.IsNullOrWhiteSpace(text))
                return;

            var item = TreeView1.Items.RecursiveFindItem(text, ref i, false, false);
            if (item == null)
            {
                if (finderGlobalNumber != 0)
                {
                    finderGlobalNumber = 0;
                    ButtonFindNext_Click(null, null);
                    return;
                }
                MessageBox.Show("Didnt finded item");//todo move resources
            }
            else
            {
                finderGlobalNumber++;
                item.ExpandParentsItems();
                item.IsSelected = true;
                item.Focus();
            }
        }

        private void TextBoxFind_TextChanged(object sender, TextChangedEventArgs e)
        {
            finderGlobalNumber = 0;
        }

        private void TextBoxFind_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                ButtonFindNext_Click(null, null);
        }

        #endregion


        #region Undo/Redo

        private class CreateFolderCommand : ICommand
        {
            public string Name => Properties.Resources.CreateFolder;
            private TreeView treeView;
            private TreeViewItem selectedItem;
            private TreeViewItem createdFolder;
            private long freeNumber;

            public CreateFolderCommand(TreeView treeView, TreeViewItem selectedItem, long freeNumber)
            {
                this.treeView = treeView;
                this.selectedItem = selectedItem;
                this.freeNumber = freeNumber;
            }

            public void Execute()
            {
                TreeViewItem fItem = treeView.Items.FindItemLike(selectedItem);

                createdFolder = new TreeViewItem() { Header = String.Concat(Properties.Resources.NewFolder, freeNumber), Tag = String.Concat("#", freeNumber) };
                if (fItem != null && fItem.IsFolder())
                {
                    fItem.Items.Add(createdFolder);
                    fItem.IsExpanded = true;
                    fItem.Items.Sort(false);
                }
                else
                {
                    treeView.Items.Add(createdFolder);
                    treeView.Items.Sort(false);
                }
            }

            public void UnExecute()
            {
                TreeViewItem fItem = treeView.Items.FindItemLike(createdFolder);

                fItem.GetParentItemCollection().Remove(fItem);
            }
        }

        private class CreateObjectCommand : ICommand
        {
            public string Name => Properties.Resources.CreateObject;
            private TreeView treeView;
            private TreeViewItem selectedItem;
            private TreeViewItem createdObject;
            private KVToken obj;
            private long freeNumber;
            private readonly ObjectTypePanel objectType;

            public CreateObjectCommand(TreeView treeView, TreeViewItem selectedItem, KVToken obj, long freeNumber, ObjectTypePanel objectType)
            {
                this.treeView = treeView;
                this.selectedItem = selectedItem;
                this.obj = obj;
                this.freeNumber = freeNumber;
                this.objectType = objectType;
            }

            public void Execute()
            {
                var fItem = treeView.Items.FindItemLike(selectedItem);

                obj.Parent.Children.Add(obj);
                if (obj.SystemComment == null)
                    obj.SystemComment = new SystemComment();
                obj.SystemComment.DeleteKV("Folder");

                createdObject = new TreeViewItem() { Header = obj.Key, Tag = freeNumber };
                if (fItem != null && fItem.IsFolder())
                {
                    fItem.Items.Add(createdObject);
                    fItem.IsExpanded = true;
                    fItem.Items.Sort(false);
                    var path = fItem.GetItemPath();
                    obj.SystemComment.AddKV("Folder", path);
                }
                else
                {
                    treeView.Items.Add(createdObject);
                    treeView.Items.Sort(false);
                }
            }

            public void UnExecute()
            {
                var fItem = treeView.Items.FindItemLike(selectedItem);

                obj.Parent.Children.Remove(obj);
                fItem.GetParentItemCollection().Remove(fItem);

                var editor = AllPanels.FindAnyEditorPanel(obj.Key, objectType)?.Content;
                if (editor != null)
                    ((IEditor)editor).ForceClose();
            }
        }

        private class RenameCommand : ICommand
        {
            public string Name => Properties.Resources.RenameFolder;
            private readonly TreeView treeView;
            private readonly TreeViewItem selectedItem;
            private readonly KVToken objectKV;
            private readonly string newText;
            private readonly string oldText;
            private readonly ObjectTypePanel objectType;

            public RenameCommand(TreeView tree, TreeViewItem selectedItem, KVToken objectKV, string newText, ObjectTypePanel objectType)
            {
                this.treeView = tree;
                this.selectedItem = selectedItem;
                this.objectKV = objectKV;
                this.newText = newText;
                this.oldText = (string)selectedItem.Header;
                this.objectType = objectType;
            }

            public void Execute()
            {
                Rename(oldText, newText);
            }

            public void UnExecute()
            {
                Rename(newText, oldText);
            }

            private void Rename(string oldText, string newText)
            {
                var fItem = treeView.Items.FindItemLike(selectedItem);

                fItem.Header = newText;
                if (fItem.IsFolder())
                {
                    fItem.RenameChildsFolders(objectKV, fItem.GetItemPath());
                }
                else
                {
                    var editor = AllPanels.FindAnyEditorPanel(oldText, objectType)?.Content;
                    if (editor != null)
                        ((IEditor)editor).PanelName = newText;
                    var obj = objectKV.GetChild(oldText);
                    obj.Key = newText;
                }

                fItem.GetParentItemCollection().Sort(false);
            }
        }

        private class DeleteCommand : ICommand
        {
            public string Name => Properties.Resources.DeleteFolder;
            private readonly TreeView treeView;
            private readonly TreeViewItem selectedItem;
            private readonly KVToken objectKV;
            private List<KVToken> deletedObjects;
            private ObjectTypePanel objectType;
            private TreeViewItem parentItem;
            private TreeViewItem deletedItem;

            public DeleteCommand(TreeView treeView, TreeViewItem selectedItem, KVToken objectKV, ObjectTypePanel objectType)
            {
                this.treeView = treeView;
                this.selectedItem = selectedItem;
                this.objectKV = objectKV;
                deletedObjects = new List<KVToken>();
                this.objectType = objectType;
            }

            public void Execute()
            {
                var fItem = treeView.Items.FindItemLike(selectedItem);
                deletedItem = fItem;
                parentItem = fItem.Parent as TreeViewItem;

                if (fItem.IsFolder())
                {
                    deletedObjects.Clear();
                    fItem.DeleteChilds(objectKV, deletedObjects);

                    foreach (var obj in deletedObjects)
                    {
                        var editor = AllPanels.FindAnyEditorPanel(obj.Key, objectType)?.Content;
                        if (editor != null)
                            ((IEditor)editor).ForceClose();

                    }
                }
                else
                {
                    var key = (string) fItem.Header;

                    deletedObjects.Add(objectKV.GetChild(key));
                    objectKV.RemoveChild(key);

                    var editor = AllPanels.FindAnyEditorPanel(key, objectType)?.Content;
                    if (editor != null)
                        ((IEditor)editor).ForceClose();
                }
                fItem.GetParentItemCollection().Remove(fItem);
            }

            public void UnExecute()
            {
                var pItem = treeView.Items.FindItemLike(parentItem);
                if (pItem == null)
                    treeView.Items.Add(deletedItem);
                else
                    pItem.Items.Add(deletedItem);

                foreach (var obj in deletedObjects)
                {
                    objectKV.Children.Add(obj);
                }
                deletedItem.GetParentItemCollection().Sort(false);
            }
        }

        private class MoveCommand : ICommand
        {
            public string Name => Properties.Resources.MoveFolderObject;
            private readonly TreeView treeView;
            private readonly KVToken objectKV;
            private TreeViewItem movingItem;
            private TreeViewItem targetItem;
            private TreeViewItem sourceItem;

            public MoveCommand(TreeView treeView, KVToken objectKV, TreeViewItem movingItem, TreeViewItem targetItem)
            {
                this.treeView = treeView;
                this.movingItem = movingItem;
                this.targetItem = targetItem;
                this.objectKV = objectKV;
            }

            public void Execute()
            {
                var mItem = treeView.Items.FindItemLike(movingItem);
                var tItem = treeView.Items.FindItemLike(targetItem);

                sourceItem = mItem.Parent as TreeViewItem;

                Move(mItem, tItem);
            }

            public void UnExecute()
            {
                var mItem = treeView.Items.FindItemLike(movingItem);
                var sItem = treeView.Items.FindItemLike(sourceItem);

                Move(mItem, sItem);
            }

            private void Move(TreeViewItem mItem, TreeViewItem tItem)
            {
                var mObj = objectKV.GetChild((string)mItem.Header);
                if (mObj != null && mObj.SystemComment == null)
                    mObj.SystemComment = new SystemComment();
                mObj?.SystemComment.DeleteKV("Folder");
                mItem.GetParentItemCollection().Remove(mItem);

                if (tItem == null)
                    treeView.Items.Add(mItem);
                else if (tItem.IsFolder())
                {
                    tItem.Items.Add(mItem);
                    mObj?.SystemComment.AddKV("Folder", tItem.GetItemPath());
                }
                else
                {
                    tItem.GetParentItemCollection().Add(mItem);
                    if (tItem.Parent is TreeViewItem)
                    {
                        mObj?.SystemComment.AddKV("Folder", ((TreeViewItem)tItem.Parent).GetItemPath());
                    }
                    
                }
                if (mItem.IsFolder())
                    mItem.RenameChildsFolders(objectKV, mItem.GetItemPath());

                if (tItem != null)
                    tItem.IsExpanded = true;
                mItem.GetParentItemCollection().Sort(false);
            }
        }

        private class EditSystemComments : ICommand
        {
            public string Name => "Edited system comments";
            private KVToken kvItem;
            private SystemComment lastSysComment;
            private SystemComment sysComment;
            private KVToken objectKV;

            public EditSystemComments(KVToken kvItem, SystemComment sysComment, KVToken objectKV)
            {
                this.kvItem = kvItem;
                lastSysComment = kvItem.SystemComment;
                this.sysComment = sysComment;
                this.objectKV = objectKV;
            }

            public void Execute()
            {
                kvItem = objectKV.GetChild(kvItem.Key);
                
                kvItem.SystemComment = sysComment;
            }

            public void UnExecute()
            {
                kvItem = objectKV.GetChild(kvItem.Key);

                kvItem.SystemComment = lastSysComment;
            }
        }



        #endregion

        
    }

    public static class MyTreeViewHelper
    {
        //
        // The TreeViewItem that the mouse is currently directly over (or null).
        //
        public static TreeViewItem _currentItem = null;

        //
        // IsMouseDirectlyOverItem:  A DependencyProperty that will be true only on the 
        // TreeViewItem that the mouse is directly over.  I.e., this won't be set on that 
        // parent item.
        //
        // This is the only public member, and is read-only.
        //

        // The property key (since this is a read-only DP)
        private static readonly DependencyPropertyKey IsMouseDirectlyOverItemKey =
            DependencyProperty.RegisterAttachedReadOnly("IsMouseDirectlyOverItem",
                                                typeof(bool),
                                                typeof(MyTreeViewHelper),
                                                new FrameworkPropertyMetadata(null, new CoerceValueCallback(CalculateIsMouseDirectlyOverItem)));

        // The DP itself
        public static readonly DependencyProperty IsMouseDirectlyOverItemProperty =
            IsMouseDirectlyOverItemKey.DependencyProperty;

        // A strongly-typed getter for the property.
        public static bool GetIsMouseDirectlyOverItem(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsMouseDirectlyOverItemProperty);
        }

        // A coercion method for the property
        private static object CalculateIsMouseDirectlyOverItem(DependencyObject item, object value)
        {
            // This method is called when the IsMouseDirectlyOver property is being calculated
            // for a TreeViewItem.  

            if (item == _currentItem)
                return true;
            else
                return false;
        }

        //
        // UpdateOverItem:  A private RoutedEvent used to find the nearest encapsulating
        // TreeViewItem to the mouse's current position.
        //

        private static readonly RoutedEvent UpdateOverItemEvent = EventManager.RegisterRoutedEvent(
            "UpdateOverItem", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MyTreeViewHelper));

        //
        // Class constructor
        //

        static MyTreeViewHelper()
        {
            // Get all Mouse enter/leave events for TreeViewItem.
            EventManager.RegisterClassHandler(typeof(TreeViewItem), TreeViewItem.MouseEnterEvent, new MouseEventHandler(OnMouseTransition), true);
            EventManager.RegisterClassHandler(typeof(TreeViewItem), TreeViewItem.MouseLeaveEvent, new MouseEventHandler(OnMouseTransition), true);

            // Listen for the UpdateOverItemEvent on all TreeViewItem's.
            EventManager.RegisterClassHandler(typeof(TreeViewItem), UpdateOverItemEvent, new RoutedEventHandler(OnUpdateOverItem));
        }


        //
        // OnUpdateOverItem:  This method is a listener for the UpdateOverItemEvent.  When it is received,
        // it means that the sender is the closest TreeViewItem to the mouse (closest in the sense of the tree,
        // not geographically).

        static void OnUpdateOverItem(object sender, RoutedEventArgs args)
        {
            // Mark this object as the tree view item over which the mouse
            // is currently positioned.
            _currentItem = sender as TreeViewItem;

            // Tell that item to re-calculate the IsMouseDirectlyOverItem property
            _currentItem.InvalidateProperty(IsMouseDirectlyOverItemProperty);

            // Prevent this event from notifying other tree view items higher in the tree.
            args.Handled = true;
        }

        //
        // OnMouseTransition:  This method is a listener for both the MouseEnter event and
        // the MouseLeave event on TreeViewItems.  It updates the _currentItem, and updates
        // the IsMouseDirectlyOverItem property on the previous TreeViewItem and the new
        // TreeViewItem.

        static void OnMouseTransition(object sender, MouseEventArgs args)
        {
            lock (IsMouseDirectlyOverItemProperty)
            {
                if (_currentItem != null)
                {
                    // Tell the item that previously had the mouse that it no longer does.
                    DependencyObject oldItem = _currentItem;
                    _currentItem = null;
                    oldItem.InvalidateProperty(IsMouseDirectlyOverItemProperty);
                }

                // Get the element that is currently under the mouse.
                IInputElement currentPosition = Mouse.DirectlyOver;

                // See if the mouse is still over something (any element, not just a tree view item).
                if (currentPosition != null)
                {
                    // Yes, the mouse is over something.
                    // Raise an event from that point.  If a TreeViewItem is anywhere above this point
                    // in the tree, it will receive this event and update _currentItem.

                    RoutedEventArgs newItemArgs = new RoutedEventArgs(UpdateOverItemEvent);
                    currentPosition.RaiseEvent(newItemArgs);

                }
            }
        }


    }
}
