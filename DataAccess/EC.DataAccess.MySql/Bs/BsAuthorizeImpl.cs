using System;
using System.Collections.Generic;
using EC.DataAccess.Bs;
using EC.Entity.Parameter.Request.Bs;
using EC.Entity.Tables.Bs;

namespace EC.DataAccess.MySql.Bs
{
    using EC.Libraries.Core.Data;

    /// <summary>
    /// 授权数据访问接口实现
    /// </summary>
    public class BsAuthorizeImpl : Base, IBsAuthorize
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        public BsAuthorize Get(int sysNo)
        {
            return DBContext.Sql("select * from agent_bsauthorize where sysNo = @sysNo")
                            .Parameter("sysNo", sysNo)
                            .QuerySingle<BsAuthorize>();
        }

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>系统编号</returns>
        public int Insert(BsAuthorize model)
        {
            return DBContext.Insert("agent_bsauthorize", model)
                            .AutoMap(x => x.SysNo)
                            .ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// 获取功能权限列表
        /// </summary>
        /// <param name="roleSysNo">角色编号</param>
        /// <returns>功能权限列表</returns>
        public IList<BsAuthorize> GetPermissionListRoleSysNo(int roleSysNo)
        {
            return DBContext.Sql("select * from agent_bsauthorize where roleSysNo=@roleSysNo;")
                           .Parameter("roleSysNo", roleSysNo)
                           .QueryMany<BsAuthorize>();
        }

        /// <summary>
        /// 删除角色所有数据
        /// </summary>
        /// <param name="roleSysNo">角色编号</param>
        /// <returns></returns>
        public int DeleteByRoleSysNo(int roleSysNo)
        {
            var sql = "delete from agent_bsauthorize where roleSysNo=@roleSysNo;";

            var result = DBContext.Sql(sql)
                                .Parameter("roleSysNo", roleSysNo)
                                .Execute();
            return result;
        }

        /// <summary>
        /// 获取授权列表
        /// </summary>
        /// <param name="request">输入参数</param>
        /// <returns>授权列表</returns>
        public IList<BsAuthorize> GetList(AuthorizeRequest request)
        {
            var data = DBContext.Select<BsAuthorize>("a.*").From("Agent_BsRole r INNER JOIN Agent_BsUserRole u ON r.SysNo = u.RoleSysNo INNER JOIN Agent_BsAuthorize a ON u.RoleSysNo = a.RoleSysNo");

            //条件查询委托
            Action<string, string, object> setWhere = (@where, name, value) => data.AndWhere(@where).Parameter(name, value);

            if (request.RoleStatus.HasValue)
            {
                setWhere("r.Status = @RoleStatus", "RoleStatus", request.RoleStatus.Value);
            }
            if (request.UserSysNo.HasValue)
            {
                setWhere("u.UserSysNo = @UserSysNo", "UserSysNo", request.UserSysNo.Value);
            }
            if (request.AuthorizeType.HasValue)
            {
                setWhere("a.AuthorizeType = @AuthorizeType", "AuthorizeType", request.AuthorizeType.Value);
            }

            return data.QueryMany();
        }
    }
}
