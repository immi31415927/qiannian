using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Entity.Parameter.Request.Video
{
    /// <summary>
    /// 视频集
    /// </summary>
    /// <remarks>2017-11-21</remarks>
    [Serializable]
    public partial class VideoItemRequest
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 视频编号
        /// </summary>
        [Description("视频编号")]
        public int VideoSysNo { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        [Description("标题")]
        public string Name { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        [Description("图片")]
        public string ImageUrl { get; set; }
        /// <summary>
        /// 播放地址
        /// </summary>
        [Description("播放地址")]
        public string PlayUrl { get; set; }
        /// <summary>
        /// 播放次数
        /// </summary>
        [Description("播放次数")]
        public int PlayNumber { get; set; }
        /// <summary>
        /// 排放
        /// </summary>
        [Description("排放")]
        public int DisplayOrder { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [Description("状态")]
        public int Status { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        public DateTime CreatedDate { get; set; }
    }
}
