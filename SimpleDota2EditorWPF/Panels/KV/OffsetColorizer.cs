using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;

namespace SimpleDota2EditorWPF.Panels.KV
{
    public class OffsetColorizer : DocumentColorizingTransformer
    {
        public string tempText;
        private SolidColorBrush[] brushes;
        private SolidColorBrush sameSelectionsBrush;
        public string selectedText;

        public OffsetColorizer()
        {
            Update();
        }

        public void Update()
        {
            brushes = new SolidColorBrush[6];
            brushes[(int)KV_STYLES.STYLE_DEFAULT] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(DataBase.Settings.HighSetts.DefaultWordColor));
            brushes[(int)KV_STYLES.STYLE_COMMENT] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(DataBase.Settings.HighSetts.CommentColor));
            brushes[(int)KV_STYLES.STYLE_KVBLOCK] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(DataBase.Settings.HighSetts.KVBlockColor));
            brushes[(int)KV_STYLES.STYLE_KEY] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(DataBase.Settings.HighSetts.KeyColor));
            brushes[(int)KV_STYLES.STYLE_VALUE_STRING] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(DataBase.Settings.HighSetts.ValueStringColor));
            brushes[(int)KV_STYLES.STYLE_VALUE_NUMBER] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(DataBase.Settings.HighSetts.ValueNumberColor));

            sameSelectionsBrush = new SolidColorBrush(Colors.Blue);
            sameSelectionsBrush.Opacity = 0.5;
        }

        protected override void ColorizeLine(DocumentLine line)
        {
            if (line.Length == 0)
                return;

            try
            {
                var pos = line.Offset;
                var endPos = line.EndOffset;
                bool key = true; // Ожидается, что будет далее, ключ(true) или значение(false)

                var ch = tempText[pos];
                colorizeSelectedWordInLine(tempText.Substring(pos, line.Length), pos);
                while (pos < endPos)
                {
                    ch = tempText[pos];
                    if (!SomeUtils.StringUtils.IsSpaceOrTab(ch) || ch != '\n')
                        switch (ch)
                        {
                            case '\"':
                                int end = nextCharThroughIs(tempText, pos, '\"');
                                if (end == -1)
                                {
                                    ChangeLinePart(pos, endPos + 1, element => element.TextRunProperties.SetForegroundBrush(
                                        brushes[key ? (int)KV_STYLES.STYLE_KEY : (int)KV_STYLES.STYLE_VALUE_STRING]));
                                    return;
                                }

                                int isBlock = nextCharThroughIs(tempText, end + 1, '{');
                                if (isBlock != -1)
                                {
                                    key = true;
                                    if (end > endPos) end = endPos;
                                    ChangeLinePart(pos, end + 1, element => element.TextRunProperties.SetForegroundBrush(
                                        brushes[(int)KV_STYLES.STYLE_KVBLOCK]));
                                    pos = end;
                                }
                                else
                                {
                                    string str = tempText.Substring(pos + 1, end - (pos + 1));
                                    int style = 0;
                                    if (SomeUtils.StringUtils.IsDigit(str))
                                        style = (int)KV_STYLES.STYLE_VALUE_NUMBER;
                                    else
                                        style = key ? (int)KV_STYLES.STYLE_KEY : (int)KV_STYLES.STYLE_VALUE_STRING;

                                    if (end > endPos) end = endPos;
                                    ChangeLinePart(pos, end + 1, element => element.TextRunProperties.SetForegroundBrush(
                                        brushes[style]));
                                    pos = end;
                                    key = !key;
                                }
                                break;

                            case '/':
                                if (pos + 1 >= tempText.Length)
                                    return;
                                if (tempText[pos + 1] != '/')
                                    break;

                                ChangeLinePart(pos, endPos, element => element.TextRunProperties.SetForegroundBrush(
                                        brushes[(int)KV_STYLES.STYLE_COMMENT]));
                                return;
                                break;

                            case '{':
                            case '}':
                                ChangeLinePart(pos, pos + 1, element => element.TextRunProperties.SetForegroundBrush(
                                        brushes[(int)KV_STYLES.STYLE_DEFAULT]));
                                break;
                        }

                    pos++;
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(@"Error - " + ex.Message);

            }
        }

        private void colorizeSelectedWordInLine(string line, int offset)
        {
            if (String.IsNullOrWhiteSpace(selectedText) || String.IsNullOrWhiteSpace(line))
                return;

            int i = line.IndexOf(selectedText, StringComparison.Ordinal);
            int end = 0;
            while (i != -1)
            {
                end = i + selectedText.Length;
                i += offset;

                ChangeLinePart(i, end + offset, element => element.TextRunProperties.SetBackgroundBrush(
                                        sameSelectionsBrush));

                if (line.Length <= end)
                    return;

                line = line.Substring(end);

                offset += end;
                i = line.IndexOf(selectedText, StringComparison.Ordinal);
            }

        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum KV_STYLES
        {
            STYLE_DEFAULT,
            STYLE_KEY,
            STYLE_KVBLOCK,
            STYLE_VALUE_NUMBER,
            STYLE_VALUE_STRING,
            STYLE_COMMENT,
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


    }
}