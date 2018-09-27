using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EC.Entity.Tables.Fore;

namespace EC.Entity.View.Fore.Ext
{
    /// <summary>
    /// 视频项
    /// </summary>
    public class VideoItemExt : FeVideoItem
    {
        /// <summary>
        /// 视频项扩展名称
        /// </summary>
        public string NameExt { get; set; }
    }
}
