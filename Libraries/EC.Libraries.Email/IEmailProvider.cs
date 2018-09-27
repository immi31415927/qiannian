using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EC.Libraries.Framework;

namespace EC.Libraries.Email
{
    /// <summary>
    /// 对外公开Email查询接口
    /// </summary>
    public interface IEmailProvider : IProxyBaseObject<IEmailProvider>
    {
        /// <summary>
        /// 发送邮箱
        /// </summary>
        /// <param name="toList">收件人</param>
        /// <param name="ccList">抄送人</param>
        /// <param name="bccList">密送人</param>
        /// <param name="subject"></param>
        /// <param name="isBodyHtml"></param>
        /// <param name="body"></param>
        void Send(string toList, string subject, bool isBodyHtml, string body, string ccList = null, string bccList = null);
    }
}
