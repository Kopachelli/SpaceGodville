using System;
using System.Collections.Generic;
using System.Text;

namespace zpgServer
{
    public static class Universe
    {
        public static List<Planet> planets = new List<Planet>();
        public static List<Ship> ships = new List<Ship>();
        public static List<Pilot> pilots = new List<Pilot>();

        public static void Create()
        {
            foreach (Planet p in Terraformer.planetLibrary)
            {
                planets.Add(new Planet(p.name, p.worldPosition, p.eventGroups));
            }
        }
        public static Ship AddNewPlayerShip(string shipName, Player forPlayer)
        {
            Ship s = new Ship(shipName, forPlayer); ships.Add(s);
            Pilot p = new Pilot(s); pilots.Add(p);
            forPlayer.ship = s;
            return s;
        }

        public static float GetDistance(Planet from, Planet to)
        {
            return Convert.ToSingle(Math.Sqrt(Math.Pow(to.worldPosition.x - from.worldPosition.x, 2) + Math.Pow(to.worldPosition.y - from.worldPosition.y, 2)));
        }

        public static void Update()
        {
            foreach (Pilot p in Universe.pilots)
            {
                if (p.updateTimer.Tick())
                {
                    p.OnUpdate();
                }
            }
            foreach (Ship s in Universe.ships)
            {
                if (s.updateTimer.Tick())
                {
                    s.OnUpdate();
                }
            }
        }
    }
}
