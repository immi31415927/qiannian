using System;
using EC.DataAccess.CRM;
using EC.Entity.Parameter.Request.CRM;
using EC.Entity.Tables.CRM;
using EC.Libraries.Core.Data;
using EC.Libraries.Core.Pager;

namespace EC.DataAccess.MySql.CRM
{
    /// <summary>
    /// 代理审核数据访问接口实现
    /// </summary>
    public class CrAgencyImpl : Base,ICrAgency
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        public CrAgency Get(int sysNo)
        {
            var sql = "select * from agent_cragency where sysNo=@sysNo;";

            var result = DBContext.Sql(sql)
                                .Parameter("sysNo", sysNo)
                                .QuerySingle<CrAgency>();
            return result;
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>会分页列表</returns>
        public PagedList<CrAgency> GetPagerList(CrAgencyQueryRequest request)
        {
            var dataCount = DBContext.Select<int>("count(0)").From("agent_cragency");
            var dataList = DBContext.Select<CrAgency>("*").From("agent_cragency");

            Action<string, string, object> setWhere = (@where, name, value) =>
            {
                dataCount.AndWhere(where).Parameter(name, value);
                dataList.AndWhere(where).Parameter(name, value);
            };

            //if (request.CustomerSysNo.HasValue)
            //{
                //setWhere(string.Format("CONCAT(',',LevelCustomerStr,',') like '%,{0},%'", request.LevelCustomerSysNo.Value), "", "");
            //}

            return new PagedList<CrAgency>
            {
                TData = dataList.Paging(request.CurrentPageIndex.GetHashCode(), request.PageSize.GetHashCode()).OrderBy("SysNo desc").QueryMany(),
                CurrentPageIndex = request.CurrentPageIndex.GetHashCode(),
                TotalCount = dataCount.QuerySingle()
            };
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>会分页列表</returns>
        public PagedList<CrAgency> GetAdminPager(CrAgencyQueryRequest request)
        {
            var dataCount = DBContext.Select<int>("count(0)").From("agent_cragency");
            var dataList = DBContext.Select<CrAgency>("*").From("agent_cragency");

            Action<string, string, object> setWhere = (@where, name, value) =>
            {
                dataCount.AndWhere(where).Parameter(name, value);
                dataList.AndWhere(where).Parameter(name, value);
            };

            //if (request.CustomerSysNo.HasValue)
            //{
            //setWhere(string.Format("CONCAT(',',LevelCustomerStr,',') like '%,{0},%'", request.LevelCustomerSysNo.Value), "", "");
            //}

            return new PagedList<CrAgency>
            {
                TData = dataList.Paging(request.CurrentPageIndex.GetHashCode(), request.PageSize.GetHashCode()).OrderBy("SysNo desc").QueryMany(),
                CurrentPageIndex = request.CurrentPageIndex.GetHashCode(),
                TotalCount = dataCount.QuerySingle()
            };
        }
    }
}
