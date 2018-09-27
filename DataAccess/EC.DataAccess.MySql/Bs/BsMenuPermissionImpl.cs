using System;
using System.Collections.Generic;
using EC.DataAccess.Bs;
using EC.Entity.Parameter.Request.Bs;
using EC.Entity.Tables.Bs;
using EC.Entity.View.Bs;

namespace EC.DataAccess.MySql.Bs
{
    using EC.Libraries.Core.Data;
    using EC.Libraries.Core.Pager;

    /// <summary>
    /// 菜单权限数据访问接口实现
    /// </summary>
    public class BsMenuPermissionImpl : Base, IBsMenuPermission
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        public BsMenuPermission Get(int sysNo)
        {
            var sql = "select * from agent_bsmenupermission where sysNo=@sysNo;";

            var result = DBContext.Sql(sql)
                                .Parameter("sysNo", sysNo)
                                .QuerySingle<BsMenuPermission>();
            return result;
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model">数据模型</param>
        public int Insert(BsMenuPermission model)
        {
            var result = DBContext.Insert<BsMenuPermission>("agent_bsmenupermission", model)
                                .AutoMap(x => x.SysNo)
                                .ExecuteReturnLastId<int>("sysNo");
            return result;
        }

        /// <summary>
        /// 删除菜单权限
        /// </summary>
        /// <param name="menuSysNo">菜单编号</param>
        /// <returns></returns>
        public int DeleteByMenuSysNo(int menuSysNo)
        {
            var sql = "delete from agent_bsmenupermission where menuSysNo=@menuSysNo;";

            var result = DBContext.Sql(sql)
                                .Parameter("menuSysNo", menuSysNo)
                                .Execute();
            return result;
        }

        /// <summary>
        /// 权限菜单编号获取菜单权限列表
        /// </summary>
        /// <param name="menuSysNo">菜单编号</param>
        /// <returns>列表</returns>
        public List<BsMenuPermission> GetByMenuSysNo(int menuSysNo)
        {
            var sql = @"SELECT * FROM agent_bspermission bp WHERE bp.sysNo in
                        (
                        SELECT bmp.PermissionSysNo FROM agent_bsmenupermission bmp WHERE bmp.menuSysNo=2
                        )";

            var result = DBContext.Sql(sql)
                                .Parameter("menuSysNo", menuSysNo)
                                .QueryMany<BsMenuPermission>();
            return result;
        }

        /// <summary>
        /// 获取菜单权限列表
        /// </summary>
        /// <returns>列表</returns>
        public List<MenuPermissionView> GetMenuPermissionViewList()
        {
            var sql = @"select bp.*,bmp.MenuSysNo from agent_bsmenupermission bmp inner join agent_bspermission bp on bmp.PermissionSysNo=bp.SysNo";

            var result = DBContext.Sql(sql)
                                .QueryMany<MenuPermissionView>();
            return result;
        }

        /// <summary>
        /// 获取菜单权限列表
        /// </summary>
        public PagedList<BsMenuPermission> GetPagingList(MenuPermissionQueryRequeest requeest)
        {
            requeest.Tables = "agent_bsmenupermission br";
            requeest.Tablefields = "br.*";
            requeest.OrderBy = "br.SysNo desc";

            var row = DBContext.Select<BsMenuPermission>(requeest.Tablefields).From(requeest.Tables);
            var count = DBContext.Select<int>("count(0)").From(requeest.Tables);

            //条件查询委托
            Action<string, string, object> setWhere = (@where, name, value) =>
            {
                row.AndWhere(where).Parameter(name, value);
                count.AndWhere(where).Parameter(name, value);

            };

            var list = new PagedList<BsMenuPermission>
            {
                TData = row.Paging(requeest.CurrentPageIndex.GetHashCode(), requeest.PageSize.GetHashCode()).OrderBy(requeest.OrderBy).QueryMany(),
                CurrentPageIndex = requeest.CurrentPageIndex.GetHashCode(),
                TotalCount = count.QuerySingle(),
            };
            return list;
        }
    }
}
