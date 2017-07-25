using System;
using System.Collections.Generic;
using System.Text;

namespace zpgServer
{
    public static class Random
    {
        static System.Random _randomizer;
        public static void RollSeed()
        {
            _randomizer = new System.Random();
        }
        public static int Get() { return _randomizer.Next(); }
        public static int Get(int max) { return _randomizer.Next(max); }
        public static int Get(int min, int max) { return _randomizer.Next(min, max); }
        public static int Get(Interval<int> interval) { return _randomizer.Next(interval.min, interval.max); }
        public static float Percentage() { return _randomizer.Next(0, 100) / 100.0f; }
    }
}
