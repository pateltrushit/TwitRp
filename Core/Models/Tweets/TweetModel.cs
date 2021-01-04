using System;
using System.Collections.Generic;
using System.Text;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Models.Entities;

namespace TwitRp.Core.Models.Tweets
{
    public class TweetModel
    {
        public string InReplyToStatusIdStr { get; set; }
        public long? InReplyToUserId { get; set; }
        public string InReplyToUserIdStr { get; set; }
        public string InReplyToScreenName { get; set; }
        public int[] ContributorsIds { get; set; }
        public IEnumerable<long> Contributors { get; set; }
        public int RetweetCount { get; set; }
        public bool Retweeted { get; set; }
        public bool IsRetweet { get; set; }
        //public TweetModel RetweetedTweet { get; set; }
        public int? QuoteCount { get; set; }
        public long? QuotedStatusId { get; set; }
        public long? InReplyToStatusId { get; set; }
        public string QuotedStatusIdStr { get; set; }
        public bool PossiblySensitive { get; set; }
        public LanguageModel Language { get; set; }
        public IPlace Place { get; set;}
        public Dictionary<string, object> Scopes { get; set; }
        public string FilterLevel { get; }
        public bool WithheldCopyright { get; }
        public IEnumerable<string> WithheldInCountries { get; }
        public string WithheldScope { get; set; }
        public List<HashtagModel> Hashtags { get; set; }
        public List<UrlModel> Urls { get; set; }
        public List<MediaModel> Media { get; set; }
        public List<IUserMentionEntity> UserMentions { get; }
        public ITweet QuotedTweet { get; }
        public int? ReplyCount { get; set; }
        public bool Truncated { get; }
        public string Source { get; set; }
        public long Id { get; set; }
        public string IdStr { get; set; }
        public string Url { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }
        public string FullText { get; set; }
        public int[] DisplayTextRange { get; set; }
        public string Text { get; set; }
        public int[] SafeDisplayTextRange { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public ITweetIdentifier CurrentUserRetweetIdentifier { get; }
        public IUser CreatedBy { get; set; }
        public TweetMode TweetMode { get; set; }
        public ICoordinates Coordinates { get; set; }
        public int FavoriteCount { get; set; }
        public bool Favorited { get; set; }

        public bool IsProcessed { get; set; }

    }
}
