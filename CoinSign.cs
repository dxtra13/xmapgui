using System.IO;

namespace XmapGui
{
    public class CoinSign : XMapWorldItem
    {
        public override bool IsMisc() { return true; }

        public int ReqCoins { get; set; }

        public CoinSign(int idx, int x, int y, ushort iD, int c)
        {
            Index = idx;
            X = x;
            Y = y;
            ID = iD;

            ReqCoins = c;
        }

        public void Write(BinaryWriter Writer)
        {
            Writer.Write(X);
            Writer.Write(Y);
            Writer.Write(ID);
            Writer.Write(ReqCoins);
        }

        public static CoinSign Read(BinaryReader Reader, int idx)
        {
            return new CoinSign(
                idx,
                Reader.ReadInt32(),
                Reader.ReadInt32(),
                Reader.ReadUInt16(),
                Reader.ReadInt32()
                );
        }
    }
}
