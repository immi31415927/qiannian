
using System.Text.RegularExpressions;

namespace EC.Libraries.Core.Validator.Rule
{
    /// <summary>
    /// 邮政编码
    /// </summary>
    /// <remarks>2013-12-30 苟治国 注释</remarks>
    public class Rule_Bank:IRule
    {
         private string m_post;

        /// <summary>
        /// 验证规则 邮编格式的字符串
        /// </summary>
        /// <param name="post">邮编字符串.</param>
        /// <param name="message">验证失败的提示消息.</param>
         /// <remarks>2013-12-30 苟治国 注释</remarks>
         public Rule_Bank(string post, string message = "银行卡号格式不正确")
        {
            m_post = post;
            Message = message;
        }

        /// <summary>
        /// 验证是否有效
        /// </summary>
        /// <returns>有效：true 无效：false</returns>
         /// <remarks>2013-12-30 苟治国 注释</remarks>
        public override bool Valid()
        {
            if (string.IsNullOrWhiteSpace(m_post)) return false;
            return Regex.IsMatch(m_post, "^[1-9][0-9]{5}$");            
        }
    }
}
