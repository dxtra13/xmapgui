using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace XmapGui
{
    public enum XTriggerConditionType
    {
        Equal,
        NotEqual, // haha
        GreaterThan,
        LesserThan
    }

    public struct XTriggerCondition
    {
        public XTriggerConditionType Type { get; set; }
        public int Compare { get; set; }
        public bool Conjunctive { get; set; }
        public int Attribute { get; set; }

        public string Operator()
        {
            switch(Type)
            {
                case XTriggerConditionType.Equal:
                    return "==";
                case XTriggerConditionType.NotEqual:
                    return "~=";
                case XTriggerConditionType.GreaterThan:
                    return ">";
                default:
                    return "<";
            }
        }

        public string AttrString()
        {
            switch(Attribute)
            {
                case 0:
                    return "_x";
                default:
                    return "_y";
            }
        }

        public XTriggerCondition(XTriggerConditionType type, int compare, bool conjunctive, int attr)
        {
            Type = type;
            Compare = compare;
            Conjunctive = conjunctive;
            Attribute = attr;
        }

        public void Write(BinaryWriter Writer)
        {
            Writer.Write((short)Type);
            Writer.Write(Compare);
            Writer.Write(Conjunctive);
            Writer.Write(Attribute);
        }

        public static XTriggerCondition Read(BinaryReader Reader)
        {
            return new XTriggerCondition(
                (XTriggerConditionType)Reader.ReadInt16(),
                Reader.ReadInt32(),
                Reader.ReadBoolean(),
                Reader.ReadInt32()
                );
        }
    }

    public class XTrigger
    {
        public List<XTriggerCondition> Conditions { get; private set; } = new List<XTriggerCondition>();
        public XEvent TriggeredEvent { get; private set; }
        public List<string[]> Arguments { get; private set; } = new List<string[]>();

        public XTrigger(XEvent E)
        {
            TriggeredEvent = E;
        }

        public void AddCondition(XTriggerConditionType Type, int Cmp, bool Logic, int Attribute)
        {
            if (Conditions.Count == 0)
                Logic = true;
            Conditions.Add(new XTriggerCondition(Type, Cmp, Logic, Attribute));
        }

        public void RemoveCondition(int IDX)
        {
            Conditions.RemoveAt(IDX);
        }

        public override string ToString()
        {
            return ToLua()[0] + " ... " + TriggeredEvent.ToString();
        }

        public string[] ToLua()
        {
            List<string> Lines = new List<string>();
            StringBuilder Builder = new StringBuilder("if(true");
            for (int i = 0; i < Conditions.Count; i++)
            {
                XTriggerCondition T = Conditions[i];
                Builder.Append(T.Conjunctive ? " and(" : " or(");
                Builder.Append($"{T.AttrString()}{T.Operator()}{T.Compare})");
            }
            Builder.Append(")then");
            Lines.Add(Builder.ToString());
            int r = 0;
            foreach (XEventAction A in TriggeredEvent.Actions)
            {
                if (r >= Arguments.Count || A.Identifier == 3)
                {
                    Lines.Add(A.LuaPrefix() + A.ToLua());
                    continue;
                }

                string[] Act = A.ToLua().Split('(', ')');
                string ActExpr = Act[1];
                string[] ActExprM = ActExpr.Split(',');
                int C = Math.Min(Arguments[r].Length, ActExprM.Length);
                for (int i = 0; i < C; i++)
                {
                    if(Arguments[r][i].Length > 0)
                        ActExprM[i] = Arguments[r][i];
                }
                Lines.Add($"{A.LuaPrefix()}{Act[0]}({string.Join(',', ActExprM)})");
                r++;
            }
            return Lines.ToArray();
        }

        public void Write(BinaryWriter Writer)
        {
            Writer.Write(Conditions.Count);
            foreach (XTriggerCondition C in Conditions)
                C.Write(Writer);
            Writer.Write(TriggeredEvent.ID);
            Writer.Write(Arguments.Count);
            foreach (string[] S in Arguments)
            {
                Writer.Write(S.Length);
                foreach (string _S in S)
                {
                    Writer.Write(_S.Length);
                    Writer.Write(Encoding.ASCII.GetBytes(_S));
                }
            }
        }

        public static XTrigger Read(BinaryReader Reader)
        {
            XTriggerCondition[] Conds = new XTriggerCondition[Reader.ReadInt32()];
            for (int i = 0; i < Conds.Length; i++)
                Conds[i] = XTriggerCondition.Read(Reader);
            XTrigger Tr = new XTrigger(ProgramState.Events[Reader.ReadInt32()]);
            foreach (XTriggerCondition Cnd in Conds)
                Tr.AddCondition(Cnd.Type, Cnd.Compare, Cnd.Conjunctive, Cnd.Attribute);
            int ArgC = Reader.ReadInt32();
            for (int i = 0; i < ArgC; i++)
            {
                string[] Args = new string[Reader.ReadInt32()];
                for (int j = 0; j < Args.Length; j++)
                {
                    int StrL = Reader.ReadInt32();
                    Args[j] = Encoding.ASCII.GetString(Reader.ReadBytes(StrL));
                }
                Tr.Arguments.Add(Args);
            }
            return Tr;
        }
    }
}
