using System.Collections.Generic;
using EC.Entity.Parameter.Request.Bs;
using EC.Entity.Tables.Bs;
using EC.Libraries.Core.Pager;

namespace EC.DataAccess.Bs
{
    /// <summary>
    /// 功能权限数据访问接口
    /// </summary>
    public interface IBsPermission
    {
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>系统编号</returns>
        int Insert(BsPermission model);

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>影响行数</returns>
        int Update(BsPermission model);

        /// <summary>
        /// 修改部分数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>影响行数</returns>
        int UpdateSection(BsPermission model);

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="sysNo">编码</param>
        /// <param name="status">状态</param>
        /// <returns>影响行数</returns>
        int UpdateStatus(int sysNo, int status);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>影响行数</returns>
        int Delete(int sysNo);

        /// <summary>
        /// 获取功能权限信息
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>功能权限信息</returns>
        BsPermission Get(int sysNo);

        /// <summary>
        /// 权限菜单编号获取菜单权限列表
        /// </summary>
        /// <param name="menuSysNo">菜单编号</param>
        /// <returns>列表</returns>
        List<BsPermission> GetByMenuSysNo(int menuSysNo);

        /// <summary>
        /// 获取功能权限列表
        /// </summary>
        /// <param name="sysNoList">系统编号列表</param>
        /// <returns>功能权限列表</returns>
        IList<BsPermission> GetListBySysNoList(List<int> sysNoList);

        /// <summary>
        /// 菜单功能权限分页列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>功能权限分页列表</returns>
        PagedList<BsPermission> GetMenuPermissionPagerList(PermissionRequest request);

        /// <summary>
        /// 获取功能权限分页列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>功能权限分页列表</returns>
        PagedList<BsPermission> GetPagerList(PermissionRequest request);
    }
}
