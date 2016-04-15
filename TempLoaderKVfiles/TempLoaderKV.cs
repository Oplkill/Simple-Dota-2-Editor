using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempLoaderKVfiles
{ //todo ahtung весь этот неймспейс - это гавнокод, подстроенный только под одни условия
    public class TempLoaderKV
    {
        public static FileKV LoadFile(string fileTxt)
        {
            FileKV fileKv = new FileKV();

            int n = 0;
            string temp = "";
            

            {
                int b = 0;
                b = find(fileTxt, n, '\"') + 1;
                n = find(fileTxt, n, '\'');
                fileKv.MainKey = fileTxt.Substring(b, n - b);
                n++;
                n = find(fileTxt, n, '{') + 1;
            }

            while (true)
            {
                temp = getBlock(fileTxt, ref n);
                if (temp == null)
                    break;

                FileKV.ObjectStruct objStruct = new FileKV.ObjectStruct();
                objStruct.Text = temp;

                int b = 0, m = 0;
                b = find(temp, 0, '\"') + 1;
                if ((m = find(temp.Substring(0, b), 0, '@')) != -1)
                {
                    objStruct.SystemComment = temp.Substring(m, find(temp, m, '\n') - m);
                }
                objStruct.Name = temp.Substring(b, find(temp, 0, '\'') - b);

                fileKv.ObjectList.Add(objStruct);
            }

            return fileKv;
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
