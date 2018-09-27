using System.ComponentModel;

namespace EC.Entity.Parameter.Response.CRM
{
    /// <summary>
    /// 注册推荐验证
    /// </summary>
    public class RegistrationRecommendResponse
    {
        /// <summary>
        /// 用户账号
        /// </summary>
        [Description("用户账号")]
        public int CustomerSysNo { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [Description("手机号")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 真实名称
        /// </summary>
        [Description("真实名称")]
        public string RealName { get; set; }

        /// <summary>
        /// OpenId
        /// </summary>
        [Description("OpenId")]
        public string OpenId { get; set; }
    }
}
