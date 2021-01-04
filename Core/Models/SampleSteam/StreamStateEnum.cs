using System;
using System.Collections.Generic;
using System.Text;

namespace TwitRp.Core.Models.SampleSteam
{
    public enum StreamStateEnum
    {
                   //
            // Summary:
            //     The stream is not running. In this state the stream configuration can be changed.
            Stop = 0,
            //
            // Summary:
            //     Stream is Running.
            Running = 1,
            //
            // Summary:
            //     Stream is paused. The stream configuration cannot be changed in this state.
            Pause = 2,

            Error = 3
    }
}
