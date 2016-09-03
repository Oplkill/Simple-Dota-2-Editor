using System;
using System.ComponentModel;
using KV_reloaded;
using SimpleDota2EditorWPF.Panels;
using Xceed.Wpf.AvalonDock.Layout;

namespace SimpleDota2EditorWPF
{
    public interface IEditor
    {
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
    }
}