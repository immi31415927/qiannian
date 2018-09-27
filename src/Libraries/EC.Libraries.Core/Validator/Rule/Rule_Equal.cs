namespace EC.Libraries.Core.Validator.Rule
{
    /// <summary>
    /// 验证两次输入字符串是否一致
    /// </summary>
    /// <remarks>2013-12-30 苟治国 注释</remarks>
    public class Rule_Equal:IRule
    {
        private string _before;
        private string _back;

        /// <summary>
        /// 验证两次输入字符串是否一致
        /// </summary>
        /// <param name="before">字符串</param>
        /// <param name="back">字符串</param>
        /// <remarks>2013-12-30 苟治国 注释</remarks>
        public Rule_Equal(string before, string back, string message = "两次输入字符串不符合")
        {
            _before = before;
            _back = back;
            Message = message;
        }

        /// <summary>
        /// 验证两次输入字符串是否一致
        /// </summary>
        /// <returns>有效：true 无效：false</returns>
        /// <remarks>2013-12-30 苟治国 注释</remarks>
        public override bool Valid()
        {
            return _before.Equals(_back);
        }
    }
}
