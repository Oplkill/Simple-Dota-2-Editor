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

        public GuiEditorPanel()
        {
            InitializeComponent();
            kvGrid = new KVGrid();
            //this.kvGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kvGrid.Location = new System.Drawing.Point(0, 0);
            this.kvGrid.Name = "kvGrid";
            this.kvGrid.Size = new System.Drawing.Size(552, 420);
            this.kvGrid.TabIndex = 0;
            this.kvGrid.Text = "";
            this.Controls.Add(kvGrid);
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
                    block.AddItem(kvGrid, tempBlock);
                    loadItems(tempBlock, kvToken.Children);
                }
                else if(kvToken.Type == KVTokenType.KVsimple)
                {
                    var kv = new KVGridItem_TextText();
                    kv.KeyText = kvToken.Key;
                    kv.ValueText = kvToken.Value;
                    block.AddItem(kvGrid, kv);
                }
            }
        }

        #region SubMenu



        public void RedoButton_Click()
        {
            
        }

        public void UndoButton_Click()
        {
            
        }

        public void CreateKVButton_Click()
        {
            
        }

        public void CreateKVBlockButton_Click()
        {
            
        }

        public void MoveUpButton_Click()
        {
            
        }

        public void MoveDownButton_Click()
        {
            
        }

        public void DeleteButton_Click()
        {
            
        }


        #endregion

        private void GuiEditorPanel_SizeChanged(object sender, EventArgs e)
        {
            //kvGrid.Dock = DockStyle.Fill;
            //kvGrid.Dock = DockStyle.None;
            kvGrid.Size = this.ClientSize;
            kvGrid.KVGrid_SizeChanged(sender, e);
        }
    }
}
