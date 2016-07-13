using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KVGridUI;
using KV_reloaded;
using SimpleDota2Editor.Properties;
using WeifenLuo.WinFormsUI.Docking;

namespace SimpleDota2Editor.Panels
{
    public partial class GuiEditorPanel : DockContent
    {
        public string PanelName
        {
            set { panelName = value; this.Text = value + (modified ? @" *" : ""); }
            get { return panelName; }
        }

        private string panelName;
        private bool modified;

        public KVToken ObjectRef;
        private KVGrid kvGrid;
        public ObjectsViewPanel.ObjectTypePanel ObjectType;

        private UndoRedoManager undoRedoManager;

        public GuiEditorPanel()
        {
            InitializeComponent();
            kvGrid = new KVGrid();
            this.kvGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kvGrid.Location = new System.Drawing.Point(0, 0);
            this.kvGrid.Name = "kvGrid";
            this.kvGrid.Size = new System.Drawing.Size(ClientSize.Width, ClientSize.Height);
            this.kvGrid.TabIndex = 0;
            this.kvGrid.Text = "";
            this.Controls.Add(kvGrid);

            resizeKvGridTimer = new TimerForm(ResizeTimerCallback);
            undoRedoManager = new UndoRedoManager();
        }

        /// <summary>
        /// Закрыть без сохранения и проверки
        /// </summary>
        public void ForceClose()
        {
            forceClose = true;
            this.Close();
        }

        private bool forceClose;

        public void SaveChanges()
        {
            if (!modified)
                return;

            ObjectRef.Children = GetKVTokens(kvGrid.MainBlock);
            modified = false;
        }

        #region ConverterToKVToken


        private List<KVToken> GetKVTokens(KVGridBlock block)
        {
            var list = new List<KVToken>();

            foreach (var item in block.Items)
            {
                var token = new KVToken();

                if (item is KVGridBlock)
                {
                    token.Type = KVTokenType.KVblock;
                    token.Key = item.KeyText;
                    token.comments = ((KVGridBlock) item).comments;
                    token.Children = GetKVTokens(item as KVGridBlock);
                }
                else if (item is KVGridItem_TextText)
                {
                    token.Type = KVTokenType.KVsimple;
                    token.Key = item.KeyText;
                    token.Value = item.ValueText;
                    token.comments = ((KVGridItem_TextText) item).comments;
                }

                list.Add(token);
            }

            return list;
        } 



#endregion

        private void GuiEditorPanel_FormClosing(object sender, FormClosingEventArgs e)
        {
            AllPanels.Form1.ShowEditorMenu(Form1.EditorType.None);
            undoRedoManager.ClearAll();

            if (forceClose)
                return;

            SaveChanges();
            //e.Cancel = true;
            //this.Hide();
        }

        public void InitGuiAndLoad()
        {
            loadItems(kvGrid.MainBlock, ObjectRef.Children);
        }

        private void loadItems(KVGridBlock block, List<KVToken> tokens)
        {
            if (tokens == null) return;

            foreach (var kvToken in tokens)
            {
                if (kvToken.Type == KVTokenType.KVblock)
                {
                    var tempBlock = new KVGridBlock();
                    tempBlock.comments = kvToken.comments;
                    tempBlock.KeyText = kvToken.Key;
                    tempBlock.OnTextChanged += SomeItemTextChanged;
                    block.AddItem(kvGrid, tempBlock, -1, false);
                    loadItems(tempBlock, kvToken.Children);
                }
                else if(kvToken.Type == KVTokenType.KVsimple)
                {
                    var kv = new KVGridItem_TextText();
                    kv.comments = kvToken.comments;
                    kv.KeyText = kvToken.Key;
                    kv.ValueText = kvToken.Value;
                    kv.OnTextChanged += SomeItemTextChanged;
                    block.AddItem(kvGrid, kv, -1, false);
                }
            }

            kvGrid.UpdateItemPositions();
        }

        #region SubMenu



        public void RedoButton_Click()
        {
            undoRedoManager.Redo();
            UpdateMenuButtons();
        }

        public void UndoButton_Click()
        {
            undoRedoManager.Undo();
            UpdateMenuButtons();
        }

        public void CreateKVButton_Click()
        {
            KVGridBlock block;
            if (kvGrid.SelectedItem == null)
                block = kvGrid.MainBlock;
            else
            {
                if (kvGrid.SelectedItem is KVGridBlock)
                    block = (KVGridBlock) kvGrid.SelectedItem;
                else
                    block = kvGrid.SelectedItem.ParentBlock;
            }

            undoRedoManager.Execute(new CreateKV(kvGrid, block, SomeItemTextChanged));
            UpdateMenuButtons();
        }

        public void CreateKVBlockButton_Click()
        {
            KVGridBlock block;
            if (kvGrid.SelectedItem == null)
                block = kvGrid.MainBlock;
            else
            {
                if (kvGrid.SelectedItem is KVGridBlock)
                    block = (KVGridBlock)kvGrid.SelectedItem;
                else
                    block = kvGrid.SelectedItem.ParentBlock;
            }

            undoRedoManager.Execute(new CreateKVBlock(kvGrid, block, SomeItemTextChanged));
            UpdateMenuButtons();
        }

        public void MoveUpButton_Click()
        {
            if (kvGrid.SelectedItem == null) return;

            undoRedoManager.Execute(new MoveUpDown(kvGrid, true, kvGrid.SelectedItem));
            UpdateMenuButtons();
        }

        public void MoveDownButton_Click()
        {
            if (kvGrid.SelectedItem == null) return;

            undoRedoManager.Execute(new MoveUpDown(kvGrid, false, kvGrid.SelectedItem));
            UpdateMenuButtons();
        }

        public void DeleteButton_Click()
        {
            if (kvGrid.SelectedItem == null) return;

            undoRedoManager.Execute(new Delete(kvGrid, kvGrid.SelectedItem));
            UpdateMenuButtons();
        }

        private void UpdateMenuButtons()
        {
            AllPanels.Form1.toolStripButtonGuiUndo.Enabled = modified = undoRedoManager.CanUndo();
            AllPanels.Form1.toolStripButtonGuiRedo.Enabled = undoRedoManager.CanRedo();

            PanelName = panelName;

            AllPanels.Form1.toolStripButtonGuiUndo.ToolTipText = 
                Resources.ObjViewPanelUndoDescription + @" """ + undoRedoManager.GetUndoActionName() + @"""";
            AllPanels.Form1.toolStripButtonGuiRedo.ToolTipText = 
                Resources.ObjViewPanelRedoDescription + @" """ + undoRedoManager.GetRedoActionName() + @"""";
        }


        #endregion


        private void SomeItemTextChanged(KVGridItemInterface item, string oldText, string newText, KVType type)
        {
            undoRedoManager.Execute(new SomeTextChanged(kvGrid, item, oldText, newText, type));
            UpdateMenuButtons();
        }


        #region UndoRedo


        private class CreateKV : ICommand
        {
            public string Name => Resources.CreateGUIKVItem;
            private KVGrid kvGrid;
            private KVGridBlock block;
            private KVGridItemInterface createdItem;
            private KVGridItemAbstract.TextChangedFunc textChangedFunc;

            public CreateKV(KVGrid kvGrid, KVGridBlock block, KVGridItemAbstract.TextChangedFunc textChangedFunc)
            {
                this.kvGrid = kvGrid;
                this.block = block;
                this.textChangedFunc = textChangedFunc;
            }

            public void Execute()
            {
                block = kvGrid.GetItemById(block.Id) as KVGridBlock;
                createdItem = new KVGridItem_TextText();
                ((KVGridItemAbstract)createdItem).OnTextChanged += textChangedFunc;
                block.AddItem(kvGrid, createdItem, -1, true);
            }

            public void UnExecute()
            {
                block = kvGrid.GetItemById(block.Id) as KVGridBlock;
                createdItem = kvGrid.GetItemById(createdItem.Id);

                block.RemoveItem(createdItem, false);
            }
        }

        private class CreateKVBlock : ICommand
        {
            public string Name => Resources.CreateGUIKVBlock;
            private KVGrid kvGrid;
            private KVGridBlock block;
            private KVGridItemInterface createdItem;
            private KVGridItemAbstract.TextChangedFunc textChangedFunc;

            public CreateKVBlock(KVGrid kvGrid, KVGridBlock block, KVGridItemAbstract.TextChangedFunc textChangedFunc)
            {
                this.kvGrid = kvGrid;
                this.block = block;
                this.textChangedFunc = textChangedFunc;
            }

            public void Execute()
            {
                block = kvGrid.GetItemById(block.Id) as KVGridBlock;
                createdItem = new KVGridBlock();
                ((KVGridItemAbstract)createdItem).OnTextChanged += textChangedFunc;
                block.AddItem(kvGrid, createdItem, -1, true);
            }

            public void UnExecute()
            {
                block = kvGrid.GetItemById(block.Id) as KVGridBlock;
                createdItem = kvGrid.GetItemById(createdItem.Id);

                block.RemoveItem(createdItem, false);
            }
        }

        private class MoveUpDown : ICommand
        {
            public string Name => (moveUp) ?
                Resources.MoveUpGUI :
                Resources.MoveDownGUI;

            private bool moveUp;
            private KVGrid kvGrid;
            private KVGridItemInterface movingItem;

            public MoveUpDown(KVGrid kvGrid, bool moveUp, KVGridItemInterface movingItem)
            {
                this.moveUp = moveUp;
                this.kvGrid = kvGrid;
                this.movingItem = movingItem;
            }

            public void Execute()
            {
                movingItem = kvGrid.GetItemById(movingItem.Id);

                if (moveUp) MoveUp();
                else MoveDown();
            }

            public void UnExecute()
            {
                movingItem = kvGrid.GetItemById(movingItem.Id);

                if (moveUp) MoveDown();
                else MoveUp();
            }

            private void MoveUp()
            {
                kvGrid.MoveItemUpThrough(movingItem);
            }

            private void MoveDown()
            {
                kvGrid.MoveItemDownThrough(movingItem);
            }
        }

        private class Delete : ICommand
        {
            public string Name => Resources.DeleteItemGUI;
            private KVGrid kvGrid;
            private KVGridItemInterface deletedItem;
            private int index;

            public Delete(KVGrid kvGrid, KVGridItemInterface deletedItem)
            {
                this.kvGrid = kvGrid;
                this.deletedItem = deletedItem;

                index = deletedItem.ParentBlock.Items.IndexOf(deletedItem);
            }

            public void Execute()
            {
                deletedItem = kvGrid.GetItemById(deletedItem.Id);

                deletedItem.ParentBlock.RemoveItem(deletedItem, false);
            }

            public void UnExecute()
            {
                deletedItem = deletedItem.ParentBlock.AddItem(kvGrid, deletedItem, index, true);
            }
        }

        private class SomeTextChanged : ICommand
        {
            public string Name => Resources.ItemTextChangedGUI;
            private KVGrid kvGrid;
            private KVGridItemInterface item;
            private readonly string oldText;
            private readonly string newText;
            private readonly KVType kvType;

            public SomeTextChanged(KVGrid kvGrid, KVGridItemInterface item, string oldText, string newText, KVType kvType)
            {
                this.kvGrid = kvGrid;
                this.item = item;
                this.oldText = oldText;
                this.newText = newText;
                this.kvType = kvType;
            }
            public void Execute()
            {
                item = kvGrid.GetItemById(item.Id);

                if (kvType == KVType.Key)
                    item.KeyText = newText;
                else
                    item.ValueText = newText;
            }

            public void UnExecute()
            {
                item = kvGrid.GetItemById(item.Id);

                if (kvType == KVType.Key)
                    item.KeyText = oldText;
                else
                    item.ValueText = oldText;
            }
        }

        #endregion


        private TimerForm resizeKvGridTimer;

        private void ResizeTimerCallback(object timer, EventArgs e)
        {
            kvGrid.Size = this.ClientSize;
            //kvGrid.KVGrid_SizeChanged(null, null);
            resizeKvGridTimer.Stop();
        }

        private void GuiEditorPanel_SizeChanged(object sender, EventArgs e)
        {
            resizeKvGridTimer.Stop();
            resizeKvGridTimer.Start(300);
        }

        private void GuiEditorPanel_Activated(object sender, EventArgs e)
        {
            AllPanels.LastActiveDocumentEditor = this;
            AllPanels.Form1.ShowEditorMenu(Form1.EditorType.Gui);
        }
    }
}
