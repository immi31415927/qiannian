using System;
using System.Collections.Generic;
using System.Linq;
using EmitMapper;
using EmitMapper.MappingConfiguration;

namespace EC.Libraries.Util.Extension
{
    public static class EntityExtension
    {
        /// <summary>
        /// 配置AutoMapper允许空转换
        /// </summary>
        public static void Config()
        {
            //允许空列表对象转换，转换结果也为空
           // Mapper.Configuration.AllowNullCollections = true;
        }

        /// <summary>
        /// 添加配置对象
        /// </summary>
        /// <typeparam name="TSource">源对象</typeparam>
        /// <typeparam name="tdestination">目标对象</typeparam>
        public static void CreateMap<TSource, tdestination>()
        {
            //Mapper.CreateMap<TSource, tdestination>();
        }

        /// <summary>
        /// 扩展方法，转换对象为指定类型
        /// </summary>
        /// <typeparam name="TResult">目标类型</typeparam>
        /// <param name="source">源对象</param>
        /// <returns>目标对象</returns>
        public static TResult MapTo<TResult>(this object source)
        {
            if (source != null)
            {
                return (TResult)ObjectMapperManager.DefaultInstance.GetMapperImpl(source.GetType(), typeof(TResult),(IMappingConfigurator)DefaultMapConfig.Instance).Map(source);
            }
            return default(TResult);
        }

        /// <summary>
        /// 扩展方法，转换对象为指定类型
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source">源对象</param>
        /// <param name="destination">目标</param>
        /// <returns>目标对象</returns>
        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<TSource, TDestination>().Map(source, destination);
          
        }

        /// <summary>
        /// 反射对象为键值对
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <returns>键值对列表</returns>
        public static IEnumerable<KeyValuePair<string, object>> ToKeyValueList<T>(this T entity) where T : class
        {
            var type = typeof(T);
            var props = type.GetProperties();
            return props.Select(p => new KeyValuePair<string, object>(p.Name, p.GetValue(entity, null)));
        }

        public static void Compute<TSource, TResult>(IEnumerable<TSource> list, TResult result, params KeyValuePair<string, string>[] computeNames)
        {
            if (computeNames == null || computeNames.Length == 0) return;

            var resultType = typeof(TResult);
            var sourceType = typeof(TSource);

            Func<string, object> compute = (propName) =>
            {
                var prop = sourceType.GetProperty(propName);
                if (prop == null) return null;

                switch (prop.PropertyType.Name)
                {
                    case "int": return list.Sum(p => Convert.ToInt32(prop.GetValue(p, null)));
                    case "decimal":
                    case "float":
                    case "double": return list.Sum(p => Convert.ToDouble(prop.GetValue(p, null)));
                    default: return null;
                }
            };

            foreach (var item in computeNames)
            {
                var prop = resultType.GetProperty(item.Key);
                if (prop == null) continue;
                prop.SetValue(resultType, compute(item.Value), null);
            }
        }
    }
}
