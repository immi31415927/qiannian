//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using EC.Libraries.Util;
//using StackExchange.Redis;
//using EC.Libraries.Framework;

//namespace EC.Libraries.Cache
//{
//    /// <summary>
//    /// Redis现实
//    /// </summary>
//    internal class RedisManager : ICache
//    {
//        /// <summary>
//        /// 缓存配置
//        /// </summary>
//        CacheConfig cacheConfig = null;

//        /// <summary>
//        /// IConnectionMultiplexer
//        /// </summary>
//        private static IConnectionMultiplexer connectionMultiplexer;

//        /// <summary>
//        /// IDatabase
//        /// </summary>
//        private static IDatabase db
//        {
//            get
//            {
//                return connectionMultiplexer.GetDatabase(); 
//            }
//        }

//        /// <summary>
//        /// 缓存类型
//        /// </summary>
//        public CacheType Type
//        {
//            get
//            {
//                return CacheType.Redis; 
//            } 
//        }

//        /// <summary>
//        /// 设置缓存配置实体
//        /// </summary>
//        /// <param name="config">缓存配置实体</param>
//        public void SetConfig(CacheConfig config)
//        {
//            cacheConfig = config;
//        }

//        /// <summary>
//        /// 初始化操作
//        /// </summary>
//        public void Initialize()
//        {
//            if (cacheConfig == null)
//            {
//                try
//                {
//                    var config = ConfigurationManager.GetSection("LibrariesConfig") as LibrariesConfig;
//                    if (config != null)
//                    {
//                        cacheConfig = config.GetObjByXml<CacheConfig>("CacheConfig");
//                        if (cacheConfig == null)
//                            throw new Exception("缺少本地缓存配置节点");
//                    }

//                }
//                catch (Exception ex)
//                {
//                    throw new Exception("缺少Redis配置节点 "+ ex.Message);
//                }
//            }
//            if (connectionMultiplexer == null)
//            {
//                lock (typeof(RedisManager))
//                {
//                    connectionMultiplexer = ConnectionMultiplexer.Connect(cacheConfig.Url);
//                }
//            }
//        }

//        /// <summary>
//        /// 获取缓存
//        /// </summary>
//        /// <param name="key">Key</param>
//        /// <returns>Key对应的Value</returns>
//        public object Get(string key)
//        {
//            return db.StringGet(key);
//        }

//        /// <summary>
//        /// 获取缓存
//        /// </summary>
//        /// <typeparam name="T">值对应的泛型类型</typeparam>
//        /// <param name="key">Key</param>
//        /// <returns>Key对应的Value</returns>
//        public T Get<T>(string key) where T : class
//        {
//            key = key.ToLower();

//            var result = db.StringGet(key);

//            if (result.IsNullOrEmpty) return default(T);
//            if (typeof(T).Equals(typeof(String))){
//                return result.ToString() as T;
//            }else{
//                return JsonUtil.ToObject<T>(result.ToString());
//            }
//        }

//        /// <summary>
//        /// 设置缓存
//        /// </summary>
//        /// <typeparam name="T">需要保存值的泛型类型</typeparam>
//        /// <param name="key">Key</param>
//        /// <param name="data">缓存的值</param>
//        /// <param name="cacheTime">缓存时长（单位：分钟）</param>
//        public void Set<T>(string key, T data, int cacheTime)
//        {
//            key = key.ToLower();

//            if (data == null) return;

//            if (typeof(T).Equals(typeof(String)))
//                db.StringSet(key, data.ToString(), TimeSpan.FromMinutes(cacheTime));
//            else
//                db.StringSet(key, data.ToJson2(), TimeSpan.FromMinutes(cacheTime));
//        }

//        /// <summary>
//        /// 检测缓存是否有效
//        /// </summary>
//        /// <param name="key">Key</param>
//        /// <returns>True=有效 False=无效</returns>
//        public bool IsSet(string key)
//        {
//            key = key.ToLower();
//            return db.KeyExists(key);
//        }

//        /// <summary>
//        /// 通过Key值移除缓存
//        /// </summary>
//        /// <param name="key">Key</param>
//        public void Remove(string key)
//        {
//            key = key.ToLower();
//            db.KeyDelete(key);
//        }

//        /// <summary>
//        /// 通过正则表达式移除缓存
//        /// </summary>
//        /// <param name="pattern">正则表达式</param>
//        public void RemoveByPattern(string pattern)
//        {
//            pattern = string.Format("*{0}*", pattern.ToLower());
//            var endpoints = connectionMultiplexer.GetEndPoints(true);
//            foreach (var endpoint in endpoints)
//            {
//                var server = connectionMultiplexer.GetServer(endpoint);
//                var config = ConfigurationOptions.Parse(server.Multiplexer.Configuration);
//                var keys = server.Keys(database: config.DefaultDatabase ?? 0, pattern: pattern, pageSize: int.MaxValue);

//                if (keys != null)
//                {
//                    foreach (var redisKey in keys)
//                    {
//                        if (db.KeyExists(redisKey))
//                        {
//                            db.KeyDelete(redisKey);
//                        }
//                    }
//                }
//            }
//        }

//        /// <summary>
//        /// 清除缓存
//        /// </summary>
//        public void Clear()
//        {
//            var endpoint = db.IdentifyEndpoint();
//            var server = connectionMultiplexer.GetServer(endpoint);
//            server.FlushDatabase(db.Database);
//        }

//        /// <summary>
//        ///获取所有key
//        /// </summary>
//        public IList<string> GetAllKey()
//        {
//            IList<string> result = new List<string>();
//            IEnumerable<RedisKey> keys = null;
//            var endpoints = connectionMultiplexer.GetEndPoints(true);
//            foreach (var endpoint in endpoints)
//            {
//                var server = connectionMultiplexer.GetServer(endpoint);
//                var config = ConfigurationOptions.Parse(server.Multiplexer.Configuration);
//                keys = server.Keys(database: config.DefaultDatabase ?? 0, pattern: "*", pageSize: int.MaxValue);
//            }

//            if (keys != null)
//            {
//                foreach (var item in keys)
//                {
//                    result.Add(item.ToString());
//                }
//            }
//            return result;
//        }

//        /// <summary>
//        /// 释放资源
//        /// </summary>
//        public void Dispose()
//        {
//            connectionMultiplexer.Dispose();
//        }
//    }
//}
