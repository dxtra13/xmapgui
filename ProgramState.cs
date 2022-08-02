using Cairo;
using Gtk;
using System.Collections.Generic;

namespace XmapGui
{
    public static class ProgramState
    {
        public static char _SEP = System.IO.Path.DirectorySeparatorChar;

        public static bool SelectionStateToggle { get; private set; }
        public static string CurrentEpisodeFolder { get; set; }

        public static ImageSurface SelectedItemOverlay { get; private set; }
        public static ImageSurface CameraImage { get; private set; }
        public static ImageSurface SignImage { get; private set; }
        public static ImageSurface[] SPathImages { get; private set; } = new ImageSurface[5];

        public static List<XEvent> Events { get; private set; } = new List<XEvent>();
        public static ListStore EventStore { get; private set; } = new ListStore(typeof(string));

        public static List<XTrigger> PathTriggers { get; private set; } = new List<XTrigger>();
        public static List<XTrigger> LevelTriggers { get; private set; } = new List<XTrigger>();
        public static ListStore PathTriggerStore { get; private set; } = new ListStore(typeof(string));
        public static ListStore LevelTriggerStore { get; private set; } = new ListStore(typeof(string));

        public static void SetMode(bool Toggle)
        {
            SelectionStateToggle = Toggle;
        }

        public static void LoadStockImages(string Dir)
        {
            SelectedItemOverlay = new ImageSurface(Dir + @$"{_SEP}select.png");
            CameraImage = new ImageSurface(Dir + @$"{_SEP}cam.png");
            SignImage = new ImageSurface(Dir + @$"{_SEP}sign.png");
            SPathImages[2] = new ImageSurface(Dir + @$"{_SEP}spath1.png");
            SPathImages[3] = new ImageSurface(Dir + @$"{_SEP}spath2.png");
            SPathImages[4] = new ImageSurface(Dir + @$"{_SEP}spath3.png");
        }

        public static void AddEvent(XEvent Event)
        {
            Events.Add(Event);
            EventStore.AppendValues(Event.ToString());
        }

        public static void AddPathTrigger(XTrigger Trigger)
        {
            PathTriggers.Add(Trigger);
            PathTriggerStore.AppendValues(Trigger.ToString());
        }

        public static void AddLevelTrigger(XTrigger Trigger)
        {
            LevelTriggers.Add(Trigger);
            LevelTriggerStore.AppendValues(Trigger.ToString());
        }

        public static void RemovePathTrigger(int Index)
        {
            if (Index < 0 || Index >= PathTriggers.Count)
                return;

            PathTriggers.RemoveAt(Index);
            TreeIter _Iter;
            PathTriggerStore.IterNthChild(out _Iter, Index);
            PathTriggerStore.Remove(ref _Iter);
        }

        public static void RemoveLevelTrigger(int Index)
        {
            if (Index < 0 || Index >= LevelTriggers.Count)
                return;

            LevelTriggers.RemoveAt(Index);
            TreeIter _Iter;
            LevelTriggerStore.IterNthChild(out _Iter, Index);
            LevelTriggerStore.Remove(ref _Iter);
        }

        public static void InitProgramData()
        {
            Events = new List<XEvent>();
            EventStore.Clear();
            PathTriggers = new List<XTrigger>();
            LevelTriggers = new List<XTrigger>();
            PathTriggerStore.Clear();
            LevelTriggers.Clear();
        }

        public static void RemoveEvent(int Index)
        {
            if (Index <= 0 || Index >= Events.Count)
                return;

            Events.RemoveAt(Index);
            for (int i = Index; i < Events.Count; i++)
                Events[i].CalibID();

            TreeIter _Iter;
            EventStore.IterNthChild(out _Iter, Index);
            EventStore.Remove(ref _Iter);
            int j = Index;
            do
            {
                if (j < Events.Count)
                {
                    EventStore.SetValues(_Iter, Events[j].ToString());
                    j++;
                }
            } while (EventStore.IterNext(ref _Iter));
        }
    }
}