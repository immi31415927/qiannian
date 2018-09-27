using System;
using System.Collections.Generic;
using EC.DataAccess.Bs;
using EC.Entity;
using EC.Entity.Tables.Bs;
using EC.Entity.View.Bs;

namespace EC.Application.Tables.Bs
{
    using EC.Libraries.Core;

    /// <summary>
    /// 菜单权限业务层
    /// </summary>
    public class MenuPermissionApp : Base<MenuPermissionApp>
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>菜单权限</returns>
        public BsMenuPermission Get(int sysNo)
        {
            var result = Using<IBsMenuPermission>().Get(sysNo);

            return result;
        }

        /// <summary>
        /// 获取菜单权限列表
        /// </summary>
        /// <returns>列表</returns>
        public List<MenuPermissionView> GetMenuPermissionViewList()
        {
            var result = Using<IBsMenuPermission>().GetMenuPermissionViewList();

            return result;
        }
    }
}
