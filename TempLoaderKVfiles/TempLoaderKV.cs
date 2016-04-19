using System.Linq;

namespace TempLoaderKVfiles
{ //todo ahtung весь этот неймспейс - это гавнокод, подстроенный только под одни условия, в нем хрен что разберешь и я хз чем лучше его заменить или нормально сделать, всем тем кто знает как лучше, отредактируйте =)
    public class TempLoaderKV
    {
        public static FileKV LoadFile(string fileTxt)
        {
            FileKV fileKv = new FileKV();

            int n = 0;
            string temp = "";

            int b = 0;
            b = find(fileTxt, n, '\"') + 1;
            n = find(fileTxt, n, '\'');
            fileKv.MainKey = fileTxt.Substring(b, n - b);
            n++;
            n = find(fileTxt, n, '{') + 1;

            while (true)
            {
                temp = getBlock(fileTxt, ref n);
                if (temp == null)
                    break;

                FileKV.ObjectStruct objStruct = new FileKV.ObjectStruct
                {
                    Name = GetObjectName(temp),
                    SystemComment = SystemComment.AnalyseSystemComment(GetAndCutSystemComment(ref temp)),
                    Text = CutObjectStructure(temp)
                };

                fileKv.ObjectList.Add(objStruct);
            }

            return fileKv;
        }

        public static string GetAndCutSystemComment(ref string text)
        {
            int b = 0, m = 0;
            b = find(text, 0, '\"') + 1;
            if ((m = find(text.Substring(0, b), 0, '@')) != -1)
            {
                int end = find(text, m, '\n');
                string sysComm = text.Substring(m, end - m);
                m = FindStartLine(text, m);
                text = text.Substring(0, m) + text.Substring(end + 1);
                return sysComm;
            }

            return "";
        }

        public static string CutObjectStructure(string text)
        {
            string comments = "";
            int b = find(text, 0, '\"');
            comments = text.Substring(0, b);
            if (text.All(t => t == '\n' || t == '\r' || t == '\t'))
                comments = "";
            text = text.Substring(b);
            b = find(text, 0, '{');
            text = text.Substring(b);
            int m = find(text, 0, '}');
            text = text.Substring(1, m - 1);
            text = comments + text;

            return text;
        }

        private static int FindStartLine(string text, int n)
        {
            while (n > 0)
            {
                if (text[n] == '\n')
                {
                    return n + 1;
                }

                n--;
            }

            return n;
        }

        public static string GetObjectName(string text)
        {
            string name = "";
            int b = 0;

            b = find(text, 0, '\"') + 1;
            name = text.Substring(b, find(text, 0, '\'') - b);

            return name;
        }

        private static string getBlock(string text, ref int n)
        {
            string temp = "";

            int b = find(text, n, '}');
            if (b == -2)
                return null;

            temp = text.Substring(n, b + 1 - n);
            n = ++b;

            return temp;
        }

        private static int find(string text, int n, char ch)
        {
            int brackets = 0;
            bool exit = false;
            while (!exit)
            {
                switch (text[n])
                {
                    case '{':
                        brackets++;
                        if (ch == '{')
                            return n;
                        break;

                    case '}':
                        brackets--;
                        if (brackets == 0)
                        {
                            exit = true;
                            if (ch == '}')
                                return n;
                        }
                        else if (brackets < 0)
                        {
                            return -2;
                        }
                        break;

                    case '/':
                        if (text[n + 2] == '@' && ch == '@')
                            return n;

                        while (text[n] != '\n')
                        {
                            n++;
                        }

                        if (ch == '\n')
                            return n;
                        break;

                    case '\"':
                        if (ch == '\"')
                            return n;
                        n++;
                        if (n >= text.Length)
                            return -1;

                        while (text[n] != '\"')
                        {
                            n++;
                        }
                        if (ch == '\'')
                            return n;
                        break;
                }

                n++;
            }

            return -1;
        }

    }
}
