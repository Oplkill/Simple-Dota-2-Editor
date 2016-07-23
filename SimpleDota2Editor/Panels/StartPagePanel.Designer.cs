namespace SimpleDota2Editor.Panels
{
    partial class StartPagePanel
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.linkLabelLoadAddon = new System.Windows.Forms.LinkLabel();
            this.listViewProjectsInFolder = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.listViewProjectsLastOpened = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // linkLabelLoadAddon
            // 
            this.linkLabelLoadAddon.AutoSize = true;
            this.linkLabelLoadAddon.Font = new System.Drawing.Font("Microsoft Sans Serif", 17F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.linkLabelLoadAddon.Location = new System.Drawing.Point(7, 9);
            this.linkLabelLoadAddon.Name = "linkLabelLoadAddon";
            this.linkLabelLoadAddon.Size = new System.Drawing.Size(137, 29);
            this.linkLabelLoadAddon.TabIndex = 0;
            this.linkLabelLoadAddon.TabStop = true;
            this.linkLabelLoadAddon.Text = "LoadAddon";
            this.linkLabelLoadAddon.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelLoadAddon_LinkClicked);
            // 
            // listViewProjectsInFolder
            // 
            this.listViewProjectsInFolder.BackColor = System.Drawing.SystemColors.Control;
            this.listViewProjectsInFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listViewProjectsInFolder.FullRowSelect = true;
            this.listViewProjectsInFolder.GridLines = true;
            this.listViewProjectsInFolder.HideSelection = false;
            this.listViewProjectsInFolder.LabelWrap = false;
            this.listViewProjectsInFolder.Location = new System.Drawing.Point(12, 82);
            this.listViewProjectsInFolder.MultiSelect = false;
            this.listViewProjectsInFolder.Name = "listViewProjectsInFolder";
            this.listViewProjectsInFolder.Size = new System.Drawing.Size(355, 310);
            this.listViewProjectsInFolder.TabIndex = 1;
            this.listViewProjectsInFolder.UseCompatibleStateImageBehavior = false;
            this.listViewProjectsInFolder.View = System.Windows.Forms.View.List;
            this.listViewProjectsInFolder.SelectedIndexChanged += new System.EventHandler(this.listViewProjectsInFolder_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(8, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(202, 22);
            this.label1.TabIndex = 2;
            this.label1.Text = "Projects in Dota 2 folder";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(400, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(177, 22);
            this.label2.TabIndex = 3;
            this.label2.Text = "Last opened projects";
            // 
            // listViewProjectsLastOpened
            // 
            this.listViewProjectsLastOpened.BackColor = System.Drawing.SystemColors.Control;
            this.listViewProjectsLastOpened.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listViewProjectsLastOpened.FullRowSelect = true;
            this.listViewProjectsLastOpened.GridLines = true;
            this.listViewProjectsLastOpened.HideSelection = false;
            this.listViewProjectsLastOpened.LabelWrap = false;
            this.listViewProjectsLastOpened.Location = new System.Drawing.Point(404, 82);
            this.listViewProjectsLastOpened.MultiSelect = false;
            this.listViewProjectsLastOpened.Name = "listViewProjectsLastOpened";
            this.listViewProjectsLastOpened.Size = new System.Drawing.Size(355, 310);
            this.listViewProjectsLastOpened.TabIndex = 1;
            this.listViewProjectsLastOpened.UseCompatibleStateImageBehavior = false;
            this.listViewProjectsLastOpened.View = System.Windows.Forms.View.List;
            // 
            // StartPagePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(842, 589);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listViewProjectsLastOpened);
            this.Controls.Add(this.listViewProjectsInFolder);
            this.Controls.Add(this.linkLabelLoadAddon);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Name = "StartPagePanel";
            this.Text = "MainPagePanel";
            this.Load += new System.EventHandler(this.StartPagePanel_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel linkLabelLoadAddon;
        private System.Windows.Forms.ListView listViewProjectsInFolder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.ListView listViewProjectsLastOpened;
    }
}