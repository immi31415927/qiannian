﻿using System.Text.RegularExpressions;

namespace EC.Libraries.Core.Validator.Rule
{
    /// <summary>
    /// 特殊符号
    /// </summary>
    /// <remarks>2013-12-30 苟治国 注释</remarks>
    public class Rule_Special:IRule
    {
        private string _special;

        /// <summary>
        /// 验证规则 邮编格式的字符串
        /// </summary>
        /// <param name="value">邮编字符串.</param>
        /// <param name="message">验证失败的提示消息.</param>
         /// <remarks>2013-12-30 苟治国 注释</remarks>
        public Rule_Special(string value, string message = "不能使用特殊符号")
        {
            _special = value;
            Message = message;
        }

        /// <summary>
        /// 验证是否有效
        /// </summary>
        /// <returns>有效：true 无效：false</returns>
        /// <remarks>2013-12-30 苟治国 注释</remarks>
        public override bool Valid()
        {
            Regex pattern = new Regex("[`~!@#$^&*()=|{}':;',\\[\\].<>/?~！@#￥……&*（）——|{}【】‘；：”“'。，、？]");
            return !pattern.IsMatch(_special);
        }
    }
}
