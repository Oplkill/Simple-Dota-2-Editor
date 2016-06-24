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
                    tempBlock.KeyText = kvToken.Key;
                    tempBlock.OnTextChanged += SomeItemTextChanged;
                    block.AddItem(kvGrid, tempBlock, -1, false);
                    loadItems(tempBlock, kvToken.Children);
                }
                else if(kvToken.Type == KVTokenType.KVsimple)
                {
                    var kv = new KVGridItem_TextText();
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
            public string Name => "Create KV item"; //todo вынести в ресурсы
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
            public string Name => "Create KV block"; //todo вынести в ресурсы
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
                "Move up" : 
                "Move down"; //todo вынести в ресурсы

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
            public string Name => "Delete"; //todo вынести в ресурсы
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
            public string Name => "Text changed"; //todo move to resource
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
    }
}
