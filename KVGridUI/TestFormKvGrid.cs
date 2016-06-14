using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KVGridUI
{
    public partial class TestFormKvGrid : Form
    {
        public TestFormKvGrid()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            kvGrid1.AddItem(ItemTypes.TextText, "", "");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            kvGrid1.SelectedItem.ParentBlock.RemoveItem(kvGrid1.SelectedItem, true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            kvGrid1.AddItem(ItemTypes.Block, "", "");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            (kvGrid1.SelectedItem as KVGridBlock)?.AddItem(kvGrid1, new KVGridItem_TextText());
        }

        private void buttonAddBlockToSelected_Click(object sender, EventArgs e)
        {
            (kvGrid1.SelectedItem as KVGridBlock)?.AddItem(kvGrid1, new KVGridBlock());
        }

        private void buttonUp_Click(object sender, EventArgs e)
        {
            if (kvGrid1.SelectedItem == null) return;

            KVGridBlock parentBlock = kvGrid1.SelectedItem.ParentBlock;

            int indexSelectedItem = parentBlock.Items.IndexOf(kvGrid1.SelectedItem);
            if (indexSelectedItem != 0)
            {
                parentBlock.SwapItems(kvGrid1.SelectedItem, parentBlock.Items[indexSelectedItem - 1]);
            }
            else
            {
                if (parentBlock.ParentBlock == null) return;


            }
        }

        private void buttonDown_Click(object sender, EventArgs e)
        {
            if (kvGrid1.SelectedItem == null) return;

            KVGridBlock parentBlock = kvGrid1.SelectedItem.ParentBlock;

            int indexSelectedItem = parentBlock.Items.IndexOf(kvGrid1.SelectedItem);
            if (indexSelectedItem != parentBlock.Items.Count - 1)
            {
                parentBlock.SwapItems(kvGrid1.SelectedItem, parentBlock.Items[indexSelectedItem + 1]);
            }
            else
            {
                if (parentBlock.ParentBlock == null) return;


            }
        }

        
    }
}
