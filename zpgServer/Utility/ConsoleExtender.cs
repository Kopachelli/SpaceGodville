using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;

namespace zpgServer
{
    public static class ConsoleEx
    {
        static string _lastErrorMessage = "";
        public static string lastErrorMessage { get { return _lastErrorMessage; } }

        static List<string> _queuedMessages = new List<string>();

        public static void FlushQueue()
        {
            foreach (string s in _queuedMessages)
            {
                Console.WriteLine(s);
            }
            _queuedMessages.Clear();
        }
        public static void Log(string msg, bool showMetadata = true)
        {
            Thread threadHandle = Thread.CurrentThread;
            string output = msg;

            if (showMetadata)
            {
                output =
                    "[" + Time.GetTimestamp() + " | "
                    + Enum.GetName(typeof(ThreadId), Core.GetThreadId(Thread.CurrentThread)).ToUpper()
                    + "] " + msg;
            }

            Console.WriteLine(output);
            ConsoleWindow.WriteLine(output);

            if (Settings.logFiles.Length == 0)
                return;
            try
            {
                StreamWriter file = File.AppendText(Settings.logFiles[0]);
                file.WriteLine(output);
                file.Close();
            }
            catch (Exception) { }
        }
        public static void Debug(string msg)
        {
            string output = "[" + Time.GetTimestamp() + " | "
                + "DEBUG" + "] " + msg;
            Console.WriteLine(output);
            ConsoleWindow.WriteLine(output);

            if (Settings.logFiles.Length == 0)
                return;
            try
            {
                StreamWriter file = File.AppendText(Settings.logFiles[0]);
                file.WriteLine(output);
                file.Close();
            }
            catch (Exception) { }
        }
        public static void Error(string msg)
        {
            _lastErrorMessage = msg;
            Log("ERROR: " + msg);
        }
        public static void NewLine()
        {
            Console.WriteLine();
            ConsoleWindow.WriteLine("\n");
            if (Settings.logFiles.Length == 0)
                return;
            try
            {
                StreamWriter file = File.AppendText(Settings.logFiles[0]);
                file.WriteLine();
                file.Close();
            }
            catch (Exception) { }
        }
    }
}
