using SimpleDockUILayoutLibrary.Components;
using SimpleDockUILayoutLibrary.Enums;

namespace SimpleDockUILayoutLibrary.Classes
{
    public class ContentDroppedEventArgs
    {
        public DockDirection DockDirection { get; set; }
        public DockContentModel DockContent { get; set; }
    }
}
