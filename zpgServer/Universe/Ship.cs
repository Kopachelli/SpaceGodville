using System;
using System.Collections.Generic;
using System.Text;

namespace zpgServer
{
    public enum ShipStatus
    {
        OnStation,
        Traveling,
        Exploring,
    }

    public class Ship
    {
        // Ship characteristics
        public string name;
        public ShipStatus status;

        // Ship links
        Pilot _pilot;
        Player _player;
        Planet _planet;
        List<LootItem> _cargo = new List<LootItem>();
        List<LootItem> _lastReceivedCargo = new List<LootItem>();
        public ShipLog log;

        // Ship timers
        public Timer travelTimer;
        public Timer updateTimer;

        // Constructor
        public Ship(string name, Player owner)
        {
            _player = owner;
            log = new ShipLog(this);
            this.name = name;
            _planet = Terraformer.FindPlanet(EventGroup.SpaceSector);
            updateTimer = new Timer(1f, true);
        }
        // Event handlers
        void OnPlanetArrival()
        {
            if ((_planet.eventGroups & EventGroup.Station) > 0)
                status = ShipStatus.OnStation;
            else
                status = ShipStatus.Exploring;
            _pilot.QueueEvent(EventSpecial.OnPlanetArrival);
        }
        public void OnUpdate()
        {
            // Timers
            if (travelTimer != null && travelTimer.Tick(updateTimer.delta)) { OnPlanetArrival(); }
        }
        // Public methods
        public void AddCargo(List<LootItem> newCargo)
        {
            _lastReceivedCargo.Clear();
            foreach (LootItem item in newCargo)
            {
                _cargo.Add(item);
                _lastReceivedCargo.Add(item);
            }
        }
        public void TravelTo(Planet target)
        {
            travelTimer = new Timer(Universe.GetDistance(_planet, target) / 10f, false);
            status = ShipStatus.Traveling;
            _planet = target;
            _pilot.QueueEvent(EventSpecial.OnTravelStart);
        }
        // Public properties
        public Planet planet { get { return _planet; } }
        public Player player { get { return _player; } }
        public List<LootItem> cargo {  get { return _cargo; } }
        public List<LootItem> lastReceivedCargo { get { return _lastReceivedCargo; } }
        public Pilot pilot
        {
            get { return _pilot; }
            set
            {
                _pilot = value;
            }
        }
    }
}
