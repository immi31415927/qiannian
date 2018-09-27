namespace EC.Entity.View.CRM
{
    /// <summary>
    /// 会员层级
    /// </summary>
    public class CustomerLevelView
    {
        /// <summary>
        /// 会员编号
        /// </summary>
        public int SysNo { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 层级字母
        /// </summary>
        public string LevelLetter { get; set; }
        
        /// <summary>
        /// 层级团队数
        /// </summary>
        public int LevelTeamCount { get; set; }
    }
}
