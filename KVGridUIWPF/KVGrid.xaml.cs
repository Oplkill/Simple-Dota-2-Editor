using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using KVGridUIWPF.Items.KeyValueItems;

namespace KVGridUIWPF
{
    /// <summary>
    /// Логика взаимодействия для KVGrid.xaml
    /// </summary>
    public partial class KVGrid : UserControl
    {
        public delegate void TextChangedFunc(KVGridItemInterface item, string oldText, string newText, KVType type);
        public KVGrid()
        {
            InitializeComponent();

            kvGridBlock1.HideKVBlockControls(true);
            kvGridBlock1.GridOwner = this;
            currentUnicId = 0; //todo generic id random

            kvGridBlock1.Id = GetMyId();
        }

        public List<KVGridItemInterface> Items => kvGridBlock1.Items;

        public KVGridBlock MainBlock => kvGridBlock1;

        public void AddItem(ItemTypes type, string key, string value)
        {
            Control item = null;
            switch (type)
            {
                case ItemTypes.TextText:
                    item = new KVGridItem_TextText() { KeyText = key, ValueText = value };
                    break;

                case ItemTypes.Block:
                    item = new KVGridBlock() { KeyText = key };
                    break;

                default:
                    return;
            }
            kvGridBlock1.AddItem(this, (KVGridItemInterface)item, -1);
            this.Height = kvGridBlock1.Height;
        }

        /// <summary>
        /// Swapping items
        /// </summary>
        public void SwapItems(KVGridItemInterface item1, KVGridItemInterface item2)
        {
            if (item1.ParentBlock == item2.ParentBlock)
            { item1.ParentBlock.SwapItems(item1, item2); return; }

            //todo
        }

        /// <summary>
        /// Getting item which upper @item. If @item first in block, it will be return block which contain @item.
        /// Returns null if @item uppest of all items of this kvGrid.
        /// </summary>
        public KVGridItemInterface GetItemUpperThatItem(KVGridItemInterface item)
        {
            var block = item.ParentBlock;
            var index = block.Items.IndexOf(item);

            if (index == 0)
            {
                if (block == MainBlock)
                    return null;
                else
                    return block;
            }
            else
                return block.Items[index - 1];
        }

        /// <summary>
        /// Getting item which downer @item.
        /// returns null if @item downer of all items of this kvGrid.
        /// </summary>
        public KVGridItemInterface GetItemDownerThatItem(KVGridItemInterface item)
        {
            var block = item.ParentBlock;
            var index = block.Items.IndexOf(item);

            if (index == block.Items.Count - 1)
            {
                if (block == MainBlock)
                    return null;
                else
                    return GetItemDownerThatItem(block);
            }
            else
                return block.Items[index + 1];
        }

        public void MoveItemUpThrough(KVGridItemInterface item)
        {
            var block = item.ParentBlock;
            var index = block.Items.IndexOf(item);

            if (index == 0)
            {
                if (block == MainBlock)
                    return;
                block.RemoveItem(item, false);
                block.ParentBlock.AddItem(this, item, block.ParentBlock.Items.IndexOf(block));
            }
            else
                block.SwapItems(block.Items[index - 1], item);
        }

        public void MoveItemDownThrough(KVGridItemInterface item)
        {
            var block = item.ParentBlock;
            var index = block.Items.IndexOf(item);

            if (index == block.Items.Count - 1)
            {
                if (block == MainBlock)
                    return;
                block.RemoveItem(item, false);
                block.ParentBlock.AddItem(this, item, block.ParentBlock.Items.IndexOf(block) + 1);
            }
            else
                block.SwapItems(block.Items[index + 1], item);
        }

        public KVGridItemInterface SelectedItem
        {
            get { return selectedItem; }
            set
            {
                if (selectedItem == value) return;

                if (selectedItem != null) selectedItem.Selected = false;
                if (value == null)
                {
                    selectedItem = null;
                    return;
                }
                if (value.GridOwner != this) return;

                selectedItem = value;
                selectedItem.Selected = true;
            }
        }

        public KVGridItemInterface GetItemById(int id)
        {
            return MainBlock.FindItemId(id);
        }

        public int GetMyId()
        {
            return currentUnicId++;
        }

        private KVGridItemInterface selectedItem;
        private int currentUnicId;
    }
}
