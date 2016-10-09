using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SomeUtils;
using Xceed.Wpf.AvalonDock.Layout;
using Xceed.Wpf.Toolkit;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;

namespace SimpleDota2EditorWPF
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private bool loading;
        private Settings startSettings;

        public SettingsWindow()
        {
            InitializeComponent();

            load();

            startSettings = DataBase.Settings.DeepClone();
        }

        private void load()
        {
            loading = true;

            loadCommon();
            loadHighlighting();
            LoadHightlightingLua();

            loading = false;
        }

        private void Update()
        {
            var editors = AllPanels.GetAllEditorPanels();
            foreach (LayoutContent editor in editors)
            {
                ((IEditor)editor.Content).Update();
            }
        }

        private void buttonDefault_Click(object sender, EventArgs e)
        {
            var rez = MessageBox.Show(Properties.Resources.SetToDefault, Properties.Resources.SetToDefaultCapture, MessageBoxButton.YesNo);
            if (rez == MessageBoxResult.No) return;

            DataBase.Settings = new Settings();
            load();
            Update();
        }

        #region Common

        private void loadCommon()
        {
            ComboBoxLanguage.Items.Clear();
            ComboBoxLanguage.Items.Add(@"English");
            ComboBoxLanguage.Items.Add(@"Русский");

            ComboBoxLanguage.SelectedIndex = (int)DataBase.Settings.Lang;
            CheckBoxEditorHeader.IsChecked = DataBase.Settings.WriteHeadLinkOnSave;
            CheckBoxLoadSaveOpenedObjects.IsChecked = DataBase.Settings.LoadSaveOpenedObjects;
            TextBoxDotaPath.Text = DataBase.Settings.DotaPath;
            //ComboBoxPrimaryEditor.SelectedIndex = (int)DataBase.Settings.EditorPriority;
        }

        private void checkBoxAddHeaderToFiles_CheckedChanged(object sender, EventArgs e)
        {
            if (loading) return;
            if (CheckBoxEditorHeader.IsChecked != null)
                DataBase.Settings.WriteHeadLinkOnSave = (bool)CheckBoxEditorHeader.IsChecked;
        }

        private void checkBoxLoadSaveOpenedObjects_CheckedChanged(object sender, EventArgs e)
        {
            if (loading) return;
            if (CheckBoxLoadSaveOpenedObjects.IsChecked != null)
                DataBase.Settings.LoadSaveOpenedObjects = (bool)CheckBoxLoadSaveOpenedObjects.IsChecked;
        }

        private void comboBoxLang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loading) return;
            DataBase.Settings.Lang = (Settings.Language)ComboBoxLanguage.SelectedIndex;
        }

        private void buttonBrowseDotaPath_Click(object sender, EventArgs e)
        {
            DataBase.OpenFolderDialog.InitialDirectory = DataBase.Settings.DotaPath;
            bool? res = DataBase.OpenFolderDialog.ShowDialog();
            if (res == true)
            {
                TextBoxDotaPath.Text = DataBase.Settings.DotaPath = DataBase.OpenFolderDialog.FileName + "\\";
            }
            DataBase.OpenFolderDialog.InitialDirectory = DataBase.Settings.DotaPath + DataBase.Settings.AddonsPath;
        }

        //private void comboBoxPrimaryEditor_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (loading) return;
        //    DataBase.Settings.EditorPriority = (Settings.EditorType)ComboBoxPrimaryEditor.SelectedIndex;
        //}

        #endregion

        #region Highlighting KV

        private void loadHighlighting()
        {
            CheckBoxFontBold.IsChecked = DataBase.Settings.HighSetts.Bold;
            CheckBoxFontItalic.IsChecked = DataBase.Settings.HighSetts.Italic;
            TextBoxFontSize.Text = DataBase.Settings.HighSetts.FontSize.ToString();

            ColorPickerHightDefault.SelectedColor = (Color)ColorConverter.ConvertFromString(DataBase.Settings.HighSetts.DefaultWordColor);
            ColorPickerHightComments.SelectedColor = (Color)ColorConverter.ConvertFromString(DataBase.Settings.HighSetts.CommentColor);
            ColorPickerHightKeyBlock.SelectedColor = (Color)ColorConverter.ConvertFromString(DataBase.Settings.HighSetts.KVBlockColor);
            ColorPickerHightKey.SelectedColor = (Color)ColorConverter.ConvertFromString(DataBase.Settings.HighSetts.KeyColor);
            ColorPickerHightValue.SelectedColor = (Color)ColorConverter.ConvertFromString(DataBase.Settings.HighSetts.ValueStringColor);
            ColorPickerHightValueNumbers.SelectedColor = (Color)ColorConverter.ConvertFromString(DataBase.Settings.HighSetts.ValueNumberColor);

            ComboBoxFonts.Items.Clear();
            foreach (FontFamily ff in Fonts.SystemFontFamilies)
            {
                ComboBoxFonts.Items.Add(ff.Source);
            }
            ComboBoxFonts.SelectedItem = DataBase.Settings.HighSetts.Font;
        }

        private void ColorPickerHight_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (loading) return;

            var picker = (ColorPicker) sender;
            if (picker.SelectedColor == null)
                return;

            if (Equals(picker, ColorPickerHightDefault))
                DataBase.Settings.HighSetts.DefaultWordColor = picker.SelectedColor.Value.ToString();
            else if (Equals(picker, ColorPickerHightComments))
                DataBase.Settings.HighSetts.CommentColor = picker.SelectedColor.Value.ToString();
            else if (Equals(picker, ColorPickerHightKey))
                DataBase.Settings.HighSetts.KeyColor = picker.SelectedColor.Value.ToString();
            else if (Equals(picker, ColorPickerHightKeyBlock))
                DataBase.Settings.HighSetts.KVBlockColor = picker.SelectedColor.Value.ToString();
            else if (Equals(picker, ColorPickerHightValue))
                DataBase.Settings.HighSetts.ValueStringColor = picker.SelectedColor.Value.ToString();
            else if (Equals(picker, ColorPickerHightValueNumbers))
                DataBase.Settings.HighSetts.ValueNumberColor = picker.SelectedColor.Value.ToString();
            Update();
        }

        private void checkBoxFontBold_CheckedChanged(object sender, EventArgs e)
        {
            if (loading) return;
            if (CheckBoxFontBold.IsChecked != null)
                DataBase.Settings.HighSetts.Bold = (bool)CheckBoxFontBold.IsChecked;
            Update();
        }

        private void checkBoxFontItalic_CheckedChanged(object sender, EventArgs e)
        {
            if (loading) return;
            if (CheckBoxFontItalic.IsChecked != null)
                DataBase.Settings.HighSetts.Italic = (bool)CheckBoxFontItalic.IsChecked;
            Update();
        }

        private void comboBoxFonts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loading) return;
            DataBase.Settings.HighSetts.Font = (string)ComboBoxFonts.SelectedItem;
            Update();
        }

        private void textBoxFontSize_TextChanged(object sender, EventArgs e)
        {
            if (loading) return;
            if (string.IsNullOrEmpty(TextBoxFontSize.Text)) return;
            int size = int.Parse(TextBoxFontSize.Text);
            DataBase.Settings.HighSetts.FontSize = (size == 0) ? 1 : size;
            Update();
        }



        #endregion

        #region Hightligting Lua

        private void LoadHightlightingLua()
        {
            CheckBoxFontBoldLua.IsChecked = DataBase.Settings.HighSettsLua.Bold;
            CheckBoxFontItalicLua.IsChecked = DataBase.Settings.HighSettsLua.Italic;
            TextBoxFontSizeLua.Text = DataBase.Settings.HighSettsLua.FontSize.ToString();

            ColorPickerLuaBlockComment.SelectedColor = (Color)ColorConverter.ConvertFromString(DataBase.Settings.HighSettsLua.BlockCommentColor);
            ColorPickerLuaChars.SelectedColor = (Color)ColorConverter.ConvertFromString(DataBase.Settings.HighSettsLua.CharColor);
            ColorPickerLuaStrings.SelectedColor = (Color)ColorConverter.ConvertFromString(DataBase.Settings.HighSettsLua.StringsColor);
            ColorPickerLuaMultiLineString.SelectedColor = (Color)ColorConverter.ConvertFromString(DataBase.Settings.HighSettsLua.MultilineStringsColor);
            ColorPickerLuaLineComment.SelectedColor = (Color)ColorConverter.ConvertFromString(DataBase.Settings.HighSettsLua.LineCommentsColor);
            ColorPickerLuaDigits.SelectedColor = (Color)ColorConverter.ConvertFromString(DataBase.Settings.HighSettsLua.DigitsColor);
            ColorPickerLuaHackUndone.SelectedColor = (Color)ColorConverter.ConvertFromString(DataBase.Settings.HighSettsLua.HackColor);
            ColorPickerLuaKeyWords.SelectedColor = (Color)ColorConverter.ConvertFromString(DataBase.Settings.HighSettsLua.KeyWordsColor);
            ColorPickerLuaPunctuations.SelectedColor = (Color)ColorConverter.ConvertFromString(DataBase.Settings.HighSettsLua.PunctuationsColor);
            ColorPickerLuaUsersFunctions.SelectedColor = (Color)ColorConverter.ConvertFromString(DataBase.Settings.HighSettsLua.UserFunctionsColor);
            ColorPickerLuaTodoFixme.SelectedColor = (Color)ColorConverter.ConvertFromString(DataBase.Settings.HighSettsLua.TodoColor);
            ColorPickerLuaTables.SelectedColor = (Color)ColorConverter.ConvertFromString(DataBase.Settings.HighSettsLua.TablesColor);

            ComboBoxFontsLua.Items.Clear();
            foreach (FontFamily ff in Fonts.SystemFontFamilies)
            {
                ComboBoxFontsLua.Items.Add(ff.Source);
            }
            ComboBoxFontsLua.SelectedItem = DataBase.Settings.HighSettsLua.Font;
        }

        private void ColorPickerHightLua_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (loading) return;

            var picker = (ColorPicker)sender;
            if (picker.SelectedColor == null)
                return;

            if (Equals(picker, ColorPickerLuaBlockComment))
                DataBase.Settings.HighSettsLua.BlockCommentColor = picker.SelectedColor.Value.ToString();
            else if (Equals(picker, ColorPickerLuaChars))
                DataBase.Settings.HighSettsLua.CharColor = picker.SelectedColor.Value.ToString();
            else if (Equals(picker, ColorPickerLuaStrings))
                DataBase.Settings.HighSettsLua.StringsColor = picker.SelectedColor.Value.ToString();
            else if (Equals(picker, ColorPickerLuaMultiLineString))
                DataBase.Settings.HighSettsLua.MultilineStringsColor = picker.SelectedColor.Value.ToString();
            else if (Equals(picker, ColorPickerLuaLineComment))
                DataBase.Settings.HighSettsLua.LineCommentsColor = picker.SelectedColor.Value.ToString();
            else if (Equals(picker, ColorPickerLuaDigits))
                DataBase.Settings.HighSettsLua.DigitsColor = picker.SelectedColor.Value.ToString();
            else if (Equals(picker, ColorPickerLuaHackUndone))
                DataBase.Settings.HighSettsLua.HackColor = picker.SelectedColor.Value.ToString();
            else if (Equals(picker, ColorPickerLuaKeyWords))
                DataBase.Settings.HighSettsLua.KeyWordsColor = picker.SelectedColor.Value.ToString();
            else if (Equals(picker, ColorPickerLuaPunctuations))
                DataBase.Settings.HighSettsLua.PunctuationsColor = picker.SelectedColor.Value.ToString();
            else if (Equals(picker, ColorPickerLuaUsersFunctions))
                DataBase.Settings.HighSettsLua.UserFunctionsColor = picker.SelectedColor.Value.ToString();
            else if (Equals(picker, ColorPickerLuaTodoFixme))
                DataBase.Settings.HighSettsLua.TodoColor = picker.SelectedColor.Value.ToString();
            else if (Equals(picker, ColorPickerLuaTables))
                DataBase.Settings.HighSettsLua.TablesColor = picker.SelectedColor.Value.ToString();
            Update();
        }

        private void checkBoxFontBoldLua_CheckedChanged(object sender, EventArgs e)
        {
            if (loading) return;
            if (CheckBoxFontBoldLua.IsChecked != null)
                DataBase.Settings.HighSettsLua.Bold = (bool)CheckBoxFontBoldLua.IsChecked;
            Update();
        }

        private void checkBoxFontItalicLua_CheckedChanged(object sender, EventArgs e)
        {
            if (loading) return;
            if (CheckBoxFontItalicLua.IsChecked != null)
                DataBase.Settings.HighSettsLua.Italic = (bool)CheckBoxFontItalicLua.IsChecked;
            Update();
        }

        private void comboBoxFontsLua_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loading) return;
            DataBase.Settings.HighSettsLua.Font = (string)ComboBoxFontsLua.SelectedItem;
            Update();
        }

        private void textBoxFontSizeLua_TextChanged(object sender, EventArgs e)
        {
            if (loading) return;
            if (string.IsNullOrEmpty(TextBoxFontSizeLua.Text)) return;
            int size = int.Parse(TextBoxFontSizeLua.Text);
            DataBase.Settings.HighSettsLua.FontSize = (size == 0) ? 1 : size;
            Update();
        }

        #endregion

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void TextBoxFontSize_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !StringUtils.IsDigit(e.Text);
        }
    }
}
