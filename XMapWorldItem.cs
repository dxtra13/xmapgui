namespace XmapGui
{
    public class XMapWorldItem : WorldItem
    {
        public override WorldItemConfig[] ConfigArray()
        {
            return new WorldItemConfig[5];
        }

        public void CalibIndex()
        {
            Index--;
        }

        public void SetPos(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }
    }
}