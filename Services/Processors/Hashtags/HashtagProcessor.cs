using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitRp.Core.Interfaces;
using TwitRp.Core.Models.Hashtags;
using TwitRp.Core.Models.Tweets;

namespace TwitRp.Services.Processors.Hashtags
{
    public class HashtagProcessor : IHashtagProcessor, IProcessor
    {
        public ILogger Logger { get; set; }
        public HashtagProcessor(IConfiguration configuration, ILogger<HashtagProcessor> logger)
        {
            Logger = logger;
        }
        public Dictionary<string, int> GetHashtagsWithCount(Dictionary<string, int> HashtagsWithCount, List<string> listOfHashTags)
        {
            List<string> distinctEmojis = listOfHashTags.Distinct().ToList();
            if (distinctEmojis != null && distinctEmojis.Count > 0)
            {
                foreach (string emoji in listOfHashTags)
                {
                    List<string> listPerEmoji = listOfHashTags.Where(x => x == emoji).ToList();
                    int count = (listPerEmoji != null) ? listPerEmoji.Count : 0;
                    if (HashtagsWithCount.ContainsKey(emoji))
                    {
                        HashtagsWithCount[emoji] += count;
                    }
                    else
                    {
                        HashtagsWithCount.Add(emoji, count);
                    }
                    HashtagsWithCount = HashtagsWithCount.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                }
            }
            return HashtagsWithCount;
        }

        public List<string> GetListOfHashtags(TweetModel tweetModel)
        {
            List<string> listOfHashtags = new List<string>();
            try
            {
                if (tweetModel != null && tweetModel.Hashtags != null && tweetModel.Hashtags.Count > 0)
                {
                    listOfHashtags.AddRange(tweetModel.Hashtags.Select(x => x.Text));
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Error at GetListOfHashtags", ex);
            }
            return listOfHashtags;
        }

        public List<string> GetListOfHashtags(List<TweetModel> listOfTweets)
        {
            List<string> listOfHashtags = new List<string>();
            try
            {
                if (listOfTweets != null && listOfTweets.Count > 0)
                {
                    foreach (TweetModel tweetModel in listOfTweets)
                    {
                        if (tweetModel != null && tweetModel.Hashtags != null && tweetModel.Hashtags.Count > 0)
                        {
                            listOfHashtags.AddRange(tweetModel.Hashtags.Select(x => x.Text));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Error at GetListOfHashtags", ex);
            }
            return listOfHashtags;
        }

        public int GetNumberOfTweetsWithHashTags(List<TweetModel> listOfTweets)
        {
            int numberOfTweetsWithHashtags = 0;
            try
            {
                if (listOfTweets != null && listOfTweets.Count > 0)
                {
                    numberOfTweetsWithHashtags = listOfTweets.Where(x => x.Hashtags != null && x.Hashtags.Count > 0).ToList().Count;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Error at GetListOfHashtags", ex);
            }
            return numberOfTweetsWithHashtags;
        }
    }
}