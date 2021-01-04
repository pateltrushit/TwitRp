using System;
using System.Collections.Generic;
using System.Text;

namespace TwitRp.Core.Models.SampleSteam
{
    public class StreamStatistics
    {
        public long TotalNumberOfTweets { get; set; } 
        public long TimeElapsedInSeconds { get; set; }
        public int TweetsPerHour { get; set; }
        public int TweetsPerMinute { get; set; }
        public int TweetsPerSecond { get; set; }
        public int TweetsContainingEmojis { get; set; }
        public int TweetsContainingHashTags { get; set; }
        public double PercentContainingHashTags { get; set; }
        public double PercentContainEmojis { get; set; }
        public Dictionary<string, int> TopEmojis { get; set; }
        public Dictionary<string, int> TopHashtags { get; set; }
        //public double PercentContainUrls { get; set; }
        //public double PercentContainPictureUrls { get; set; }
        //public double PercentContaintVideoUrls { get; set; }

    }
}
