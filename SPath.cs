using System.IO;

namespace XmapGui
{
    public class SPath : XMapWorldItem
    {
        public override bool IsMisc() { return true; }

        public int WaitTimer { get; set; }

        public SPath(int idx, int x, int y, ushort iD, int w)
        {
            Index = idx;
            X = x;
            Y = y;
            ID = iD;

            WaitTimer = w;
        }

        public void Write(BinaryWriter Writer)
        {
            Writer.Write(X);
            Writer.Write(Y);
            Writer.Write(ID);
            Writer.Write(WaitTimer);
        }

        public static SPath Read(BinaryReader Reader, int idx)
        {
            return new SPath(
                idx,
                Reader.ReadInt32(),
                Reader.ReadInt32(),
                Reader.ReadUInt16(),
                Reader.ReadInt32()
                );
        }
    }
}
