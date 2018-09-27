using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Libraries.WeiXin.Model
{
    public class JSApiTicketResponse : WXResult
    {
        /// <summary>
        /// 错误码
        /// </summary>
        public string errcode { get; set; }

        /// <summary>
        /// 	错误信息
        /// </summary>
        public string errmsg { get; set; }

        /// <summary>
        /// pi_ticket，卡券接口中签名所需凭证
        /// </summary>
        public string ticket { get; set; }

        /// <summary>
        /// 有效时间
        /// </summary>
        public string expires_in { get; set; }

        /// <summary>
        /// 公众号的唯一
        /// </summary>
        public string appId { get; set; }

        /// <summary>
        /// url
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 有效时间
        /// </summary>
        public string jsapi_ticket { get; set; }

        /// <summary>
        /// 生成签名的随机串
        /// </summary>
        public string noncestr { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public string timestamp { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string signature { get; set; }
    }
}
