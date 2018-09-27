using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EC.Entity.Tables.Finance;

namespace EC.Entity.View.Fn.Ext
{
    /// <summary>
    /// 奖金记录扩展
    /// </summary>
    public class BonusLogExt : FnBonusLog
    {
        /// <summary>
        /// 来源会员名称
        /// </summary>
        public string SourceName { get; set; }

        /// <summary>
        /// 来源会员层级
        /// </summary>
        public int SourceLevel { get; set; }

        /// <summary>
        /// 来源会员层级名称
        /// </summary>
        public string SourceLevelName { get; set; }
    }
}
