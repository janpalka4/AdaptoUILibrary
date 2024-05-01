﻿using SimpleDockUILayoutLibrary.Enums;

namespace SimpleDockUILayoutLibrary.Classes
{
    public class DockLayoutModel
    {
        public DockLayoutType DockType { get; set; }
        public DockContentModel? ChildContent { get; set; }
        public List<DockLayoutModel> ChildLayouts { get; set; } = new List<DockLayoutModel>(capacity: 2);
        public DockLayoutModel? Parent { get; set; }
        public string GUID { get; set; }

        public DockLayoutModel()
        {
            GUID = Guid.NewGuid().ToString();
        }

        public DockLayoutModel (DockLayoutType dockType, DockContentModel? childContent, List<DockLayoutModel> childLayouts, DockLayoutModel? parent)
        {
            DockType = dockType;
            ChildContent = childContent;
            ChildLayouts = childLayouts;
            Parent = parent;
            GUID = Guid.NewGuid().ToString();

            if(ChildContent is not null)
                ChildContent.Parent = this;
        }

        public DockLayoutModel(string guid)
        {
            GUID = guid;
        }

        public DockLayoutModel(DockLayoutType dockType, DockContentModel? childContent, List<DockLayoutModel> childLayouts, DockLayoutModel? parent,string guid)
        {
            DockType = dockType;
            ChildContent = childContent;
            ChildLayouts = childLayouts;
            Parent = parent;
            GUID = guid;
        }

        internal void AppendChild(DockDirection direction, DockContentModel model) 
        {
            if(model.Parent is not null)
            {
                model.Parent.RemoveChild(model.GUID);
            }

            if(ChildContent is null)
            {
                ChildContent = model;
                ChildContent.Parent = this;
                ChildContent.Docked = true;
                DockType = DockLayoutType.FULL;
                return;
            }

            DockType = direction == DockDirection.LEFT || direction == DockDirection.RIGHT ? DockLayoutType.HORIZONTAL : DockLayoutType.VERTICAL;

            DockLayoutModel layoutModel = new DockLayoutModel(DockLayoutType.FULL, model, new List<DockLayoutModel>(), this);
            DockLayoutModel layoutOld = new DockLayoutModel(DockLayoutType.FULL, ChildContent, new List<DockLayoutModel>(), this);

            ChildContent = null;

            ChildLayouts.Add(layoutOld);
            ChildLayouts.Add(layoutModel);
            model.Docked = true;

            if(direction == DockDirection.LEFT || direction == DockDirection.TOP) 
            {
                ChildLayouts.Reverse();
            }

            CloseEmpty();
        }

        internal void RemoveChild(string GUID)
        {
            if(ChildContent is not null && ChildContent.GUID == GUID)
            {
                if(DockType == DockLayoutType.FULL)
                {
                    Parent?.RemoveChild(this.GUID);
                    ChildContent = null;
                }else
                    ChildContent = null;
            }
            else if(ChildLayouts.Any(x => x.GUID == GUID))
            {
                DockContentModel? dockContent = ChildLayouts.First(x => x.GUID != GUID).ChildContent;

                if(dockContent is not null)
                {
                    if (ChildLayouts.All(x => x.DockType == DockLayoutType.FULL))
                    {
                        DockType = DockLayoutType.FULL;
                        ChildContent = dockContent;
                        ChildLayouts.Clear();
                    }
                    else
                    {
                        DockLayoutModel? toRemove = ChildLayouts.FirstOrDefault(x => x.GUID == GUID);
                        if(toRemove is not null && (toRemove.ChildLayouts.Count < 2 || toRemove.ChildLayouts.Any(x => x.ChildContent is null && x.ChildLayouts.Count == 0)))
                        {
                            DockLayoutModel? oldLayout = toRemove.ChildLayouts.FirstOrDefault(x => !(x.ChildContent is null && x.ChildLayouts.Count == 0));
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
        }

        internal void CloseEmpty()
        {
            if (ChildContent is null) 
            { 
                if(ChildLayouts.Count < 2 || ChildLayouts.Any(x => x.ChildContent is null && x.ChildLayouts.Count == 0))
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

                            if(ChildContent is not null)
                                ChildContent.Parent = this;
                            foreach(DockLayoutModel childLayout in ChildLayouts)
                                childLayout.Parent = this;

                        }
                    }
                }
            }

            Parent?.CloseEmpty();
        }
    }
}