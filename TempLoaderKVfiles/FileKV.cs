using System.Collections.Generic;
using System.Linq;

namespace TempLoaderKVfiles
{
    public class FileKV
    {
        public string MainKey;

        public List<ObjectStruct> ObjectList = new List<ObjectStruct>();

        public class ObjectStruct
        {
            public string Text;
            public string Name;
            public SystemComment SystemComment;
        }

        public ObjectStruct FindObject(string name)
        {
            return ObjectList.FirstOrDefault(obj => obj.Name == name);
        }

        public void RemoveObject(string name)
        {
            ObjectList.Remove(FindObject(name));
        }

        public override string ToString()
        {
            string str = "";

            foreach (var obj in ObjectList)
            {
                str += "\n";
                str += obj.SystemComment.ToString();
                str += "\"" + obj.Name + "\"\n{";
                if (obj.Text[obj.Text.Length - 1] != '\n')
                    str += "\n";
                str += obj.Text;
                if (obj.Text.Length > 0 && obj.Text[obj.Text.Length - 1] != '\n')
                    str += "\n";
                str += "}";
            }

            string ch = "";
            if (str.Length != 0)
                if (str[0] != '\n')
                    ch = "\n";

            str = "\"" + MainKey + "\"\n{" + ch + str;

            str += (str[str.Length - 1] != '\n') ? "\n}" : "}";

            return str;
        }
    }
}