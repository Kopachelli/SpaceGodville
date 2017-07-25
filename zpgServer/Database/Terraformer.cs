using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace zpgServer
{
    public static class Terraformer
    {
        public static List<Planet> planetLibrary = new List<Planet>();

        public static void LoadPlanets()
        {
            Add("Большие Тарелки", new Vector2<int>(0, 0), EventGroup.Station);
            //Add("Космос", new Vector2<int>(0, 0), EventGroup.SpaceSector);
            Add("Апофиги 6", new Vector2<int>(177, -378), EventGroup.Planet);
            Add("Юстус 3", new Vector2<int>(-95, 100), EventGroup.Planet);
            Add("Ироста 17", new Vector2<int>(-5, -99), EventGroup.Planet);
            Add("Третий Медвед", new Vector2<int>(247, 384), EventGroup.SpaceSector);

            /*XmlSerializer ser = new XmlSerializer(typeof(List<Planet>));
            TextWriter writer = new StreamWriter("planet.xml");
            ser.Serialize(writer, planetLibrary);
            writer.Close();*/
        }
        public static void Add(string name, Vector2<int> position, int eventGroups)
        {
            Planet planet = new Planet(name, position, eventGroups);
            planetLibrary.Add(planet);
        }
        public static Planet FindPlanet(int eventGroup)
        {
            List<Planet> filteredPlanets = new List<Planet>();

            foreach (Planet planet in planetLibrary)
            {
                if ((planet.eventGroups & eventGroup) > 0)
                {
                    filteredPlanets.Add(planet);
                }
            }
            return filteredPlanets[Random.Get(0, filteredPlanets.Count)];
        }
    }
}
