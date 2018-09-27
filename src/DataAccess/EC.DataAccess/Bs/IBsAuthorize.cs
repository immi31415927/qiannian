using System.Collections.Generic;
using EC.Entity.Parameter.Request.Bs;
using EC.Entity.Tables.Bs;

namespace EC.DataAccess.Bs
{
    /// <summary>
    /// 授权数据访问接口
    /// </summary>
    public interface IBsAuthorize
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        BsAuthorize Get(int sysNo);

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="model">实体</param>
        int Insert(BsAuthorize model);

        /// <summary>
        /// 获取功能权限列表
        /// </summary>
        /// <param name="roleSysNo">角色编号</param>
        /// <returns>功能权限列表</returns>
        IList<BsAuthorize> GetPermissionListRoleSysNo(int roleSysNo);

        /// <summary>
        /// 删除角色所有数据
        /// </summary>
        /// <param name="roleSysNo">角色编号</param>
        /// <returns></returns>
        int DeleteByRoleSysNo(int roleSysNo);

        /// <summary>
        /// 获取授权列表
        /// </summary>
        /// <param name="request">输入参数</param>
        /// <returns>授权列表</returns>
        IList<BsAuthorize> GetList(AuthorizeRequest request);
    }
}
