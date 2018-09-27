using System;
using System.ComponentModel;

namespace EC.Entity.Parameter.Request.CRM
{
    /// <summary>
    /// 
    /// </summary>
    public class UserInfoRequest
    {
        /// <summary>
        /// 推荐人编号
        /// </summary>
        [Description("推荐人编号")]
        public int? ReferrerSysNo { get; set; }

        /// <summary>
        /// OpenId
        /// </summary>
        [Description("OpenId")]
        public string OpenId { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [Description("手机号")]
        public string Tel { get; set; }
    }
}
