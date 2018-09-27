using System;
using System.ComponentModel;

namespace EC.Entity.Parameter.Request.CRM
{
    /// <summary>
    /// 修改个人资料
    /// </summary>
    public class ProfileRequest
    {
        /// <summary>
        /// 真实名称
        /// </summary>
        [Description("真实名称")]
        public string RealName { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        [Description("头像")]
        public string HeadImgUrl { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        [Description("手机号")]
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 身份证
        /// </summary>
        [Description("身份证")]
        public string IDNumber { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        [Description("邮箱")]
        public string Email { get; set; }
        /// <summary>
        /// 银行
        /// </summary>
        [Description("银行")]
        public int Bank { get; set; }
        /// <summary>
        /// 银行卡号
        /// </summary>
        [Description("银行卡号")]
        public string BankNumber { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        [Description("验证码")]
        public string VerifyCode { get; set; }
    }
}
