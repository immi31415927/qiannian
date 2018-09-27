namespace EC.Entity.Enum
{
    /// <summary>
    /// 会员枚举
    /// </summary>
    public class CustomerEnum
    {
        /// <summary>
        /// 邮箱状态
        /// </summary>
        public enum EmailStatus
        {
            已验证 = 1,
            未验证 = 0
        }

        /// <summary>
        /// 代理级别
        /// </summary>
        public enum Grade
        {
            普通会员 = 10,
            普通代理 = 20,
            区域代理 = 30,
            全国代理 = 40,
            股东 = 50
        }
        public enum NewGrade
        {
            普通会员 = 10,
            一星代理 = 20,
            二星代理 = 30,
            三星代理 = 40,
            四星代理 = 50,
            五星代理 = 60,
            六星代理 = 70,
            七星代理 = 80,
            八星代理 = 90
        }

        /// <summary>
        /// 银行
        /// </summary>
        public enum BankType
        {
            工商银行 = 1,
            建设银行 = 2,
            中国银行 = 3,
            农业银行 = 4,
            兴业银行 = 5,
            中信银行 = 6,
        }

        /// <summary>
        /// 是否过期
        /// </summary>
        public enum Expires
        {
            已过期 = 10,
            未过期 = 20
        }

        /// <summary>
        /// 注册支付类型
        /// </summary>
        public enum RegisterPayType
        {
            VIP会员 = 200,
            区域代理 = 1100,
            全国代理 = 3400,
            免费会员 = 0
        }
    }
}
