using Gtk;

namespace XmapGui
{
    public static class GuiUtils
    {
        public static VBox CompactPack(Widget PackedWidget, bool HExpand, bool HFill, bool VExpand, bool VFill, uint HPadding, uint VPadding)
        {
            HBox _TempH = new HBox();
            VBox _TempV = new VBox();

            _TempH.PackStart(PackedWidget, HExpand, HFill, HPadding);
            _TempV.PackStart(_TempH, VExpand, VFill, VPadding);

            return _TempV;
        }

        public static VBox CompactPackMultiV(bool Expand, bool Fill, uint Padding, bool HExpand, bool HFill, bool VExpand, bool VFill, uint HPadding, uint VPadding, params Widget[] PackedWidgets)
        {
            VBox _TempV = new VBox();

            foreach (Widget W in PackedWidgets)
                _TempV.PackStart(CompactPack(W, HExpand, HFill, VExpand, VFill, HPadding, VPadding), Expand, Fill, Padding);

            return _TempV;
        }

        public static HBox CompactPackMultiH(bool Expand, bool Fill, uint Padding, bool HExpand, bool HFill, bool VExpand, bool VFill, uint HPadding, uint VPadding, params Widget[] PackedWidgets)
        {
            HBox _TempH = new HBox();

            foreach (Widget W in PackedWidgets)
                _TempH.PackStart(CompactPack(W, HExpand, HFill, VExpand, VFill, HPadding, VPadding), Expand, Fill, Padding);

            return _TempH;
        }

        public static void SetSensitive(bool Sense, params Widget[] Widgets)
        {
            foreach (Widget W in Widgets)
                W.Sensitive = Sense;
        }

        public static void MultiHide(params Widget[] Widgets)
        {
            foreach (Widget W in Widgets)
                W.Hide();
        }

        public static void MultiShow(params Widget[] Widgets)
        {
            foreach (Widget W in Widgets)
                W.Show();
        }

        public static int TreeViewIndex(TreeView T)
        {
            TreePath Temp = null;
            T.GetCursor(out Temp, out _);
            if (Temp == null)
                return -1;

            int I = Temp.Indices[0];
            Temp.Dispose();
            return I;
        }

        public static void SetListVal(ListStore L, int I, string S)
        {
            TreeIter _Iter;
            L.IterNthChild(out _Iter, I);
            L.SetValues(_Iter, S);
        }
    }
}
