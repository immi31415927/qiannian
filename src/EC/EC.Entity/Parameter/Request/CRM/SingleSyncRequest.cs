using System.ComponentModel;

namespace EC.Entity.Parameter.Request.CRM
{
    /// <summary>
    /// 单条同步
    /// </summary>
    public class SingleSyncRequest
    {
        /// <summary>
        /// 会员编号
        /// </summary>
        [Description("真实名称")]
        public int CustomerSysNo { get; set; }

        /// <summary>
        /// 推荐人
        /// </summary>
        [Description("推荐人")]
        public int ReferrerSysNo { get; set; }
        
    }
}
