using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;

namespace zpgServer
{
    public static class GUICore
    {
        public static ConsoleWindow consoleWindow;

        public static ThreadState threadState = ThreadState.NotStarted;
        public static void OnStart() { threadState = ThreadState.Running; }
        public static void OnStop()
        {
            threadState = ThreadState.Stopping;
        }

        public static void ThreadMain()
        {
            //ConsoleWindow window = new ConsoleWindow();
            //window.ShowDialog();
            consoleWindow = new ConsoleWindow();
            System.Windows.Forms.Application.Run(consoleWindow);
            ConsoleEx.Log("Graphical console closed");
            Core.Shutdown();
        }
    }
}
