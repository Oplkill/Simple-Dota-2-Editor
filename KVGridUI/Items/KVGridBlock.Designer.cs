namespace KVGridUI
{
    partial class KVGridBlock
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
            this.buttonCollapse = new System.Windows.Forms.Button();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.kvsfiTextBoxKey = new KVGridUI.Items.SubFieldItems.KVSFITextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.buttonCollapse);
            this.splitContainer1.Panel1.Click += new System.EventHandler(this.select_Click);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(435, 104);
            this.splitContainer1.SplitterDistance = 25;
            this.splitContainer1.TabIndex = 0;
            // 
            // buttonCollapse
            // 
            this.buttonCollapse.Location = new System.Drawing.Point(0, 0);
            this.buttonCollapse.Name = "buttonCollapse";
            this.buttonCollapse.Size = new System.Drawing.Size(22, 23);
            this.buttonCollapse.TabIndex = 0;
            this.buttonCollapse.Text = "-";
            this.buttonCollapse.UseVisualStyleBackColor = true;
            this.buttonCollapse.Click += new System.EventHandler(this.buttonCollapse_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.kvsfiTextBoxKey);
            this.splitContainer2.Size = new System.Drawing.Size(406, 104);
            this.splitContainer2.SplitterDistance = 25;
            this.splitContainer2.SplitterWidth = 1;
            this.splitContainer2.TabIndex = 0;
            // 
            // kvsfiTextBoxKey
            // 
            this.kvsfiTextBoxKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kvsfiTextBoxKey.Location = new System.Drawing.Point(0, 0);
            this.kvsfiTextBoxKey.Name = "kvsfiTextBoxKey";
            this.kvsfiTextBoxKey.Size = new System.Drawing.Size(406, 25);
            this.kvsfiTextBoxKey.TabIndex = 0;
            // 
            // KVGridBlock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.splitContainer1);
            this.Name = "KVGridBlock";
            this.Size = new System.Drawing.Size(435, 104);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button buttonCollapse;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private Items.SubFieldItems.KVSFITextBox kvsfiTextBoxKey;
    }
}
