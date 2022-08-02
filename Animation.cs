using System.IO;

namespace XmapGui
{
    public class Animation : XMapWorldItem
    {
        public override bool IsAnime() { return true; }

        public double SpeedX { get; set; }
        public double SpeedY { get; set; }
        public double AccelX { get; set; }
        public double AccelY { get; set; }
        public uint Lifetime { get; set; }
        public ushort StartFrame { get; set; }

        public ushort CurrentFrame { get; private set; }
        public ushort CurrentFrameTimer { get; private set; } = 0;

        public static string AnimeIdentifier(int ID)
        {
            return $"wanime-{ID}";
        }

        public override WorldItemConfig[] ConfigArray()
        {
            return WorldState.AnimeConfig;
        }

        public Animation(int idx, int x, int y, ushort iD, double speedX = 0, double speedY = 0, double accelX = 0, double accelY = 0, uint lifetime = 0, ushort startFrame = 0)
        {
            Index = idx;
            X = x;
            Y = y;
            ID = iD;

            SpeedX = speedX;
            SpeedY = speedY;
            AccelX = accelX;
            AccelY = accelY;
            Lifetime = lifetime;
            StartFrame = startFrame;
            CurrentFrame = startFrame;
        }

        public void Write(BinaryWriter Writer)
        {
            Writer.Write(X);
            Writer.Write(Y);
            Writer.Write(ID);
            Writer.Write(SpeedX);
            Writer.Write(SpeedY);
            Writer.Write(AccelX);
            Writer.Write(AccelY);
            Writer.Write(Lifetime);
            Writer.Write(StartFrame);
        }

        public static Animation Read(BinaryReader Reader, int idx)
        {
            return new Animation(
                idx,
                Reader.ReadInt32(),
                Reader.ReadInt32(),
                Reader.ReadUInt16(),
                Reader.ReadDouble(),
                Reader.ReadDouble(),
                Reader.ReadDouble(),
                Reader.ReadDouble(),
                Reader.ReadUInt32(),
                Reader.ReadUInt16()
                );
        }

        public void Tick()
        {
            CurrentFrameTimer++;
            if (CurrentFrameTimer >= ConfigArray()[ID].FrameSpeed)
            {
                CurrentFrame = (ushort)((CurrentFrame + 1) % ConfigArray()[ID].Frames);
                CurrentFrameTimer = 0;
            }
        }
    }
}
