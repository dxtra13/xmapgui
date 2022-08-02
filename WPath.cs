namespace XmapGui
{
    public class WPath : WorldItem
    {
        public override bool IsPath() { return true; }

        public static string PathIdentifier(int ID)
        {
            return $"path-{ID}";
        }

        public override WorldItemConfig[] ConfigArray()
        {
            return WorldState.PathConfig;
        }

        public WPath(int idx, int x, int y, ushort iD)
        {
            Index = idx;
            X = x;
            Y = y;
            ID = iD;
        }
    }
}
