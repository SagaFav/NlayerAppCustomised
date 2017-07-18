
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
namespace Infrastructure.Cache
{
    /// <summary>
    /// 一个简单的内存缓存的实现
    /// </summary>
    public class MCache : ICache
    {
        ObjectCache cache = MemoryCache.Default;

        public T Get<T>(string key) where T : class
        {
            return (T)cache.Get(key);
        }
        public List<T> GetByKeyFilter<T>(string filter) where T : class
        {
            return cache.Where(s => s.Key.Contains(filter)).Select(s => (T)s.Value).ToList();
        }
        public void Set<T>(string key, T value) where T : class
        { 
            CacheItemPolicy policy = new CacheItemPolicy(); 
            var item = cache.Get(key);
            if (item != null)
                cache.Set(key, value, policy);
            else
                cache.Add(key, value, policy);
        }

        public void Remove(string key)
        {
            cache.Remove(key);
        }
        public void Set<T>(string key, T value, TimeSpan absoluteExpiration,TimeSpan slidingExpiration) where T : class
        {
            CacheItemPolicy policy = new CacheItemPolicy();
            if (absoluteExpiration != TimeSpan.Zero)
            {
                policy.AbsoluteExpiration = DateTime.Now.Add(absoluteExpiration);
            }
            if (slidingExpiration != TimeSpan.Zero)
            {
                policy.SlidingExpiration = slidingExpiration;
            }
            var item = cache.Get(key);
            if (item != null)
                cache.Set(key, value, policy);
            else
                cache.Add(key, value, policy);
        }
    }
}
