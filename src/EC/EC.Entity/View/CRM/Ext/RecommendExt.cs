using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EC.Entity.Tables.CRM;

namespace EC.Entity.View.CRM.Ext
{
    /// <summary>
    /// 关注扩展表
    /// </summary>
    public class RecommendExt : CrRecommend
    {
        /// <summary>
        /// 电话号码
        /// </summary>
        public string TelNumber { get; set; }

        /// <summary>
        /// 会员等级（VIP会员(10)  普通代理 (20)  区域代理 (30)  全国代理 (40)  股东 (50)）
        /// </summary>
        public int Grade { get; set; }

        /// <summary>
        /// 是否注册（0 未注册）
        /// </summary>
        public int IsRegister { get; set; }
    }
}
