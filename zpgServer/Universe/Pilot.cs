using System;
using System.Collections.Generic;
using System.Text;

namespace zpgServer
{
    public enum Gender
    {
        Male,
        Female,
    }

    public class Pilot
    {
        const int blockedEventCooldown = 5;
        const float updateChanceMinimal = 0.01f;
        const float updateChanceStep = 0.01f;

        // Hero characteristics
        public string name;
        public Gender gender;
        public int _level = 0;
        public int _exp = 0;

        // Hero variables
        float _stamina = 100f;
        float _curiosity = 0f;
        int _money = 0;
        public StoryStage storyStage = StoryStage.Initial;

        // Hero links
        public Ship ship;

        // Hero timers
        public Timer updateTimer;

        // Event cooldown
        float _updateChanceCumulative = 1.00f;
        public List<Event> blockedEvents = new List<Event>();
        Event _queuedEvent = null;

        // Last referenced links
        int _lastReceivedMoney = 0;
        

        // Constructor
        public Pilot(Ship vessel)
        {
            int genderRoll = Random.Get(0, 2);
            if (genderRoll == 0) { name = "Каваргус"; gender = Gender.Male; }
            else if (genderRoll == 1) { name = "Каваргуся"; gender = Gender.Female; }

            _stamina = 85f;
            _curiosity = 100f;
            ship = vessel;
            vessel.pilot = this;
            updateTimer = new Timer(1f, true);
            OnUpdate();
        }
        // Public properties
        public int lastReceivedMoney { get { return _lastReceivedMoney; } }
        public float stamina
        {
            get { return _stamina; }
            set { _stamina = Math.Max(0f, Math.Min(100f, value)); }
        }
        public float curiosity
        {
            get { return _curiosity; }
            set { _curiosity = Math.Max(0f, Math.Min(100f, value)); }
        }
        public int money
        {
            get { return _money; }
            set
            {
                int val = Math.Max(0, value);
                _lastReceivedMoney = val - _money;
                _money = val;
            }
        }
        public int level { get { return _level; } }
        public int exp
        {
            get { return _exp; }
            set
            {
                int delta = value - _exp;
                if (delta < 0)
                    return;

                _exp = value;
                // Determine new level
                int newLevel = 0;
                for (int i = 0; i < Settings.levelExpReq.Length; i++)
                {
                    if (_exp >= Settings.levelExpReq[i])
                    {
                        newLevel = i;
                    }
                }
                // If a new level is reached, invoke OnLevelUp.
                if (newLevel > _level)
                {
                    OnLevelUp(newLevel - _level);
                }
            }

        }
        // Events
        void OnLevelUp(int levelDelta)
        {
            _level += levelDelta;
            QueueEvent(EventSpecial.OnLevelUp);
        }
        // Public functions
        public void OnUpdate()
        {
            // Update stats
            if (ship.status == ShipStatus.Exploring)
            {
                stamina -= 0.1f * Time.deltaTime;
                curiosity -= 0.1f * Time.deltaTime;
            }
            else if (ship.status == ShipStatus.OnStation)
            {
                stamina += 0.1f * Time.deltaTime;
                curiosity += 0.1f * Time.deltaTime;
            }

            // Random event
            if (Random.Percentage() <= _updateChanceCumulative || ship.log.GetMessageCount() == 0)
            {
                // Random event occured! Check if something is queued
                if (_queuedEvent != null)
                {
                    // Invoke the queued event
                    RunEvent(_queuedEvent);
                    // Clear the queued event
                    _queuedEvent = null;
                }
                // If not - select some random event
                else
                {
                    int eventGroups = EventGroup.None;
                    if (ship.status == ShipStatus.Traveling) { eventGroups = EventGroup.InTravel; }
                    else if (ship.planet == null) { eventGroups = EventGroup.SpaceSector; }
                    else { eventGroups = ship.planet.eventGroups; }
                    // Invoke an event
                    RunEvent(eventGroups);
                }
                // Reset chance
                _updateChanceCumulative = updateChanceMinimal;
            }
            else
            {
                _updateChanceCumulative += updateChanceStep;
            }

            // Traveling
            if (ship.status == ShipStatus.Exploring && (stamina < 5f || curiosity < 5f))
                ship.TravelTo(Terraformer.FindPlanet(EventGroup.Station));
            if (ship.status == ShipStatus.OnStation && (stamina > 80f && curiosity > 90f) && storyStage > StoryStage.ShipExploration)
                ship.TravelTo(Terraformer.FindPlanet(EventGroup.Planet | EventGroup.SpaceSector));
        }
        public Event GetEvent(int eventGroup)
        {
            Event e = Destiny.FindEvent(this, eventGroup);
            if (e == null)
            {
                ConsoleEx.Log("ERROR: No event found in destiny. Emptying queue and retrying.");
                blockedEvents.Clear();
                e = Destiny.FindEvent(this, eventGroup);
                if (e == null)
                {
                    ConsoleEx.Log("ERROR: No suitable event. Please check event filters for group id "
                        + eventGroup + ".");
                    return null;
                }
            }
            return e;
        }
        public Event GetEvent(EventSpecial specialGroup)
        {
            Event e = Destiny.FindEvent(this, specialGroup);
            if (e == null)
            {
                ConsoleEx.Log("ERROR: No event found in destiny. Emptying queue and retrying.");
                blockedEvents.Clear();
                e = Destiny.FindEvent(this, specialGroup);
                if (e == null)
                {
                    ConsoleEx.Log("ERROR: No suitable event. Please check event filters for special event "
                        + Enum.GetName(typeof(EventSpecial), specialGroup) + ".");
                    return null;
                }
            }
            return e;
        }
        public void RunEvent(int eventGroup) { RunEvent(GetEvent(eventGroup)); }
        public void RunEvent(EventSpecial specialGroup) { RunEvent(GetEvent(specialGroup)); }
        public void RunEvent(Event eventHandle)
        {
            /* If the code fails on the next line with a nullPointerException, then
            * it couldn't find an event suitable to all the filters. And for the
            * debugging purposes it should crash. Try-catch comes later.
            */
            eventHandle.AddReward(this);
            ship.log.Add(eventHandle.messageLink);
            // Event cooldown
            blockedEvents.Add(eventHandle);
            while (blockedEvents.Count > blockedEventCooldown) { blockedEvents.RemoveAt(0); }
        }
        public void QueueEvent(int eventGroup)
        {
            if (_queuedEvent == null) { _queuedEvent = GetEvent(eventGroup); }
        }
        public void QueueEvent(EventSpecial specialGroup)
        {
            if (_queuedEvent == null) { _queuedEvent = GetEvent(specialGroup); }
        }
    }
}
