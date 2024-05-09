using AdaptoUILibrary.Components;
using AdaptoUILibrary.Enums;

namespace AdaptoUILibrary.Classes
{
    public class ContentDroppedEventArgs
    {
        public DockDirection DockDirection { get; set; }
        public DockContentModel DockContent { get; set; }
        public bool OnRoot { get; set; }
    }
}
