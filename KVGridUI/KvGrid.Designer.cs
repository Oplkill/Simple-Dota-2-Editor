namespace KVGridUI
{
    partial class KVGrid
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.kvGridBlock1 = new KVGridUI.KVGridBlock();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.kvGridBlock1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.vScrollBar1);
            this.splitContainer1.Panel2MinSize = 20;
            this.splitContainer1.Size = new System.Drawing.Size(594, 253);
            this.splitContainer1.SplitterDistance = 570;
            this.splitContainer1.TabIndex = 1;
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vScrollBar1.Location = new System.Drawing.Point(0, 0);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(20, 253);
            this.vScrollBar1.TabIndex = 0;
            this.vScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar1_Scroll);
            // 
            // kvGridBlock1
            // 
            this.kvGridBlock1.AutoSize = true;
            this.kvGridBlock1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.kvGridBlock1.GridOwner = null;
            this.kvGridBlock1.Id = -1;
            this.kvGridBlock1.ItemWidth = 570;
            this.kvGridBlock1.KeyText = "";
            this.kvGridBlock1.Location = new System.Drawing.Point(0, 0);
            this.kvGridBlock1.Name = "kvGridBlock1";
            this.kvGridBlock1.ParentBlock = null;
            this.kvGridBlock1.Selected = false;
            this.kvGridBlock1.Size = new System.Drawing.Size(570, 253);
            this.kvGridBlock1.TabIndex = 0;
            this.kvGridBlock1.ValueText = null;
            // 
            // KVGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.Controls.Add(this.splitContainer1);
            this.Name = "KVGrid";
            this.Size = new System.Drawing.Size(594, 253);
            this.SizeChanged += new System.EventHandler(this.KVGrid_SizeChanged);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private KVGridBlock kvGridBlock1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.VScrollBar vScrollBar1;
    }
}
