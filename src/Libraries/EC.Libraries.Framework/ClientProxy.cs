using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace EC.Libraries.Framework
{

    /// <summary>
    /// 用于实现对基类库代理调用的代理泛型类，该类更高效，推荐使用
    /// </summary>
    public class ClientProxy<T> where T : IProxyBaseObject<T>
    {
        private static readonly ConcurrentDictionary<string, IProxyBaseObject<T>> factories = new ConcurrentDictionary<string, IProxyBaseObject<T>>();
        private static readonly object sync = new object();

        public static T GetInstance(BaseConfig config = null)
        {
            var obj = (IProxyBaseObject<T>)null;
            var key = typeof(T).FullName + (config != null ? "." + config.Provider : "");
            lock (sync)
            {
                if (!factories.TryGetValue(key, out obj))
                {
                    var assemblyString = typeof(T).Namespace;
                    if (assemblyString != null)
                        foreach (Type t in Assembly.Load(assemblyString).GetTypes())
                        {
                            if (t.IsClass && !t.IsAbstract && t.GetInterface(typeof(T).Name) != (Type)null)
                            {
                                var proxyBaseObject = Activator.CreateInstance(t) as IProxyBaseObject<T>;
                                if (proxyBaseObject != null)
                                {
                                    proxyBaseObject.Initialize(config);
                                    obj = proxyBaseObject;
                                }
                                break;
                            }
                        }
                    if (obj == null)
                        throw new Exception("无法实例化该接口");
                    factories.TryAdd(key, obj);
                }
                else if (config != null)
                {
                    obj.Initialize(config);
                }
            }
            return obj.Instance;
        }
    }

    /// <summary>
    /// 用于实现对基类库代理调用的代理类，建议使用泛型类
    /// </summary>
    public class ClientProxy
    {
        private static readonly Dictionary<string, object> factories = new Dictionary<string, object>();
        private static readonly object sync = new object();


        public static T GetInstance<T>(BaseConfig config = null) where T : IProxyBaseObject<T>
        {
            lock (sync)
            {
                var obj = (object)null;
                var key = typeof(T).FullName + (config != null ? "." + config.Provider : "");
                if (config != null || !factories.TryGetValue(key, out obj))
                {
                    var assemblyString = typeof(T).Namespace;
                    if (assemblyString != null)
                        foreach (Type t in Assembly.Load(assemblyString).GetTypes())
                        {
                            if (t.IsClass && !t.IsAbstract && t.GetInterface(typeof(T).Name) != (Type)null)
                            {
                                var proxyBaseObject = Activator.CreateInstance(t) as IProxyBaseObject<T>;
                                if (proxyBaseObject != null)
                                {
                                    proxyBaseObject.Initialize(config);
                                    obj = proxyBaseObject.Instance;
                                }
                                break;
                            }
                        }
                    if (obj == null)
                        throw new Exception("无法实例化该接口");
                    factories.Remove(key);
                    factories.Add(key, obj);
                }
                return (T)obj;
            }
        }
    }

}
