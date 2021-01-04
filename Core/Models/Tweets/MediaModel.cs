using System;
using System.Collections.Generic;
using System.Text;

namespace TwitRp.Core.Models.Tweets
{
    public class MediaModel
    {
        public long? Id { get; set; }
        
        public string IdStr { get; set; }

        public string MediaURL { get; set; }

        public string MediaURLHttps { get; set; }

        public string URL { get; set; }

        public string DisplayURL { get; set; }

        public string ExpandedURL { get; set; }

        public string MediaType { get; set; }
        
        public int[] Indices { get; set; }
    }
}
