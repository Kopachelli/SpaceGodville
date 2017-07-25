using System;
using System.Collections.Generic;
using System.Text;

namespace zpgServer
{
    public enum GenderFilter
    {
        Any,
        MaleOnly,
        FemaleOnly,
    }

    public class EventFilters
    {
        public GenderFilter pilotGender = GenderFilter.Any;
        public Interval<int> pilotMoney = new Interval<int>(0, Int32.MaxValue);
        public Interval<StoryStage> storyStage = new Interval<StoryStage>(StoryStage.Initial, StoryStage.Completed);

        public bool IsGood(Pilot target)
        {
            // Gender check
            if (!(pilotGender == GenderFilter.Any
                || (pilotGender == GenderFilter.MaleOnly && target.gender == Gender.Male)
                || (pilotGender == GenderFilter.FemaleOnly && target.gender == Gender.Female)))
            {
                return false;
            }
            // Money check
            if (target.money < pilotMoney.min || target.money > pilotMoney.max) { return false; }
            // Story check
            if (target.storyStage < storyStage.min || target.storyStage > storyStage.max) { return false; }

            // All good
            return true;
        }

        public static EventFilters none
        {
            get { return new EventFilters(); }
        }
        public static EventFilters StoryRequired(StoryStage stage)
        {
            EventFilters output = new EventFilters();
            output.storyStage = new Interval<StoryStage>(stage, stage);
            return output;
        }
        public static EventFilters StoryCompleted(StoryStage stage)
        {
            EventFilters output = new EventFilters();
            output.storyStage = new Interval<StoryStage>(stage + 1, StoryStage.Completed);
            return output;
        }
    }
}
