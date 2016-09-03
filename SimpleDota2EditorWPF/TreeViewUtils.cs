using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using KV_reloaded;

namespace SimpleDota2EditorWPF
{
    public static class TreeViewUtils
    {
        public static TreeViewItem FindItem(this ItemCollection items, string name)
        {
            return items.Cast<TreeViewItem>().FirstOrDefault(node => ((string)node.Tag) == string.Concat("#", name));
        }

        public static string GetItemPath(this TreeViewItem item, string lastPath = "")
        {
            if (item.Parent is TreeViewItem)
            {
                string str = ((TreeViewItem)item.Parent).GetItemPath(lastPath);
                lastPath = str + "\\" + item.Header;
            }
            else
                lastPath = (string)item.Header;

            return lastPath;
        }

        public static void RenameChildsFolders(this TreeViewItem item, KVToken kvToken, string path)
        {
            foreach (TreeViewItem nod in item.Items)
            {
                if (nod.IsFolder())
                {
                    nod.RenameChildsFolders(kvToken, path + "\\" + nod.Header);
                }
                else
                {
                    var obj = kvToken.GetChild((string)nod.Header);
                    if (obj.SystemComment == null)
                        obj.SystemComment = new SystemComment();
                    obj.SystemComment.DeleteKV("Folder");
                    obj.SystemComment.AddKV("Folder", path);
                }
            }
        }

        public static void DeleteChilds(this TreeViewItem item, KVToken kvToken, List<KVToken> deletedTokens)
        {
            foreach (TreeViewItem nod in item.Items)
            {
                if (nod.IsFolder())
                {
                    nod.DeleteChilds(kvToken, deletedTokens);
                }
                else
                {
                    deletedTokens.Add(kvToken.GetChild((string)nod.Header));
                    kvToken.RemoveChild((string)nod.Header);
                }
            }
        }

        public static bool IsFolder(this TreeViewItem node)
        {
            if (node?.Tag is string)
                return ((string)node.Tag).Contains("#");

            return false;
        }

        public static TreeViewItem FindItemLike(this ItemCollection items, TreeViewItem item)
        {
            foreach (TreeViewItem n in items)
            {
                if (n.IsLikeItem(item))
                    return n;
                if (n.IsFolder())
                {
                    var temp = n.Items.FindItemLike(item);
                    if (temp != null)
                        return temp;
                }
            }

            return null;
        }

        public static bool IsLikeItem(this TreeViewItem origItem, TreeViewItem item)
        {
            if (origItem == null || item == null)
                return false;

            if (Equals(origItem, item))
                return true;

            return (origItem.Tag?.ToString() == item.Tag?.ToString() 
                && origItem.Header?.ToString() == item.Header?.ToString() 
                && origItem.Uid == item.Uid);
        }

        public static ItemCollection GetParentItemCollection(this TreeViewItem item)
        {
            if (item.Parent is TreeViewItem)
                return ((TreeViewItem) item.Parent).Items;
            else
                return ((TreeView)item.Parent).Items;
        }

        public static bool CanMoveItemToItem(TreeViewItem movingItem, TreeViewItem targetItem)
        {
            if (movingItem == null) return false;
            if (targetItem == null) return true;

            if (Equals(movingItem, targetItem)) return false;

            //if (targetItem.ItemContainsItem(movingItem)) return false;
            if (movingItem.ItemContainsItem(targetItem)) return false;

            return true;
        }

        public static bool ItemContainsItem(this TreeViewItem item1, TreeViewItem item2)
        {
            foreach (TreeViewItem itm in item1.Items)
            {
                if (Equals(itm, item2))
                    return true;

                if (itm.ItemContainsItem(item2))
                    return true;
            }

            return false;
        }

        public static T FindParent<T>(FrameworkElement current)
            where T : FrameworkElement
        {
            do
            {
                current = VisualTreeHelper.GetParent(current) as FrameworkElement;
                if (current is T)
                {
                    return (T)current;
                }
            }
            while (current != null);
            return null;
        }

        // Helper to search up the VisualTree
        public static T FindAnchestor<T>(DependencyObject current)
            where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }

        public static void SwapItems(this ItemCollection collection, object obj1, object obj2)
        {
            int obj1Index = collection.IndexOf(obj1);
            int obj2Index = collection.IndexOf(obj2);
            if (obj1Index == obj2Index) return;
            collection.Remove(obj1);
            collection.Remove(obj2);
            if (obj1Index < obj2Index)
            {
                collection.Insert(obj1Index, obj2);
                collection.Insert(obj2Index, obj1);
            }
            else
            {
                collection.Insert(obj2Index, obj1);
                collection.Insert(obj1Index, obj2);
            }
        }

        #region sorting

        class ItemSorterRule : IComparer
        {
            //True: A-Z. False: Z-A
            public bool SortByAlphavet;

            public int Compare(object x, object y)
            {
                TreeViewItem itemX = (TreeViewItem)x;
                TreeViewItem itemY = (TreeViewItem)y;

                if (itemX.IsFolder() && !itemY.IsFolder())
                    return -10;
                if (!itemX.IsFolder() && itemY.IsFolder())
                    return 10;

                return String.Compare((string)itemX.Header, (string)itemY.Header, StringComparison.Ordinal);
            }
        }

        static ItemSorterRule itemSorterRule = new ItemSorterRule();

        public static void Sort(this ItemCollection collection, bool recursive = true)
        {
            QuickSortCollection(collection, 0, collection.Count - 1);
            if (recursive)
                foreach (TreeViewItem item in collection.Cast<TreeViewItem>().Where(item => item.Items.Count > 1))
                {
                    item.Items.Sort();
                }
        }

        private static void QuickSortCollection(ItemCollection collection, int a, int b)
        {
            int A = a;
            int B = b;
            object mid;

            if (b > a)
            {

                // Находим разделительный элемент в середине массива
                mid = collection[(a + b) / 2];

                // Обходим массив
                while (A <= B)
                {
                    /* Находим элемент, который больше или равен
                    * разделительному элементу от левого индекса.
                    */
                    //while ((A < b) && (collection[A] < mid)) ++A;
                    while ((A < b) && (itemSorterRule.Compare(collection[A], mid)) < 0) ++A;

                    /* Находим элемент, который меньше или равен
                     * разделительному элементу от правого индекса.
                     */
                    //while ((B > a) && (collection[B] > mid)) --B;
                    while ((B > a) && (itemSorterRule.Compare(collection[B], mid)) > 0) --B;

                    // Если индексы не пересекаются, меняем
                    if (A <= B)
                    {
                        collection.SwapItems(collection[A], collection[B]);

                        ++A;
                        --B;
                    }
                }

                /* Если правый индекс не достиг левой границы массива,
                 * нужно повторить сортировку левой части.
                 */
                if (a < B) QuickSortCollection(collection, a, B);

                /* Если левый индекс не достиг правой границы массива,
                 * нужно повторить сортировку правой части.
                 */
                if (A < b) QuickSortCollection(collection, A, b);

            }
        }

        #endregion

    }
}