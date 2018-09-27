using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Entity.Parameter.Request.WeiXin
{
    /// <summary>
    /// 发送信息请求对象
    /// </summary>
    public class SendMessageRequest
    {
        /// <summary>
        /// 微信AppId
        /// </summary>
        public string WeiXinAppId { get; set; }

        /// <summary>
        /// 微信AppSecret
        /// </summary>
        public string WeiXinAppSecret { get; set; }

        /// <summary>
        /// 接收信息者的微信OpenId
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 模板Id
        /// </summary>
        public string TemplateId { get; set; }

        /// <summary>
        /// 数据参数
        /// new
        /// {
        ///    first = new { color = "#f0f0f1", value = "first" },
        ///    reason = new { color = "#0fe123", value = "reason" },
        ///    refund = new { color = "#c321f0", value = 0.101 },
        ///    Remark = new { color = "#3f333", value = "备注" }
        /// }
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 链接地址
        /// </summary>
        public string Url { get; set; }
    }
}
