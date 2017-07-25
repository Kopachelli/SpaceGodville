using System;
using System.Collections.Generic;
using System.Text;

namespace zpgServer
{
    public static class Time
    {
        static bool _isInitialized = false;

        static int _lastTimeframe;
        static float _deltaTime;

        public static float deltaTime { get { return _deltaTime; } }

        public static void Tick()
        {
            if (_isInitialized)
            {
                int newTimeframe = Environment.TickCount;

                _deltaTime = (newTimeframe - _lastTimeframe) / 1000.00f;

                _lastTimeframe = newTimeframe;
            }
            else
            {
                _isInitialized = true;
                _lastTimeframe = Environment.TickCount;
            }
        }

        public static String GetTimestamp()
        {
            return DateTime.UtcNow.ToLocalTime().ToLongTimeString();
        }
    }
}
