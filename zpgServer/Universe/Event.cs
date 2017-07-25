using System;
using System.Collections.Generic;
using System.Text;

namespace zpgServer
{
    public class Event
    {
        public string messageLink;
        public int chance;
        public int eventGroup;
        public EventSpecial specialGroup;
        public Reward reward;
        public EventFilters filters;

        protected Event()
        {
            this.chance = Event.defaultChance;
            this.reward = null;
            this.filters = null;
            this.eventGroup = EventGroup.None;
            this.specialGroup = EventSpecial.None;
        }
        public Event(int eventGroup, string messageLink, Reward reward, EventFilters filters, int chance)
        {
            this.eventGroup = eventGroup;
            this.messageLink = messageLink;
            this.reward = reward;
            this.filters = filters;
            this.chance = chance;
            this.specialGroup = EventSpecial.None;
        }
        public Event(EventSpecial specialGroup, string messageLink, Reward reward, EventFilters filters, int chance)
        {
            this.specialGroup = specialGroup;
            this.messageLink = messageLink;
            this.reward = reward;
            this.filters = filters;
            this.chance = chance;
            this.eventGroup = EventGroup.None;
        }

        public void AddReward(Pilot target)
        {
            // Exp
            target.exp += reward.exp;
            // Stamina
            target.stamina += reward.stamina;
            // Curiosity
            target.curiosity += reward.curiosity;
            // Money
            target.money += Random.Get(reward.money);
            // Story
            if (reward.storyAdvance != StoryStage.Null)
                target.storyStage = reward.storyAdvance;
            // Loot
            List<LootItem> lootList = new List<LootItem>();
            for (int i = 0; i < reward.loot.Length; i++)
            {
                if (reward.loot[i] != Quality.None)
                {
                    lootList.Add(Looter.GetRandom(reward.loot[i]));
                }
            }
            target.ship.AddCargo(lootList);
        }

        public const int defaultChance = 10;
    }
}
