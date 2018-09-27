using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EC.Entity.Tables.Fore;

namespace EC.Entity.View.Fore.Ext
{
    /// <summary>
    /// 视频扩展
    /// </summary>
    public class VideoExt : FeVideo
    {
        /// <summary>
        /// 播放次数
        /// </summary>
        public int PlayNumber { get; set; }
    }
}
