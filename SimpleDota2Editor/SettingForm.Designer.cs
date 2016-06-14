namespace SimpleDota2Editor
{
    partial class SettingForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonDefault = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.comboBoxPrimaryEditor = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.checkBoxAddHeaderToFiles = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.buttonBrowseDotaPath = new System.Windows.Forms.Button();
            this.textBoxDotaPath = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBoxLang = new System.Windows.Forms.ComboBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxFontSize = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBoxFonts = new System.Windows.Forms.ComboBox();
            this.checkBoxFontBold = new System.Windows.Forms.CheckBox();
            this.checkBoxFontItalic = new System.Windows.Forms.CheckBox();
            this.checkBoxFontUnderline = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonValueNumberColor = new System.Windows.Forms.Button();
            this.buttonValueColor = new System.Windows.Forms.Button();
            this.buttonKeyBlockColor = new System.Windows.Forms.Button();
            this.buttonKeyColor = new System.Windows.Forms.Button();
            this.buttonCommentsColor = new System.Windows.Forms.Button();
            this.buttonDefaultTextColor = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.tabPage8 = new System.Windows.Forms.TabPage();
            this.tabPage9 = new System.Windows.Forms.TabPage();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            // 
            // splitContainer2
            // 
            resources.ApplyResources(this.splitContainer2, "splitContainer2");
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.listBox1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.buttonCancel);
            this.splitContainer2.Panel2.Controls.Add(this.buttonDefault);
            // 
            // listBox1
            // 
            resources.ApplyResources(this.listBox1, "listBox1");
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Name = "listBox1";
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // buttonCancel
            // 
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonDefault
            // 
            resources.ApplyResources(this.buttonDefault, "buttonDefault");
            this.buttonDefault.Name = "buttonDefault";
            this.buttonDefault.UseVisualStyleBackColor = true;
            this.buttonDefault.Click += new System.EventHandler(this.buttonDefault_Click);
            // 
            // tabControl1
            // 
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage6);
            this.tabControl1.Controls.Add(this.tabPage7);
            this.tabControl1.Controls.Add(this.tabPage8);
            this.tabControl1.Controls.Add(this.tabPage9);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.comboBoxPrimaryEditor);
            this.tabPage1.Controls.Add(this.label9);
            this.tabPage1.Controls.Add(this.checkBoxAddHeaderToFiles);
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox1);
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // comboBoxPrimaryEditor
            // 
            this.comboBoxPrimaryEditor.FormattingEnabled = true;
            this.comboBoxPrimaryEditor.Items.AddRange(new object[] {
            resources.GetString("comboBoxPrimaryEditor.Items"),
            resources.GetString("comboBoxPrimaryEditor.Items1")});
            resources.ApplyResources(this.comboBoxPrimaryEditor, "comboBoxPrimaryEditor");
            this.comboBoxPrimaryEditor.Name = "comboBoxPrimaryEditor";
            this.comboBoxPrimaryEditor.SelectedIndexChanged += new System.EventHandler(this.comboBoxPrimaryEditor_SelectedIndexChanged);
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // checkBoxAddHeaderToFiles
            // 
            resources.ApplyResources(this.checkBoxAddHeaderToFiles, "checkBoxAddHeaderToFiles");
            this.checkBoxAddHeaderToFiles.Name = "checkBoxAddHeaderToFiles";
            this.checkBoxAddHeaderToFiles.UseVisualStyleBackColor = true;
            this.checkBoxAddHeaderToFiles.CheckedChanged += new System.EventHandler(this.checkBoxAddHeaderToFiles_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.buttonBrowseDotaPath);
            this.groupBox3.Controls.Add(this.textBoxDotaPath);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // buttonBrowseDotaPath
            // 
            resources.ApplyResources(this.buttonBrowseDotaPath, "buttonBrowseDotaPath");
            this.buttonBrowseDotaPath.Name = "buttonBrowseDotaPath";
            this.buttonBrowseDotaPath.UseVisualStyleBackColor = true;
            this.buttonBrowseDotaPath.Click += new System.EventHandler(this.buttonBrowseDotaPath_Click);
            // 
            // textBoxDotaPath
            // 
            resources.ApplyResources(this.textBoxDotaPath, "textBoxDotaPath");
            this.textBoxDotaPath.Name = "textBoxDotaPath";
            this.textBoxDotaPath.ReadOnly = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBoxLang);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // comboBoxLang
            // 
            this.comboBoxLang.BackColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.comboBoxLang, "comboBoxLang");
            this.comboBoxLang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLang.ForeColor = System.Drawing.SystemColors.WindowText;
            this.comboBoxLang.FormattingEnabled = true;
            this.comboBoxLang.Items.AddRange(new object[] {
            resources.GetString("comboBoxLang.Items"),
            resources.GetString("comboBoxLang.Items1")});
            this.comboBoxLang.Name = "comboBoxLang";
            this.comboBoxLang.SelectedIndexChanged += new System.EventHandler(this.comboBoxLang_SelectedIndexChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Controls.Add(this.groupBox2);
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.textBoxFontSize);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.comboBoxFonts);
            this.groupBox4.Controls.Add(this.checkBoxFontBold);
            this.groupBox4.Controls.Add(this.checkBoxFontItalic);
            this.groupBox4.Controls.Add(this.checkBoxFontUnderline);
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // textBoxFontSize
            // 
            resources.ApplyResources(this.textBoxFontSize, "textBoxFontSize");
            this.textBoxFontSize.Name = "textBoxFontSize";
            this.textBoxFontSize.TextChanged += new System.EventHandler(this.textBoxFontSize_TextChanged);
            this.textBoxFontSize.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress_OnlyNumbers);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // comboBoxFonts
            // 
            this.comboBoxFonts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFonts.FormattingEnabled = true;
            resources.ApplyResources(this.comboBoxFonts, "comboBoxFonts");
            this.comboBoxFonts.Name = "comboBoxFonts";
            this.comboBoxFonts.SelectedIndexChanged += new System.EventHandler(this.comboBoxFonts_SelectedIndexChanged);
            // 
            // checkBoxFontBold
            // 
            resources.ApplyResources(this.checkBoxFontBold, "checkBoxFontBold");
            this.checkBoxFontBold.Name = "checkBoxFontBold";
            this.checkBoxFontBold.UseVisualStyleBackColor = true;
            this.checkBoxFontBold.CheckedChanged += new System.EventHandler(this.checkBoxFontBold_CheckedChanged);
            // 
            // checkBoxFontItalic
            // 
            resources.ApplyResources(this.checkBoxFontItalic, "checkBoxFontItalic");
            this.checkBoxFontItalic.Name = "checkBoxFontItalic";
            this.checkBoxFontItalic.UseVisualStyleBackColor = true;
            this.checkBoxFontItalic.CheckedChanged += new System.EventHandler(this.checkBoxFontItalic_CheckedChanged);
            // 
            // checkBoxFontUnderline
            // 
            resources.ApplyResources(this.checkBoxFontUnderline, "checkBoxFontUnderline");
            this.checkBoxFontUnderline.Name = "checkBoxFontUnderline";
            this.checkBoxFontUnderline.UseVisualStyleBackColor = true;
            this.checkBoxFontUnderline.CheckedChanged += new System.EventHandler(this.checkBoxFontUnderline_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.buttonValueNumberColor);
            this.groupBox2.Controls.Add(this.buttonValueColor);
            this.groupBox2.Controls.Add(this.buttonKeyBlockColor);
            this.groupBox2.Controls.Add(this.buttonKeyColor);
            this.groupBox2.Controls.Add(this.buttonCommentsColor);
            this.groupBox2.Controls.Add(this.buttonDefaultTextColor);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // buttonValueNumberColor
            // 
            resources.ApplyResources(this.buttonValueNumberColor, "buttonValueNumberColor");
            this.buttonValueNumberColor.Name = "buttonValueNumberColor";
            this.buttonValueNumberColor.UseVisualStyleBackColor = true;
            this.buttonValueNumberColor.Click += new System.EventHandler(this.buttonColor_Click);
            // 
            // buttonValueColor
            // 
            resources.ApplyResources(this.buttonValueColor, "buttonValueColor");
            this.buttonValueColor.Name = "buttonValueColor";
            this.buttonValueColor.UseVisualStyleBackColor = true;
            this.buttonValueColor.Click += new System.EventHandler(this.buttonColor_Click);
            // 
            // buttonKeyBlockColor
            // 
            resources.ApplyResources(this.buttonKeyBlockColor, "buttonKeyBlockColor");
            this.buttonKeyBlockColor.Name = "buttonKeyBlockColor";
            this.buttonKeyBlockColor.UseVisualStyleBackColor = true;
            this.buttonKeyBlockColor.Click += new System.EventHandler(this.buttonColor_Click);
            // 
            // buttonKeyColor
            // 
            resources.ApplyResources(this.buttonKeyColor, "buttonKeyColor");
            this.buttonKeyColor.Name = "buttonKeyColor";
            this.buttonKeyColor.UseVisualStyleBackColor = true;
            this.buttonKeyColor.Click += new System.EventHandler(this.buttonColor_Click);
            // 
            // buttonCommentsColor
            // 
            resources.ApplyResources(this.buttonCommentsColor, "buttonCommentsColor");
            this.buttonCommentsColor.Name = "buttonCommentsColor";
            this.buttonCommentsColor.UseVisualStyleBackColor = true;
            this.buttonCommentsColor.Click += new System.EventHandler(this.buttonColor_Click);
            // 
            // buttonDefaultTextColor
            // 
            resources.ApplyResources(this.buttonDefaultTextColor, "buttonDefaultTextColor");
            this.buttonDefaultTextColor.Name = "buttonDefaultTextColor";
            this.buttonDefaultTextColor.UseVisualStyleBackColor = true;
            this.buttonDefaultTextColor.Click += new System.EventHandler(this.buttonColor_Click);
            // 
            // tabPage3
            // 
            resources.ApplyResources(this.tabPage3, "tabPage3");
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            resources.ApplyResources(this.tabPage4, "tabPage4");
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            resources.ApplyResources(this.tabPage5, "tabPage5");
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // tabPage6
            // 
            resources.ApplyResources(this.tabPage6, "tabPage6");
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // tabPage7
            // 
            resources.ApplyResources(this.tabPage7, "tabPage7");
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // tabPage8
            // 
            resources.ApplyResources(this.tabPage8, "tabPage8");
            this.tabPage8.Name = "tabPage8";
            this.tabPage8.UseVisualStyleBackColor = true;
            // 
            // tabPage9
            // 
            resources.ApplyResources(this.tabPage9, "tabPage9");
            this.tabPage9.Name = "tabPage9";
            this.tabPage9.UseVisualStyleBackColor = true;
            // 
            // SettingForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "SettingForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingForm_FormClosing);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.TabPage tabPage7;
        private System.Windows.Forms.TabPage tabPage8;
        private System.Windows.Forms.TabPage tabPage9;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox comboBoxLang;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button buttonBrowseDotaPath;
        private System.Windows.Forms.TextBox textBoxDotaPath;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button buttonDefaultTextColor;
        private System.Windows.Forms.CheckBox checkBoxAddHeaderToFiles;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonValueNumberColor;
        private System.Windows.Forms.Button buttonValueColor;
        private System.Windows.Forms.Button buttonKeyBlockColor;
        private System.Windows.Forms.Button buttonKeyColor;
        private System.Windows.Forms.Button buttonCommentsColor;
        private System.Windows.Forms.CheckBox checkBoxFontBold;
        private System.Windows.Forms.CheckBox checkBoxFontItalic;
        private System.Windows.Forms.CheckBox checkBoxFontUnderline;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBoxFonts;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxFontSize;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button buttonDefault;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.ComboBox comboBoxPrimaryEditor;
        private System.Windows.Forms.Label label9;
    }
}