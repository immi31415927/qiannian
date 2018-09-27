using System;
using System.Text.RegularExpressions;

namespace EC.Libraries.Core.Validator.Rule
{

    /// <summary>
    /// 验证字母数字规则
    /// </summary>
    /// <remarks>2013-12-30 苟治国 注释</remarks>
    public class Rule_LetterAndNumber : IRule
    {
        /// <summary>
        /// 
        /// </summary>
        private string text;

        /// <summary>
        /// 验证规则 qq格式的字符串
        /// </summary>
        /// <param name="text">字符串.</param>
        /// <param name="message">验证失败的提示消息.</param>
        public Rule_LetterAndNumber(string text, string message = "由字母和数字组成")
        {
            this.text = text;
            Message = message;
        }

        /// <summary>
        /// 验证是否有效
        /// </summary>
        /// <param></param>
        /// <returns>有效：true 无效：false</returns>
        public override bool Valid()
        {
            if (string.IsNullOrWhiteSpace(text))
                return false;

            return Regex.Matches(text, "[a-zA-Z]").Count > 0 && Regex.Matches(text, "[0-9]").Count > 0;
        }
    }
}
