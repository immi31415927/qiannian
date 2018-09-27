namespace EC.Entity
{
    using System.ComponentModel;

    /// <summary>
    /// 分页条件
    /// </summary>
    public class PageCondition
    {
        protected PageCondition()
        {
            CurrentPageIndex = CurrentPageIndex ?? 1;
            PageSize = 10;
        }

        /// <summary>
        /// 表或视图
        /// </summary>
        [Description("表或视图")]
        public string Tables { get; set; }

        /// <summary>
        /// 字段
        /// </summary>
        [Description("字段")]
        public string Tablefields { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public string OrderBy { get; set; }

        /// <summary>
        /// 开始索引页
        /// </summary>
        [Description("当前页索引")]
        public int? CurrentPageIndex { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        [Description("页码")]
        public int? PageSize { get; set; }
    }
}
