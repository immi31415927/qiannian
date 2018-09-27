﻿using System;
using System.ComponentModel;

namespace EC.Entity.Tables.Bs
{
    /// <summary>
    /// 用户角色
    /// </summary>
    /// <remarks>2017-08-28</remarks>
    [Serializable]
    public class BsRole
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Description("名称")]
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Description("描述")]
        public string Description { get; set; }

        /// <summary>
        /// 状态 启用（1）禁用（0）
        /// </summary>
        [Description("状态 启用（1）禁用（0）")]
        public int Status { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [Description("创建人")]
        public int CreatedBy { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Description("描述")]
        public DateTime CreatedDate { get; set; }
    }
}
