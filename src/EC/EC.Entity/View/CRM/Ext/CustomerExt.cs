using EC.Entity.Tables.CRM;

namespace EC.Entity.View.CRM.Ext
{
    /// <summary>
    /// 会员扩展信息
    /// </summary>
    public class CustomerExt : CrCustomer
    {
        /// <summary>
        /// 推荐人名称
        /// </summary>
        public string ReferrerName { get; set; }

        /// <summary>
        /// 直推人数
        /// </summary>
        public int ReferrerCount { get; set; }
    }
}
