using System;
using System.ComponentModel;

namespace EC.Entity.Parameter.Request.CRM
{
    /// <summary>
    /// 更新头像
    /// </summary>
    public class UpdateHeadRequest
    {
        /// <summary>
        /// 头像
        /// </summary>
        [Description("头像")]
        public string HeadImgUrl { get; set; }
    }
}
