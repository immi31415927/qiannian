using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EC.Entity.Tables.Bs;

namespace EC.DataAccess.Bs
{
    /// <summary>
    /// 用户角色关联接口
    /// </summary>
    public interface IBsUserRole
    {
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="list">列表</param>
        /// <returns>影响行数</returns>
        int BatchInsert(List<BsUserRole> list);

        /// <summary>
        /// 删除通过用户编号
        /// </summary>
        /// <param name="userSysNo">用户编号</param>
        /// <returns>影响行数</returns>
        int DeleteByUserSysNo(int userSysNo);

        /// <summary>
        /// 获取角色关联列表通过用户编号
        /// </summary>
        /// <param name="userSysNo">用户编号</param>
        /// <returns>角色关联列表</returns>
        IList<BsUserRole> GetListByUserSysNo(int userSysNo);
    }
}
