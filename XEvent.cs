using System.Collections.Generic;
using System.IO;

namespace XmapGui
{
    public class XEvent
    {
        public int ID { get; private set; }
        public List<XEventAction> Actions { get; private set; }

        public override string ToString()
        {
            if (ID == 0)
                return "(None)";
            return $"EVENT#{ID}";
        }

        public XEvent(int iD)
        {
            ID = iD;
            Actions = new List<XEventAction>();
        }

        public void AddAction(XEventAction A)
        {
            Actions.Add(A);
        }

        public void RemoveAction(int IDX)
        {
            if (IDX < 0 || IDX >= Actions.Count)
                return;

            Actions.RemoveAt(IDX);
        }

        public void CalibID()
        {
            ID--;
        }

        public void Write(BinaryWriter Writer)
        {
            Writer.Write(ID);
            Writer.Write(Actions.Count);
            for (int i = 0; i < Actions.Count; i++)
                Actions[i].Write(Writer);
        }

        public static XEvent Read(BinaryReader Reader)
        {
            XEvent Ev = new XEvent(Reader.ReadInt32());
            int Count = Reader.ReadInt32();
            for (int i = 0; i < Count; i++)
            {
                byte Identifier = Reader.ReadByte();
                Ev.Actions.Add(XEventAction.Read(Identifier, Reader));
            }
            return Ev;
        }
    }
}
