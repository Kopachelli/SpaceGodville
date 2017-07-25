using System;
using System.Collections.Generic;
using System.Text;

namespace zpgServer
{
    public class Settings
    {
        public const float playerDisconnectTimer = 60.00f;
        public const string playersXmlPath = "data/players.xml";
        public const string playersTempXmlPath = "data/player.xml.new";
        public const string playersBackupXmlPath = "data/player.xml.backup";
        public static readonly string[] logFiles = 
            { "log/log00.txt", "log/log01.txt", "log/log02.txt", "log/log03.txt" };
        public static readonly string[] eventsXmlPaths =
            { "data/eventsTianara.xml", "data/eventsSvillaya.xml"};
        public static readonly string[] itemsXmlPaths =
            { "data/itemsTianara.xml", "data/itemsSvillaya.xml"};
        public static readonly int[] levelExpReq =
            { 0, 100, 250, 450, 700, 1000, 1350, 1750, 2200, 2700 };
    }

}
