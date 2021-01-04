using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Core.Models;
using Tweetinvi.Streaming;
using Tweetinvi.Streaming.Parameters;
using TwitRp.Core.Interfaces;
using TwitRp.Core.Interfaces.Repository;
using TwitRp.Core.Models.SampleSteam;
using TwitRp.Core.Models.Tweets;
using TwitRp.Core.Models.Twitter;
using TwitRp.Data.Mappers;


namespace TwitRp.Services.Processors.SampleStream
{
    public class SampleStreamProcessor : IStreamProcessor,IProcessor
    {
        public TwitterClient UserClient { get; set; }
        public ISampleStream SampleStream { get; set; }
        public ILogger Logger { get; set; }
        public IConfiguration Configuration { get; set; }
        public TwitterCredentialsOptions TwitterCredentialsOptions { get; set; }
        public SampleStreamOptions SampleStreamOptions { get; set; }

        public IEmojiProcessor EmojiProcessor { get; set; }

        public IHashtagProcessor HashtagProcessor { get; set; }

        public ICacheRepository CacheRepository { get; set; }

        public ITweetRepository TweetRepository { get; set; }

        public DateTime StartTime { get; set; }

        public List<Tweet> Tweets = new List<Tweet>();
        public int TimeElapsedInSeconds = 0;
        public int TweetsPerSecond = 0;
        public int NumberOfTotalTweetsPerSession = 0;
        //public Dictionary<string, int> EmojisWithCount = new Dictionary<string, int>();
        public int NumberOfTweetsWithEmojis = 0;
        public int NumberOfTweetsWithHashTags = 0;



        public SampleStreamProcessor(IConfiguration configuration, 
                    ITweetRepository tweetRepository, 
                    ICacheRepository cacheRepository,
                    ILogger<SampleStreamProcessor> logger, 
                    IEmojiProcessor emojiProcessor,
                    IHashtagProcessor hashtagProcessor)
        {
            Logger = logger;
            EmojiProcessor = emojiProcessor;
            HashtagProcessor = hashtagProcessor;
            TweetRepository = tweetRepository;
            CacheRepository = cacheRepository;
            Configuration = configuration;
            TwitterCredentialsOptions = configuration.GetSection(TwitterCredentialsOptions.TwitterCredentials).Get<TwitterCredentialsOptions>();
            SampleStreamOptions = configuration.GetSection(SampleStreamOptions.SampleStream).Get<SampleStreamOptions>();
            UserClient = new TwitterClient(TwitterCredentialsOptions.ConsumerKey, TwitterCredentialsOptions.ConsumerSecret, TwitterCredentialsOptions.AccessToken, TwitterCredentialsOptions.AccessSecret);
            SampleStream = UserClient.Streams.CreateSampleStream();
            SampleStream.AddLanguageFilter(SampleStreamOptions.LanguageFilter);
            SampleStream.FilterLevel = (StreamFilterLevel)SampleStreamOptions.FilterLevel;
            SampleStream.TweetReceived += (sender, eventArgs) =>
            {
                Tweets.Add((Tweet)eventArgs.Tweet);
                if (NumberOfTotalTweetsPerSession > SampleStreamOptions.MaxAllowedTweetsLimit)
                {
                    StopStream();
                }
                if (Tweets.Count > SampleStreamOptions.TweetsPageSize || GetStreamStateAsync().Result == StreamStateEnum.Stop)
                {
                    ProcessTweetsAsync();
                }
                
            };
        }

        public async Task StartStreamAsync()
        {
            try
            {
                if (this.SampleStream.StreamState != StreamState.Running)
                {
                    StartTime = DateTime.Now;
                    NumberOfTotalTweetsPerSession = 0;
                    SampleStream.StartAsync();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error occurred during stream processing", null);
                StopStream();
            }

        }

        public void StopStream()
        {
            try
            {
                if (SampleStream.StreamState != StreamState.Stop)
                {
                    SampleStream.Stop();
                    Logger.LogInformation("SampledStream Stopped");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Error occurred at StopStream", ex);
            }
        }

        public async Task ProcessTweetsAsync()
        {
            List<TweetModel> tweetsToProcess = new List<TweetModel>();
            try
            {
                if(this.GetStreamStateAsync().Result == StreamStateEnum.Running)
                {
                    tweetsToProcess = TweetMapper.MapTweets(Tweets.GetRange(0, Tweets.Count));
                    if (tweetsToProcess != null && tweetsToProcess.Count > 0)
                    {
                        Tweets.RemoveRange(0, tweetsToProcess.Count);
                        List<Task> tasks = new List<Task>();

                        tasks.Add(ProcessEmojisAsync(tweetsToProcess));
                        tasks.Add(ProcessHashTagsAsync(tweetsToProcess));
                        tasks.Add(SaveStatisticsAsync(tweetsToProcess));
                        tasks.Add(SaveTweetsAsync(tweetsToProcess));
                        tasks.Add(PrintStatisticsAsync());

                        Task.WaitAll(tasks.ToArray());
                    }
                }
                
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error at ProcessTweets", null);
            }
        }

        public async Task<StreamStateEnum> GetStreamStateAsync()
        {
            StreamStateEnum streamStatus = StreamStateEnum.Error;
            try
            {
                if (this.SampleStream != null)
                {
                    switch (this.SampleStream.StreamState)
                    {
                        case StreamState.Pause:
                            streamStatus = StreamStateEnum.Pause;
                            break;
                        case StreamState.Stop:
                            streamStatus = StreamStateEnum.Stop;
                            break;
                        case StreamState.Running:
                            streamStatus = StreamStateEnum.Running;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error at GetStreamState", null);
            }
            
            return streamStatus;
        }

        public StreamStatistics GetStreamStatistics()
        {
            StreamStatistics streamStatistics = new StreamStatistics();
            try
            {
                streamStatistics.TweetsPerSecond = (int)CacheRepository.Get<int?>("TweetsPerSecond");
                streamStatistics.TweetsPerMinute = (int)CacheRepository.Get<int>("TweetsPerMinute");
                streamStatistics.TweetsPerHour = (int)CacheRepository.Get<int>("TweetsPerHour");
                streamStatistics.TotalNumberOfTweets = (int)CacheRepository.Get<int>("NumberOfTotalTweets");
                streamStatistics.TimeElapsedInSeconds = (long)CacheRepository.Get<int?>("TimeElapsedInSeconds");
                streamStatistics.TimeElapsedInSeconds = (long)CacheRepository.Get<int?>("TweetsPerHour");
                Dictionary<string, int> emojisWithCount = CacheRepository.Get<Dictionary<string, int>>("EmojisWithCount");
                streamStatistics.TopEmojis = CacheRepository.Get<Dictionary<string, int>>("TopEmojis");
                streamStatistics.TopHashtags = CacheRepository.Get<Dictionary<string, int>>("TopHashtags");
                streamStatistics.TweetsContainingHashTags = CacheRepository.Get<int>("TweetsWithHashTags");
                streamStatistics.TweetsContainingEmojis = CacheRepository.Get<int>("TweetsWithEmojis");
                if(streamStatistics.TotalNumberOfTweets > 0)
                {
                    streamStatistics.PercentContainingHashTags = (streamStatistics.TweetsContainingHashTags * 100) / streamStatistics.TotalNumberOfTweets;
                }
                
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error at getTweetsToProcess", null);
            }
            return streamStatistics;
        }

        public async Task SaveStatisticsAsync(List<TweetModel> tweets)
        {
            await Task.Run(() => {
                int numberOfTotalTweets = (int)CacheRepository.Get<int>("NumberOfTotalTweets");
                NumberOfTotalTweetsPerSession += tweets.Count;
                numberOfTotalTweets += NumberOfTotalTweetsPerSession;
                TimeSpan timeSpan = DateTime.Now - StartTime;
                int? timeElapsedInSecondsFromCache = CacheRepository.Get<int?>("TimeElapsedInSeconds");
                TimeElapsedInSeconds = ((int)timeSpan.TotalSeconds);
                TweetsPerSecond = (int)Math.Round((double)(numberOfTotalTweets / TimeElapsedInSeconds));
                int tweetsPerMinute = TweetsPerSecond * 60;
                int tweetsPerHour = tweetsPerMinute * 60;
                CacheRepository.Set<int>("NumberOfTotalTweets", numberOfTotalTweets, 9999);
                CacheRepository.Set<long>("TimeElapsedInSeconds", TimeElapsedInSeconds, 9999);
                CacheRepository.Set<int>("TweetsPerSecond", TweetsPerSecond, 9999);
                CacheRepository.Set<long>("TweetsPerMinute", tweetsPerMinute, 9999);
                CacheRepository.Set<long>("TweetsPerHour", tweetsPerHour, 9999);
            });
        }

        public async Task PrintStatisticsAsync()
        {
            await Task.Run(() => {
                Logger.LogInformation($"Total Number of Tweets: {CacheRepository.Get<int>("NumberOfTotalTweets")}");
                Logger.LogInformation($"Number of Tweets Per Second: {CacheRepository.Get<int>("TweetsPerSecond")}");
                Dictionary<string,int> topEmojisWithCount = CacheRepository.Get<Dictionary<string, int>>("TopEmojis");
                if (topEmojisWithCount != null && topEmojisWithCount.Count > 0)
                {
                    foreach (KeyValuePair<string, int> emojiWithCount in topEmojisWithCount)
                    {
                        Logger.LogInformation($"Emoji: {emojiWithCount.Key}, Count: {emojiWithCount.Value}");
                    }
                }
            });
        }

        public async Task SaveTweetsAsync(List<TweetModel> tweets)
        {
            try
            {
                await Task.Run(() => {
                    if (Tweets.Count > 0)
                    {
                        TweetRepository.SaveListOfTweets(tweets);
                    }
                });

            }
            catch (Exception ex)
            {
                Logger.LogInformation($"Error at SaveTweetsAsync");
            }
        }


        public async Task ProcessEmojisAsync(List<TweetModel> tweets)
        {
            List<string> listOfEmojis = new List<string>();
            try
            {
                await Task.Run(() =>
                    {
                        if (tweets != null && tweets.Count > 0)
                        {
                            List<string> listOfTweetTexts = tweets.Select(x => x.FullText).ToList();
                            listOfEmojis.AddRange(this.EmojiProcessor.GetListOfEmojis(listOfTweetTexts));
                            Dictionary<string, int> emojisWithCount = CacheRepository.Get<Dictionary<string, int>>("EmojisWithCount");
                            if (emojisWithCount == null) emojisWithCount = new Dictionary<string, int>();
                            emojisWithCount = this.EmojiProcessor.GetEmojisWithCount(emojisWithCount, listOfEmojis);
                            Dictionary<string, int> topEmojis = emojisWithCount.OrderByDescending(x => x.Value).Take(25).ToDictionary(x => x.Key, x => x.Value);
                            CacheRepository.Set<Dictionary<string, int>>("EmojisWithCount", emojisWithCount, 9999);
                            CacheRepository.Set<Dictionary<string, int>>("TopEmojis", topEmojis, 9999);
                        }
                    }
                );
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error at ProcessEmojis", null);
            }
        }

        public async Task ProcessHashTagsAsync(List<TweetModel> tweets)
        {
            List<string> listOfHashtags = new List<string>();
            try
            {
                await Task.Run(() =>
                {
                    if (tweets != null && tweets.Count > 0)
                    {
                        List<string> listOfTweetTexts = tweets.Select(x => x.FullText).ToList();
                        listOfHashtags.AddRange(this.HashtagProcessor.GetListOfHashtags(tweets));
                        Dictionary<string, int> hashTagsWithCount = CacheRepository.Get<Dictionary<string, int>>("HashtagsWithCount") != null ? CacheRepository.Get<Dictionary<string, int>>("HashtagsWithCount") : new Dictionary<string, int>() ;
                        int tweetsWithHashtags = CacheRepository.Get<int>("TweetsWithHashTags");
                        hashTagsWithCount = this.HashtagProcessor.GetHashtagsWithCount(hashTagsWithCount, listOfHashtags);
                        if (hashTagsWithCount == null) hashTagsWithCount = new Dictionary<string, int>();
                        Dictionary<string, int> topHashtags = hashTagsWithCount.OrderByDescending(x => x.Value).Take(25).ToDictionary(x => x.Key, x => x.Value);
                        tweetsWithHashtags += this.HashtagProcessor.GetNumberOfTweetsWithHashTags(tweets);
                        CacheRepository.Set<Dictionary<string, int>>("HashtagsWithCount", hashTagsWithCount, 9999);
                        CacheRepository.Set<Dictionary<string, int>>("TopHashtags", topHashtags, 9999);
                        CacheRepository.Set<Dictionary<string, int>>("TweetsWithHashTags", topHashtags, 9999);
                        CacheRepository.Set<int>("TweetsWithHashTags", tweetsWithHashtags, 9999);
                    }
                }
                );
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error at ProcessEmojis", null);
            }
        }

    }
}
