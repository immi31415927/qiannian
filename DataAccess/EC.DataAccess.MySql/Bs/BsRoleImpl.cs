using System;
using System.Collections.Generic;
using EC.DataAccess.Bs;
using EC.Entity.Parameter.Request.Auth;
using EC.Entity.Tables.Bs;

namespace EC.DataAccess.MySql.Bs
{
    using EC.Libraries.Core.Data;
    using EC.Libraries.Core.Pager;

    /// <summary>
    /// 角色数据访问接口实现
    /// </summary>
    public class BsRoleImpl : Base, IBsRole
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        public BsRole Get(int sysNo)
        {
            var sql = "select * from agent_bsrole where sysNo=@sysNo;";

            var result = DBContext.Sql(sql)
                                .Parameter("sysNo", sysNo)
                                .QuerySingle<BsRole>();
            return result;
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model">数据模型</param>
        public int Insert(BsRole model)
        {
            return DBContext.Insert<BsRole>("agent_bsrole", model)
                            .AutoMap(x => x.SysNo)
                            .ExecuteReturnLastId<int>("sysNo");
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">数据模型</param>
        public int Update(BsRole model)
        {
            return DBContext.Update<BsRole>("agent_bsrole", model)
                            .AutoMap(x => x.SysNo)
                            .Where(x => x.SysNo)
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
            return DBContext.Update("agent_bsrole")
                            .Column("status", status)
                            .Where("sysNo", sysNo)
                            .Execute();
        }

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>角色列表</returns>
        public IList<BsRole> GetList(RoleQueryRequeest request)
        {
            var data = DBContext.Select<BsRole>("r.*").From("Agent_BsRole r");

            //条件查询委托
            Action<string, string, object> setWhere = (@where, name, value) => data.AndWhere(@where).Parameter(name, value);

            if (request.Status.HasValue)
            {
                setWhere("r.Status = @Status", "Status", request.Status.Value);
            }

            return data.QueryMany();
        }

        /// <summary>
        /// 获取角色列表
        /// </summary>
        public PagedList<BsRole> GetPagingList(RoleQueryRequeest requeest)
        {
            requeest.Tables = "agent_bsrole br";
            requeest.Tablefields = "br.*";
            requeest.OrderBy = "br.SysNo desc";

            //数据
            var selectorRow = DBContext.Select<BsRole>(requeest.Tablefields).From(requeest.Tables);
            BuildSelectRow(selectorRow, requeest);
            var rows = selectorRow.QueryMany();
            //总数
            var selectorCount = DBContext.Select<int>("count(0)").From(requeest.Tables);
            BuildSelectCount(selectorCount, requeest);
            var totalCount = selectorCount.QuerySingle();

            var list = new PagedList<BsRole>{
                TData = rows,
                TotalCount = totalCount,
            };
            //设置索引
            if (requeest.CurrentPageIndex.HasValue)
                list.CurrentPageIndex = requeest.CurrentPageIndex.Value;

            return list;
        }

        /// <summary>
        /// 构造数据查询条件
        /// </summary>
        private static void BuildSelectRow<T>(ISelectBuilder<T> selector, RoleQueryRequeest requeest)
        {
            if (requeest.Status.HasValue)
                selector.AndWhere("br.Status=@Status").Parameter("Status", requeest.Status);
            if (!string.IsNullOrEmpty(requeest.OrderBy))
                selector.OrderBy(requeest.OrderBy);
            if (requeest.CurrentPageIndex.HasValue && requeest.PageSize.HasValue)
                selector.Paging(requeest.CurrentPageIndex.Value, requeest.PageSize.Value);
        }

        /// <summary>
        /// 构造总数查询条件
        /// </summary>
        private static void BuildSelectCount<T>(ISelectBuilder<T> selector, RoleQueryRequeest requeest)
        {
            if (requeest.Status.HasValue)
                selector.AndWhere("br.Status=@Status").Parameter("Status", requeest.Status);
        }

        /// <summary>
        /// 获取角色列表
        /// </summary>
        public PagedList<BsRole> GetPaging(RoleQueryRequeest requeest)
        {
            requeest.Tables = "agent_bsrole br";
            requeest.Tablefields = "br.*";
            requeest.OrderBy = "br.SysNo desc";

            var row = DBContext.Select<BsRole>(requeest.Tablefields).From(requeest.Tables);
            var count = DBContext.Select<int>("count(0)").From(requeest.Tables);

            //条件查询委托
            Action<string, string, object> setWhere = (@where, name, value) =>
            {
                row.AndWhere(where).Parameter(name, value);
                count.AndWhere(where).Parameter(name, value);

            };

            if (requeest.Status.HasValue)
                setWhere("br.Status=@Status", "Status", requeest.Status);

            var list = new PagedList<BsRole>
            {
                TData = row.Paging(requeest.CurrentPageIndex.GetHashCode(), requeest.PageSize.GetHashCode()).OrderBy(requeest.OrderBy).QueryMany(),
                CurrentPageIndex = requeest.CurrentPageIndex.GetHashCode(),
                TotalCount = count.QuerySingle(),
            };
            return list;
        }
    }
}
