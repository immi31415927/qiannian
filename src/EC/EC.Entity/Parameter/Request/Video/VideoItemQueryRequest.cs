using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Entity.Parameter.Request.Video
{
    /// <summary>
    /// 视频项查询
    /// </summary>
    public class VideoItemQueryRequest : PageCondition
    {
        /// <summary>
        /// 视频启用状态
        /// </summary>
        public int? VideoStatus { get; set; }

        /// <summary>
        /// 视频类型:免费（10）、VIP（20）
        /// </summary>
        public int? VideoType { get; set; }

        /// <summary>
        /// 视频项启用状态
        /// </summary>
        public int? VideoItemStatus { get; set; }

        /// <summary>
        /// 视频名称
        /// </summary>
        public string VideoName { get; set; }

        /// <summary>
        /// 视频编号
        /// </summary>
        public int? VideoSysNo { get; set; }
    }
}
