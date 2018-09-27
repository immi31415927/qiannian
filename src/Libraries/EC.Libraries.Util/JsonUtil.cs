using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Script.Serialization;

namespace EC.Libraries.Util
{
    /// <summary>
    /// Json序列化工具类    
    /// </summary>
    public static class JsonUtil
    {
        /// <summary>
        /// 对象序列化为Json字符串
        /// 优先尝试使用效率高的DataContractJsonSerializer类转换,
        /// 失败后使用JavaScriptSerializerToJson类转换
        /// </summary>
        /// <param name="obj">要序列化的对象</param>
        /// <param name="recursionDepth">序列化深度，可不传值</param>
        /// <returns>Json字符串</returns>
        public static string ToJson(this object obj, int recursionDepth = 100)
        {
            try
            {
                var serializer = new DataContractJsonSerializer(obj.GetType());
                using (var stream = new MemoryStream())
                {
                    serializer.WriteObject(stream, obj);
                    return Encoding.UTF8.GetString(stream.ToArray());
                }
            }
            catch
            {
                try
                {
                    var serializer = new JavaScriptSerializer { RecursionLimit = recursionDepth };
                    return serializer.Serialize(obj);
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// 任意类型转换为Json(上面的方法被修改后影响到我编辑器上传文件了，特意再写个方法)
        /// </summary>
        /// <param name="obj">任意数据类型</param>
        /// <param name="recursionDepth">序列化深度</param>
        /// <returns>json字符串</returns>
        public static string ToJson2(this object obj, int recursionDepth = 100)
        {
            var serializer = new JavaScriptSerializer { RecursionLimit = recursionDepth };
            return serializer.Serialize(obj);
        }

        /// <summary>
        /// 把Json字符串反序列化为对象
        /// 优先尝试使用效率高的DataContractJsonSerializer类转换,
        /// 失败后使用JavaScriptSerializerToJson类转换
        /// </summary>        
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">Json字符串</param>
        /// <param name="recursionDepth">反序列化深度，可不传值</param>
        /// <returns>反序列化得到的对象</returns>
        public static T ToObject<T>(this string obj, int recursionDepth = 100) where T : class
        {
            if (string.IsNullOrWhiteSpace(obj)) return null;
            try
            {
                var serializer = new DataContractJsonSerializer(typeof(T));

                using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(obj)))
                {
                    return (T)serializer.ReadObject(ms);
                }
            }
            catch
            {
                try
                {
                    var serializer = new JavaScriptSerializer();
                    return serializer.Deserialize<T>(obj);
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}
