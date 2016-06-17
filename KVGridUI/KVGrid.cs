using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace KVGridUI
{
    public partial class KVGrid : UserControl
    {
        public KVGrid()
        {
            InitializeComponent();

            kvGridBlock1.HideKVBlockControls(true);
            kvGridBlock1.GridOwner = this;
        }

        public List<KVGridItemInterface> Items => kvGridBlock1.Items;

        public KVGridBlock MainBlock => kvGridBlock1;

        public void AddItem(ItemTypes type, string key, string value)
        {
            Control item = null;
            switch (type)
            {
                case ItemTypes.TextText:
                    item = new KVGridItem_TextText() {KeyText = key, ValueText = value};
                    break;

                case ItemTypes.Block:
                    item = new KVGridBlock() {KeyText = key};
                    break;

                default:
                    return;
            }
            kvGridBlock1.AddItem(this, (KVGridItemInterface)item, true);
            this.Height = kvGridBlock1.Height;
        }

        /// <summary>
        /// Swapping items
        /// </summary>
        public void SwapItems(KVGridItemInterface item1, KVGridItemInterface item2)
        {
            if(item1.ParentBlock == item2.ParentBlock)
            { item1.ParentBlock.SwapItems(item1, item2); return; }

            //todo
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

        public void UpdateItemPositions()
        {
            kvGridBlock1.UpdateItemPositions();
        }

        public void Size_Changes()
        {
            kvGridBlock1.Dock = DockStyle.None;
            kvGridBlock1.ItemWidth = this.Width;
            kvGridBlock1.Size = ClientSize;
        }

        public void KVGrid_SizeChanged(object sender, EventArgs e)
        {
            //kvGridBlock1.Dock = DockStyle.None;
            //kvGridBlock1.ItemWidth = this.Width;
            //kvGridBlock1.Size = ClientSize;
        }

        private KVGridItemInterface selectedItem;
    }
}
