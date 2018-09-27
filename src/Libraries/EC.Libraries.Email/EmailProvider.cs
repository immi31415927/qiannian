using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace EC.Libraries.Email
{
    using EC.Libraries.Framework;

    /// <summary>
    /// Email查询提供类
    /// </summary>
    internal class EmailProvider : IEmailProvider
    {
        /// <summary>
        /// 锁对象
        /// </summary>
        private static object _lock = new object();

        /// <summary>
        /// 配置
        /// </summary>
        private static EmailConfig _emailConfig = null;

        /// <summary>
        /// 获取所需的基础调用实体
        /// </summary>
        public IEmailProvider Instance
        {
            get { return this; }
        }

        public void Initialize(BaseConfig config = null)
        {
            lock (_lock)
            {
                if (config != null) _emailConfig = config as EmailConfig;

                if (_emailConfig == null)
                {
                    _emailConfig = Config.GetConfig<EmailConfig>();

                    if (_emailConfig == null) throw new Exception("缺少EmailConfig配置");
                }
            }
        }


        /// <summary>
        /// 发送邮箱
        /// </summary>
        /// <param name="toList">收件人</param>
        /// <param name="ccList">抄送人</param>
        /// <param name="bccList">密送人</param>
        /// <param name="subject"></param>
        /// <param name="isBodyHtml"></param>
        /// <param name="body"></param>
        public void Send(string toList,string subject, bool isBodyHtml, string body,string ccList=null,string bccList=null)
        {
            try
            {
                SmtpClient smtp = new SmtpClient();                 //实例化一个SmtpClient  
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;   //将smtp的出站方式设为 Network  
                smtp.EnableSsl = true;                              //smtp服务器是否启用SSL加密  
                smtp.Host = _emailConfig.SmtpHost;                  //指定 smtp 服务器地址  
                smtp.Port = _emailConfig.SmtpPort;                  //指定 smtp 服务器的端口，默认是25，如果采用默认端口，可省去  
                smtp.UseDefaultCredentials = true;                  //如果你的SMTP服务器不需要身份认证，则使用下面的方式，不过，目前基本没有不需要认证的了  
                smtp.Credentials = new NetworkCredential(_emailConfig.FromEmailAddress, _emailConfig.FormEmailPassword);    //如果需要认证，则用下面的方式  

                MailMessage mm = new MailMessage(); //实例化一个邮件类  
                mm.Priority = MailPriority.Normal; //邮件的优先级，分为 Low, Normal, High，通常用 Normal即可  
                mm.From = new MailAddress(_emailConfig.FromEmailAddress, "管理员", Encoding.GetEncoding(936));


                //收件人  
                if (!string.IsNullOrEmpty(toList))
                    mm.To.Add(toList);
                //抄送人  
                if (!string.IsNullOrEmpty(ccList))
                    mm.CC.Add(ccList);
                //密送人  
                if (!string.IsNullOrEmpty(bccList))
                    mm.Bcc.Add(bccList);

                mm.Subject = subject;                           //邮件标题  
                mm.SubjectEncoding = Encoding.GetEncoding(936); //这里非常重要，如果你的邮件标题包含中文，这里一定要指定，否则对方收到的极有可能是乱码。  
                mm.IsBodyHtml = isBodyHtml;                     //邮件正文是否是HTML格式  
                mm.BodyEncoding = Encoding.GetEncoding(936);    //邮件正文的编码， 设置不正确， 接收者会收到乱码  
                mm.Body = body;                                 //邮件正文  
                //邮件附件  
                //if (this.AttachmentList != null && this.AttachmentList.Count > 0)
                //{
                //    foreach (Attachment attachment in this.AttachmentList)
                //    {
                //        mm.Attachments.Add(attachment);
                //    }
                //}
                //发送邮件，如果不返回异常， 则大功告成了。  
                smtp.Send(mm);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public void Dispose()
        {
        }
    }
}
