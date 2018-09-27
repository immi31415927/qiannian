using System;
using System.Collections.Generic;
using EC.DataAccess.Bs;
using EC.Entity.Parameter.Request.Bs;
using EC.Entity.Tables.Bs;
using EC.Entity.View.Bs;
using EC.Libraries.Core.Pager;

namespace EC.DataAccess.MySql.Bs
{
    using EC.Libraries.Core.Data;

    /// <summary>
    /// 功能权限数据访问接口实现
    /// </summary>
    public class BsPermissionImpl : Base, IBsPermission
    {
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>系统编号</returns>
        public int Insert(BsPermission model)
        {
            return DBContext.Insert("Agent_BsPermission", model)
                            .AutoMap(x => x.SysNo)
                            .ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>影响行数</returns>
        public int Update(BsPermission model)
        {
            return DBContext.Update("Agent_BsPermission", model)
                            .AutoMap(x => x.SysNo)
                            .Where(x => x.SysNo)
                            .Execute();
        }

        /// <summary>
        /// 修改部分数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>影响行数</returns>
        public int UpdateSection(BsPermission model)
        {
            return DBContext.Update("Agent_BsPermission")
                            .Column("Name", model.Name)
                            .Column("Code", model.Code)
                            .Column("Description", model.Description)
                            .Column("Status", model.Status)
                            .Where("SysNo", model.SysNo)
                            .Execute();
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="sysNo">编码</param>
        /// <param name="status">状态</param>
        /// <returns>影响行数</returns>
        public int UpdateStatus(int sysNo, int status)
        {
            return DBContext.Update("Agent_BsPermission")
                            .Column("Status", status)
                            .Where("SysNo", sysNo)
                            .Execute();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>影响行数</returns>
        public int Delete(int sysNo)
        {
            return DBContext.Delete("Agent_BsPermission")
                            .Where("SysNo", sysNo)
                            .Execute();
        }

        /// <summary>
        /// 获取功能权限信息
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>功能权限信息</returns>
        public BsPermission Get(int sysNo)
        {
            return DBContext.Sql("select * from agent_bspermission where sysNo=@sysNo;")
                            .Parameter("sysNo", sysNo)
                            .QuerySingle<BsPermission>();
        }

        /// <summary>
        /// 权限菜单编号获取菜单权限列表
        /// </summary>
        /// <param name="menuSysNo">菜单编号</param>
        /// <returns>列表</returns>
        public List<BsPermission> GetByMenuSysNo(int menuSysNo)
        {
            var sql = @"SELECT * FROM agent_bspermission bp WHERE bp.sysNo in
                        (
                        SELECT bmp.PermissionSysNo FROM agent_bsmenupermission bmp WHERE bmp.menuSysNo=@menuSysNo
                        )";

            var result = DBContext.Sql(sql)
                                .Parameter("menuSysNo", menuSysNo)
                                .QueryMany<BsPermission>();
            return result;
        }

        /// <summary>
        /// 获取功能权限列表
        /// </summary>
        /// <param name="sysNoList">系统编号列表</param>
        /// <returns>功能权限列表</returns>
        public IList<BsPermission> GetListBySysNoList(List<int> sysNoList)
        {
            return DBContext.Sql("Select * From Agent_BsPermission Where SysNo in(@SysNos)")
                            .Parameter("SysNos", sysNoList.ToArray())
                            .QueryMany<BsPermission>();
        }

        /// <summary>
        /// 菜单功能权限分页列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>功能权限分页列表</returns>
        public PagedList<BsPermission> GetMenuPermissionPagerList(PermissionRequest request)
        {
            var dataCount = DBContext.Select<int>("count(0)").From("agent_bspermission bp");
            var dataList = DBContext.Select<BsPermission>("*").From("agent_bspermission bp");

            //条件查询委托
            Action<string, string, object> setWhere = (@where, name, value) =>
            {
                dataCount.AndWhere(where).Parameter(name, value);
                dataList.AndWhere(where).Parameter(name, value);
            };

            setWhere("bp.sysNo not in(SELECT bmp.PermissionSysNo FROM agent_bsmenupermission bmp where 1=1)", "", "");

            return new PagedList<BsPermission>
            {
                TData = dataList.Paging(request.CurrentPageIndex.GetHashCode(), request.PageSize.GetHashCode()).OrderBy("SysNo desc").QueryMany(),
                CurrentPageIndex = request.CurrentPageIndex.GetHashCode(),
                TotalCount = dataCount.QuerySingle()
            };
        } 

        /// <summary>
        /// 获取功能权限分页列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>功能权限分页列表</returns>
        public PagedList<BsPermission> GetPagerList(PermissionRequest request)
        {
            var dataCount = DBContext.Select<int>("count(0)").From("Agent_BsPermission");
            var dataList = DBContext.Select<BsPermission>("*").From("Agent_BsPermission");

            //条件查询委托
            Action<string, string, object> setWhere = (@where, name, value) =>
            {
                dataCount.AndWhere(where).Parameter(name, value);
                dataList.AndWhere(where).Parameter(name, value);
            };

            //if (input.ParentSysNo.HasValue)
            //{
            //    setWhere("ParentSysNo=@ParentSysNo", "ParentSysNo", input.ParentSysNo.Value);
            //}
            //if (input.Status.HasValue)
            //{
            //    setWhere("Status=@Status", "Status", input.Status.Value);
            //}
            //if (!string.IsNullOrEmpty(input.Name))
            //{
            //    setWhere("Name LIKE CONCAT('%',@Name,'%')", "Name", input.Name);
            //}
            //if (!string.IsNullOrEmpty(input.Email))
            //{
            //    setWhere("Email LIKE CONCAT('%',@Email,'%')", "Email", input.Email);
            //}
            //if (!string.IsNullOrEmpty(input.CompanyName))
            //{
            //    setWhere("CompanyName LIKE CONCAT('%',@CompanyName,'%')", "CompanyName", input.CompanyName);
            //}
            //if (!string.IsNullOrEmpty(input.PhoneNumber))
            //{
            //    setWhere("PhoneNumber LIKE CONCAT('%',@PhoneNumber,'%')", "PhoneNumber", input.PhoneNumber);
            //}

            return new PagedList<BsPermission>
            {
                TData = dataList.Paging(request.CurrentPageIndex.GetHashCode(), request.PageSize.GetHashCode()).OrderBy("SysNo desc").QueryMany(),
                CurrentPageIndex = request.CurrentPageIndex.GetHashCode(),
                TotalCount = dataCount.QuerySingle()
            };
        } 
    }
}
