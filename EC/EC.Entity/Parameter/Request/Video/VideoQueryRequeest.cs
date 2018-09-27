namespace EC.Entity.Parameter.Request.Video
{
    /// <summary>
    /// 专题查询
    /// </summary>
    public class VideoQueryRequeest : PageCondition
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 启用状态
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 类型:免费（10）、VIP（20）
        /// </summary>
        public int? Type { get; set; }

        /// <summary>
        /// 标志：最新（10）、热门（20）、推荐（30）
        /// </summary>
        public int? Sign { get; set; }

        /// <summary>
        /// 视频分类编号
        /// </summary>
        public int? VideoCategorySysNo { get; set; }

        /// <summary>
        /// 视频名称
        /// </summary>
        public string VideoName { get; set; }
    }
}
