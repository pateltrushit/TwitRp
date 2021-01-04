using System;
using System.Collections.Generic;
using System.Text;

namespace TwitRp.Core.Models.Logging
{
    public class LoggingOptions
    {
        public const string Logging = "Logging";

        public string LogLevel { get; set; }
        public ConsoleOptions Console { get; set; }
        public SeriLogOptions SeriLog { get; set; }
    }

    public class ConsoleOptions
    {
        public string LogLevel { get; set; }
    }
}
