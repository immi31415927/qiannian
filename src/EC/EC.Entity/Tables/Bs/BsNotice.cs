using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Entity.Tables.Bs
{
    /// <summary>
    /// 公告(数据库表名 Agent_BsNotice)
    /// </summary>
    public class BsNotice
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }

        /// <summary>
        /// 自定义编号
        /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// 类型 系统公告（10）公司新闻（20）行业新闻（30）
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }


        /// <summary>
        /// 语言
        /// </summary>
        public int CountrySysNo { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        public string Synopsis { get; set; }

        /// <summary>
        /// 简介图地址
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 显示序号
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedDate { get; set; }
    }
}
