using System;  
using System.Collections.Generic;
using System.Text;
using Tweetinvi.Core.Models;
using Tweetinvi.Models.Entities;
using TwitRp.Core.Models.Tweets;

namespace TwitRp.Data.Mappers
{
    public class MediaMapper
    {
        public static List<MediaModel> MapListOfMedia(List<IMediaEntity> listOfMedia)
        {
            List<MediaModel> mediaModels = new List<MediaModel>();
            try
            {
                if (listOfMedia != null && listOfMedia.Count > 0)
                {
                    foreach (IMediaEntity media in listOfMedia)
                    {
                        mediaModels.Add(new MediaModel
                        {
                            Id = media.Id,
                            IdStr = media.IdStr,
                            DisplayURL = media.DisplayURL,
                            MediaURL = media.MediaURL,
                            ExpandedURL = media.ExpandedURL,
                            MediaType = media.MediaType,
                            URL = media.URL,
                            MediaURLHttps = media.MediaURLHttps,
                            Indices = (media.Indices != null && media.Indices.Length > 0) ? media.Indices : null
                        });
                    }
                }
            }
            catch
            {
            }
            return mediaModels;
        }

    }
}
