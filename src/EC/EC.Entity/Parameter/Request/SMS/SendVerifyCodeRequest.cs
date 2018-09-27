namespace EC.Entity.Parameter.Request.SMS
{
    /// <summary>
    /// 用户注册验证码对象
    /// </summary>
    public class SendVerifyCodeRequest
    {
        /// <summary>
        /// Key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string PhoneNo { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string VerifyCode { get; set; }

        /// <summary>
        /// 短信验证码
        /// </summary>
        public string SmsVerifyCode { get; set; }
    }
}
