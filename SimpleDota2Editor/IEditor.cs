using KV_reloaded;
using SimpleDota2Editor.Panels;

namespace SimpleDota2Editor
{
    public interface IEditor
    {
        string PanelName { get; set; }
        KVToken ObjectRef { get; set; }
        ObjectsViewPanel.ObjectTypePanel ObjectType { get; set; }
        Settings.EditorType EditorType { get; }
    }
}