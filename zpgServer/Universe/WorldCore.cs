using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace zpgServer
{
    public static class WorldCore
    {
        public static ThreadState threadState = ThreadState.NotStarted;
        public static void OnStart() { threadState = ThreadState.Running; }
        public static void OnStop() { threadState = ThreadState.Stopping; }

        public static void WorldThread()
        {
            while (threadState != ThreadState.Stopping)
            {
                Time.Tick();
                Universe.Update();
                foreach (Player player in Authorization.playerList) { player.Tick(); }

                Thread.Sleep(250);
            }
        }
    }
}
