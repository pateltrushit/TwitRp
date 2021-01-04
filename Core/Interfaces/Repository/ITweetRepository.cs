using System;
using System.Collections.Generic;
using System.Text;
using TwitRp.Core.Models.Tweets;

namespace TwitRp.Core.Interfaces.Repository
{
    public interface ITweetRepository
    {
        int GetTweetsCount();
        List<TweetModel> GetAllTweets();

        List<TweetModel> GetTweetsToProcess(int count);

        bool SaveListOfTweets(List<TweetModel> Tweets);

    }
}
