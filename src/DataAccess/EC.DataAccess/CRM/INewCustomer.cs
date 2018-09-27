using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.DataAccess.CRM
{
    using EC.Entity.Parameter.NewCustomer;
    using EC.Entity.Tables.CRM;

    /// <summary>
    /// 新会员数据接口
    /// </summary>
    public interface INewCustomer
    {
        /// <summary>
        /// 获取会员
        /// </summary>
        /// <param name="sysno">系统编号</param>
        /// <returns></returns>
        CrCustomer Get(int sysno);

        /// <summary>
        /// 获取会员列表
        /// </summary>
        /// <returns>会员扩展信息列表</returns>
        IList<CrCustomer> GetAll();
    }
}
