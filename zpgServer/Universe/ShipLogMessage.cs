using System;
using System.Collections.Generic;
using System.Text;

namespace zpgServer
{
    public class ShipLogMessage
    {
        string _text;
        string _timestamp;

        public String text
        {
            get { return _text; }
        }
        public string timestamp
        {
            get { return _timestamp; }
        }

        public ShipLogMessage(string text)
        {
            _text = text;
            _timestamp = Time.GetTimestamp();
        }
    }
}
