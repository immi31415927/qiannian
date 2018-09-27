using EC.Entity.Tables.Bs;

namespace EC.DataAccess.Bs
{
    /// <summary>
    /// 日志数据访问接口
    /// </summary>
    public interface IBsLog
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        BsLog Get(int sysNo);

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model">数据模型</param>
        int Insert(BsLog model);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">数据模型</param>
        int Update(BsLog model);
    }
}
