using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Entity.Tables.CRM
{
    /// <summary>
    /// 注册
    /// </summary>
    public partial class CrRecommend
    {
        /// <summary>
        ///系统编号 
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }

        /// <summary>
        ///头像 
        /// </summary>
        [Description("头像")]
        public string HeadImgUrl { get; set; }

        /// <summary>
        ///头像 
        /// </summary>
        [Description("头像")]
        public string Nickname { get; set; }

        /// <summary>
        ///呢称 
        /// </summary>
        [Description("呢称")]
        public string Openid { get; set; }

        /// <summary>
        ///推荐人 
        /// </summary>
        [Description("推荐人")]
        public int ReferrerSysNo { get; set; }

        /// <summary>
        ///创建时间 
        /// </summary>
        [Description("创建")]
        public DateTime CreatedDate { get; set; }
    }
}
