namespace XmapGui
{
    public class Level : WorldItem
    {
        public override bool IsLevel() { return true; }

        public bool PathBackground { get; private set; }
        public bool BigBackground { get; private set; }

        public static string LevelIdentifier(int ID)
        {
            return $"level-{ID}";
        }

        public override WorldItemConfig[] ConfigArray()
        {
            return WorldState.LevelConfig;
        }

        public Level(int idx, int x, int y, ushort iD, bool pb, bool bb)
        {
            Index = idx;
            X = x;
            Y = y;
            ID = iD;
            PathBackground = pb;
            BigBackground = bb;
        }
    }
}
