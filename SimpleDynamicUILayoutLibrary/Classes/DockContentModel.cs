

using SimpleDockUILayoutLibrary.Components;

namespace SimpleDockUILayoutLibrary.Classes
{
    public class DockContentModel
    {
        public Type ContentType { get; set; }
        public DockLayoutModel? Parent { get; set; }
        public Dictionary<string, object>? ContentParameters { get; set; }
        public bool Docked { get; set; }
        public string GUID { get; private set; }
        public string Name { get; set; } = "Content";

        public DockContentModel(string Name) { 
            GUID = Guid.NewGuid().ToString();
            this.Name = Name;
        }

        public DockContentModel(string GUID,string Name)
        {
            this.GUID = GUID;
            this.Name = Name;
        }

    }
}
