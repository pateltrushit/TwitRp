using System;   
using System.Collections.Generic;
using System.Text;
using Tweetinvi.Core.Models;
using Tweetinvi.Models.Entities;
using TwitRp.Core.Models.Tweets;

namespace TwitRp.Data.Mappers
{
    public class HashtagMapper
    {
        public static List<HashtagModel> MapHashTags(List<IHashtagEntity> hashtags)
        {
            List<HashtagModel> hashTagModels = new List<HashtagModel>();
            try
            {
                if (hashtags != null && hashtags.Count > 0)
                {
                    foreach (IHashtagEntity hashtag in hashtags)
                    {
                        hashTagModels.Add(new HashtagModel
                        {
                            Text = hashtag.Text,
                            Indices = (hashtag.Indices != null && hashtag.Indices.Length > 0) ? hashtag.Indices : null
                        });
                    }
                }
            }
            catch
            {
            }
            return hashTagModels;
        }

    }
}
