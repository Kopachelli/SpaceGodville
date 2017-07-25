using System;
using System.Collections.Generic;
using System.Text;

namespace zpgServer
{
    public class Reward
    {
        public int exp = 1;
        public int stamina = 0;
        public int curiosity = 0;
        public Interval<int> money = new Interval<int>(0, 0);
        public StoryStage storyAdvance = StoryStage.Null;
        public Quality[] loot = { Quality.None, Quality.None, Quality.None, Quality.None };

        public static Reward none
        {
            get { return new Reward(); }
        }
        public static Reward AsExp(int amount)
        {
            Reward output = new Reward();
            output.exp = amount;
            return output;
        }
        public static Reward AsStamina(int amount)
        {
            Reward output = new Reward();
            output.stamina = amount;
            return output;
        }
        public static Reward AsCuriosity(int amount)
        {
            Reward output = new Reward();
            output.curiosity = amount;
            return output;
        }
        public static Reward AsMoney(int amount)
        {
            return AsMoney(amount, amount);
        }
        public static Reward AsMoney(int min, int max)
        {
            Reward output = new Reward();
            output.money = new Interval<int>(min, max);
            return output;
        }
        public static Reward AsStoryProgress(StoryStage newStage)
        {
            Reward output = new Reward();
            output.storyAdvance = newStage;
            return output;
        }
        public static Reward AsLoot(Quality quality, int count = 1)
        {
            count = Math.Max(1, Math.Min(count, 4));
            Reward output = new Reward();
            output.loot[0] = quality;
            if (count > 1) { output.loot[1] = quality; }
            if (count > 2) { output.loot[2] = quality; }
            if (count > 3) { output.loot[3] = quality; }
            return output;
        }
        public static Reward AsLoot(Quality itemA = Quality.None, Quality itemB = Quality.None, Quality itemC = Quality.None, Quality itemD = Quality.None)
        {
            Reward output = new Reward();
            output.loot[0] = itemA;
            output.loot[1] = itemB;
            output.loot[2] = itemC;
            output.loot[3] = itemD;
            return output;
        }

        public void ParseLoot(string input)
        {

        }
    }
}
