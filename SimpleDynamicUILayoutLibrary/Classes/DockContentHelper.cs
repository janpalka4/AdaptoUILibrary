

using SimpleDockUILayoutLibrary.Components;

namespace SimpleDockUILayoutLibrary.Classes
{
    internal class DockContentHelper
    {
        internal EventHandler<DockContentModel> OnContentWindowCreationRequested { get; set; }
        internal EventHandler<DockContentModel> OnContentWindowRemovalRequested { get; set; }

        internal void CreateContentWindow(DockContentModel content)
        {
            if(OnContentWindowCreationRequested is not null)
                OnContentWindowCreationRequested.Invoke(this,content);
        }

        internal void RemoveContentWindow(DockContentModel content)
        {
            if(OnContentWindowRemovalRequested is not null)
                OnContentWindowRemovalRequested.Invoke(this,content);
        }
    }
}
