using System.IO;
using IniParser;
using IniParser.Model;

namespace XmapGui
{
    public class WorldItemConfig
    {
        public ushort Width { get; set; }
        public ushort Height { get; set; }
        public ushort Frames { get; set; }
        public ushort FrameSpeed { get; set; }
        public double Priority { get; set; }
        public ushort PFrames { get; set; }

        private WorldItemConfig(ushort W, ushort H, ushort F, ushort FS, double P, ushort PF)
        {
            Width = W;
            Height = H;
            Frames = F;
            FrameSpeed = FS;
            Priority = P;
            PFrames = PF;
        }

        public static WorldItemConfig Read(string FromFile, double DefaultPriority, ushort DefaultWidth, ushort DefaultHeight, ushort DefaultFrames, ushort DefaultFrameSpeed)
        {
            FileIniDataParser Parser = new FileIniDataParser();
            IniData Data = File.Exists(FromFile) ? Parser.ReadFile(FromFile) : new IniData();

            string DWidth, DHeight, DFrames, DFrameSpeed, DPriority, DPFrames;

            return new WorldItemConfig(
                    Data.TryGetKey("width", out DWidth) ? ushort.Parse(DWidth) : DefaultWidth,
                    Data.TryGetKey("height", out DHeight) ? ushort.Parse(DHeight) : DefaultHeight,
                    Data.TryGetKey("frames", out DFrames) ? ushort.Parse(DFrames) : DefaultFrames,
                    Data.TryGetKey("framespeed", out DFrameSpeed) ? ushort.Parse(DFrameSpeed) : DefaultFrameSpeed,
                    Data.TryGetKey("priority", out DPriority) ? double.Parse(DPriority) : DefaultPriority,
                    Data.TryGetKey("pframes", out DPFrames) ? ushort.Parse(DPFrames) : (ushort)1
                );
        }

        public static void SaveConfiguration(string FilePath, ushort W, ushort H, ushort F, ushort FS, double P, ushort PF = 0)
        {
            using (StreamWriter Writer = new StreamWriter(FilePath))
            {
                Writer.WriteLine($"width={W}");
                Writer.WriteLine($"height={H}");
                Writer.WriteLine($"frames={F}");
                Writer.WriteLine($"framespeed={FS}");
                Writer.WriteLine($"priority={P}");
                if (PF > 0) { Writer.WriteLine($"pframes={PF}"); }
            }
        }
    }
}
