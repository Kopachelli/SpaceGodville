using System;
using System.Collections.Generic;
using System.Text;

namespace zpgServer
{
    public class Interval<T>
    {
        public T min;
        public T max;

        public Interval(T min, T max)
        {
            this.min = min;
            this.max = max;
        }
        public override string ToString()
        {
            return min.ToString() + "-" + max.ToString();
        }
    }

    public static class Interval
    {
        public static Interval<int> ParseToInt(string input)
        {
            if (input == null || input.Length == 0 || !input.Contains("-"))
                return new Interval<int>(0, 0);

            input = input.ToLower();
            input = input.Replace("inf", Int32.MaxValue.ToString());

            int separatorPos = input.IndexOf('-');
            int left = Int32.Parse(input.Substring(0, separatorPos));
            int right = Int32.Parse(input.Substring(separatorPos + 1));
            return new Interval<int>(left, right);
        }
        public static Interval<StoryStage> ParseToStoryStage(string input)
        {
            if (input == null || input.Length == 0 || !input.Contains("-"))
                return new Interval<StoryStage>(StoryStage.Initial, StoryStage.Completed);

            input = input.ToLower();

            int separatorPos = input.IndexOf('-');
            StoryStage left = StoryStage.Initial, right = StoryStage.Completed;
            try
            {
                left = (StoryStage)Enum.Parse(typeof(StoryStage), input.Substring(0, separatorPos));
                right = (StoryStage)Enum.Parse(typeof(StoryStage), input.Substring(separatorPos + 1));
            }
            catch (Exception) { }
            return new Interval<StoryStage>(left, right);
        }
    }
}
