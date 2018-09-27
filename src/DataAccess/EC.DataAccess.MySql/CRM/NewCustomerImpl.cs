using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.DataAccess.MySql.CRM
{
    using EC.DataAccess.CRM;
    using EC.Entity.Parameter.NewCustomer;
    using EC.Entity.Tables.CRM;
    using EC.Libraries.Core.Data;

    /// <summary>
    /// 新会员数据接口现实
    /// </summary>
    public class NewCustomerImpl : Base, INewCustomer
    {
        /// <summary>
        /// 获取会员
        /// </summary>
        /// <param name="sysno">系统编号</param>
        /// <returns></returns>
        public CrCustomer Get(int sysno)
        {
            var sql = "select * from crcustomer where sysno=@sysno;";

            var result = DBContext.Sql(sql)
                                .Parameter("sysno", sysno)
                                .QuerySingle<CrCustomer>();
            return result;
        }

        /// <summary>
        /// 获取会员列表
        /// </summary>
        /// <returns>会员扩展信息列表</returns>
        public IList<CrCustomer> GetAll()
        {
            var sql = "select * from crcustomer;";

            return DBContext.Sql(sql).QueryMany<CrCustomer>();
        }
    }
}
