using EC.DataAccess.Bs;
using EC.Entity.Tables.Bs;

namespace EC.DataAccess.MySql.Bs
{
    using EC.Libraries.Core.Data;

    /// <summary>
    /// 日志 数据访问接口实现
    /// </summary>
    public class BsLogImpl : Base, IBsLog
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        public BsLog Get(int sysNo)
        {
            var sql = "select * from agent_bslog where sysNo=@sysNo;";

            var result = DBContext.Sql(sql)
                                .Parameter("sysNo", sysNo)
                                .QuerySingle<BsLog>();
            return result;
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model">数据模型</param>
        public int Insert(BsLog model)
        {
            var result = DBContext.Insert<BsLog>("agent_bslog", model)
                                .AutoMap(x => x.SysNo)
                                .ExecuteReturnLastId<int>("sysNo");
            return result;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">数据模型</param>
        public int Update(BsLog model)
        {
            int rowsAffected = DBContext.Update<BsLog>("agent_bslog", model)
                                      .AutoMap(x => x.SysNo)
                                      .Where(x => x.SysNo)
                                      .Execute();
            return rowsAffected;
        }
    }
}
