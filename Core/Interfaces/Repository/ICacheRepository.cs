using System;
using System.Collections.Generic;
using System.Text;

namespace TwitRp.Core.Interfaces.Repository
{
    public interface ICacheRepository
    {
        T Get<T>(string key);
        void Set<T>(string key, T data, int cacheTime);

        void Remove(string key);

        void Clear();
    }
}
