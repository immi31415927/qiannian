namespace EC.Libraries.Redis
{
    using EC.Libraries.Framework;

    /// <summary>
    /// 缓存配置实体
    /// </summary>
    public class RedisConfig : BaseConfig
    {
        /// <summary>
        /// 连接缓存服务器的Url，若使用Local，则此值无效
        /// </summary>
        public string Url { set; get; }
    }
}
