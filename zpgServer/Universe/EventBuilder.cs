using System;
using System.Collections.Generic;
using System.Text;

namespace zpgServer
{
    public class EventBuilder : Event
    {
        public string text;
        public string originFile;
        public string rawEventGroup;
        public string rawEventSpecial;
        public string rawStoryAdvance;
        public string rawRewardMoney;
        public string rawRewardLoot;
        public string rawFilterPilotGender;
        public string rawFilterPilotMoney;
        public string rawFilterStoryStage;

        public EventBuilder(string originFile)
        {
            this.originFile = originFile;
            this.reward = new Reward();
            this.filters = new EventFilters();
        }
        public void ParseRawData()
        {
            // Message link
            messageLink = Localization.Add(text);

            // Event groups
            if (rawEventGroup != null)
            {
                string[] eventGroups = rawEventGroup.Replace(" ", "").Split('|');
                int eventGroupValue = 0;
                foreach (string group in eventGroups)
                {
                    try { eventGroupValue = eventGroupValue | (int)(typeof(EventGroup).GetField(group).GetValue(null)); }
                    catch (Exception) { }
                }
                eventGroup = eventGroupValue;
            }

            // Event special
            if (rawEventSpecial != null)
            {
                foreach (string name in Enum.GetNames(typeof(EventSpecial)))
                {
                    if (rawEventSpecial == name)
                    {
                        try
                        {
                            specialGroup = (EventSpecial)Enum.Parse(typeof(EventSpecial), name);
                            break;
                        }
                        catch (Exception) { }
                    }
                }
            }

            // Story advance reward
            if (rawStoryAdvance != null)
            {
                reward.storyAdvance = (StoryStage)Enum.Parse(typeof(StoryStage), rawStoryAdvance);
            }

            // Money reward
            reward.money = Interval.ParseToInt(rawRewardMoney);

            // Loot reward
            if (rawRewardLoot != null)
            {
                string[] lootItems = rawRewardLoot.Replace(" ", "").Split('|');
                try
                {
                    for (int i = 0; i < Math.Min(lootItems.Length, 4); i++)
                    {
                        reward.loot[i] = (Quality)Enum.Parse(typeof(Quality), lootItems[i]);
                    }
                }
                catch (Exception) { }
            }

            // Gender filter
            if (rawFilterPilotGender != null)
            {
                try { filters.pilotGender = (GenderFilter)Enum.Parse(typeof(GenderFilter), rawFilterPilotGender); }
                catch (Exception) { }
            }

            // Money filter
            filters.pilotMoney = Interval.ParseToInt(rawFilterPilotMoney);

            // Story stage filter
            filters.storyStage = Interval.ParseToStoryStage(rawFilterStoryStage);
        }
    }
}
