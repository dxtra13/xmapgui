using System;
using System.IO;

namespace XmapGui
{
    public abstract class XEventAction
    {
        public abstract string ToLua();
        public abstract string LuaPrefix();

        public abstract byte Identifier { get; }

        public static XEventAction Read(byte ID, BinaryReader Reader)
        {
            switch(ID)
            {
                case 0:
                    return new XTileAction(
                        Reader.ReadInt32(),
                        Reader.ReadBoolean(),
                        Reader.ReadInt32(),
                        Reader.ReadInt32(),
                        Reader.ReadInt32(),
                        Reader.ReadInt32(),
                        Reader.ReadInt32(),
                        Reader.ReadInt32()
                        );
                case 1:
                    return new XPathAction(
                        Reader.ReadInt32(),
                        Reader.ReadInt32()
                        );
                case 2:
                    return new XMusicAction(
                        Reader.ReadUInt16()
                        );
                case 3:
                    return new XPlayerAction(
                        Reader.ReadBoolean()
                        );
                case 4:
                    return new XCamAction(
                        Reader.ReadInt32(),
                        Reader.ReadInt32(),
                        Reader.ReadInt32(),
                        Reader.ReadInt32()
                        );
                case 5:
                    return new XAnimationAction(
                        Reader.ReadUInt16(),
                        Reader.ReadInt32(),
                        Reader.ReadInt32(),
                        Reader.ReadDouble(),
                        Reader.ReadDouble(),
                        Reader.ReadDouble(),
                        Reader.ReadDouble(),
                        Reader.ReadUInt32(),
                        Reader.ReadUInt16()
                        );
                default:
                    throw new Exception("Invalid event action format.");
            }
        }

        public abstract void Write(BinaryWriter Writer);
    }

    public class XTileAction : XEventAction
    {
        public int ACType { get; private set; }
        public bool NoLog { get; private set; }
        public int[] Parameters { get; private set; } = new int[6];
        public override byte Identifier { get { return 0; } }

        public XTileAction(int A, bool L, params int[] Params)
        {
            ACType = A;
            NoLog = L;
            int Len = 6;
            if (ACType == 1)
                Len = 5;
            for (int i = 0; i < Len; i++)
                Parameters[i] = Params[i];
        }

        public override string LuaPrefix()
        {
            return "xmap.xtile.";
        }

        public override string ToLua()
        {
            string FName;
            switch (ACType)
            {
                case 0:
                    FName = "translateGroup";
                    break;
                case 1:
                    FName = "setIDGroup";
                    break;
                default:
                    FName = "swapGroup";
                    break;
            }

            return $"{FName}({Parameters[0]},{Parameters[1]},{Parameters[2]},{Parameters[3]},{Parameters[4]}," + ((ACType != 1) ? $"{Parameters[5]}," : "") + NoLog.ToString().ToLower() + ")";
        }

        public override void Write(BinaryWriter Writer)
        {
            Writer.Write(Identifier);
            Writer.Write(ACType);
            Writer.Write(NoLog);
            for (int i = 0; i < Parameters.Length; i++)
                Writer.Write(Parameters[i]);
        }
    }

    public class XPathAction : XEventAction
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public override byte Identifier { get { return 1; } }

        public XPathAction(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string LuaPrefix()
        {
            return "";
        }

        public override string ToLua()
        {
            return $"queuePathAction({X},{Y})";
        }

        public override void Write(BinaryWriter Writer)
        {
            Writer.Write(Identifier);
            Writer.Write(X);
            Writer.Write(Y);
        }
    }

    public class XMusicAction : XEventAction
    {
        public ushort ID { get; private set; }
        public override byte Identifier { get { return 2; } }

        public XMusicAction(ushort iD)
        {
            ID = iD;
        }

        public override string LuaPrefix()
        {
            return "xmap.xmusic.";
        }

        public override string ToLua()
        {
            return $"play({ID})";
        }

        public override void Write(BinaryWriter Writer)
        {
            Writer.Write(Identifier);
            Writer.Write(ID);
        }
    }

    public class XPlayerAction : XEventAction
    {
        public bool Visible { get; private set; }
        public override byte Identifier { get { return 3; } }

        public XPlayerAction(bool V)
        {
            Visible = V;
        }

        public override string LuaPrefix()
        {
            return "xmap.chocoMap.";
        }

        public override string ToLua()
        {
            return "playerVisible=" + Visible.ToString().ToLower();
        }

        public override void Write(BinaryWriter Writer)
        {
            Writer.Write(Identifier);
            Writer.Write(Visible);
        }
    }

    public class XCamAction : XEventAction
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public override byte Identifier { get { return 4; } }

        public XCamAction(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public override string LuaPrefix()
        {
            return "xmap.chocoMap.";
        }

        public override string ToLua()
        {
            return $"addCameraBoundary({X},{Y},{Width},{Height})";
        }

        public override void Write(BinaryWriter Writer)
        {
            Writer.Write(Identifier);
            Writer.Write(X);
            Writer.Write(Y);
            Writer.Write(Width);
            Writer.Write(Height);
        }
    }

    public class XAnimationAction : XEventAction
    {
        public ushort ID { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public double SpeedX { get; private set; }
        public double SpeedY { get; private set; }
        public double AccelX { get; private set; }
        public double AccelY { get; private set; }
        public uint Lifetime { get; private set; }
        public ushort StartFrame { get; private set; }
        public override byte Identifier { get { return 5; } }

        public XAnimationAction(ushort iD, int x, int y, double speedX, double speedY, double accelX, double accelY, uint lifetime, ushort startFrame)
        {
            ID = iD;
            X = x;
            Y = y;
            SpeedX = speedX;
            SpeedY = speedY;
            AccelX = accelX;
            AccelY = accelY;
            Lifetime = lifetime;
            StartFrame = startFrame;
        }

        public override string LuaPrefix()
        {
            return "xmap.wanime.";
        }

        public override string ToLua()
        {
            string LT = Lifetime.ToString();
            if (Lifetime == 0)
                LT = "nil";
            return $"addAnimation({ID},{X},{Y},{SpeedX},{SpeedY},{AccelX},{AccelY},{LT},{StartFrame})";
        }

        public override void Write(BinaryWriter Writer)
        {
            Writer.Write(Identifier);
            Writer.Write(ID);
            Writer.Write(X);
            Writer.Write(Y);
            Writer.Write(SpeedX);
            Writer.Write(SpeedY);
            Writer.Write(AccelX);
            Writer.Write(AccelY);
            Writer.Write(Lifetime);
            Writer.Write(StartFrame);
        }
    }
}
