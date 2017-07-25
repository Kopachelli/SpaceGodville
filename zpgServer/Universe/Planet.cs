using System;
using System.Collections.Generic;
using System.Text;

namespace zpgServer
{
    public class Planet
    {
        // Planet variables
        string _name;
        Vector2<int> _worldPosition;
        int _eventGroups;

        // Constructor
        public Planet()
        {
            _name = "missingno";
            _eventGroups = EventGroup.None;
            _worldPosition = new Vector2<int>(0, 0);
        }
        public Planet(string name, Vector2<int> position, int eventGroups = EventGroup.Planet)
        {
            _name = name;
            _eventGroups = eventGroups;
            _worldPosition = position;
        }

        // Public properties
        public string name { get { return _name; } }
        public Vector2<int> worldPosition { get { return _worldPosition; } }
        public int eventGroups { get { return _eventGroups; } }
    }
}
