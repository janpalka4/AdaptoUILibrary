using AdaptoUILibrary.Components;

namespace AdaptoUILibrary.Classes
{
    internal class DragTemp
    {
        internal EventHandler<bool> IsWeightBeingResizedChanged;

        internal DockContentModel? Item { get; set; }
        internal bool IsWeightBeingResized { get; set; }

        internal void SetIsWeightBeingResized(bool isWeightBeingResized)
        {
            IsWeightBeingResized = isWeightBeingResized;
            IsWeightBeingResizedChanged?.Invoke(this,IsWeightBeingResized);
        }
    }
}
