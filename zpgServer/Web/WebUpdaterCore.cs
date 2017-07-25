using System;
using System.Collections.Generic;
using System.Text;

namespace zpgServer
{
    public static class WebUpdaterCore
    {
        public static ThreadState threadState = ThreadState.NotStarted;
        public static void OnStart() { threadState = ThreadState.Running; }
        public static void OnStop() { threadState = ThreadState.Stopping; }

        public static List<PlayerResponsePair> waitingPlayers = new List<PlayerResponsePair>();

        public static void Enqueue(PlayerResponsePair data)
        {
            if (!waitingPlayers.Contains(data))
            {
                waitingPlayers.Add(data);
            }
        }

        public static void ThreadMain()
        {
            while (threadState != ThreadState.Stopping)
            {
                List<PlayerResponsePair> servedPlayers = new List<PlayerResponsePair>();
                // Serving
                foreach (PlayerResponsePair data in waitingPlayers)
                {
                    if (data.player.isWaitingForUpdate)
                    {
                        WebWriter.Reply(data.response, WebConstructor.GetShipLog(data.player.ship));
                        data.player.NotifyOnFlush();
                        servedPlayers.Add(data);
                    }
                }
                if (servedPlayers.Count > 0)
                    ConsoleEx.Log("Served updates to " + servedPlayers.Count + " player(s).");
                // Cleaning up
                foreach (PlayerResponsePair data in servedPlayers)
                {
                    waitingPlayers.Remove(data);
                }
                // Waiting
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}
