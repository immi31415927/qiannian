using System.ComponentModel;

namespace EC.Entity.Parameter.Response.CRM
{
    public class LoginResponse
    {
        /// <summary>
        /// 编号
        /// </summary>
        [Description("编号")]
        public int Id { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        [Description("电话")]
        public string Tel { get; set; }
        /// <summary>
        /// OpenId
        /// </summary>
        [Description("OpenId")]
        public string OpenId { get; set; }
        /// <summary>
        /// Nickname
        /// </summary>
        [Description("Nickname")]
        public string Nickname { get; set; }
        /// <summary>
        /// HeadImgUrl
        /// </summary>
        [Description("HeadImgUrl")]
        public string HeadImgUrl { get; set; }
        /// <summary>
        /// 是否锁定
        /// </summary>
        [Description("是否锁定")]
        public int sid { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Description("密码")]
        public string pass { get; set; }
        /// <summary>
        /// 密钥
        /// </summary>
        [Description("密钥")]
        public string code { get; set; }

    }
}
