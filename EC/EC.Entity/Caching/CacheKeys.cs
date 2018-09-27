using System.ComponentModel;

namespace EC.Entity.Caching
{
    public class CacheKeys
    {
        /// <summary>
        /// 缓存关键字 带下划线_的是动态关键字
        /// </summary>
        public enum Items
        {

            [Description("手机号")]
            SMS_,

            [Description("发送数量")]
            SMSCount_,
            [Description("视频")]
            Video_,
            [Description("推荐")]
            Recommend,
            [Description("首页推荐")]
            HomeRecommend_,
        }
    }
}
