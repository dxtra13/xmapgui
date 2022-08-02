namespace XmapGui
{
    public class Scene : WorldItem
    {
        public override bool IsScene() { return true; }

        public static string SceneIdentifier(int ID)
        {
            return $"scene-{ID}";
        }

        public override WorldItemConfig[] ConfigArray()
        {
            return WorldState.SceneConfig;
        }

        public Scene(int idx, int x, int y, ushort iD)
        {
            Index = idx;
            X = x;
            Y = y;
            ID = iD;
        }
    }
}
