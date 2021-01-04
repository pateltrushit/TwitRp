using System;
using System.Collections.Generic;
using System.Text;

namespace TwitRp.Core.Models.SampleSteam
{
    public class SampleStreamOptions
    {
        public const string SampleStream = "SampleStream";

        public string LanguageFilter { get; set; }
        public short FilterLevel { get; set; }
        public long MaxAllowedTweetsLimit { get; set; }
        public int TweetsPageSize { get; set; }
    }
}
