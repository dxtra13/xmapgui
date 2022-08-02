namespace XmapGui
{
    public class Tile : WorldItem
    {
        public override bool IsTile() { return true; }

        public static string TileIdentifier(int ID)
        {
            return $"tile-{ID}";
        }

        public override WorldItemConfig[] ConfigArray()
        {
            return WorldState.TileConfig;
        }

        public Tile(int idx, int x, int y, ushort iD)
        {
            Index = idx;
            X = x;
            Y = y;
            ID = iD;
        }
    }
}
