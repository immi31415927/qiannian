using System;
using System.Collections.Generic;
using System.Text;
using EC.DataAccess.Bs;
using EC.Entity.Tables.Bs;
using EC.Libraries.Core.Data;

namespace EC.DataAccess.MySql.Bs
{
    /// <summary>
    /// 用户角色关联接口实现
    /// </summary>
    public class BsUserRoleImpl : Base, IBsUserRole
    {
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="list">列表</param>
        /// <returns>影响行数</returns>
        public int BatchInsert(List<BsUserRole> list)
        {
            var strSql = new StringBuilder();
            strSql.Append("INSERT INTO Agent_BsUserRole(UserSysNo,RoleSysNo,CreatedBy,CreatedDate) VALUES");

            for (var i = 0; i < list.Count; i++)
            {
                var item = list[i];
                strSql.Append(string.Format("({0},{1},{2},DATE_FORMAT('{3}', '%Y-%m-%d %h:%i:%s'))",
                         item.UserSysNo, item.RoleSysNo, item.CreatedBy, item.CreatedDate));
                if (i < list.Count - 1)
                {
                    strSql.Append(",");
                }
            }
            strSql.Append(";");

            return DBContext.Sql(strSql.ToString())
                             .Execute();
        }

        /// <summary>
        /// 删除通过用户编号
        /// </summary>
        /// <param name="userSysNo">用户编号</param>
        /// <returns>影响行数</returns>
        public int DeleteByUserSysNo(int userSysNo)
        {
            return DBContext.Delete("Agent_BsUserRole")
                            .Where("UserSysNo", userSysNo)
                            .Execute();
        }

        /// <summary>
        /// 获取角色关联列表通过用户编号
        /// </summary>
        /// <param name="userSysNo">用户编号</param>
        /// <returns>角色关联列表</returns>
        public IList<BsUserRole> GetListByUserSysNo(int userSysNo)
        {
            return DBContext.Select<BsUserRole>("*")
                            .From("Agent_BsUserRole")
                            .Where("UserSysNo = @UserSysNo")
                            .Parameter("UserSysNo", userSysNo)
                            .QueryMany();
        }
    }
}
