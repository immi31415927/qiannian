using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Entity.Parameter.Request.Video
{
    /// <summary>
    /// 收藏查询
    /// </summary>
    public class VideoFavQueryRequest : PageCondition
    {
        /// <summary>
        /// 视频编号
        /// </summary>
        public int? VideoSysNo { get; set; }

        /// <summary>
        /// 类型：视频收藏（10） 播放记录（20）
        /// </summary>
        public int? Type { get; set; }

        /// <summary>
        /// 会员编号
        /// </summary>
        public int? CustomerSysNo { get; set; }
    }
}
