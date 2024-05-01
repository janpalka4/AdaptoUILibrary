

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

        public DockContentModel() { 
            GUID = Guid.NewGuid().ToString();
        }

        public DockContentModel(string GUID)
        {
            this.GUID = GUID;
        }

    }
}
