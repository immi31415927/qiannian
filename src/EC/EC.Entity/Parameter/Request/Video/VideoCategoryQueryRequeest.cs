
namespace EC.Entity.Parameter.Request.Video
{
    /// <summary>
    /// 查询
    /// </summary>
    public class VideoCategoryQueryRequeest : PageCondition
    {
        /// <summary>
        /// 父级编号
        /// </summary>
        public int? ParentSysNo { get; set; }

        /// <summary>
        /// 启用状态
        /// </summary>
        public int? Status { get; set; }
    }
}
