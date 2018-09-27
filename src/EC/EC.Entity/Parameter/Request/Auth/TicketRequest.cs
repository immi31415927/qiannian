using System;
using System.ComponentModel;

namespace EC.Entity.Parameter.Request.Auth
{
    /// <summary>
    /// 用户对象
    /// </summary>
    public class TicketRequest
    {
        /// <summary>
        /// 编号
        /// </summary>
        [Description("编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        [Description("电话")]
        public string Account { get; set; }
        /// <summary>
        /// 经理编号
        /// </summary>
        [Description("经理编号")]
        public int ReferrerSysNo { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        [Description("编码")]
        public string SerialNumber { get; set; }
        /// <summary>
        /// OpenId
        /// </summary>
        [Description("OpenId")]
        public string OpenId { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        [Description("手机")]
        public string PhoneNumber { get; set; }
        /// <summary>
        /// RealName
        /// </summary>
        [Description("RealName")]
        public string RealName { get; set; }
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
        /// 会员等级
        /// </summary>
        [Description("会员等级")]
        public int Grade { get; set; }
        /// <summary>
        /// 关注时间
        /// </summary>
        [Description("关注时间")]
        public string FollowDate { get; set; }
        /// <summary>
        /// 过期日期
        /// </summary>
        public string ExpiresTime { get; set; }
    }
}
