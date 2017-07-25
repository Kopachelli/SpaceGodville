using System;
using System.Collections.Generic;
using System.Text;

namespace zpgServer
{
    public class ShipLog
    {
        const int logLengthLimit = 10;

        List<ShipLogMessage> _messageList = new List<ShipLogMessage>();

        public Ship ship;

        public ShipLog(Ship owner)
        {
            ship = owner;
        }
        public void Add(string textLink)
        {
            string message = Localization.ParseForShip(ship, Localization.GetRandom(textLink));
            _messageList.Add(new ShipLogMessage(message));
            // Clear up
            while (_messageList.Count > logLengthLimit)
            {
                _messageList.RemoveAt(0);
            }
            ship.player.NotifyOnUpdate();
            //ConsoleEx.Log(ship.pilot.name + ": " + message);
        }
        public string GetMessage(int index) { return _messageList[index].text; }
        public string GetMessageTimestamp(int index) { return _messageList[index].timestamp; }
        public int GetMessageCount() { return _messageList.Count; }
    }
}
