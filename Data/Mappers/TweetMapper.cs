using System;   
using System.Collections.Generic;
using System.Text;
using Tweetinvi.Core.Models;
using Tweetinvi.Models.Entities;
using TwitRp.Core.Models.Tweets;

namespace TwitRp.Data.Mappers
{
    public class TweetMapper
    {
        public static TweetModel MapTweet(Tweet tweet)
        {
            TweetModel tweetModel = new TweetModel();
            try
            {
                if (tweet != null)
                {
                    tweetModel = new TweetModel()
                    {
                         Id = tweet.Id,
                         Text = tweet.Text,
                         FullText = tweet.FullText,
                         Url = !string.IsNullOrEmpty(tweet.Url) ? tweet.Url : string.Empty,
                         Urls  = UrlMapper.MapUrls(tweet.Urls),
                         TweetMode = tweet.TweetMode,
                         Contributors = tweet.Contributors != null ? tweet.Contributors : null,
                         Retweeted = tweet.Retweeted,
                         RetweetCount = tweet.RetweetCount,
                         Hashtags = HashtagMapper.MapHashTags(tweet.Hashtags),
                         Media = MediaMapper.MapListOfMedia(tweet.Media)
                    };
                }
            }
            catch
            { 
            }
            return tweetModel;
        }

        public static List<TweetModel> MapTweets(List<Tweet> tweets)
        {
            List<TweetModel> tweetModels = new List<TweetModel>();
            try
            {
                if (tweets != null && tweets.Count > 0)
                {
                    foreach(Tweet tweet in tweets)
                        tweetModels.Add(MapTweet(tweet));
                }
            }
            catch
            { 
            }
            return tweetModels;
        }
    }
}
