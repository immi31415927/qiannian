using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Entity.View.Fore
{
    /// <summary>
    /// 视频项
    /// </summary>
    public class VideoItemView
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }

        /// <summary>
        /// 视频编号
        /// </summary>
        public int VideoSysNo { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 排放
        /// </summary>
        public int DisplayOrder { get; set; }
    }
}
