using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace XmapGui
{
    public static class WorldState
    {
        public const ushort XG_WORLD_TILE_LIMIT = 20000;
        public const ushort XG_WORLD_PATH_LIMIT = 2000;
        public const ushort XG_WORLD_MUSIC_LIMIT = 1000;
        public const ushort XG_WORLD_SCENE_LIMIT = 5000;
        public const ushort XG_WORLD_LEVEL_LIMIT = 400;

        public const ushort XG_WORLD_TILE_MAX_ID = 400;
        public const ushort XG_WORLD_SCENE_MAX_ID = 100;
        public const ushort XG_WORLD_PATH_MAX_ID = 100;
        public const ushort XG_WORLD_LEVEL_MAX_ID = 100;
        public const ushort XG_WORLD_ANIME_MAX_ID = 100;

        public const double XG_TILE_DEFAULT_PRIORITY = 1;
        public const double XG_SCENE_DEFAULT_PRIORITY = 5;
        public const double XG_PATH_DEFAULT_PRIORITY = 7;
        public const double XG_LEVEL_DEFAULT_PRIORITY = 7;
        public const double XG_ANIME_DEFAULT_PRIORITY = 7.5;

        public const string NEXT = "\"next\"";
        public const string TRUE = "#TRUE#";

        public static long CurrentTick { get; private set; }

        public static WorldItem SelectedWorldItem { get; set; } = null;

        public static WorldItemConfig[] TileConfig { get; private set; } = new WorldItemConfig[XG_WORLD_TILE_MAX_ID + 1];
        public static Cairo.ImageSurface[] TileImages { get; private set; } = new Cairo.ImageSurface[XG_WORLD_TILE_MAX_ID + 1];
        public static Tile[] Tiles { get; private set; } = new Tile[XG_WORLD_TILE_LIMIT];

        public static WorldItemConfig[] SceneConfig { get; private set; } = new WorldItemConfig[XG_WORLD_SCENE_MAX_ID + 1];
        public static Cairo.ImageSurface[] SceneImages { get; private set; } = new Cairo.ImageSurface[XG_WORLD_SCENE_MAX_ID + 1];
        public static Scene[] Scenes { get; private set; } = new Scene[XG_WORLD_SCENE_LIMIT];

        public static WorldItemConfig[] PathConfig { get; private set; } = new WorldItemConfig[XG_WORLD_PATH_MAX_ID + 1];
        public static Cairo.ImageSurface[] PathImages { get; private set; } = new Cairo.ImageSurface[XG_WORLD_PATH_MAX_ID + 1];
        public static WPath[] Paths { get; private set; } = new WPath[XG_WORLD_PATH_LIMIT];

        public static WorldItemConfig[] LevelConfig { get; private set; } = new WorldItemConfig[XG_WORLD_LEVEL_MAX_ID + 1];
        public static Cairo.ImageSurface[] LevelImages { get; private set; } = new Cairo.ImageSurface[XG_WORLD_LEVEL_MAX_ID + 1];
        public static Level[] Levels { get; private set; } = new Level[XG_WORLD_LEVEL_LIMIT];

        public static WorldItemConfig MusicConfig { get; private set; }
        public static Cairo.ImageSurface MusicImage { get; private set; }
        public static Music[] MusicBoxes { get; private set; } = new Music[XG_WORLD_MUSIC_LIMIT];

        public static WorldItemConfig[] AnimeConfig { get; private set; } = new WorldItemConfig[XG_WORLD_ANIME_MAX_ID + 1];
        public static Cairo.ImageSurface[] AnimeImages { get; private set; } = new Cairo.ImageSurface[XG_WORLD_ANIME_MAX_ID + 1];
        public static List<Animation> Animations { get; private set; } = new List<Animation>();

        public static List<CamBoundary> CameraBoundaries { get; private set; } = new List<CamBoundary>();

        public static List<SPath> SpecialPaths { get; private set; } = new List<SPath>();

        public static List<CoinSign> Signs { get; private set; } = new List<CoinSign>();

        public static int[] PathEventAssoc { get; private set; }
        public static int[] LevelEventAssoc { get; private set; }

        public static int LastUsefulTileIndex { get; private set; } = -1;
        public static int LastUsefulSceneIndex { get; private set; } = -1;
        public static int LastUsefulPathIndex { get; private set; } = -1;
        public static int LastUsefulLevelIndex { get; private set; } = -1;
        public static int LastUsefulMusicIndex { get; private set; } = -1;

        public static int[] TileFrameData { get; private set; } = new int[XG_WORLD_TILE_MAX_ID + 1];
        public static int[] SceneFrameData { get; private set; } = new int[XG_WORLD_SCENE_MAX_ID + 1];
        public static int[] PathFrameData { get; private set; } = new int[XG_WORLD_PATH_MAX_ID + 1];
        public static int[] LevelFrameData { get; private set; } = new int[XG_WORLD_LEVEL_MAX_ID + 1];

        public static void Tick()
        {
            CurrentTick++;
        }

        private static void ReadWorldItems(string Dir)
        {
            char _SEP = ProgramState._SEP;

            for (int i = 1; i <= XG_WORLD_TILE_MAX_ID; i++)
            {
                string ImgPath = Path.Combine(Dir, Tile.TileIdentifier(i)) + ".png";
                if (!File.Exists(ImgPath))
                    ImgPath = Dir + @$"{_SEP}..{_SEP}..{_SEP}graphics{_SEP}tile{_SEP}" + Tile.TileIdentifier(i) + ".png";
                if (!File.Exists(ImgPath))
                    throw new Exception($"Invalid SMBX version or invalid custom graphics format for {Tile.TileIdentifier(i)}.");

                TileImages[i] = new Cairo.ImageSurface(ImgPath);
                TileFrameData[i] = 0;

                ushort DWidth = 32;
                ushort DHeight = 32;
                ushort DFrames = 1;
                ushort DFrameSpeed = 8;

                #region Tile Specific Configs
                switch (i)
                {
                    case 8:
                        DWidth = 64;
                        DHeight = 64;
                        break;
                    case 9:
                        DWidth = 96;
                        DHeight = 96;
                        break;
                    case 12:
                        DWidth = 64;
                        DHeight = 64;
                        break;
                    case 13:
                        DWidth = 96;
                        DHeight = 96;
                        break;
                    case 14:
                        DFrames = 4;
                        DFrameSpeed = 16;
                        break;
                    case 27:
                        DWidth = 128;
                        DHeight = 128;
                        DFrames = 4;
                        DFrameSpeed = 16;
                        break;
                    case 241:
                        DFrames = 4;
                        DFrameSpeed = 12;
                        break;
                    case 325:
                        DWidth = 64;
                        DHeight = 64;
                        break;
                    default:
                        break;
                }
                #endregion

                TileConfig[i] = WorldItemConfig.Read(Path.Combine(Dir, Tile.TileIdentifier(i)) + ".txt", XG_TILE_DEFAULT_PRIORITY, DWidth, DHeight, DFrames, DFrameSpeed);
            }

            for (int i = 1; i <= XG_WORLD_SCENE_MAX_ID; i++)
            {
                string ImgPath = Path.Combine(Dir, Scene.SceneIdentifier(i)) + ".png";
                if (!File.Exists(ImgPath))
                    ImgPath = Dir + @$"{_SEP}..{_SEP}..{_SEP}graphics{_SEP}scene{_SEP}" + Scene.SceneIdentifier(i) + ".png";
                if (!File.Exists(ImgPath))
                    throw new Exception($"Invalid SMBX version or invalid custom graphics format for {Scene.SceneIdentifier(i)}.");

                SceneImages[i] = new Cairo.ImageSurface(ImgPath);
                TileFrameData[i] = 0;

                ushort DWidth = 32;
                ushort DHeight = 32;
                ushort DFrames = 1;
                ushort DFrameSpeed = 8;

                #region Scenery Specific Configs
                switch (i)
                {
                    case 1:
                    case 4:
                    case 5:
                    case 6:
                    case 9:
                    case 10:
                    case 12:
                        DFrames = 4;
                        DFrameSpeed = 12;
                        break;
                    case 15:
                    case 16:
                    case 17:
                    case 18:
                        DWidth = 16;
                        DHeight = 16;
                        break;
                    case 20:
                        DWidth = 64;
                        DHeight = 64;
                        break;
                    case 21:
                    case 24:
                        DWidth = 16;
                        DHeight = 16;
                        break;
                    case 27:
                    case 28:
                        DWidth = 48;
                        DHeight = 16;
                        DFrames = 12;
                        break;
                    case 29:
                    case 30:
                        DWidth = 64;
                        DHeight = 16;
                        DFrames = 12;
                        break;
                    case 33:
                    case 34:
                        DWidth = 14;
                        DHeight = 14;
                        DFrames = 14;
                        DFrameSpeed = 6;
                        break;
                    case 35:
                    case 36:
                    case 37:
                    case 38:
                    case 39:
                        DWidth = 16;
                        DHeight = 16;
                        break;
                    case 44:
                        DWidth = 62;
                        break;
                    case 50:
                        DWidth = 64;
                        DHeight = 48;
                        break;
                    case 51:
                        DWidth = 30;
                        DFrames = 4;
                        break;
                    case 52:
                    case 53:
                        DFrames = 4;
                        DFrameSpeed = 12;
                        break;
                    case 54:
                    case 55:
                        DWidth = 30;
                        DHeight = 24;
                        DFrames = 4;
                        DFrameSpeed = 12;
                        break;
                    case 57:
                        DWidth = 64;
                        DHeight = 64;
                        break;
                    case 58:
                    case 59:
                        DWidth = 16;
                        DHeight = 16;
                        break;
                    case 60:
                        DWidth = 48;
                        DHeight = 48;
                        break;
                    case 61:
                        DWidth = 64;
                        DHeight = 76;
                        break;
                    case 62:
                        DFrames = 8;
                        DFrameSpeed = 6;
                        break;
                    case 63:
                        DWidth = 16;
                        DHeight = 16;
                        DFrames = 8;
                        DFrameSpeed = 6;
                        break;
                    default:
                        break;
                }
                #endregion

                SceneConfig[i] = WorldItemConfig.Read(Path.Combine(Dir, Scene.SceneIdentifier(i)) + ".txt", XG_SCENE_DEFAULT_PRIORITY, DWidth, DHeight, DFrames, DFrameSpeed);
            }

            for (int i = 1; i <= XG_WORLD_PATH_MAX_ID; i++)
            {
                string ImgPath = Path.Combine(Dir, WPath.PathIdentifier(i)) + ".png";
                if (!File.Exists(ImgPath))
                    ImgPath = Dir + @$"{_SEP}..{_SEP}..{_SEP}graphics{_SEP}path{_SEP}" + WPath.PathIdentifier(i) + ".png";
                if (!File.Exists(ImgPath))
                    throw new Exception($"Invalid SMBX version or invalid custom graphics format for {WPath.PathIdentifier(i)}.");

                PathImages[i] = new Cairo.ImageSurface(ImgPath);
                PathFrameData[i] = 0;

                ushort DWidth = 32;
                ushort DHeight = 32;
                ushort DFrames = 1;
                ushort DFrameSpeed = 8;

                PathConfig[i] = WorldItemConfig.Read(Path.Combine(Dir, WPath.PathIdentifier(i)) + ".txt", XG_PATH_DEFAULT_PRIORITY, DWidth, DHeight, DFrames, DFrameSpeed);
            }

            for (int i = 0; i <= XG_WORLD_LEVEL_MAX_ID; i++)
            {
                string ImgPath = Path.Combine(Dir, Level.LevelIdentifier(i)) + ".png";
                if (!File.Exists(ImgPath))
                    ImgPath = Dir + @$"{_SEP}..{_SEP}..{_SEP}graphics{_SEP}level{_SEP}" + Level.LevelIdentifier(i) + ".png";
                if (!File.Exists(ImgPath))
                    throw new Exception($"Invalid SMBX version or invalid custom graphics format for {Level.LevelIdentifier(i)}.");

                LevelImages[i] = new Cairo.ImageSurface(ImgPath);
                LevelFrameData[i] = 0;

                ushort DWidth = 32;
                ushort DHeight = 32;
                ushort DFrames = 1;
                ushort DFrameSpeed = 8;

                #region Level Specific Configs
                switch (i)
                {
                    case 2:
                        DFrames = 6;
                        DFrameSpeed = 6;
                        break;
                    case 8:
                        DFrames = 4;
                        DFrameSpeed = 10;
                        break;
                    case 9:
                        DFrames = 6;
                        DFrameSpeed = 6;
                        break;
                    case 12:
                        DFrames = 2;
                        break;
                    case 13:
                    case 14:
                    case 15:
                        DFrames = 6;
                        DFrameSpeed = 6;
                        break;
                    case 21:
                        DHeight = 48;
                        break;
                    case 22:
                        DWidth = 64;
                        DHeight = 64;
                        break;
                    case 23:
                        DWidth = 96;
                        DHeight = 96;
                        break;
                    case 24:
                        DHeight = 48;
                        break;
                    case 25:
                    case 26:
                        DFrames = 4;
                        break;
                    case 28:
                        DHeight = 44;
                        break;
                    case 29:
                        DWidth = 64;
                        break;
                    case 31:
                    case 32:
                    case 37:
                    case 38:
                    case 39:
                    case 40:
                        DFrames = 6;
                        DFrameSpeed = 6;
                        break;
                    default: break;
                }
                #endregion

                LevelConfig[i] = WorldItemConfig.Read(Path.Combine(Dir, Level.LevelIdentifier(i)) + ".txt", XG_LEVEL_DEFAULT_PRIORITY, DWidth, DHeight, DFrames, DFrameSpeed);
            }

            string _ImgPath = Path.Combine(@$".{_SEP}res", Music.MusicIdentifier()) + ".png";
            if (!File.Exists(_ImgPath))
                throw new Exception($"Missing stock image file \"{Music.MusicIdentifier()}\".");

            MusicImage = new Cairo.ImageSurface(_ImgPath);
            MusicConfig = WorldItemConfig.Read("", 10, 32, 32, 1, 8);

            for (int i = 1; i <= XG_WORLD_ANIME_MAX_ID; i++)
            {
                string ImgPath = Path.Combine(Dir, Animation.AnimeIdentifier(i)) + ".png";
                if (!File.Exists(ImgPath))
                    continue;

                AnimeImages[i] = new Cairo.ImageSurface(ImgPath);
                AnimeConfig[i] = WorldItemConfig.Read(Path.Combine(Dir, Animation.AnimeIdentifier(i)) + ".txt", XG_ANIME_DEFAULT_PRIORITY, 32, 32, 1, 8);
            }

            ProgramState.InitProgramData();
            ProgramState.AddEvent(new XEvent(0));

            Animations = new List<Animation>();
            CameraBoundaries = new List<CamBoundary>();
            SpecialPaths = new List<SPath>();
            Signs = new List<CoinSign>();

            if (File.Exists(Path.Combine(Dir, "xgui.bin")))
            {
                // read xmap-exclusive stuff here
                using (FileStream FStream = new FileStream(Path.Combine(Dir, "xgui.bin"), FileMode.Open))
                {
                    using (BinaryReader Reader = new BinaryReader(FStream))
                    {
                        string Signature = "XGUI";
                        string ReadSig = Encoding.ASCII.GetString(Reader.ReadBytes(4));

                        if (Signature.CompareTo(ReadSig) != 0)
                            throw new Exception("Invalid XMapGui data file format.");
                        bool KeepReading = true;
                        while (Reader.BaseStream.CanRead && KeepReading)
                        {
                            string Chunk = Encoding.ASCII.GetString(Reader.ReadBytes(4));
                            int C;
                            switch (Chunk)
                            {
                                case "XANI":
                                    C = Reader.ReadInt32();
                                    for (int i = 0; i < C; i++)
                                        Animations.Add(Animation.Read(Reader, i));
                                    break;
                                case "XCAM":
                                    C = Reader.ReadInt32();
                                    for (int i = 0; i < C; i++)
                                        CameraBoundaries.Add(CamBoundary.Read(Reader, i));
                                    break;
                                case "XPTH":
                                    C = Reader.ReadInt32();
                                    for (int i = 0; i < C; i++)
                                        SpecialPaths.Add(SPath.Read(Reader, i));
                                    break;
                                case "XSGN":
                                    C = Reader.ReadInt32();
                                    for (int i = 0; i < C; i++)
                                        Signs.Add(CoinSign.Read(Reader, i));
                                    break;
                                case "XEVT":
                                    C = Reader.ReadInt32();
                                    for (int i = 0; i < C; i++)
                                        ProgramState.AddEvent(XEvent.Read(Reader));
                                    C = Reader.ReadInt32();
                                    for (int i = 0; i < C; i++)
                                        PathEventAssoc[Reader.ReadInt32()] = Reader.ReadInt32();
                                    C = Reader.ReadInt32();
                                    for (int i = 0; i < C; i++)
                                        LevelEventAssoc[Reader.ReadInt32()] = Reader.ReadInt32();
                                    break;
                                case "XTRG":
                                    C = Reader.ReadInt32();
                                    for (int i = 0; i < C; i++)
                                        ProgramState.AddPathTrigger(XTrigger.Read(Reader));
                                    C = Reader.ReadInt32();
                                    for (int i = 0; i < C; i++)
                                        ProgramState.AddLevelTrigger(XTrigger.Read(Reader));
                                    break;
                                case "XEND":
                                    KeepReading = false;
                                    break;
                            }
                        }
                    }
                }
            }
        }

        public static void ReadWorld(string FromFile)
        {
            ProgramState.CurrentEpisodeFolder = Path.GetDirectoryName(FromFile);

            using (StreamReader Reader = new StreamReader(FromFile))
            {
                int SMBXVersion;
                string SMBXVersionString = Reader.ReadLine();
                bool Valid = int.TryParse(SMBXVersionString, out SMBXVersion);
                if (!Valid)
                    throw new Exception($"Invalid SMBX version (expected number, found {SMBXVersionString})");
                if (SMBXVersion < 56 || SMBXVersion > 64)
                    throw new Exception($"Invalid SMBX version (expected 56-64, found {SMBXVersion})");

                // header, useless for us
                for (int i = 0; i < 15; i++)
                    Reader.ReadLine();

                // tiles
                int NextTileIndex = 0, NextSceneIndex = 0, NextPathIndex = 0, NextLevelIndex = 0, NextMusicIndex = 0;
                string NextData;
                while ((NextData = Reader.ReadLine()).CompareTo(NEXT) != 0)
                {
                    string DX = NextData;
                    string DY = Reader.ReadLine();
                    string DI = Reader.ReadLine();

                    Tiles[NextTileIndex] = new Tile(
                            NextTileIndex,
                            int.Parse(DX),
                            int.Parse(DY),
                            ushort.Parse(DI)
                        );
                    NextTileIndex++;
                }
                LastUsefulTileIndex = NextTileIndex - 1;

                // scenery
                while ((NextData = Reader.ReadLine()).CompareTo(NEXT) != 0)
                {
                    string DX = NextData;
                    string DY = Reader.ReadLine();
                    string DI = Reader.ReadLine();

                    Scenes[NextSceneIndex] = new Scene(
                        NextSceneIndex,
                        int.Parse(DX),
                        int.Parse(DY),
                        ushort.Parse(DI)
                    );
                    NextSceneIndex++;
                }
                LastUsefulSceneIndex = NextSceneIndex - 1;

                // paths
                while ((NextData = Reader.ReadLine()).CompareTo(NEXT) != 0)
                {
                    string DX = NextData;
                    string DY = Reader.ReadLine();
                    string DI = Reader.ReadLine();

                    Paths[NextPathIndex] = new WPath(
                        NextPathIndex,
                        int.Parse(DX),
                        int.Parse(DY),
                        ushort.Parse(DI)
                    );
                    NextPathIndex++;
                }
                LastUsefulPathIndex = NextPathIndex - 1;

                // levels
                while ((NextData = Reader.ReadLine()).CompareTo(NEXT) != 0)
                {
                    string DX = NextData;
                    string DY = Reader.ReadLine();
                    string DI = Reader.ReadLine();

                    // stuff we don't need
                    for (int i = 0; i < 8; i++)
                        Reader.ReadLine();

                    bool PB = Reader.ReadLine().CompareTo(TRUE) == 0;

                    // more stuff we don't need
                    for (int i = 0; i < 3; i++)
                        Reader.ReadLine();

                    bool BB = Reader.ReadLine().CompareTo(TRUE) == 0;

                    Levels[NextLevelIndex] = new Level(
                        NextLevelIndex,
                        int.Parse(DX),
                        int.Parse(DY),
                        ushort.Parse(DI),
                        PB,
                        BB
                    );
                    NextLevelIndex++;
                }
                LastUsefulLevelIndex = NextLevelIndex - 1;

                // music boxes
                while ((NextData = Reader.ReadLine()).CompareTo(NEXT) != 0)
                {
                    string DX = NextData;
                    string DY = Reader.ReadLine();
                    string DI = Reader.ReadLine();

                    MusicBoxes[NextMusicIndex] = new Music(
                        NextMusicIndex,
                        int.Parse(DX),
                        int.Parse(DY),
                        ushort.Parse(DI)
                    );
                    NextMusicIndex++;
                }
                LastUsefulMusicIndex = NextMusicIndex - 1;
            }

            PathEventAssoc = new int[LastUsefulPathIndex + 1];
            LevelEventAssoc = new int[LastUsefulLevelIndex + 1];
            ReadWorldItems(ProgramState.CurrentEpisodeFolder);
        }

        public static void SaveXMapData()
        {
            string Dir = ProgramState.CurrentEpisodeFolder;

            if (File.Exists(Path.Combine(Dir, "xgui.bin")))
                File.Delete(Path.Combine(Dir, "xgui.bin"));

            using (FileStream FStream = new FileStream(Path.Combine(Dir, "xgui.bin"), FileMode.OpenOrCreate))
            {
                using (BinaryWriter Writer = new BinaryWriter(FStream))
                {
                    Writer.Write(Encoding.ASCII.GetBytes("XGUI"));
                    Writer.Write(Encoding.ASCII.GetBytes("XANI"));
                    Writer.Write(Animations.Count);
                    foreach (Animation A in Animations)
                        A.Write(Writer);
                    Writer.Write(Encoding.ASCII.GetBytes("XCAM"));
                    Writer.Write(CameraBoundaries.Count);
                    foreach (CamBoundary C in CameraBoundaries)
                        C.Write(Writer);
                    Writer.Write(Encoding.ASCII.GetBytes("XPTH"));
                    Writer.Write(SpecialPaths.Count);
                    foreach (SPath P in SpecialPaths)
                        P.Write(Writer);
                    Writer.Write(Encoding.ASCII.GetBytes("XSGN"));
                    Writer.Write(Signs.Count);
                    foreach (CoinSign S in Signs)
                        S.Write(Writer);
                    Writer.Write(Encoding.ASCII.GetBytes("XEVT"));
                    Writer.Write(ProgramState.Events.Count - 1);
                    foreach (XEvent E in ProgramState.Events)
                        if(E.ID > 0)
                            E.Write(Writer);
                    Writer.Write(PathEventAssoc.Count(x => x != 0));
                    for (int i = 0; i < PathEventAssoc.Length; i++)
                    {
                        if (PathEventAssoc[i] != 0)
                        {
                            Writer.Write(i);
                            Writer.Write(PathEventAssoc[i]);
                        }
                    }
                    Writer.Write(LevelEventAssoc.Count(x => x != 0));
                    for (int i = 0; i < LevelEventAssoc.Length; i++)
                    {
                        if (LevelEventAssoc[i] != 0)
                        {
                            Writer.Write(i);
                            Writer.Write(LevelEventAssoc[i]);
                        }
                    }
                    Writer.Write(Encoding.ASCII.GetBytes("XTRG"));
                    Writer.Write(ProgramState.PathTriggers.Count);
                    foreach (XTrigger T in ProgramState.PathTriggers)
                        T.Write(Writer);
                    Writer.Write(ProgramState.LevelTriggers.Count);
                    foreach (XTrigger T in ProgramState.LevelTriggers)
                        T.Write(Writer);
                    Writer.Write(Encoding.ASCII.GetBytes("XEND"));
                }
            }

            LuaWriter.WriteFull(Dir);
        }
    }
}
