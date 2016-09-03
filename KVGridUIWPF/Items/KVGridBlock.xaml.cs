using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace KVGridUIWPF
{
    /// <summary>
    /// Логика взаимодействия для KVGridBlock.xaml
    /// </summary>
    public partial class KVGridBlock : UserControl, KVGridItemInterface
    {
        public KVGridBlock()
        {
            InitConstructor(-1);
        }

        public KVGridBlock(int id = -1)
        {
            InitConstructor(id);
        }

        private void InitConstructor(int id)
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
            if (hide)
            {
                Grid1.ColumnDefinitions[0].Width = new GridLength(0);
                Grid1.RowDefinitions[0].Height = new GridLength(0);
            }
            else
            {
                Grid1.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                Grid1.RowDefinitions[0].Height = new GridLength(1, GridUnitType.Star);
            }
        }

        public KVGridItemInterface AddItem(KVGrid owner, KVGridItemInterface item, int position)
        {
            if (item == null) return null;

            if (item.Id == -1)
                item.Id = owner.GetMyId();
            item.ParentBlock = this;
            item.GridOwner = owner;
            if (position == -1)
            {
                kvItems.Add(item);
                position = kvItems.Count - 1;
                Grid2.RowDefinitions.Add(new RowDefinition());
            }
            else
            {
                kvItems.Insert(position, item);
                Grid2.RowDefinitions.Add(new RowDefinition());
                for (int i = position; i < Grid2.Children.Count; i++)
                {
                    Grid2.Children[i].SetValue(Grid.RowProperty, i + 1);
                    //Grid2.RowDefinitions[i].Height = GridLength.Auto;
                }
            }
            var ctrl = ((UserControl)item);

            
            ctrl.HorizontalAlignment = HorizontalAlignment.Stretch;
            ctrl.VerticalAlignment = VerticalAlignment.Top;
            ctrl.SetValue(Grid.ColumnProperty, 1);
            ctrl.SetValue(Grid.RowProperty, position);
            Grid2.Children.Insert(position, ctrl);
            Grid2.RowDefinitions[position].Height = GridLength.Auto;

            return item;
        }

        public void RemoveItem(KVGridItemInterface item, bool disposeItem)
        {
            if (item == null) return;

            if (GridOwner.SelectedItem == item)
                GridOwner.SelectedItem = null;

            int id = Items.IndexOf(item);
            Items.Remove(item);
            Grid2.Children.Remove((UserControl)item);

            for (int i = id; i < Grid2.Children.Count; i++)
            {
                Grid2.Children[i].SetValue(Grid.RowProperty, i);
            }
            Grid2.RowDefinitions.RemoveAt(Grid2.RowDefinitions.Count - 1);
        }

        public void MoveItemTo(KVGridItemInterface item, int index)
        {
            //todo Oplkill wpf
            if (!Equals(item.ParentBlock, this)) return;

            bool selectedItem = GridOwner.SelectedItem == item;
            RemoveItem(item, false);
            if (index >= Items.Count)
                Items.Add(item);
            else
                Items.Insert(index, item);

            if (selectedItem)
                GridOwner.SelectedItem = item;
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
                    var itemTemp = ((KVGridBlock)item).FindItemId(id);
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

            Grid2.Children[index1].SetValue(Grid.RowProperty, index2);
            Grid2.Children[index2].SetValue(Grid.RowProperty, index1);
        }

        private void buttonCollapse_Click(object sender, EventArgs e)
        {
            select_Click(sender, e);

            if (collapsed)
            {
                collapsed = false;
                Grid1.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Star);
                buttonCollapse.Content = @"-";
            }
            else
            {
                collapsed = true;
                Grid1.RowDefinitions[1].Height = new GridLength(0);
                buttonCollapse.Content = @"+";
            }
        }

        private void select_Click(object sender, EventArgs e)
        {
            GridOwner.SelectedItem = this;
        }


        #region variables

        

        public bool Selected
        {
            get
            {
                //todo Oplkill wpf
                //return splitContainer1.BackColor != Control.DefaultBackColor;
                return false;
            }
            set
            {
                //todo Oplkill wpf
                //if (this.Selected == value) return;

                //if (value)
                //{
                //    splitContainer1.BackColor = Color.DarkBlue;
                //    //GridOwner.SelectedItem = this;
                //}
                //else
                //{
                //    splitContainer1.BackColor = Control.DefaultBackColor;
                //    //GridOwner.SelectedItem = null;
                //}
            }
        }

        public int Id { get; set; }
        public KVGrid.TextChangedFunc OnTextChanged { get; set; }

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
        public string ValueText { get { return null; } set { } }
        public List<KVGridItemInterface> Items => kvItems;

        private List<KVGridItemInterface> kvItems;
        private bool collapsed;

        #endregion
    }
}
