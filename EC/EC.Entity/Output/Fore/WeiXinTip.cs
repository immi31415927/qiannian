using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Entity.Output.Fore
{
    public class WeiXinTip
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public String Nickname { get; set; }

        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public String OpenId { get; set; }

        /// <summary>
        /// Keyword1
        /// </summary>
        [Description("Keyword1")]
        public String Keyword1 { get; set; }


        /// <summary>
        /// Keyword2
        /// </summary>
        [Description("Keyword2")]
        public String Keyword2 { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        public String Remark { get; set; }
    }
}
