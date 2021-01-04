using System;
using System.Collections.Generic;
using System.Text;
using TwitRp.Core.Models.Tweets;

namespace TwitRp.Core.Interfaces
{
    public interface IHashtagProcessor
    {
        List<string> GetListOfHashtags(TweetModel inputString);
        List<string> GetListOfHashtags(List<TweetModel> listOfTweets);
        Dictionary<string, int> GetHashtagsWithCount(Dictionary<string, int> HashtagsWithCount, List<string> listOfHashtags);

        int GetNumberOfTweetsWithHashTags(List<TweetModel> listOfTweets);
    }
}
