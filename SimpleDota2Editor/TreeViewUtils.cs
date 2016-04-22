using System.Linq;
using System.Windows.Forms;
using KV_reloaded;

namespace SimpleDota2Editor
{
    public static class TreeViewUtils
    {
        public static TreeNode FindNode(this TreeNodeCollection treeNodes, string name)
        {
            return treeNodes.Cast<TreeNode>().FirstOrDefault(node => node.Name == "#" + name);
        }

        public static string GetNodePath(this TreeNode node, string lastPath)
        {
            if (node.Parent != null)
            {
                string str = node.Parent.GetNodePath(lastPath);
                lastPath = str + "\\" + node.Text;
            }
            else
                lastPath = node.Text;

            return lastPath;
        }

        public static void RenameChildsFolders(this TreeNode node, KVToken kvToken, string path)
        {
            foreach (TreeNode nod in node.Nodes)
            {
                if (nod.Name.Contains("#"))
                {
                    nod.RenameChildsFolders(kvToken, path + "\\" + nod.Text);
                }
                else
                {
                    var obj = kvToken.GetChild(nod.Text);
                    var kv = obj.SystemComment.FindKV("Folder");
                    kv.Value = path;
                }
            }
        }

        public static void DeleteChilds(this TreeNode node, KVToken kvToken)
        {
            foreach (TreeNode nod in node.Nodes)
            {
                if (nod.Name.Contains("#"))
                {
                    nod.DeleteChilds(kvToken);
                }
                else
                {
                    kvToken.RemoveChild(nod.Text);
                }
            }
        }
    }
}