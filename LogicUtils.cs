using System;
using System.Collections.Generic;

namespace XmapGui
{
    public static class LogicUtils
    {
        public static void BinaryInsert<T>(List<T> DestList, T Item) where T : IComparable<T>
        {
            int InsertIndex = DestList.BinarySearch(Item);
            if (InsertIndex < 0) InsertIndex = ~InsertIndex;
            DestList.Insert(InsertIndex, Item);
        }

        private static bool Range(int N, int Low, int High)
        {
            return N >= Low && N <= High;
        }

        public static bool RectIntersect(int X1, int Y1, int W1, int H1, int X2, int Y2, int W2, int H2)
        {
            return (Range(X1, X2, X2 + W2) || Range(X2, X1, X1 + W1)) && (Range(Y1, Y2, Y2 + H2) || Range(Y2, Y1, Y1 + H1));
        }

        public static int Clamp(int N, int Min, int Max)
        {
            return Math.Max(Min, Math.Min(N, Max));
        }
    }
}
