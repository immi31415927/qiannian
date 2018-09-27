using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Entity
{
    /// <summary>
    /// App页面参数实体
    /// </summary>
    public class AppPage
    {
        /// <summary>
        /// App标题
        /// </summary>
        public string AppTitle { get; set; }

        /// <summary>
        /// 头部左返回链接
        /// </summary>
        public string TopLeftUrl { get; set; }

        /// <summary>
        /// 头部右返回链接
        /// </summary>
        public string TopRightUrl { get; set; }

        /// <summary>
        /// 选中页面 （）
        /// </summary>
        public string SeletedPage { get; set; }
    }
}
