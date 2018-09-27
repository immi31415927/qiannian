using System.ComponentModel;

namespace EC.Entity.Parameter.Request.CRM
{
    /// <summary>
    /// 登录参数
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// 用户账号
        /// </summary>
        [Description("用户账号")]
        public string Account { get; set; }
        /// <summary>
        /// OpenId
        /// </summary>
        [Description("OpenId")]
        public string OpenId { get; set; }
        /// <summary>
        /// Nickname
        /// </summary>
        [Description("Nickname")]
        public string Nickname { get; set; }
        /// <summary>
        /// HeadImgUrl
        /// </summary>
        [Description("HeadImgUrl")]
        public string HeadImgUrl { get; set; }
        /// <summary>
        /// 用户密码
        /// </summary>
        [Description("用户密码")]
        public string Password { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        [Description("验证码")]
        public string VerifyCode { get; set; }
    }
}
