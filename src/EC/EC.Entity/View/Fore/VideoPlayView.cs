using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Entity.View.Fore
{
    /// <summary>
    /// 视频播放视图实体
    /// </summary>
    public class VideoPlayView
    {
        /// <summary>
        /// 视频编号
        /// </summary>
        public int VideoSysNo { get; set; }

        /// <summary>
        /// 播放视频项编号
        /// </summary>
        public int PlayVideoItemSysNo { get; set; }

        /// <summary>
        /// 播放图片类型（0 -- 播放 1--不是VIP 2--视频不存在）
        /// </summary>
        public int PlayImageType { get; set; }

        /// <summary>
        /// 播放视频展示图片
        /// </summary>
        public string PlayImageUrl { get; set; }

        /// <summary>
        /// 播放地址
        /// </summary>
        public string PlayUrl { get; set; }

        /// <summary>
        /// 视频类别
        /// </summary>
        public int VideoCategorySysNo { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 浏览次数
        /// </summary>
        public int Views { get; set; }

        /// <summary>
        /// 点赞次数
        /// </summary>
        public int Hots { get; set; }

        /// <summary>
        /// 分享次数
        /// </summary>
        public int Shares { get; set; }

        /// <summary>
        /// 播放次数
        /// </summary>
        public int PlayNumber { get; set; }

        /// <summary>
        /// 视频列表
        /// </summary>
        public List<VideoItemView> VideoItemList { get; set; }
    }
}
