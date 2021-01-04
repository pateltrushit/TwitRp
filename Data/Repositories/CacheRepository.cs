using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using TwitRp.Core.Interfaces.Repository;

namespace TwitRp.Data.Repositories
{
    public class CacheRepository: ICacheRepository
    {
        Dictionary<string, string> CacheItems = new Dictionary<string, string>();
        public void Clear()
        {
            throw new NotImplementedException();
        }

        public T Get<T>(string key)
        {
            T cacheValue = default(T);
            try
            {
                if (CacheItems != null && CacheItems.Count > 0 && CacheItems.ContainsKey(key))
                {
                    cacheValue = JsonConvert.DeserializeObject<T>(CacheItems[key]);
                }
            }
            catch
            {
            }
            return cacheValue;
        }

        public void Remove(string key)
        {
            try
            {
                if (!string.IsNullOrEmpty(key)&& CacheItems.ContainsKey(key))
                {
                    CacheItems.Remove("someKeyName");
                }
            }
            catch
            {
            }
        }

        public void Set<T>(string key, T data, int cacheTime)
        {
            try
            {
                if (data != null && !string.IsNullOrEmpty(key))
                {
                    if (CacheItems.ContainsKey(key))
                    {
                        CacheItems[key] = JsonConvert.SerializeObject(data);
                    }
                    else
                    {
                        CacheItems.Add(key, JsonConvert.SerializeObject(data));
                    }
                }
            }
            catch
            {
            }
        }
    }
}
