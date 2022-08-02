using System.IO;

namespace XmapGui
{
    public class CamBoundary : XMapWorldItem
    {
        public override bool IsMisc() { return true; }

        public int Width { get; set; }
        public int Height { get; set; }

        public CamBoundary(int idx, int x, int y, int w, int h)
        {
            Index = idx;
            X = x;
            Y = y;
            ID = 0;

            Width = w;
            Height = h;
        }

        public void Write(BinaryWriter Writer)
        {
            Writer.Write(X);
            Writer.Write(Y);
            Writer.Write(Width);
            Writer.Write(Height);
        }

        public static CamBoundary Read(BinaryReader Reader, int idx)
        {
            return new CamBoundary(
                idx,
                Reader.ReadInt32(),
                Reader.ReadInt32(),
                Reader.ReadInt32(),
                Reader.ReadInt32()
                );
        }
    }
}