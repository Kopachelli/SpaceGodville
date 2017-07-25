using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;

namespace zpgServer
{
    public enum DiskAction
    {
        UpdateAccountDatabase,
    }
    class DiskCore
    {
        public static ThreadState threadState = ThreadState.NotStarted;
        public static void OnStart() { threadState = ThreadState.Running; }
        public static void OnStop()
        {
            threadState = ThreadState.Stopping;
        }

        static List<DiskAction> _queuedActions = new List<DiskAction>();
        public static void Enqueue(DiskAction action)
        {
            if (!_queuedActions.Contains(action))
            {
                _queuedActions.Add(action);
            }
        }

        static void FlushQueue()
        {
            foreach (DiskAction action in _queuedActions)
            {
                switch (action)
                {
                    case DiskAction.UpdateAccountDatabase:
                        XmlManager.SaveAccounts();
                        break;
                }
           
            }
            _queuedActions.Clear();
        }
        public static void ThreadMain()
        {
            while (threadState != ThreadState.Stopping)
            {
                FlushQueue();
                Thread.Sleep(1000);
            }
            // Final flush, just to be sure
            FlushQueue();
        }
    }
}
