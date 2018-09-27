using System;
using EC.DataAccess.Fn;
using EC.Entity.Parameter.Request.Fn;
using EC.Entity.Tables.Finance;
using EC.Libraries.Core.Pager;
using EC.Libraries.Core.Data;
using System.Collections.Generic;
namespace EC.DataAccess.MySql.Fn
{
    /// <summary>
    /// 提现申请数据访问接口实现
    /// </summary>
    public class FnCashoutImpl : Base, IFnCashout
    {

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        public FnCashout Get(int sysNo)
        {
            var sql = "select * from agent_fncashout where sysNo=@sysNo;";

            var result = DBContext.Sql(sql)
                                .Parameter("sysNo", sysNo)
                                .QuerySingle<FnCashout>();
            return result;
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model">数据模型</param>
        public int Insert(FnCashout model)
        {
            var result = DBContext.Insert<FnCashout>("agent_fncashout", model)
                                .AutoMap(x => x.SysNo)
                                .ExecuteReturnLastId<int>("sysNo");
            return result;
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="sysNo">编码</param>
        /// <param name="status">状态</param>
        /// <returns>影响行数</returns>
        public int UpdateStatus(int sysNo, int status, string reject)
        {
            return DBContext.Update("agent_fncashout")
                            .Column("status", status)
                            .Column("reject", reject)
                            .Where("sysNo", sysNo)
                            .Execute();
        }

        /// <summary>
        /// 查询某个时间段提现记录
        /// </summary>
        /// <param name="requeest">参数</param>
        public List<FnCashout> GetPeriodTimes(FnCashoutQueryRequest requeest)
        {
            //测试SQL:SELECT * FROM bssms WHERE CustomerSysNo='15008228718' AND CreatedDate>='2017-11-22 00:00:00' AND CreatedDate<='2017-11-22 23:59:59'
            var strSql = string.Format("SELECT * FROM agent_fncashout WHERE CustomerSysNo={0} AND CreatedDate>='{1}' AND CreatedDate<='{2}'", requeest.CustomerSysNo.Value,requeest.StartTime,requeest.EndTime);

            var result = DBContext.Sql(strSql)
                                //.Parameter("CustomerSysNo", requeest.CustomerSysNo.HasValue)
                                //.Parameter("StartTime", requeest.StartTime)
                                //.Parameter("EndTime", requeest.EndTime)
                                .QueryMany<FnCashout>();
            return result;
        }

        /// <summary>
        /// 获取提现申请分页列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>提现申请分页列表</returns>
        public PagedList<FnCashout> GetPagerList(FnCashoutQueryRequest request)
        {
            var dataCount = DBContext.Select<int>("count(0)").From("agent_fncashout");
            var dataList = DBContext.Select<FnCashout>("*").From("agent_fncashout");

            Action<string, string, object> setWhere = (@where, name, value) =>
            {
                dataCount.AndWhere(where).Parameter(name, value);
                dataList.AndWhere(where).Parameter(name, value);
            };

            if (request.CustomerSysNo.HasValue)
            {
                setWhere("CustomerSysNo = @CustomerSysNo", "CustomerSysNo", request.CustomerSysNo.Value);
            }


            return new PagedList<FnCashout>
            {
                TData = dataList.Paging(request.CurrentPageIndex.GetHashCode(), request.PageSize.GetHashCode()).OrderBy("SysNo desc").QueryMany(),
                CurrentPageIndex = request.CurrentPageIndex.GetHashCode(),
                TotalCount = dataCount.QuerySingle()
            };
        }

        
    }
}
