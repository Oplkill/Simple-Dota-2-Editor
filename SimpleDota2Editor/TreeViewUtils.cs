using System;
using System.Collections;
using System.Collections.Generic;
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
                if (nod.IsFolder())
                {
                    nod.RenameChildsFolders(kvToken, path + "\\" + nod.Text);
                }
                else
                {
                    var obj = kvToken.GetChild(nod.Text);
                    if (obj.SystemComment == null)
                    {
                        obj.SystemComment = new SystemComment();
                        obj.SystemComment.AddKV(new KV() { Key = "Folder", Value = path });
                    }
                    else
                    {
                        var kv = obj.SystemComment.FindKV("Folder");
                        if (kv == null)
                            obj.SystemComment.AddKV(new KV() { Key = "Folder", Value = path });
                        else
                            kv.Value = path;
                    }
                }
            }
        }

        public static void DeleteChilds(this TreeNode node, KVToken kvToken, List<KVToken> deletedTokens)
        {
            foreach (TreeNode nod in node.Nodes)
            {
                if (nod.IsFolder())
                {
                    nod.DeleteChilds(kvToken, deletedTokens);
                }
                else
                {
                    deletedTokens.Add(kvToken.GetChild(nod.Text));
                    kvToken.RemoveChild(nod.Text);
                }
            }
        }

        public static bool IsFolder(this TreeNode node)
        {
            if (node == null)
                return false;

            return node.Name.Contains("#");
        }

        public static TreeNode FindNodeLike(this TreeNodeCollection nodes, TreeNode node)
        {
            foreach (TreeNode n in nodes)
            {
                if (n.IsLikeNode(node))
                    return n;
                if (n.Nodes.Count > 0)
                {
                    var temp = n.Nodes.FindNodeLike(node);
                    if (temp.IsLikeNode(node))
                        return temp;
                }
            }

            return null;
        }

        public static bool IsLikeNode(this TreeNode origNode, TreeNode node)
        {
            if (origNode == null || node == null)
                return false;

            if (origNode == node)
                return true;

            return (origNode.Name == node.Name && origNode.Text == node.Text && origNode.Index == node.Index);
        }
    }

    class NodeSorter : IComparer
    {
        public int Compare(object x, object y)
        {
            TreeNode nodeX = (TreeNode)x;
            TreeNode nodeY = (TreeNode)y;

            if (nodeX.IsFolder() && !nodeY.IsFolder())
                return 0;
            if (!nodeX.IsFolder() && nodeY.IsFolder())
                return 1;

            return String.Compare(nodeX.Text, nodeY.Text, StringComparison.Ordinal);
        }
    }
}