using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitRp.Core.Interfaces.Repository;
using TwitRp.Core.Models.Tweets;

namespace TwitRp.Data.Repositories
{
    public class TweetRepository : ITweetRepository
    {
        public List<TweetModel> AllTweets { get; set; }

        public int GetTweetsCount()
        {
            int tweetsCount = 0;
            try
            {
                if (AllTweets != null && AllTweets.Count > 0)
                    tweetsCount = AllTweets.Count;
            }
            catch
            { 
            }
            return tweetsCount;
        }
        public List<TweetModel> GetAllTweets()
        {
            List<TweetModel> tweets = new List<TweetModel>();
            try
            {
                tweets = AllTweets;
            }
            catch
            {
            }
            return tweets;
        }
        public List<TweetModel> GetTweetsToProcess(int pageSize)
        {
            List<TweetModel> tweets = new List<TweetModel>();
            try
            {
                if (AllTweets != null && AllTweets.Count > 0 && pageSize > 0)
                {
                    if(pageSize > AllTweets.Count)
                        tweets = AllTweets.Where(x => !x.IsProcessed).ToList();
                    else
                        tweets = AllTweets.Where(x => !x.IsProcessed).ToList().Take(pageSize).ToList();
                }
            }
            catch
            {
            }
            return tweets;
        }

        public bool SaveListOfTweets(List<TweetModel> tweets)
        {
            bool result = false;
            try
            {
                if (AllTweets == null) AllTweets = new List<TweetModel>();
                if (tweets != null && tweets.Count > 0)
                {
                    AllTweets.AddRange(tweets);
                }
            }
            catch
            {
            }
            return result;
        }

    }
}
