using System;
using System.Collections.Generic;
using System.Text;

namespace TwitRp.Core.Models.Logging
{
    public class SeriLogOptions
    {
        public const string SeriLog = "SeriLog";
        public string LogFilePath { get; set; }
        public int RetainedFileCountLimit { get; set; }
        public bool Dispose { get; set; }
    }
}
