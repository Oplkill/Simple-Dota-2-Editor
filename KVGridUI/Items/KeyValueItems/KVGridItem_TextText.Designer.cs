namespace KVGridUI
{
    partial class KVGridItem_TextText
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
            this.kvsfiTextBoxKey = new KVGridUI.Items.SubFieldItems.KVSFITextBox();
            this.kvsfiTextBoxValue = new KVGridUI.Items.SubFieldItems.KVSFITextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.kvsfiTextBoxKey);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.kvsfiTextBoxValue);
            this.splitContainer1.Size = new System.Drawing.Size(365, 21);
            this.splitContainer1.SplitterDistance = 175;
            this.splitContainer1.TabIndex = 2;
            this.splitContainer1.Click += new System.EventHandler(this.select_Click);
            // 
            // kvsfiTextBoxKey
            // 
            this.kvsfiTextBoxKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kvsfiTextBoxKey.Location = new System.Drawing.Point(0, 0);
            this.kvsfiTextBoxKey.Name = "kvsfiTextBoxKey";
            this.kvsfiTextBoxKey.Size = new System.Drawing.Size(175, 21);
            this.kvsfiTextBoxKey.TabIndex = 0;
            this.kvsfiTextBoxKey.Click += new System.EventHandler(this.select_Click);
            // 
            // kvsfiTextBoxValue
            // 
            this.kvsfiTextBoxValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kvsfiTextBoxValue.Location = new System.Drawing.Point(0, 0);
            this.kvsfiTextBoxValue.Name = "kvsfiTextBoxValue";
            this.kvsfiTextBoxValue.Size = new System.Drawing.Size(186, 21);
            this.kvsfiTextBoxValue.TabIndex = 0;
            this.kvsfiTextBoxValue.Click += new System.EventHandler(this.select_Click);
            // 
            // KVGridItem_TextText
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.splitContainer1);
            this.Name = "KVGridItem_TextText";
            this.Size = new System.Drawing.Size(365, 21);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Items.SubFieldItems.KVSFITextBox kvsfiTextBoxKey;
        private Items.SubFieldItems.KVSFITextBox kvsfiTextBoxValue;
    }
}
