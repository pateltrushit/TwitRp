using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TwitRp.Core.Models.SampleSteam;

namespace TwitRp.Core.Interfaces
{
    public interface IStreamProcessor
    {
        Task<StreamStateEnum> GetStreamStateAsync();
        StreamStatistics GetStreamStatistics();
        Task StartStreamAsync();
        void StopStream();

    }
}
