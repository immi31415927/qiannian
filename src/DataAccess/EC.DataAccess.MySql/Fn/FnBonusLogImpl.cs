using System;
using System.Collections.Generic;
using System.Text;
using EC.DataAccess.Fn;
using EC.Entity.Parameter.Request.Fn;
using EC.Entity.Tables.Finance;
using EC.Entity.View.Fn;
using EC.Entity.View.Fn.Ext;
using EC.Libraries.Core.Pager;

namespace EC.DataAccess.MySql.Fn
{
    using EC.Libraries.Core.Data;

    /// <summary>
    /// 奖金记录数据访问接口实现
    /// </summary>
    public class FnBonusLogImpl : Base, IFnBonusLog
    {
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>系统编号</returns>
        public int Insert(FnBonusLog model)
        {
            return DBContext.Insert("agent_fnbonuslog", model)
                            .AutoMap(x => x.SysNo)
                            .ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        public FnBonusLog Get(int sysNo)
        {
            var sql = "select * from agent_fnbonuslog where sysNo=@sysNo;";

            var result = DBContext.Sql(sql)
                                .Parameter("sysNo", sysNo)
                                .QuerySingle<FnBonusLog>();
            return result;
        }

        /// <summary>
        /// 获取奖金记录分页列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>奖金记录分页列表</returns>
        public PagedList<FnBonusLog> GetPagerList(FnBonusLogQueryRequest request)
        {
            var dataCount = DBContext.Select<int>("count(0)").From("agent_fnbonuslog");
            var dataList = DBContext.Select<FnBonusLog>("*").From("agent_fnbonuslog");

            Action<string, string, object> setWhere = (@where, name, value) =>
            {
                dataCount.AndWhere(where).Parameter(name, value);
                dataList.AndWhere(where).Parameter(name, value);
            };

            if (request.CustomerSysNo.HasValue)
            {
                setWhere("CustomerSysNo = @CustomerSysNo", "CustomerSysNo", request.CustomerSysNo.Value);
            }

            if (request.Type.HasValue)
            {
                setWhere("Type = @Type", "Type", request.Type.Value);
            }
            return new PagedList<FnBonusLog>
            {
                TData = dataList.Paging(request.CurrentPageIndex.GetHashCode(), request.PageSize.GetHashCode()).OrderBy("SysNo desc").QueryMany(),
                CurrentPageIndex = request.CurrentPageIndex.GetHashCode(),
                TotalCount = dataCount.QuerySingle()
            };
        }

        /// <summary>
        /// 获取奖金扩展记录分页列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>奖金扩展记录分页列表</returns>
        public PagedList<BonusLogExt> GetPagerExtList(FnBonusLogQueryRequest request)
        {
            const string sqlFrom = "agent_fnbonuslog l inner join CrCustomer sc on l.SourceSysNo = sc.SysNo inner join CrCustomer c on l.CustomerSysNo = c.SysNo";
            var dataCount = DBContext.Select<int>("count(0)").From(sqlFrom);
            var dataList = DBContext.Select<BonusLogExt>("l.*,sc.RealName as SourceName,(sc.Level-c.Level) as SourceLevel").From(sqlFrom);

            Action<string, string, object> setWhere = (@where, name, value) =>
            {
                dataCount.AndWhere(where).Parameter(name, value);
                dataList.AndWhere(where).Parameter(name, value);
            };

            if (request.CustomerSysNo.HasValue)
            {
                setWhere("l.CustomerSysNo = @CustomerSysNo", "CustomerSysNo", request.CustomerSysNo.Value);
            }

            if (request.Type.HasValue)
            {
                setWhere("l.Type = @Type", "Type", request.Type.Value);
            }
            return new PagedList<BonusLogExt>
            {
                TData = dataList.Paging(request.CurrentPageIndex.GetHashCode(), request.PageSize.GetHashCode()).OrderBy("l.SysNo desc").QueryMany(),
                CurrentPageIndex = request.CurrentPageIndex.GetHashCode(),
                TotalCount = dataCount.QuerySingle()
            };
        }

        /// <summary>
        /// 后台获取奖金记录查询
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>后台获取奖金分页列表</returns>
        public PagedList<BonusLogResponse> GetAdminPager(BonusLogAdminQueryRequest request)
        {
            var dataCount = DBContext.Select<int>("count(0)").From("agent_fnbonuslog bl");
            var dataList = DBContext.Select<BonusLogResponse>("bl.SysNo,(SELECT ce.RealName FROM crcustomer ce WHERE ce.SysNo=bl.CustomerSysNo) as Receiver,(SELECT ce.ReferrerSysNo FROM crcustomer ce WHERE ce.SysNo=bl.CustomerSysNo) as ReceiverReferrerSysNo,(SELECT ce.RealName FROM crcustomer ce WHERE ce.SysNo=bl.SourceSysNo) as Supplier,bl.SourceSerialNumber,bl.Amount,bl.Type,bl.Remarks,bl.`Status`,bl.CreatedDate").From("agent_fnbonuslog bl");

            Action<string, string, object> setWhere = (@where, name, value) =>
            {
                dataCount.AndWhere(where).Parameter(name, value);
                dataList.AndWhere(where).Parameter(name, value);
            };
            return new PagedList<BonusLogResponse>
            {
                TData = dataList.Paging(request.CurrentPageIndex.GetHashCode(), request.PageSize.GetHashCode()).OrderBy("SysNo desc").QueryMany(),
                CurrentPageIndex = request.CurrentPageIndex.GetHashCode(),
                TotalCount = dataCount.QuerySingle()
            };
        }

        /// <summary>
        /// 批量添加奖金日志
        /// </summary>
        /// <param name="batchInsertList">参数</param>
        /// <returns>影响行数</returns>
        public int BatchInsert(List<FnBonusLog> batchInsertList)
        {
            var sqlStr = new StringBuilder();

            foreach (var item in batchInsertList)
            {
                sqlStr.Append(string.Format("insert into agent_fnbonuslog(CustomerSysNo,SourceSysNo,SourceSerialNumber,Amount,Type,Remarks,Status,CreatedDate) VALUES ({0},{1},'{2}',{3},{4},'{5}','{6}','{7}');", item.CustomerSysNo, item.SourceSysNo, item.SourceSerialNumber, item.Amount, item.Type, item.Remarks,item.Status, item.CreatedDate));
            }
            return DBContext.Sql(sqlStr.ToString())
                .Execute();
        }

        /// <summary>
        /// 获取奖金总额
        /// </summary>
        /// <param name="customerSysNo"></param>
        /// <returns></returns>
        public int GetBonusSum(int customerSysNo)
        {
            //SELECT SUM(Amount) AS Amount from agent_fnbonuslog WHERE CustomerSysNo=1
            var sql = "SELECT SUM(Amount) AS Amount from agent_fnbonuslog WHERE CustomerSysNo=@customerSysNo";

            var result = DBContext.Sql(sql)
                                .Parameter("customerSysNo", customerSysNo)
                                .QuerySingle<int>();
            return result;

        }
    }
}
