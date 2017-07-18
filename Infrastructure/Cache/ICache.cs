using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Cache
{
    public interface ICache
    {
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T">缓存对象类型</typeparam>
        /// <param name="key">键</param>
        T Get<T>(string key) where T : class;
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T">缓存对象类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="timeSpan">缓存时间</param>
        void Set<T>(string key, T value, TimeSpan absoluteExpiration, TimeSpan slidingExpiration) where T : class;
        void Remove(string key);
        void Set<T>(string key, T value) where T : class;
        List<T> GetByKeyFilter<T>(string filter) where T : class;
    }
}
