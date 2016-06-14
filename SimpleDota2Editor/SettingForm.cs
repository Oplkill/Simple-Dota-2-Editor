using System;
using System.Drawing;
using System.Windows.Forms;
using SimpleDota2Editor.Properties;

namespace SimpleDota2Editor
{
    public partial class SettingForm : Form
    {
        private bool loading;
        private Settings startSettings;

        public SettingForm()
        {
            InitializeComponent();

            this.Size = new Size(Size.Width - 10, Size.Height - 30);

            InitListBox();
            load();
        }

        private void InitListBox()
        {
            listBox1.Items.Clear();
            listBox1.Items.Add(Resources.SettingsMenuCommon);
            listBox1.Items.Add(Resources.SettingsMenuHighlighting);
            listBox1.SelectedIndex = 0;
        }

        private void load()
        {
            loading = true;

            loadCommon();
            loadHighlighting();

            loading = false;
        }

        private void buttonDefault_Click(object sender, EventArgs e)
        {
            var rez = MessageBox.Show(Resources.SetToDefault, Resources.SetToDefaultCapture, MessageBoxButtons.YesNo);
            if (rez == DialogResult.Cancel) return;

            DataBase.Settings = new Settings();
            load();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            var rez = MessageBox.Show(Resources.SetToStartSettings, Resources.SetToStartSettingsCapture, MessageBoxButtons.YesNo);
            if (rez == DialogResult.Cancel) return;

            DataBase.Settings = startSettings;
            load();
        }

        #region Common

        private void loadCommon()
        {
            comboBoxLang.Items.Clear();
            comboBoxLang.Items.Add(@"English");
            comboBoxLang.Items.Add(@"Русский");

            comboBoxLang.SelectedIndex = (int)DataBase.Settings.Lang;
            checkBoxAddHeaderToFiles.Checked = DataBase.Settings.WriteHeadLinkOnSave;
            textBoxDotaPath.Text = DataBase.Settings.DotaPath;
            comboBoxPrimaryEditor.SelectedIndex = (int) DataBase.Settings.EditorPriority;
        }

        private void checkBoxAddHeaderToFiles_CheckedChanged(object sender, EventArgs e)
        {
            if (loading) return;
            DataBase.Settings.WriteHeadLinkOnSave = checkBoxAddHeaderToFiles.Checked;
        }

        private void comboBoxLang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loading) return;
            DataBase.Settings.Lang = (Settings.Language)comboBoxLang.SelectedIndex;
        }

        private void buttonBrowseDotaPath_Click(object sender, EventArgs e)
        {
            var res = folderBrowserDialog1.ShowDialog();
            if (res != DialogResult.OK) return;

            textBoxDotaPath.Text = DataBase.Settings.DotaPath = folderBrowserDialog1.SelectedPath;
        }

        private void comboBoxPrimaryEditor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loading) return;
            DataBase.Settings.EditorPriority = (Settings.EditorType) comboBoxPrimaryEditor.SelectedIndex;
        }

        #endregion

        #region Highlighting

        private void loadHighlighting()
        {
            checkBoxFontBold.Checked = DataBase.Settings.HighSetts.Bold;
            checkBoxFontItalic.Checked = DataBase.Settings.HighSetts.Italic;
            checkBoxFontUnderline.Checked = DataBase.Settings.HighSetts.Underline;
            textBoxFontSize.Text = DataBase.Settings.HighSetts.FontSize.ToString();

            buttonDefaultTextColor.BackColor = ColorTranslator.FromHtml(DataBase.Settings.HighSetts.DefaultWordColor);
            buttonCommentsColor.BackColor = ColorTranslator.FromHtml(DataBase.Settings.HighSetts.CommentColor);
            buttonKeyBlockColor.BackColor = ColorTranslator.FromHtml(DataBase.Settings.HighSetts.KVBlockColor);
            buttonKeyColor.BackColor = ColorTranslator.FromHtml(DataBase.Settings.HighSetts.KeyColor);
            buttonValueColor.BackColor = ColorTranslator.FromHtml(DataBase.Settings.HighSetts.ValueStringColor);
            buttonValueNumberColor.BackColor = ColorTranslator.FromHtml(DataBase.Settings.HighSetts.ValueNumberColor);

            comboBoxFonts.Items.Clear();
            foreach (FontFamily item in FontFamily.Families)
                comboBoxFonts.Items.Add(item.Name);
            comboBoxFonts.SelectedItem = DataBase.Settings.HighSetts.Font;
        }

        private void buttonColor_Click(object sender, EventArgs e)
        {
            Button butt = (Button)sender;
            colorDialog1.Color = butt.BackColor;
            colorDialog1.ShowDialog();
            butt.BackColor = colorDialog1.Color;

            if (butt == buttonDefaultTextColor)
                DataBase.Settings.HighSetts.DefaultWordColor = ColorTranslator.ToHtml(colorDialog1.Color);
            else if (butt == buttonCommentsColor)
                DataBase.Settings.HighSetts.CommentColor = ColorTranslator.ToHtml(colorDialog1.Color);
            else if (butt == buttonKeyBlockColor)
                DataBase.Settings.HighSetts.KVBlockColor = ColorTranslator.ToHtml(colorDialog1.Color);
            else if (butt == buttonKeyColor)
                DataBase.Settings.HighSetts.KeyColor = ColorTranslator.ToHtml(colorDialog1.Color);
            else if (butt == buttonValueColor)
                DataBase.Settings.HighSetts.ValueStringColor = ColorTranslator.ToHtml(colorDialog1.Color);
            else if (butt == buttonValueNumberColor)
                DataBase.Settings.HighSetts.ValueNumberColor = ColorTranslator.ToHtml(colorDialog1.Color);
        }

        private void checkBoxFontBold_CheckedChanged(object sender, EventArgs e)
        {
            if (loading) return;
            DataBase.Settings.HighSetts.Bold = checkBoxFontBold.Checked;
        }

        private void checkBoxFontItalic_CheckedChanged(object sender, EventArgs e)
        {
            if (loading) return;
            DataBase.Settings.HighSetts.Italic = checkBoxFontItalic.Checked;
        }

        private void checkBoxFontUnderline_CheckedChanged(object sender, EventArgs e)
        {
            if (loading) return;
            DataBase.Settings.HighSetts.Underline = checkBoxFontUnderline.Checked;
        }

        private void comboBoxFonts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loading) return;
            DataBase.Settings.HighSetts.Font = (string)comboBoxFonts.SelectedItem;
        }

        private void textBoxFontSize_TextChanged(object sender, EventArgs e)
        {
            if (loading) return;
            if (string.IsNullOrEmpty(textBoxFontSize.Text)) return;
            int size = int.Parse(textBoxFontSize.Text);
            DataBase.Settings.HighSetts.FontSize = size == 0 ? 1 : size;
        }

        #endregion


        private void SettingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
                return;

            tabControl1.SelectedIndex = listBox1.SelectedIndex;
        }

        private void textBox_KeyPress_OnlyNumbers(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != Convert.ToChar(8))
            {
                e.Handled = true;
            }
        }

        
    }
}
