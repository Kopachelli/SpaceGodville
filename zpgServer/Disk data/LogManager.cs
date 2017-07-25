using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace zpgServer
{
    public static class LogManager
    {
        public static void CleanUpFiles()
        {
            if (Settings.logFiles.Length == 0)
                return;

            Directory.CreateDirectory("log");
            File.Delete(Settings.logFiles[Settings.logFiles.Length - 1]);
            if (Settings.logFiles.Length >= 2)
            {
                for (int i = Settings.logFiles.Length - 2; i >= 0; i--)
                {
                    if (File.Exists(Settings.logFiles[i]))
                    {
                        File.Move(Settings.logFiles[i], Settings.logFiles[i + 1]);
                    }
                }
            }
        }
    }
}
