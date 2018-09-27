using System.Configuration;

namespace EC.Libraries.Framework
{
    /// <summary>
    /// 配置工具类
    /// </summary>
    public class Config
    {
        /// <summary>
        /// 获取配置实体
        /// </summary>
        /// <typeparam name="T">配置实体</typeparam>
        /// <param name="configPath">配置节点</param>
        /// <returns>配置实体</returns>
        public static T GetConfig<T>(string configPath = "") where T : new()
        {
            var _configPath = !string.IsNullOrEmpty(configPath) ? configPath : typeof(T).Name;
            T _config = default(T);
            var config = ConfigurationManager.GetSection("LibrariesConfig") as LibrariesConfig;
            if (config != null)
            {
                _config = config.GetObjByXml<T>(_configPath);
            }
            return _config;
        }
    }
}
