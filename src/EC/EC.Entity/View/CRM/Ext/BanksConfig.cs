using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace EC.Entity.View.CRM.Ext
{
    /// <summary>
    /// 银行
    /// </summary>
    [Serializable()]
    public class BanksConfig
    {
        /// <summary>
        /// 标识配置
        /// </summary>
        public List<BanksItem> BanksItem { get; set; }
    }

    /// <summary>
    ///  配置选项
    /// </summary>
    [Serializable()]
    public class BanksItem
    {
        /// <summary>
        /// 自定义编号
        /// </summary>
        [Description("自定义编号")]
        public int Id { get; set; }

        /// <summary>
        /// 银行名称
        /// </summary>
        [Description("银行名称")]
        public string Name { get; set; }
    }
}
