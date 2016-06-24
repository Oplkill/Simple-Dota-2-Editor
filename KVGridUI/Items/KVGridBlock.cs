using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace KVGridUI
{
    public partial class KVGridBlock : KVGridItemAbstract, KVGridItemInterface
    {
        public KVGridBlock(int id = -1)
        {
            this.Id = id;
            kvItems = new List<KVGridItemInterface>();
            ItemType = ItemTypes.Block;

            InitializeComponent();

            kvsfiTextBoxKey.OnActivateClick += select_Click;
            kvsfiTextBoxKey.OnTextChanged += KeyTextChanged;
        }

        private void KeyTextChanged(string oldText, string newText)
        {
            OnTextChanged?.Invoke(this, oldText, newText, KVType.Key);
        }

        /// <summary>
        /// Hides Plus/Minus and Main key controls
        /// </summary>
        public void HideKVBlockControls(bool hide)
        {
            splitContainer1.Panel1Collapsed = hide;
            splitContainer2.Panel1Collapsed = hide;
        }

        public KVGridItemInterface AddItem(KVGrid owner, KVGridItemInterface item, int position = -1)
        {
            if (item == null) return null;

            if (item.Id == -1)
                item.Id = owner.GetMyId();
            item.ParentBlock = this;
            item.GridOwner = owner;
            if (position == -1)
                kvItems.Add(item);
            else 
                kvItems.Insert(position, item);
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

            return item;
        }

        public void RemoveItem(KVGridItemInterface item, bool disposeItem)
        {
            if (item == null) return;

            if (GridOwner.SelectedItem == item)
                GridOwner.SelectedItem = null;

            Items.Remove(item);
            splitContainer2.Panel2.Controls.Remove((UserControl)item);
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

        public KVGridItemInterface FindItemId(int id)
        {
            if (this.Id == id) return this;

            foreach (var item in Items)
            {
                if (item.Id == id) return item;

                if (item is KVGridBlock)
                {
                    var itemTemp = ((KVGridBlock) item).FindItemId(id);
                    if (itemTemp != null)
                        return itemTemp;
                }
            }

            return null;
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

            if (!collapsed)
                foreach (var item in kvItems)
                {
                    (item as KVGridBlock)?.UpdateItemPositions();

                    var ctrl = (UserControl)item;
                    ctrl.Location = new Point(ctrl.Location.X, y);
                    y += item.UpdateHeight();
                }

            this.Height = ItemHeight = splitContainer2.Panel1.Height + splitContainer2.SplitterWidth + splitContainer2.SplitterIncrement + y;
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

            //ParentBlock?.UpdateItemPositions();
            GridOwner.UpdateItemPositions();
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

        public int Id { get; set; }

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
