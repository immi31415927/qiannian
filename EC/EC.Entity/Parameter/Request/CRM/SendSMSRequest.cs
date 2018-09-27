namespace EC.Entity.Parameter.Request.CRM
{
    public class SendSMSRequest
    {
        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Key
        /// </summary>
        public string SMSCacheKey { get; set; }

        /// <summary>
        /// Key
        /// </summary>
        public string SMSCountCacheKey { get; set; }

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
