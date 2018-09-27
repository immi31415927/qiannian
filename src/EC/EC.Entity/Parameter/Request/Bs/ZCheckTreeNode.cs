﻿using System;
using System.Collections.Generic;

namespace EC.Entity.Parameter.Request.Bs
{
    /// <summary>
    /// Checkbox Ztree
    /// </summary>
    public class ZCheckTreeNode
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 父编号
        /// </summary>
        public string pId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool @checked { get; set; }
        /// <summary>
        /// 是否打开
        /// </summary>
        public bool open { get; set; }
        /// <summary>
        /// 0 菜单 1 权限
        /// </summary>
        public int nodetype { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string icon { get; set; }
    }
}
