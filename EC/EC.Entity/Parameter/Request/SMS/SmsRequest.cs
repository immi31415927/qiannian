namespace EC.Entity.Parameter.Request.SMS
{
    /// <summary>
    /// 短信
    /// </summary>
    public class SmsRequest
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Key
        /// </summary>
        public string VerifyCodeCacheKey { get; set; }

        /// <summary>
        /// 随机码
        /// </summary>
        public string Rand { get; set; }

        /// <summary>
        /// 缓存时间
        /// </summary>
        public int catchTime { get; set; }
    }
}
