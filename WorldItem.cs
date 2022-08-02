using System;

namespace XmapGui
{
    public abstract class WorldItem : IComparable<WorldItem>
    {
        public virtual int X { get; protected set; }
        public virtual int Y { get; protected set; }
        public virtual int Index { get; protected set; }
        public virtual ushort ID { get; protected set; }

        public virtual bool IsTile() { return false; }
        public virtual bool IsScene() { return false; }
        public virtual bool IsPath() { return false; }
        public virtual bool IsLevel() { return false; }
        public virtual bool IsMusic() { return false; }
        public virtual bool IsAnime() { return false; }
        public virtual bool IsMisc() { return false; }

        public abstract WorldItemConfig[] ConfigArray();

        public int CompareTo(WorldItem other)
        {
            if (!(IsMusic() || other.IsMusic()) && !(IsMisc() || other.IsMisc()))
            {
                double SelfPriority = ConfigArray()[ID].Priority;
                double OtherPriority = other.ConfigArray()[other.ID].Priority;

                if (SelfPriority > OtherPriority)
                    return 1;
                if (SelfPriority < OtherPriority)
                    return -1;
            }

            if (IsTile())
            {
                if (!other.IsTile())
                    return -1;
            }
            else if (IsScene())
            {
                if (!other.IsScene())
                {
                    if (other.IsTile())
                        return 1;
                    return -1;
                }
            }
            else if (IsPath())
            {
                if (!other.IsPath())
                {
                    if (other.IsTile() || other.IsScene())
                        return 1;
                    return -1;
                }
            }
            else if (IsLevel())
            {
                if (!other.IsLevel())
                {
                    if (other.IsTile() || other.IsScene())
                        return 1;
                    return -1;
                }
            }
            else if (IsMusic())
            {
                if (!other.IsMusic())
                    return 1;
            }
            else if (IsAnime())
            {
                if (!other.IsAnime())
                {
                    if (other.IsMusic() || other.IsMisc())
                        return -1;
                    return 1;
                }
            }
            else if (IsMisc())
            {
                if (!other.IsMisc())
                    return 1;
            }

            return Index - other.Index;
        }
    }
}
