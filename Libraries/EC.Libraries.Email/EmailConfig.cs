using System;

namespace EC.Libraries.Email
{
    using EC.Libraries.Framework;

    /// <summary>
    /// Email配置文件
    /// </summary>
    public class EmailConfig : BaseConfig
    {
        /// <summary>
        /// Smtp 服务器
        /// </summary>
        public string SmtpHost { get; set; }

        /// <summary>
        /// Smtp 服务器端口
        /// </summary>
        public int SmtpPort { get; set; }

        /// <summary>
        /// 发送者 Email 地址
        /// </summary>
        public string FromEmailAddress { get; set; }

        /// <summary>
        /// 发送者 Email 密码
        /// </summary>
        public string FormEmailPassword { get; set; }
    }
}
