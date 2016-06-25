using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using KV_reloaded;
using ScintillaNET;
using WeifenLuo.WinFormsUI.Docking;

namespace SimpleDota2Editor.Panels
{
    public partial class TextEditorPanel : DockContent
    {
        public string PanelName
        {
            set { panelName = value; this.Text = value + (scintilla1.Modified ? @" *" : ""); }
            get { return panelName; }
        }

        private string panelName;
        public KVToken ObjectRef;
        private bool loading;

        public TextEditorPanel()
        {
            InitializeComponent();

            UpdateStyle();
        }

        public void UpdateStyle() //todo переименовать
        {
            scintilla1.Margins[0].Width = DataBase.Settings.HighSetts.MarginWidth;

            scintilla1.StyleResetDefault();
            scintilla1.Styles[Style.Default].Font = DataBase.Settings.HighSetts.Font;
            scintilla1.Styles[Style.Default].Size = DataBase.Settings.HighSetts.FontSize;
            scintilla1.Styles[Style.Default].Bold = DataBase.Settings.HighSetts.Bold;
            scintilla1.Styles[Style.Default].Italic = DataBase.Settings.HighSetts.Italic;
            scintilla1.Styles[Style.Default].Underline = DataBase.Settings.HighSetts.Underline;
            scintilla1.StyleClearAll();

            scintilla1.Styles[(int)KV_STYLES.STYLE_DEFAULT].ForeColor = ColorTranslator.FromHtml(DataBase.Settings.HighSetts.DefaultWordColor);
            scintilla1.Styles[(int)KV_STYLES.STYLE_COMMENT].ForeColor = ColorTranslator.FromHtml(DataBase.Settings.HighSetts.CommentColor);
            scintilla1.Styles[(int)KV_STYLES.STYLE_KEY].ForeColor = ColorTranslator.FromHtml(DataBase.Settings.HighSetts.KeyColor);
            scintilla1.Styles[(int)KV_STYLES.STYLE_KVBLOCK].ForeColor = ColorTranslator.FromHtml(DataBase.Settings.HighSetts.KVBlockColor);
            scintilla1.Styles[(int)KV_STYLES.STYLE_VALUE_NUMBER].ForeColor = ColorTranslator.FromHtml(DataBase.Settings.HighSetts.ValueNumberColor);
            scintilla1.Styles[(int)KV_STYLES.STYLE_VALUE_STRING].ForeColor = ColorTranslator.FromHtml(DataBase.Settings.HighSetts.ValueStringColor);
        }

        public void UpdateTextControlMenu()
        {
            AllPanels.Form1.toolStripButtonEditorUndo.Enabled = scintilla1.CanUndo;
            AllPanels.Form1.toolStripButtonEditorRedo.Enabled = scintilla1.CanRedo;
        }

        public void SetText(string text)
        {
            loading = true;
            scintilla1.Text = text;
            scintilla1.SetSavePoint();
            scintilla1.IdleStyling = IdleStyling.All;
            
            loading = false;
        }

        public bool CloseMe()
        {
            var canClose = SaveChanges();

            if (canClose)
            {
                return true;
            }

            var msg =
                MessageBox.Show(
                    "Text contains errors! Do you want close it?! All unsaved changes in this Object will be lost",
                    "Text contains errors!", MessageBoxButtons.YesNo, MessageBoxIcon.Error); //todo move to resource

            if (msg == DialogResult.Yes)
            {
                forceClose = true;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Закрыть без сохранения и проверки
        /// </summary>
        public void ForceClose()
        {
            forceClose = true;
            this.Close();
        }

        private bool forceClose;
        private void TextEditorPanel_FormClosing(object sender, FormClosingEventArgs e)
        {
            AllPanels.Form1.ShowEditorMenu(Form1.EditorType.None);

            if (forceClose)
                return;

            SaveChanges();
            e.Cancel = true;
            this.Hide();
        }

        public bool SaveChanges()
        {
            if (!scintilla1.Modified)
                return true;

            try
            {
                ObjectRef.Children = TokenAnalizer.AnaliseText(scintilla1.Text);
            }
            catch (Exception)
            {
                return false;
            }
            scintilla1.SetSavePoint();

            return true;
        }

        private void scintilla1_SavePointLeft(object sender, EventArgs e)
        {
            if (loading) return;
            this.Text = PanelName + @" *";
            DataBase.Edited = true;
            UpdateTextControlMenu();
        }

        private void scintilla1_SavePointReached(object sender, EventArgs e)
        {
            this.Text = PanelName;
            UpdateTextControlMenu();
        }

        private void scintilla1_StyleNeeded(object sender, StyleNeededEventArgs e)
        {
            try
            {

                //var pos = scintilla1.GetEndStyled();
                var pos = 0;
                var endPos = e.Position;
                bool key = true; // Ожидается, что будет далее, ключ(true) или значение(false)

                var ch = scintilla1.Text[pos];
                while (pos < endPos)
                {
                    ch = scintilla1.Text[pos];
                    if (!isSpace(ch) || ch != '\n')
                        switch (ch)
                        {
                            case '\"':
                                scintilla1.StartStyling(pos);

                                int end = nextCharThroughIs(scintilla1.Text, pos, '\"');
                                if (end == -1)
                                {
                                    scintilla1.SetStyling(endPos - pos, key ? (int)KV_STYLES.STYLE_KEY : (int)KV_STYLES.STYLE_VALUE_STRING);
                                    return;
                                }

                                int isBlock = nextCharThroughIs(scintilla1.Text, end + 1, '{');
                                if (isBlock != -1)
                                {
                                    key = true;
                                    scintilla1.SetStyling(end - pos + 1, (int)KV_STYLES.STYLE_KVBLOCK);
                                    pos = end;
                                }
                                else
                                {
                                    string str = scintilla1.Text.Substring(pos + 1, end - (pos + 1));
                                    int style = 0;
                                    if (isDigit(str))
                                        style = (int) KV_STYLES.STYLE_VALUE_NUMBER;
                                    else
                                        style = key ? (int) KV_STYLES.STYLE_KEY : (int) KV_STYLES.STYLE_VALUE_STRING;

                                    scintilla1.SetStyling(end - pos + 1, style);
                                    pos = end;
                                    key = !key;
                                }
                                break;

                            case '/':
                                if (pos + 1 >= scintilla1.Text.Length)
                                    return;
                                if (scintilla1.Text[pos + 1] != '/')
                                    break;

                                scintilla1.StartStyling(pos);
                                end = pos + 2;
                                while (end < scintilla1.Text.Length)
                                {
                                    if (scintilla1.Text[end++] == '\n')
                                        break;
                                }
                                scintilla1.SetStyling(end - pos, (int)KV_STYLES.STYLE_COMMENT);
                                pos = end - 1;
                                break;

                            case '{':
                            case '}':
                                scintilla1.StartStyling(pos);
                                scintilla1.SetStyling(1, (int)KV_STYLES.STYLE_DEFAULT);
                                break;
                        }

                    pos++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"Error - " + ex.Message);
                
            }
        }

        private bool isDigit(string str) //todo вынести
        {
            return str.All(ch => char.IsDigit(ch) || ch == '.');
        }

        private bool isSpace(char ch) //todo вынести
        {
            return (ch == ' ' || ch == '\t' || ch == '\r');
        }

        private int nextCharThroughIs(string str, int pos, char target)
        {
            int n = pos;
            while (n < str.Length)
            {
                switch (str[n])
                {
                    case '{':
                        if (target == '{')
                            return n;
                        break;

                    case '/':
                        while (str[n] != '\n')
                        {
                            n++;
                        }
                        break;

                    case '\"':
                        if (target == '{')
                            return -1;
                        n++;
                        if (n >= str.Length)
                            return -1;

                        while (str[n] != '\"')
                        {
                            n++;
                            if (n >= str.Length)
                                return -1;
                        }
                        if (target == '\"')
                            return n;
                        break;
                }

                n++;
            }

            return -1;
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private enum KV_STYLES
        {
            STYLE_DEFAULT,
            STYLE_KEY,
            STYLE_KVBLOCK,
            STYLE_VALUE_NUMBER,
            STYLE_VALUE_STRING,
            STYLE_COMMENT,
        }

        public void ButtonUndo_Click()
        {
            if(scintilla1.CanUndo)
                scintilla1.Undo();
            UpdateTextControlMenu();
        }

        public void ButtonRedo_Click()
        {
            if (scintilla1.CanRedo)
                scintilla1.Redo();
            UpdateTextControlMenu();
        }

        public void ButtonCommentIt_Click()
        {
            //todo
        }

        public void ButtonUnCommentIt_Click()
        {
            //todo
        }

        private void TextEditorPanel_DockStateChanged(object sender, EventArgs e)
        {
            UpdateTextControlMenu();
        }

        private void TextEditorPanel_Activated(object sender, EventArgs e)
        {
            AllPanels.Form1.ShowEditorMenu(Form1.EditorType.Text);
        }
    }
}
