using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Threading;
using System.Runtime.InteropServices;

namespace zpgServer
{
    public enum ThreadId
    {
        Main,
        GUI,
        World,
        Web,
        Disk,
        Update,
        Worker,
    }

    public enum ThreadState
    {
        NotStarted,
        Running,
        Stopping,
        Stopped,
    }

    class Core
    {
        static ThreadState state = ThreadState.Running;
        static Dictionary<Thread, ThreadId> threadLibrary = new Dictionary<Thread, ThreadId>();
        static void Main(string[] args)
        {
            threadLibrary.Add(Thread.CurrentThread, ThreadId.Main);
            LogManager.CleanUpFiles();
            ConsoleEx.Log("Thread started");

            ConsoleEx.Log("Loading strings");
            Localization.Load();

            ConsoleEx.Log("Loading looters");
            Looter.LoadItems();

            ConsoleEx.Log("Loading destiny");
            Destiny.LoadEvents();

            ConsoleEx.Log("Loading planets");
            Terraformer.LoadPlanets();

            ConsoleEx.Log("Loading accounts");
            Authorization.LoadAccounts();

            ConsoleEx.Log("Rolling the dice");
            Random.RollSeed();

            ConsoleEx.Log("Creating the universe");
            Universe.Create();

            ConsoleEx.Log("Generating RSA keys");
            WebSecurity.GenerateKeys();

            ConsoleEx.Log("Collecting command line data");
            CmdParser.Initialize();

            ConsoleEx.Log("Initializing web thread");
            Thread webThread = new Thread(WebCore.WebThread);
            WebCore.OnStart();
            webThread.Start();
            threadLibrary.Add(webThread, ThreadId.Web);

            ConsoleEx.Log("Initializing world thread");
            Thread worldThread = new Thread(WorldCore.WorldThread);
            WorldCore.OnStart();
            worldThread.Start();
            threadLibrary.Add(worldThread, ThreadId.World);

            ConsoleEx.Log("Initializing disk thread");
            Thread diskThread = new Thread(DiskCore.ThreadMain);
            DiskCore.OnStart();
            diskThread.Start();
            threadLibrary.Add(diskThread, ThreadId.Disk);

            ConsoleEx.Log("Initializing update thread");
            Thread updateThread = new Thread(WebUpdaterCore.ThreadMain);
            WebUpdaterCore.OnStart();
            updateThread.Start();
            threadLibrary.Add(updateThread, ThreadId.Update);

            ConsoleEx.Log("Initializing GUI thread");
            Thread guiThread = new Thread(GUICore.ThreadMain);
            GUICore.OnStart();
            guiThread.Start();
            threadLibrary.Add(guiThread, ThreadId.GUI);

            ConsoleEx.Log("Switching to graphical console");
            HideConsole();

            while (state != ThreadState.Stopping)
            {
                Thread.Sleep(1);
            }

            ConsoleEx.Log("Initializing shutdown sequence");

            //ConsoleEx.Log("Switching back to native console");
            //ShowConsole();

            GUICore.OnStop();

            ConsoleEx.Log("Shutting down update thread");
            WebUpdaterCore.OnStop();
            updateThread.Join(1000);
            updateThread.Abort();

            ConsoleEx.Log("Shutting down disk thread");
            DiskCore.OnStop();
            diskThread.Join(1000);
            diskThread.Abort();

            ConsoleEx.Log("Shutting down world thread");
            WorldCore.OnStop();
            worldThread.Join(1000);
            worldThread.Abort();

            ConsoleEx.Log("Shutting down web thread");
            WebCore.OnStop();
            webThread.Join(1000);
            webThread.Abort();

            ConsoleEx.Log("Shutting down main thread");
            ConsoleEx.Log("Goodbye");
        }

        public static ThreadId GetThreadId(Thread handle)
        {
            ThreadId threadId = ThreadId.Worker;
            try { threadId = threadLibrary[handle]; }
            catch (Exception) { }
            return threadId;
        }
        public static void Shutdown()
        {
            state = ThreadState.Stopping;
        }
        public static void ShowConsole() { ShowWindow(GetConsoleWindow(), SW_SHOW); }
        public static void HideConsole() { ShowWindow(GetConsoleWindow(), SW_HIDE); }

        // Ugly stuff
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;
    }
}
