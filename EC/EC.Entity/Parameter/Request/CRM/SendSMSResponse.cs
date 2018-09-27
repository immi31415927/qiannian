using System;

namespace EC.Entity.Parameter.Request.CRM
{
    public class SendSMSResponse
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string PhoneNumber { get; set; }
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
