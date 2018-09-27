using System;
using System.ComponentModel;

namespace EC.Entity.Tables.Fore
{
    /// <summary>
	/// 视频记录信息
	/// </summary>
	[Serializable]
    public partial class FeVideoRecord
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
        /// 类型：视频收藏（10） 播放记录（20）
        /// </summary>
        [Description("类型：视频收藏（10） 播放记录（20）")]
        public int Type { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Description("名称")]
        public string Name { get; set; }

        /// <summary>
        /// 资源编号
        /// </summary>
        [Description("资源编号")]
        public int CustomerSysNo { get; set; }

 		/// <summary>
		/// 创建时间
		/// </summary>
		[Description("创建时间")]
		public DateTime CreatedDate { get; set; }
 	}
}

