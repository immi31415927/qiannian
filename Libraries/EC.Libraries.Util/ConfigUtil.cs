using System.IO;
using System.Web;

namespace EC.Libraries.Util
{
    /// <summary>
    /// 配置类
    /// </summary>
    public class ConfigUtil
    {

        /// <summary>
        /// 获取配置文件信息
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="configFileName">配置文件名称</param>
        /// <returns>配置文件</returns>
        public static T GetConfig<T>(string configFileName)
        {
            return SerializationUtil.XmlDeserialize<T>(File.ReadAllText(HttpContext.Current.Server.MapPath("~/config/") + configFileName));
        }

        /// <summary>
        /// 获取配置文件信息
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="filePath">配置文件名称</param>
        /// <returns>配置文件</returns>
        /// <remarks>2016-3-2 苟治国 创建</remarks>
        public static T GetAreaConfig<T>(string  filePath)
        {
            return SerializationUtil.XmlDeserialize<T>(File.ReadAllText(HttpContext.Current.Server.MapPath(filePath)));
        }
    }
}
