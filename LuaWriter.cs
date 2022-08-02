using System.IO;

namespace XmapGui
{
    public static class LuaWriter
    {
        public const string LIB_REQ = "local xmap=require(\"xmap\")";
        public const string SELF = "local xgui={}";
        public const string EOF = "return xgui";
        public const string END = "end";

        public static string StaticAnimation(Animation A)
        {
            uint LT = A.Lifetime;
            if (LT != 0)
                return $"xmap.wanime.addAnimation({A.ID},{A.X},{A.Y},{A.SpeedX},{A.SpeedY},{A.AccelX},{A.AccelY},{A.Lifetime},{A.StartFrame})";
            else
                return $"xmap.wanime.addAnimation({A.ID},{A.X},{A.Y},{A.SpeedX},{A.SpeedY},{A.AccelX},{A.AccelY},nil,{A.StartFrame})";
        }

        public static string StaticCam(CamBoundary C)
        {
            return $"xmap.chocoMap.addCameraBoundary({C.X},{C.Y},{C.Width},{C.Height})";
        }

        public static string StaticPath(SPath P)
        {
            string SType = "xmap.xpath.SP_BLOCKADE";
            if (P.ID == 3)
                SType = "xmap.xpath.SP_SKIP";
            else if (P.ID == 4)
                SType = "xmap.xpath.SP_WAIT";
            return $"xmap.xpath.addSpecialPath({SType},{P.X},{P.Y},{P.WaitTimer})";
        }

        public static string StaticSign(CoinSign S)
        {
            return $"xmap.coinSign.addSign({S.X},{S.Y},{S.ReqCoins})";
        }

        public static string EvAction(XEventAction A)
        {
            return A.LuaPrefix() + A.ToLua();
        }

        public static string XEv(XEvent E)
        {
            return $"local function EVENT{E.ID}()";
        }

        public static string XEvCall(int ID)
        {
            return $"EVENT{ID}()";
        }

        public static string PathEventAssoc(int Index)
        {
            return $"if(P and P.idx=={Index})then";
        }

        public static string LevelEventAssoc(int Index)
        {
            return $"if(I=={Index})then";
        }

        public static string RegisteredEvent(string EvName)
        {
            return $"registerEvent(xgui,\"{EvName}\")";
        }

        public static void WriteFull(string Dir)
        {
            using (StreamWriter Writer = new StreamWriter(Path.Combine(Dir, "xgui.lua")))
            {
                Writer.WriteLine(LIB_REQ);
                Writer.WriteLine(SELF);
                Writer.WriteLine("local pathActions={}");

                Writer.WriteLine("function xgui.onInitAPI()");
                Writer.WriteLine(RegisteredEvent("onStart"));
                Writer.WriteLine(RegisteredEvent("onTick"));
                Writer.WriteLine(RegisteredEvent("onPathOpened"));
                Writer.WriteLine(RegisteredEvent("onLevelOpened"));
                Writer.WriteLine(END);

                Writer.WriteLine("local function queuePathAction(x,y)");
                Writer.WriteLine("pathActions[#pathActions+1]={x,y}");
                Writer.WriteLine(END);

                foreach (XEvent E in ProgramState.Events)
                {
                    if (E.ID <= 0)
                        continue;

                    Writer.WriteLine(XEv(E));
                    foreach (XEventAction A in E.Actions)
                        Writer.WriteLine(EvAction(A));
                    Writer.WriteLine(END);
                }

                Writer.WriteLine("function xgui.onStart()");
                Writer.WriteLine("xmap.xmusic.seize()");
                foreach (Animation A in WorldState.Animations)
                    Writer.WriteLine(StaticAnimation(A));
                foreach (CamBoundary C in WorldState.CameraBoundaries)
                    Writer.WriteLine(StaticCam(C));
                foreach (SPath P in WorldState.SpecialPaths)
                    Writer.WriteLine(StaticPath(P));
                foreach (CoinSign S in WorldState.Signs)
                    Writer.WriteLine(StaticSign(S));
                Writer.WriteLine(END);

                Writer.WriteLine("function xgui.onTick()");
                Writer.WriteLine("local d=true");
                Writer.WriteLine("if(#pathActions==0)then");
                Writer.WriteLine("return");
                Writer.WriteLine(END);
                Writer.WriteLine("for k,v in ipairs(pathActions)do");
                Writer.WriteLine("if(v~=0)then");
                Writer.WriteLine("xmap.xpath.openPath(v[1],v[2])");
                Writer.WriteLine("pathActions[k]=0");
                Writer.WriteLine("d=false");
                Writer.WriteLine("break");
                Writer.WriteLine(END);
                Writer.WriteLine(END);
                Writer.WriteLine("if(d)then");
                Writer.WriteLine("pathActions={}");
                Writer.WriteLine(END);
                Writer.WriteLine(END);

                Writer.WriteLine("function xgui.onPathOpened(P)");
                Writer.WriteLine("local _x");
                Writer.WriteLine("local _y");
                Writer.WriteLine("if(P)then");
                Writer.WriteLine("_x=P.x");
                Writer.WriteLine("_y=P.y");
                for (int i = 0; i < ProgramState.PathTriggers.Count; i++)
                {
                    string[] Lines = ProgramState.PathTriggers[i].ToLua();
                    foreach (string Line in Lines)
                        Writer.WriteLine(Line);
                    Writer.WriteLine(END);
                }
                Writer.WriteLine(END);
                for (int i = 0; i < WorldState.PathEventAssoc.Length; i++)
                {
                    if (WorldState.PathEventAssoc[i] != 0)
                    {
                        Writer.WriteLine(PathEventAssoc(i));
                        Writer.WriteLine(XEvCall(WorldState.PathEventAssoc[i]));
                        Writer.WriteLine(END);
                    }
                }
                Writer.WriteLine(END);

                Writer.WriteLine("function xgui.onLevelOpened(I)");
                Writer.WriteLine("local _x=mem(mem(0x00B25994,FIELD_DWORD)+(0x64*I)+0x00,FIELD_DFLOAT)");
                Writer.WriteLine("local _y=mem(mem(0x00B25994,FIELD_DWORD)+(0x64*I)+0x08,FIELD_DFLOAT)");
                for (int i = 0; i < ProgramState.LevelTriggers.Count; i++)
                {
                    string[] Lines = ProgramState.LevelTriggers[i].ToLua();
                    foreach (string Line in Lines)
                        Writer.WriteLine(Line);
                    Writer.WriteLine(END);
                }
                for (int i = 0; i < WorldState.LevelEventAssoc.Length; i++)
                {
                    if (WorldState.LevelEventAssoc[i] != 0)
                    {
                        Writer.WriteLine(LevelEventAssoc(i));
                        Writer.WriteLine(XEvCall(WorldState.LevelEventAssoc[i]));
                        Writer.WriteLine(END);
                    }
                }
                Writer.WriteLine(END);

                Writer.WriteLine(EOF);
            }
        }
    }
}
