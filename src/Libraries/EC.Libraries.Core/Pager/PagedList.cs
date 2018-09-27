
using System;
using System.Collections.Generic;

namespace EC.Libraries.Core.Pager
{
    public class PagedList<T> : PagedList
    {
        public IList<T> TData { get; set; }
    }

    /// <summary>
    /// 分页模型
    /// </summary>>
    public class PagedList : IPagedList
    {
        private int pageSize = 10;
        private bool isLoading = true;
        private string _onBegin = "AjaxStart";
        private string _onComplete = "AjaxStop";
        private StyleEnum _style = StyleEnum.Default;

        /// <summary>
        /// 当前页数据
        /// </summary>
        [Obsolete("应该使用泛型分页实体PagedList<T> 中的TData ")]
        public virtual object Data { get; set; }
        /// <summary>
        /// 当前索引
        /// </summary>
        public int CurrentPageIndex { get; set; }

        /// <summary>
        /// 每页显示数
        /// </summary>
        public int PageSize 
        {
            get { return pageSize; }
            set { pageSize = value; }
        }

        /// <summary>
        /// 数据总数
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 分页数
        /// </summary>
        public int TotalPageCount { get { return (int)Math.Ceiling(TotalCount / (double)PageSize); } }

        /// <summary>
        /// 是否提示正在加载
        /// </summary>
        public bool IsLoading
        {
            get { return isLoading; }
            set { isLoading = value; }
        }

        /// <summary>
        /// 获取或设置在实例化响应数据之后但在更新页面之前，要调用的 JavaScript 函数
        /// </summary>
        public string OnComplete {
            get { return _onComplete; }
            set { _onComplete = value; }
        }

        /// <summary>
        /// 获取或设置要在更新页面之前立即调用的 JavaScript 函数的名称
        /// </summary>
        public string OnBegin
        {
            get { return _onBegin; }
            set { _onBegin = value; }
        }
        /// <summary>
        /// 风格样式
        /// </summary>
        public StyleEnum Style
        {
            get { return _style; }
            set { _style = value; }
        }

        public enum StyleEnum
        {
            /// <summary>
            /// 迷你分页样式(用于弹出框)
            /// </summary>
            Mini = 0,

            /// <summary>
            /// 默认分页样式
            /// </summary>
            Default,

            /// <summary>
            /// 前台小分页样式
            /// </summary>
            WebSmall,

            /// <summary>
            /// 前台分页默认样式
            /// </summary>
            WebDefault,
        }
    }
}
