using SimpleDockUILayoutLibrary.Enums;

namespace SimpleDockUILayoutLibrary.Classes
{
    public class DockLayoutModel
    {
        public DockLayoutType DockType { get; set; }
        public List<DockContentModel>? ChildContent { get; set; }
        public List<DockLayoutModel> ChildLayouts { get; set; } = new List<DockLayoutModel>(capacity: 2);
        public DockLayoutModel? Parent { get; set; }
        public string GUID { get; set; }

        public DockLayoutModel()
        {
            GUID = Guid.NewGuid().ToString();
        }

        public DockLayoutModel(DockLayoutType dockType, List<DockContentModel>? childContent, List<DockLayoutModel> childLayouts, DockLayoutModel? parent)
        {
            DockType = dockType;
            ChildContent = childContent;
            ChildLayouts = childLayouts;
            Parent = parent;
            GUID = Guid.NewGuid().ToString();

            if (ChildContent is not null)
                foreach (DockContentModel childContentModel in ChildContent)
                    childContentModel.Parent = this;
        }

        public DockLayoutModel(string guid)
        {
            GUID = guid;
        }

        public DockLayoutModel(DockLayoutType dockType, List<DockContentModel>? childContent, List<DockLayoutModel> childLayouts, DockLayoutModel? parent, string guid)
        {
            DockType = dockType;
            ChildContent = childContent;
            ChildLayouts = childLayouts;
            Parent = parent;
            GUID = guid;
        }

        internal void AppendChild(DockDirection direction, DockContentModel model)
        {
            if (model.Parent is not null)
            {
                model.Parent.RemoveChild(model.GUID);
            }

            if (ChildContent is null && ChildLayouts.Count == 0)
            {
                ChildContent = new List<DockContentModel>() { model };
                ChildContent[0].Parent = this;
                ChildContent[0].Docked = true;
                DockType = DockLayoutType.FULL;
                return;
            }

            if (direction == DockDirection.CENTER)
            {
                ChildContent.Add(model);
                model.Parent = this;
                model.Docked = true;
                DockType = DockLayoutType.FULL;
                CloseEmpty();
                return;
            }

            bool divideRoot = Parent is null && DockType != DockLayoutType.FULL;

            DockLayoutModel layoutModel = new DockLayoutModel(DockLayoutType.FULL, new List<DockContentModel>() { model }, new List<DockLayoutModel>(), this);
            DockLayoutModel layoutOld = !divideRoot
                ? new DockLayoutModel(DockLayoutType.FULL, ChildContent, new List<DockLayoutModel>(), this)
                : new DockLayoutModel(DockType,null,new List<DockLayoutModel>(),this);

            if (divideRoot)
            {
                foreach(DockLayoutModel dockLayoutModel in ChildLayouts)
                {
                    dockLayoutModel.Parent = layoutOld;
                    layoutOld.ChildLayouts.Add(layoutModel);
                }
            }

            DockType = direction == DockDirection.LEFT || direction == DockDirection.RIGHT ? DockLayoutType.HORIZONTAL : DockLayoutType.VERTICAL;

            ChildContent = null;

            ChildLayouts.Clear();

            ChildLayouts.Add(layoutOld);
            ChildLayouts.Add(layoutModel);
            model.Docked = true;

            if (direction == DockDirection.LEFT || direction == DockDirection.TOP)
            {
                ChildLayouts.Reverse();
            }

            CloseEmpty();
        }

        internal void RemoveChild(string GUID)
        {

            if (ChildContent is not null && ChildContent.Any(x => x.GUID == GUID))
            {
                if (DockType == DockLayoutType.FULL)
                {
                    if (ChildContent.Count < 2)
                        Parent?.RemoveChild(this.GUID);
                    ChildContent.RemoveAll(x => x.GUID == GUID);
                }
                else
                    ChildContent = null;
            }
            else if (ChildLayouts.Any(x => x.GUID == GUID))
            {
                List<DockContentModel>? dockContent = ChildLayouts.First(x => x.GUID != GUID).ChildContent;


                if (ChildLayouts.All(x => x.DockType == DockLayoutType.FULL))
                {
                    if (dockContent is not null)
                    {
                        DockType = DockLayoutType.FULL;
                        ChildContent = dockContent;
                        ChildLayouts.Clear();
                    }
                }
                else
                {
                    DockLayoutModel? toRemove = ChildLayouts.FirstOrDefault(x => x.GUID == GUID);
                    if (toRemove is not null && (toRemove.ChildLayouts.Count < 2 || toRemove.ChildLayouts.Any(x => x.ChildContent is null && x.ChildLayouts.Count == 0)))
                    {
                        DockLayoutModel? oldLayout = toRemove.ChildLayouts.FirstOrDefault(x => !((x.ChildContent is null || x.ChildContent.Count == 0) && x.ChildLayouts.Count == 0));
                        if (oldLayout is not null)
                        {
                            DockLayoutModel newLayout = new DockLayoutModel(oldLayout.DockType, oldLayout.ChildContent, oldLayout.ChildLayouts, this);
                            bool flip = ChildLayouts[0].GUID == GUID;
                            ChildLayouts.RemoveAll(x => x.GUID == toRemove.GUID);
                            ChildLayouts.Add(newLayout);
                            if (flip)
                                ChildLayouts.Reverse();
                        }
                    }
                }

            }
        }

        internal void CloseEmpty()
        {
            if (ChildContent is null || (ChildContent is not null && ChildContent.Count == 0))
            {
                if (ChildLayouts.Count < 2 || ChildLayouts.Any(x => (x.ChildContent is not null && x.ChildContent.Count == 0) && x.ChildLayouts.Count == 0))
                {
                    if (Parent is not null)
                        Parent.RemoveChild(this.GUID);
                    else
                    {
                        if (ChildLayouts.FirstOrDefault(x => x.ChildLayouts.Count != 0) is DockLayoutModel model)
                        {
                            DockType = model.DockType;
                            ChildContent = model.ChildContent;
                            ChildLayouts = model.ChildLayouts;

                            if (ChildContent is not null)
                                foreach (DockContentModel dockContentModel in ChildContent)
                                    dockContentModel.Parent = this;
                            foreach (DockLayoutModel childLayout in ChildLayouts)
                                childLayout.Parent = this;

                        }
                    }
                }
            }

            Parent?.CloseEmpty();
        }
    }
}
