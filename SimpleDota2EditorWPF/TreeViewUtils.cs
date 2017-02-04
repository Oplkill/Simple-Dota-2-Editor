using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using KV_reloaded;

namespace SimpleDota2EditorWPF
{
    public static class TreeViewUtils
    {
        /// <summary>
        /// Recursive finding any item in this item collection
        /// </summary>
        /// <param name="skipItems">How many skip finded items. (-1) or (0) for get first match item</param>
        /// <param name="sensitivity">Text sensivity</param>
        /// <returns>Returns null if didnt finded</returns>
        public static TreeViewItem RecursiveFindItem(this ItemCollection items, string name, ref int skipItems, bool sensitivity, bool fullNameItem)
        {
            if (!sensitivity)
                name = name.ToLower();

            foreach (TreeViewItem item in items)
            {
                var headerText = item.GetHeaderTextBlock();
                if (!sensitivity)
                    headerText.Text = headerText.Text.ToLower();
                if (headerText.Text == name || (!fullNameItem && headerText.Text.Contains(name)))
                    if (skipItems-- <= 0)
                        return item;
                var retItem = item.Items?.RecursiveFindItem(name, ref skipItems, sensitivity, fullNameItem);
                if (retItem != null)
                    return retItem;
            }

            return null;
        }

        public static TreeViewItem FindItem(this ItemCollection items, string name)
        {
            return items.Cast<TreeViewItem>().FirstOrDefault(node => ((string)node.Tag) == string.Concat("#", name));
        }

        public static string GetItemPath(this TreeViewItem item, string lastPath = "")
        {
            if (item.Parent is TreeViewItem)
            {
                string str = ((TreeViewItem)item.Parent).GetItemPath(lastPath);
                lastPath = str + "\\" + item.GetHeaderTextBlock().Text;
            }
            else
                lastPath = item.GetHeaderTextBlock().Text;

            return lastPath;
        }

        public static void RenameChildsFolders(this TreeViewItem item, KVToken kvToken, string path)
        {
            foreach (TreeViewItem nod in item.Items)
            {
                if (nod.IsFolder())
                {
                    nod.RenameChildsFolders(kvToken, path + "\\" + nod.GetHeaderTextBlock().Text);
                }
                else
                {
                    var obj = kvToken.GetChild(nod.GetHeaderTextBlock().Text);
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
                    deletedTokens.Add(kvToken.GetChild(nod.GetHeaderTextBlock().Text));
                    kvToken.RemoveChild(nod.GetHeaderTextBlock().Text);
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
                && origItem.GetHeaderTextBlock().Text == item.GetHeaderTextBlock().Text
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

        public static void ExpandParentsItems(this TreeViewItem item)
        {
            if (item.Parent is TreeViewItem)
            {
                ((TreeViewItem) item.Parent).IsExpanded = true;
                ((TreeViewItem)item.Parent).ExpandParentsItems();
            }
        }

        public static TextBlock GetHeaderTextBlock(this TreeViewItem item)
        {
            return (TextBlock)((StackPanel)item.Header).Children[item.IsFolder() ? 2 : 1];
        }

        //public static string GetIconFolder()
        //{
        //    string execName =
        //       Assembly.GetExecutingAssembly().
        //          GetModules()[0].FullyQualifiedName;
        //    string currentFolder = System.IO.
        //          Path.GetDirectoryName(execName);
        //    string icons = System.IO.Path.Combine(
        //          currentFolder, "icons");
        //    return icons;
        //}

        public static void ItemFolderCollapsedExpanded(object sender, RoutedEventArgs args)
        {
            var pan = ((StackPanel)((TreeViewItem)sender).Header).Children;
            bool showFirstImg = args.RoutedEvent == TreeViewItem.CollapsedEvent;
            pan[0].Visibility = showFirstImg ? Visibility.Visible : Visibility.Collapsed;
            pan[1].Visibility = !showFirstImg ? Visibility.Visible : Visibility.Collapsed;
            args.Handled = true;
        }

        public static TreeViewItem CreateTreeViewItemFolder(string header, Image img1, Image img2, string uid, object tag = null)
        {
            TreeViewItem child = new TreeViewItem() { Uid = uid, Tag = tag };
            StackPanel pan = new StackPanel();
            pan.Orientation = Orientation.Horizontal;

            pan.Children.Add(new Image() { Height = img1.Height, Source = img1.Source });
            pan.Children.Add(new Image() { Height = img2.Height, Source = img2.Source });
            pan.Children[1].Visibility = Visibility.Collapsed;

            pan.Children.Add(new TextBlock() { Text = header });
            child.Header = pan;
            return child;
        }

        public static TreeViewItem CreateTreeViewItem(string header, string iconPath, string uid, object tag = null)
        {
            TreeViewItem child = new TreeViewItem() {Uid = uid, Tag = tag};
            StackPanel pan = new StackPanel();
            if (!String.IsNullOrEmpty(iconPath))
            {
                pan.Orientation = Orientation.Horizontal;

                PngBitmapDecoder icon = 
                    new PngBitmapDecoder(new Uri(iconPath, UriKind.RelativeOrAbsolute), 
                    BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                //BitmapSource bitmapSource = decoder.Frames[0];

                //IconBitmapDecoder icon = new IconBitmapDecoder(
                //    new Uri(iconPath, UriKind.RelativeOrAbsolute),
                //    BitmapCreateOptions.None,
                //    BitmapCacheOption.OnLoad);
                Image image = new Image();
                image.Height = 16;
                image.Source = icon.Frames[0];
                pan.Children.Add(image);
            }
            else
                pan.Children.Add(new Image());
            pan.Children.Add(new TextBlock() {Text = header});
            child.Header = pan;
            return child;
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

                return String.Compare(itemX.GetHeaderTextBlock().Text, itemY.GetHeaderTextBlock().Text, 
                    StringComparison.Ordinal);
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