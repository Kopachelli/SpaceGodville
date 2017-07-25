using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace zpgServer
{
    public static class WebConstructor
    {
        public const string headOpener = "<html><head><script src='scripts/core.js'></script><meta charset='utf-8'>";
        public const string headToBody = "</head><body>";
        public const string bodyCloser = "</body></html>";

        public const string defaultHeader = headOpener + headToBody;
        public const string defaultFooter = bodyCloser;
        public static string GetHeroPage(Ship ship)
        {
            string page = File.ReadAllText("www/hero.template");
            page = page.Replace("<shipLog />", GetShipLog(ship));
            page = page.Replace("<pilotName />", ship.pilot.name);
            page = page.Replace("<shipName />", ship.name);
            page = page.Replace("<sessionKey />", ship.player.sessionKey);
            return page;
        }
        public static string GetShipLog(Ship ship)
        {
            string response = "";
            for (int i = ship.log.GetMessageCount() - 1; i >= 0; i--)
            {
                response += "<p>(" + ship.log.GetMessageTimestamp(i) + ") " + ship.log.GetMessage(i) + "</p>";
            }
            return response;
        }
    }
}
