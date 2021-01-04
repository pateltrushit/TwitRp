using System;
using System.Collections.Generic;
using System.Text;

namespace TwitRp.Core.Models.Twitter
{
    public class TwitterCredentialsOptions
    {
        public const string TwitterCredentials = "TwitterCredentials";

        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
        public string AccessToken { get; set; }
        public string AccessSecret { get; set; }
    }
}
