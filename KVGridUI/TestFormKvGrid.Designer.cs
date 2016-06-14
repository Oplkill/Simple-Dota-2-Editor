namespace KVGridUI
{
    partial class TestFormKvGrid
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

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.buttonUp = new System.Windows.Forms.Button();
            this.buttonDown = new System.Windows.Forms.Button();
            this.kvGrid1 = new KVGridUI.KVGrid();
            this.buttonAddBlockToSelected = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Add item";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.buttonAddBlockToSelected);
            this.splitContainer1.Panel1.Controls.Add(this.buttonDown);
            this.splitContainer1.Panel1.Controls.Add(this.buttonUp);
            this.splitContainer1.Panel1.Controls.Add(this.button4);
            this.splitContainer1.Panel1.Controls.Add(this.button3);
            this.splitContainer1.Panel1.Controls.Add(this.button2);
            this.splitContainer1.Panel1.Controls.Add(this.button1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.AutoScroll = true;
            this.splitContainer1.Panel2.Controls.Add(this.kvGrid1);
            this.splitContainer1.Size = new System.Drawing.Size(883, 569);
            this.splitContainer1.SplitterDistance = 48;
            this.splitContainer1.TabIndex = 2;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(174, 12);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(142, 23);
            this.button4.TabIndex = 4;
            this.button4.Text = "Add item to selected block";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(93, 12);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 3;
            this.button3.Text = "Add block";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(506, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Remove";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // buttonUp
            // 
            this.buttonUp.Location = new System.Drawing.Point(584, 12);
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.Size = new System.Drawing.Size(44, 23);
            this.buttonUp.TabIndex = 5;
            this.buttonUp.Text = "UP";
            this.buttonUp.UseVisualStyleBackColor = true;
            this.buttonUp.Click += new System.EventHandler(this.buttonUp_Click);
            // 
            // buttonDown
            // 
            this.buttonDown.Location = new System.Drawing.Point(634, 12);
            this.buttonDown.Name = "buttonDown";
            this.buttonDown.Size = new System.Drawing.Size(54, 23);
            this.buttonDown.TabIndex = 5;
            this.buttonDown.Text = "DOWN";
            this.buttonDown.UseVisualStyleBackColor = true;
            this.buttonDown.Click += new System.EventHandler(this.buttonDown_Click);
            // 
            // kvGrid1
            // 
            this.kvGrid1.AutoScroll = true;
            this.kvGrid1.AutoSize = true;
            this.kvGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kvGrid1.Location = new System.Drawing.Point(0, 0);
            this.kvGrid1.Name = "kvGrid1";
            this.kvGrid1.SelectedItem = null;
            this.kvGrid1.Size = new System.Drawing.Size(883, 517);
            this.kvGrid1.TabIndex = 0;
            // 
            // buttonAddBlockToSelected
            // 
            this.buttonAddBlockToSelected.Location = new System.Drawing.Point(322, 12);
            this.buttonAddBlockToSelected.Name = "buttonAddBlockToSelected";
            this.buttonAddBlockToSelected.Size = new System.Drawing.Size(152, 23);
            this.buttonAddBlockToSelected.TabIndex = 6;
            this.buttonAddBlockToSelected.Text = "Add block to selected block";
            this.buttonAddBlockToSelected.UseVisualStyleBackColor = true;
            this.buttonAddBlockToSelected.Click += new System.EventHandler(this.buttonAddBlockToSelected_Click);
            // 
            // TestFormKvGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(883, 569);
            this.Controls.Add(this.splitContainer1);
            this.Name = "TestFormKvGrid";
            this.Text = "Form1";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button button2;
        private KVGrid kvGrid1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button buttonDown;
        private System.Windows.Forms.Button buttonUp;
        private System.Windows.Forms.Button buttonAddBlockToSelected;
    }
}

