using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Entity.Enum
{
    public class ForeEnum
    {
        /// <summary>
        /// 视频状态
        /// </summary>
        public enum VideoStatus
        {
            禁用 = 0,
            启用 = 1,
        }

        /// <summary>
        /// 视频类型
        /// </summary>
        public enum VideoType
        {
            免费 = 10,
            Vip = 20,
        }
        /// <summary>
        /// 标志：最新（10）、热门（20）、推荐（30）
        /// </summary>
        public enum VideoSign
        {
            最新 = 10,
            热门 = 20,
            推荐 = 30,
        }

        /// <summary>
        /// 版本状态枚举
        /// </summary>
        public enum VideoCategoryStatus
        {
            禁用 = 0,
            启用 = 1,
        }

        /// <summary>
        /// 播放图片类型
        /// </summary>
        public enum PlayImageType
        {
            播放 = 0,
            不为vip或过期 = 1,
            视频不存在 = 2,
        }

        /// <summary>
        /// 视频记录类型
        /// </summary>
        public enum VideoRecordType
        {
            视频收藏 = 10,
            播放记录 = 20,
        }
    }
}
