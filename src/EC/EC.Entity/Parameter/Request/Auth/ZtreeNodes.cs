using System;
using System.ComponentModel;

namespace EC.Entity.Parameter.Request.Auth
{
    /// <summary>
    /// ZTREE节点
    /// </summary>
    public class ZtreeNodes
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Description("名称")]
        public string name { get; set; }

        /// <summary>
        /// 父节点
        /// </summary>
        [Description("父节点")]
        public int pId { get; set; }
    }
}
