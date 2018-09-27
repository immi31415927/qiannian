using System.Collections.Generic;
using EC.Entity.Parameter.Request.Bs;
using EC.Entity.Tables.Bs;
using EC.Entity.View.Bs;

namespace EC.DataAccess.Bs
{
    using EC.Libraries.Core.Pager;

    /// <summary>
    /// 菜单权限数据访问接口
    /// </summary>
    public interface IBsMenuPermission
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        BsMenuPermission Get(int sysNo);

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model">数据模型</param>
        int Insert(BsMenuPermission model);

        /// <summary>
        /// 删除菜单权限
        /// </summary>
        /// <param name="menuSysNo">菜单编号</param>
        /// <returns></returns>
        int DeleteByMenuSysNo(int menuSysNo);

        /// <summary>
        /// 权限菜单编号获取菜单权限列表
        /// </summary>
        /// <param name="menuSysNo">菜单编号</param>
        /// <returns>列表</returns>
        List<BsMenuPermission> GetByMenuSysNo(int menuSysNo);

        /// <summary>
        /// 获取菜单权限列表
        /// </summary>
        /// <returns>列表</returns>
        List<MenuPermissionView> GetMenuPermissionViewList();

        /// <summary>
        /// 获取菜单权限列表
        /// </summary>
        PagedList<BsMenuPermission> GetPagingList(MenuPermissionQueryRequeest requeest);
    }
}
