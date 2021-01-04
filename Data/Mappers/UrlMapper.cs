using System;   
using System.Collections.Generic;
using System.Text;
using Tweetinvi.Core.Models;
using Tweetinvi.Models.Entities;
using TwitRp.Core.Models.Tweets;

namespace TwitRp.Data.Mappers
{
    public class UrlMapper
    {
        public static List<UrlModel> MapUrls(List<IUrlEntity> urls)
        {
            List<UrlModel> urlModels = new List<UrlModel>();
            try
            {
                if (urls != null && urls.Count > 0)
                {
                    foreach (IUrlEntity url in urls)
                    {
                        urlModels.Add(new UrlModel
                        {
                            DisplayedURL = url.DisplayedURL,
                            ExpandedURL = url.ExpandedURL,
                            URL = url.URL,
                            Indices = (url.Indices != null && url.Indices.Length > 0) ? url.Indices : null
                        });
                    }
                }
            }
            catch
            {
            }
            return urlModels;
        }

    }
}
