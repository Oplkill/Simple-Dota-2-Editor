using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace KVGridUI
{
    public partial class KVGridBlock : UserControl, KVGridItemInterface
    {
        public KVGridBlock()
        {
            kvItems = new List<KVGridItemInterface>();
            ItemType = ItemTypes.Block;

            InitializeComponent();

            kvsfiTextBoxKey.OnActivateClick += select_Click;
        }

        /// <summary>
        /// Hides Plus/Minus and Main key controls
        /// </summary>
        public void HideKVBlockControls(bool hide)
        {
            splitContainer1.Panel1Collapsed = hide;
            splitContainer2.Panel1Collapsed = hide;
        }

        public void AddItem(KVGrid owner, KVGridItemInterface item)
        {
            if (item == null) return;

            item.ParentBlock = this;
            item.GridOwner = owner;
            kvItems.Add(item);
            var ctrl = ((UserControl)item);

            splitContainer2.Panel2.Controls.Add(ctrl);
            item.ItemWidth = this.Width;
            int y = kvItems.Cast<UserControl>().Sum(ctrlItem => ctrlItem.Size.Height);
            ctrl.Location = new Point(0, y);

            if (item is KVGridBlock)
            {
                (item as KVGridBlock).UpdateItemPositions();
                (item).Selected = false;
            }
            UpdateItemPositions();
            GridOwner.UpdateItemPositions();
        }

        public void RemoveItem(KVGridItemInterface item, bool disposeItem)
        {
            if (item == null) return;

            if (GridOwner.SelectedItem == item)
                GridOwner.SelectedItem = null;

            Items.Remove(item);
            if (disposeItem)
                ((UserControl)item).Dispose();
            GridOwner.UpdateItemPositions();
        }

        public void MoveItemTo(KVGridItemInterface item, int index)
        {
            if (item.ParentBlock != this) return;

            bool selectedItem = GridOwner.SelectedItem == item;
            RemoveItem(item, false);
            if (index >= Items.Count)
                Items.Add(item);
            else
                Items.Insert(index, item);

            if (selectedItem)
                GridOwner.SelectedItem = item;

            GridOwner.UpdateItemPositions();
        }

        public KVGridItemInterface FindItem(string key, string value)
        {
            return Items.FirstOrDefault(item => item.KeyText == key && item.ValueText == value);
        }

        /// <summary>
        /// Swapping items only if they both in this block
        /// </summary>
        public void SwapItems(KVGridItemInterface item1, KVGridItemInterface item2)
        {
            if (!Items.Contains(item1) || !Items.Contains(item2)) return;

            int index1 = Items.IndexOf(item1);
            int index2 = Items.IndexOf(item2);

            Items[index1] = item2;
            Items[index2] = item1;

            this.UpdateItemPositions();
        }

        public int UpdateHeight()
        {
            ItemHeight = splitContainer2.Panel1.Height + splitContainer2.SplitterWidth + splitContainer2.SplitterIncrement;

            if (collapsed)
                return ItemHeight;

            foreach (var item in kvItems)
            {
                ItemHeight += item.UpdateHeight();
            }

            return ItemHeight;
        }

        public void UpdateItemPositions()
        {
            int y = 0;

            foreach (var item in kvItems)
            {
                (item as KVGridBlock)?.UpdateItemPositions();

                var ctrl = (UserControl)item;
                ctrl.Location = new Point(ctrl.Location.X, y);
                y += item.UpdateHeight();
            }

            this.Height = UpdateHeight();
        }

        private void buttonCollapse_Click(object sender, EventArgs e)
        {
            select_Click(sender, e);

            if (collapsed)
            {
                collapsed = false;
                this.Height = UpdateHeight();
                buttonCollapse.Text = @"-";
            }
            else
            {
                collapsed = true;
                this.Height = splitContainer2.Panel1.Height;
                buttonCollapse.Text = @"+";
            }

            ParentBlock?.UpdateItemPositions();
        }

        private void select_Click(object sender, EventArgs e)
        {
            GridOwner.SelectedItem = this;
        }


        #region variables


        public int ItemWidth
        {
            get
            {
                return this.Width;
            }
            set
            {
                this.Width = value;

                foreach (var item in Items)
                {
                    item.ItemWidth = value;
                }
            }
        }

        public bool Selected
        {
            get { return splitContainer1.BackColor != Control.DefaultBackColor; }
            set
            {
                if (this.Selected == value) return;

                if (value)
                {
                    splitContainer1.BackColor = Color.DarkBlue;
                    //GridOwner.SelectedItem = this;
                }
                else
                {
                    splitContainer1.BackColor = Control.DefaultBackColor;
                    //GridOwner.SelectedItem = null;
                }
            }
        }

        public int ItemHeight { get; private set; }
        public KVGridBlock ParentBlock { get; set; }
        public ItemTypes ItemType { get; private set; }
        /// <summary>
        /// KVgrid owner
        /// </summary>
        public KVGrid GridOwner { get; set; }

        public string KeyText
        {
            get { return kvsfiTextBoxKey.Text; }
            set { kvsfiTextBoxKey.Text = value; }
        }
        public string ValueText {get { return null; } set {} }
        public List<KVGridItemInterface> Items => kvItems;

        private List<KVGridItemInterface> kvItems;
        private bool collapsed;

        #endregion

        
    }
}
