using System;
using System.ComponentModel;

namespace EC.Entity.Tables.Fore
{
    /// <summary>
	/// 视频
	/// </summary>
    /// <remarks>2017-11-21</remarks>
	[Serializable]
	public partial class FeVideo
	{
	  	/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 资源编号
		/// </summary>
		[Description("资源编号")]
        public int VideoCategorySysNo { get; set; }
 		/// <summary>
        /// 标题
		/// </summary>
		[Description("标题")]
		public string Title { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        [Description("图片地址")]
        public string ImageUrl { get; set; }
 		/// <summary>
		/// 作者
		/// </summary>
		[Description("作者")]
		public string Author { get; set; }
 		/// <summary>
		/// 内容
		/// </summary>
		[Description("内容")]
		public string Content { get; set; }
 		/// <summary>
		/// 类型：免费（10）、VIP（20）
		/// </summary>
		[Description("类型：免费（10）、VIP（20）")]
		public int Type { get; set; }
 		/// <summary>
		/// 播放地址：{Id:0,Name:'',Url:'',DisplayOrder:''}
		/// </summary>
		[Description("播放地址：{Id:0,Name:'',Url:'',DisplayOrder:0}")]
		public string PlayUrl { get; set; }
 		/// <summary>
        /// 浏览次数
		/// </summary>
		[Description("浏览次数")]
		public int Views { get; set; }
 		/// <summary>
		/// 点赞次数
		/// </summary>
		[Description("点赞次数")]
		public int Hots { get; set; }
 		/// <summary>
		/// 分享次数
		/// </summary>
		[Description("分享次数")]
		public int Shares { get; set; }
        /// <summary>
        /// 标志：最新（10）、热门（20）、推荐（30）
        /// </summary>
        [Description("标志：最新（10）、热门（20）、推荐（30）")]
        public int Sign { get; set; }
        /// <summary>
        /// 状态：启用（1）、禁用（0）
        /// </summary>
        [Description("状态：启用（1）、禁用（0）")]
        public int Status { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [Description("排序")]
        public int DisplayOrder { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        [Description("创建人")]
        public int CreatedBy { get; set; }
 		/// <summary>
		/// 创建时间
		/// </summary>
		[Description("创建时间")]
		public DateTime CreatedDate { get; set; }

 	}
}

