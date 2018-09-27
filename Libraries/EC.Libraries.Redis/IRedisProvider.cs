using System;
using System.Collections.Generic;

namespace EC.Libraries.Redis
{
    using EC.Libraries.Framework;

    /// <summary>
    /// 对外公开Redis查询接口
    /// </summary>
    public interface IRedisProvider : IProxyBaseObject<IRedisProvider>
    {
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Key对应的Value</returns>
        object Get(string key);

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T">值对应的泛型类型</typeparam>
        /// <param name="key">Key</param>
        /// <returns>Key对应的Value</returns>
        T Get<T>(string key) where T : class;

        /// <summary>
        /// 获取或设置缓存(默认60分)
        /// </summary>
        /// <typeparam name="T">值对应的泛型类型</typeparam>
        /// <param name="key">Key</param>
        /// <param name="acquire">缓存方法</param>
        /// <param name="cacheTime">缓存时长（单位：分钟）</param>
        /// <returns>T</returns>
        /// <remarks>2016-04-19 余勇 添加</remarks>
        T Get<T>(string key, Func<T> acquire, int cacheTime = 60) where T : class;

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T">需要保存值的泛型类型</typeparam>
        /// <param name="key">Key</param>
        /// <param name="data">缓存的值</param>
        /// <param name="cacheTime">缓存时长（单位：分钟）</param>
        void Set<T>(string key, T data, int cacheTime);

        /// <summary>
        /// 检测缓存是否有效
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>True=有效 False=无效</returns>
        bool IsSet(string key);

        /// <summary>
        /// 通过Key值移除缓存
        /// </summary>
        /// <param name="key">Key</param>
        void Remove(string key);

        /// <summary>
        /// 通过正则表达式移除缓存
        /// </summary>
        /// <param name="pattern">正则表达式</param>
        void RemoveByPattern(string pattern);

        /// <summary>
        /// 清除缓存
        /// </summary>
        void Clear();

        /// <summary>
        ///获取所有key
        /// </summary>
        IList<string> GetAllKey();

        /// <summary>
        /// 键值递增
        /// </summary>
        /// <param name="key">键码</param>
        /// <param name="amount">递增值</param>
        /// <returns>返回值</returns>
        long Increment(string key, uint amount);

        /// <summary>
        /// 键值递减
        /// </summary>
        /// <param name="key">键码</param>
        /// <param name="amount">递减值</param>
        /// <returns>返回值</returns>
        long Decrement(string key, uint amount);

        /// <summary>
        /// 获取递增、递减Key的当前值
        /// </summary>
        /// <param name="key">键码</param>
        /// <returns>当前值</returns>
        long GetCountVal(string key);
    }
}
