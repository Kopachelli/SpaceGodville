using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace zpgServer
{
    public static class InputCore
    {
        public static ThreadState threadState = ThreadState.NotStarted;
        public static void OnStart() { threadState = ThreadState.Running; }
        public static void OnStop() { threadState = ThreadState.Stopping; }

        public static void InputThread()
        {
            /*while (threadState != ThreadState.Stopping)
            {
                string line = Console.ReadLine();
                ConsoleEx.Log(line);
                ConsoleEx.FlushQueue();
            }*/
        }
    }
}
