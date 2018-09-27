using System.Collections.Generic;
using System.Linq;
using System.Text;
using EC.DataAccess.Bs;
using EC.Entity.Tables.Bs;

namespace EC.DataAccess.MySql.Bs
{
    using EC.Libraries.Core.Data;

    /// <summary>
    /// 码表 数据访问接口实现
    /// </summary>
    public class BsCodeImpl : Base,IBsCode
    {
        /// <summary>
        /// 批量更新子码表值
        /// </summary>
        /// <param name="codeList">子码表列表</param>
        /// <returns>影响行数</returns>
        public int BatchUpdate(List<BsCode> codeList)
        {
            var sqlStr = new StringBuilder();
            sqlStr.Append("UPDATE agent_bscode SET Value = CASE Type");

            foreach (var item in codeList)
            {
                sqlStr.Append(string.Format(" WHEN {0} THEN {1} ", item.Type, item.Value));
            }

            sqlStr.Append("END");

            return DBContext.Sql(string.Format("{0} WHERE Type IN({1})", sqlStr.ToString(), string.Join(",", codeList.Select(p => p.Type))))
                            .Execute();
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        public BsCode Get(int sysNo)
        {
            var strSql = "select * from agent_bscode where sysNo=@sysNo;";

            var result = DBContext.Sql(strSql)
                                .Parameter("sysNo", sysNo)
                                .QuerySingle<BsCode>();
            return result;
        }

        /// <summary>
        /// 查询码表通过类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>码表列表</returns>
        public BsCode GetByType(int type)
        {
            var strSql = "select * from agent_bscode where type=@type;";

            var result = DBContext.Sql(strSql)
                                .Parameter("type", type)
                                .QuerySingle<BsCode>();
            return result;
        } 

        /// <summary>
        /// 查询码表列表通过类型列表
        /// </summary>
        /// <param name="list">类型列表</param>
        /// <returns>码表列表</returns>
        public IList<BsCode> GetListByTypeList(List<int> list)
        {
            return DBContext.Sql(string.Format("select * from agent_bscode where type in({0})", string.Join(",", list)))
                           .QueryMany<BsCode>();
        } 
    }
}
