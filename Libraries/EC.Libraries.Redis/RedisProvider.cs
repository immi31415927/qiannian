using System;
using System.Collections.Generic;
using EC.Libraries.Util;
using EC.Libraries.Framework;
using StackExchange.Redis;

namespace EC.Libraries.Redis
{

    /// <summary>
    /// Redis查询提供类
    /// </summary>
    internal class RedisProvider : IRedisProvider
    {
        /// <summary>
        /// 锁对象
        /// </summary>
        private static object _lock = new object();

        /// <summary>
        /// Redis配置
        /// </summary>
        private static RedisConfig _redisConfig = null;

        /// <summary>
        /// IConnectionMultiplexer
        /// </summary>
        private static IConnectionMultiplexer connectionMultiplexer;

        /// <summary>
        /// IDatabase
        /// </summary>
        private static IDatabase db
        {
            get
            {
                return connectionMultiplexer.GetDatabase();
            }
        }

        /// <summary>
        /// 获取所需的基础调用实体
        /// </summary>
        public IRedisProvider Instance
        {
            get { return this; }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="config"></param>
        public void Initialize(BaseConfig config = null)
        {
            lock (_lock)
            {
                if (config != null) _redisConfig = config as RedisConfig;

                if (_redisConfig == null)
                {
                    _redisConfig = Config.GetConfig<RedisConfig>();

                    if (_redisConfig == null) throw new Exception("缺少RedisConfig配置");
                }

                if (connectionMultiplexer == null)
                {
                    lock (typeof(RedisProvider))
                    {
                        connectionMultiplexer = ConnectionMultiplexer.Connect(_redisConfig.Url);
                    }
                }
            }
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Key对应的Value</returns>
        public object Get(string key)
        {
            return db.StringGet(key);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T">值对应的泛型类型</typeparam>
        /// <param name="key">Key</param>
        /// <returns>Key对应的Value</returns>
        public T Get<T>(string key) where T : class
        {
            key = key.ToLower();

            var result = db.StringGet(key);

            if (result.IsNullOrEmpty) return default(T);
            if (typeof(T).Equals(typeof(String)))
            {
                return result.ToString() as T;
            }
            else
            {
                return JsonUtil.ToObject<T>(result.ToString());
            }
        }

        /// <summary>
        /// 获取或设置缓存(默认60分)
        /// </summary>
        /// <typeparam name="T">值对应的泛型类型</typeparam>
        /// <param name="key">Key</param>
        /// <param name="acquire">缓存方法</param>
        /// <param name="cacheTime">缓存时长（单位：分钟）</param>
        /// <returns>T</returns>
        public T Get<T>(string key, Func<T> acquire, int cacheTime = 60) where T : class
        {
            key = key.ToLower();

            if (db.KeyExists(key))
            {
                return this.Get<T>(key);
            }
            else
            {
                var result = acquire();
                this.Set(key, result, cacheTime);
                return result;
            }
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T">需要保存值的泛型类型</typeparam>
        /// <param name="key">Key</param>
        /// <param name="data">缓存的值</param>
        /// <param name="cacheTime">缓存时长（单位：分钟）</param>
        public void Set<T>(string key, T data, int cacheTime)
        {
            key = key.ToLower();

            if (data == null) return;

            if (typeof(T).Equals(typeof(String)))
                db.StringSet(key, data.ToString(), TimeSpan.FromMinutes(cacheTime));
            else
                db.StringSet(key, data.ToJson2(), TimeSpan.FromMinutes(cacheTime));
        }

        /// <summary>
        /// 检测缓存是否有效
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>True=有效 False=无效</returns>
        public bool IsSet(string key)
        {
            key = key.ToLower();
            return db.KeyExists(key);
        }

        /// <summary>
        /// 通过Key值移除缓存
        /// </summary>
        /// <param name="key">Key</param>
        public void Remove(string key)
        {
            key = key.ToLower();
            db.KeyDelete(key);
        }

        /// <summary>
        /// 通过正则表达式移除缓存
        /// </summary>
        /// <param name="pattern">正则表达式</param>
        public void RemoveByPattern(string pattern)
        {
            pattern = string.Format("*{0}*", pattern.ToLower());
            var endpoints = connectionMultiplexer.GetEndPoints(true);
            foreach (var endpoint in endpoints)
            {
                var server = connectionMultiplexer.GetServer(endpoint);
                var config = ConfigurationOptions.Parse(server.Multiplexer.Configuration);
                var keys = server.Keys(database: config.DefaultDatabase ?? 0, pattern: pattern, pageSize: int.MaxValue);

                if (keys != null)
                {
                    foreach (var redisKey in keys)
                    {
                        if (db.KeyExists(redisKey))
                        {
                            db.KeyDelete(redisKey);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        public void Clear()
        {
            var endpoint = db.IdentifyEndpoint();
            var server = connectionMultiplexer.GetServer(endpoint);
            server.FlushDatabase(db.Database);
        }

        /// <summary>
        ///获取所有key
        /// </summary>
        public IList<string> GetAllKey()
        {
            IList<string> result = new List<string>();
            IEnumerable<RedisKey> keys = null;
            var endpoints = connectionMultiplexer.GetEndPoints(true);
            foreach (var endpoint in endpoints)
            {
                var server = connectionMultiplexer.GetServer(endpoint);
                var config = ConfigurationOptions.Parse(server.Multiplexer.Configuration);
                keys = server.Keys(database: config.DefaultDatabase ?? 0, pattern: "*", pageSize: int.MaxValue);
            }

            if (keys != null)
            {
                foreach (var item in keys)
                {
                    result.Add(item.ToString());
                }
            }
            return result;
        }

        /// <summary>
        /// 键值递增
        /// </summary>
        /// <param name="key">键码</param>
        /// <param name="amount">递增值</param>
        /// <returns>返回值</returns>
        public long Increment(string key, uint amount)
        {
            return db.HashIncrement(key, "count", amount);
        }

        /// <summary>
        /// 键值递减
        /// </summary>
        /// <param name="key">键码</param>
        /// <param name="amount">递减值</param>
        /// <returns>返回值</returns>
        public long Decrement(string key, uint amount)
        {
            return db.HashDecrement(key, "count", amount);
        }

        /// <summary>
        /// 获取递增、递减Key的当前值
        /// </summary>
        /// <param name="key">键码</param>
        /// <returns>当前值</returns>
        public long GetCountVal(string key)
        {
            var result = db.HashGet(key, "count");

            if (result.IsNullOrEmpty) return 0;

            return long.Parse(result);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            connectionMultiplexer.Dispose();
        }
    }
}
