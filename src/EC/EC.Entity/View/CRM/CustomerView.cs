using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Entity.View.CRM
{
    /// <summary>
    /// 会员视图实体
    /// </summary>
    public class CustomerView
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// 经理编号
        /// </summary>
        public int ReferrerSysNo { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 真实名称
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 呢称
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string HeadImgUrl { get; set; }

        /// <summary>
        /// 身份证
        /// </summary>
        public string IDNumber { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 团队人数
        /// </summary>
        public int TeamCount { get; set; }

        /// <summary>
        /// 会员等级（VIP会员(10)  普通代理 (20)  区域代理 (30)  全国代理 (40)  股东 (50)）
        /// </summary>
        public int Grade { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 过期日期
        /// </summary>
        public string ExpiresTime { get; set; }

        /// <summary>
        /// 关注时间
        /// </summary>
        public string FollowDate { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime RegisterDate { get; set; }

        /// <summary>
        /// 状态：启用（1）、禁用（0）
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedDate { get; set; }
    }
}
