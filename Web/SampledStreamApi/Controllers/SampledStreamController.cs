using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TwitRp.Core.Interfaces;
using TwitRp.Core.Models.SampleSteam;
using TwitRp.Core.Models.Twitter;

namespace TwitRp.SampledStreamApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SampledStreamController : ControllerBase
    {
        private readonly ILogger<SampledStreamController> _logger;
        public TwitterCredentialsOptions twitterCredentialsOptions { get; private set; }

        static IStreamProcessor SampleStreamProcessor;

        public SampledStreamController(ILogger<SampledStreamController> logger, IConfiguration configuration, IStreamProcessor sampleStreamProcessor)
        {
            _logger = logger;
            SampleStreamProcessor = sampleStreamProcessor;
            twitterCredentialsOptions = configuration.GetSection(TwitterCredentialsOptions.TwitterCredentials).Get<TwitterCredentialsOptions>();
        }

        [HttpGet]
        [Route("/status")]
        public async Task<string> GetStreamStatusAsync()
        {
            string status = "Error";
            if (SampleStreamProcessor != null)
            {
                switch (await SampleStreamProcessor.GetStreamStateAsync())
                {
                    case StreamStateEnum.Pause:
                        status = "Paused";
                        break;
                    case StreamStateEnum.Stop:
                        status = "Stopped";
                        break;
                    case StreamStateEnum.Running:
                        status = "Running";
                        break;
                }
            }
            return status;
        }


        [HttpGet]
        [Route("/statistics")]
        public async Task<StreamStatistics> GetStreamStatistics()
        {
            StreamStatistics streamStatistics = new StreamStatistics();
            if (SampleStreamProcessor != null)
            {
                streamStatistics = SampleStreamProcessor.GetStreamStatistics();
            }
            return streamStatistics;
        }

        [HttpPost]
        [Route("/start")]
        public async Task<string> StartStream()
        {
            string status = string.Empty;
            if (SampleStreamProcessor != null && await SampleStreamProcessor.GetStreamStateAsync() !=  StreamStateEnum.Running)
            {
                SampleStreamProcessor.StartStreamAsync();
                status = "stream started...";
            }
            return status;
        }

        [HttpPost]
        [Route("/stop")]
        public async Task<string> StopStream()
        {
            string status = string.Empty;
            if (SampleStreamProcessor != null && await SampleStreamProcessor.GetStreamStateAsync() != StreamStateEnum.Stop)
            {
                SampleStreamProcessor.StopStream();
                status = "stream stopped.";
            }
            return status;
        }
    }
}
