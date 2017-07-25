using System;
using System.Collections.Generic;
using System.Text;

namespace zpgServer
{
    public enum EventSpecial
    {
        None,
        OnTravelStart,
        OnPlanetArrival,
        OnLevelUp,
    }

    public static class EventGroup
    {
        public const int None = 0;
        public const int SpaceSector = 1;
        public const int InTravel = 2;
        public const int Planet = 4;
        public const int Station = 8;

        public const int Any = 8191;
    }
}
