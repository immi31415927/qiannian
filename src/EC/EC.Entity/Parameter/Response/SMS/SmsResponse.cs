using System;

namespace EC.Entity.Parameter.Response.SMS
{
    /// <summary>
    /// 短信
    /// </summary>
    public class SmsResponse
    {

        /// <summary>
        /// 用户名
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 随机码
        /// </summary>
        public string Rand { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime SendTime { get; set; }
    }
}
