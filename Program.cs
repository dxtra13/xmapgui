using System;
using Gtk;

namespace XmapGui
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            try
            {
                Application.Init();
                new MainWindow(WindowType.Toplevel, "XmapGui").ShowAll();
                Application.Run();
            }
            catch (Exception e)
            {
                MessageDialog Error = new MessageDialog(null, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "A fatal error has occured and the application will now exit.\n" + e.Message);
                Error.Title = "Error";

                Error.Run();
                Error.Destroy();
                Application.Quit();
            }
        }
    }
}
