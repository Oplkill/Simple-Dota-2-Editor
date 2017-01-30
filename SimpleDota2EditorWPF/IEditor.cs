using System;
using System.ComponentModel;
using KV_reloaded;
using SimpleDota2EditorWPF.Dialogs;
using SimpleDota2EditorWPF.Panels;
using Xceed.Wpf.AvalonDock.Layout;

namespace SimpleDota2EditorWPF
{
    public interface IEditor
    {
        bool Edited { get; set; }
        string PanelName { get; set; }
        KVToken ObjectRef { get; set; }
        ObjectsViewPanel.ObjectTypePanel ObjectType { get; set; }
        Settings.EditorType EditorType { get; }
        void ForceClose();
        ErrorParser SaveChanges();
        void Closing(object sender, CancelEventArgs e);
        LayoutDocument PanelDocument { get; set; }
        void Update();
        void IsActiveChanged(object sender, EventArgs e);
        IEditor ParentEditor { get; set; }

        /// <summary>
        /// Find next, after caret selection
        /// </summary>
        /// <returns>Reached end of document?</returns>
        bool FindNext(FindStruct find);
        /// <summary>
        /// Find prev, before caret selection
        /// </summary>
        /// <returns>Reached start of document?</returns>
        bool FindPrev(FindStruct find);
        /// <summary>
        /// Count all substrings of text in document
        /// </summary>
        /// <returns>Number substrings of text in document</returns>
        int CountIt(FindStruct find);
        /// <summary>
        /// Replacing one pare of text in document and finding next
        /// </summary>
        /// <returns>Is all replaced</returns>
        bool Replace(FindStruct find);
    }
}