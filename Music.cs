namespace XmapGui
{
    public class Music : WorldItem
    {
        public override bool IsMusic() { return true; }

        public static string MusicIdentifier()
        {
            return "music";
        }

        public override WorldItemConfig[] ConfigArray()
        {
            return new WorldItemConfig[17];
        }

        public Music(int idx, int x, int y, ushort iD)
        {
            Index = idx;
            X = x;
            Y = y;
            ID = iD;
        }
    }
}
