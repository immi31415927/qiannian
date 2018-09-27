using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Entity.Parameter.Request.CRM
{
    /// <summary>
    /// 注册同步参数
    /// </summary>
    public class RegisteredSyncRequest
    {
        /// <summary>
        /// 真实名称
        /// </summary>
        [Description("真实名称")]
        public int SysNo { get; set; }
    }
}
