using System;
using System.Collections.Generic;
using System.Text;
using EC.DataAccess.Fn;
using EC.Entity.Enum;
using EC.Entity.Parameter.Request.Fn;
using EC.Entity.Tables.Finance;
using EC.Entity.View.Fn.Ext;
using EC.Libraries.Core.Data;
using EC.Libraries.Core.Pager;

namespace EC.DataAccess.MySql.Fn
{
    /// <summary>
    /// 交易日志接口实现
    /// </summary>
    public class FnTradeLogImpl : Base, IFnTradeLog
    {
        #region 常量

        private const string StrTableName = "Agent_FnTradeLog";

        #endregion

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>系统编号</returns>
        public int Insert(FnTradeLog model)
        {
            return DBContext.Insert(StrTableName, model)
                            .AutoMap(x => x.SysNo)
                            .ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="list">列表</param>
        /// <returns>影响行数</returns>
        public int BatchInsert(List<FnTradeLog> list)
        {
            var strSql = new StringBuilder();
            strSql.Append(string.Format("INSERT INTO {0}(SourceType,SourceSysNo,OutOperatorType,OutOperatorSysNo,InOperatorType,InOperatorSysNo,TradeAmount,CurrentStockAmount,Remarks,CreatedDate) VALUES", StrTableName));

            for (var i = 0; i < list.Count; i++)
            {
                var item = list[i];
                strSql.Append(string.Format("({0},{1},{2},{3},{4},{5},{6},{7},'{8}',DATE_FORMAT('{9}', '%Y-%m-%d %h:%i:%s'))",
                         item.SourceType, item.SourceSysNo, item.OutOperatorType, item.OutOperatorSysNo, item.InOperatorType, item.InOperatorSysNo, item.TradeAmount, item.CurrentStockAmount, item.Remarks, item.CreatedDate));
                if (i < list.Count - 1)
                {
                    strSql.Append(",");
                }
            }
            strSql.Append(";");

            return DBContext.Sql(strSql.ToString())
                            .Execute();
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>影响行数</returns>
        public int Update(FnTradeLog model)
        {
            return DBContext.Update(StrTableName, model)
                            .AutoMap(x => x.SysNo)
                            .Where(x => x.SysNo)
                            .Execute();
        }

        /// <summary>
        /// 获取交易日志信息
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>交易日志信息</returns>
        public FnTradeLog Get(int sysNo)
        {
            return DBContext.Select<FnTradeLog>("*")
                            .From(StrTableName)
                            .Where("sysNo = @sysNo")
                            .Parameter("sysNo", sysNo)
                            .QuerySingle();
        }

        /// <summary>
        /// 获取交易日志列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>交易日志列表</returns>
        public IList<FnTradeLog> GetList(TradeLogRequest request)
        {
            var dataList = DBContext.Select<FnTradeLog>("*").From(StrTableName);

            Action<string, string, object> setWhere = (@where, name, value) => dataList.AndWhere(@where).Parameter(name, value);

            return dataList.QueryMany();
        }

        /// <summary>
        /// 获取交易日志分页列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>交易日志分页列表</returns>
        public PagedList<TradeLogExt> GetPagerList(TradeLogRequest request)
        {
            var fromStr = string.Format(@"(SELECT  t.*,CASE t.OutOperatorType WHEN {0} THEN (SELECT c.RealName FROM Agent_CustomerExt c WHERE c.CustomerSysNo = t.OutOperatorSysNo) ELSE 
            (SELECT u.`Name` FROM Agent_BsUser u WHERE u.SysNo = t.OutOperatorSysNo) END as OutName,
            CASE t.InOperatorType WHEN {1} THEN (SELECT c.RealName FROM Agent_CustomerExt c WHERE c.CustomerSysNo = t.InOperatorSysNo) ELSE 
            (SELECT u.`Name` FROM Agent_BsUser u WHERE u.SysNo = t.InOperatorSysNo) END as InName
            FROM Agent_FnTradeLog t) l", OperatorTypeEnum.会员.GetHashCode(), OperatorTypeEnum.会员.GetHashCode());

            var dataCount = DBContext.Select<int>("count(0)").From(fromStr);
            var dataList = DBContext.Select<TradeLogExt>("l.*").From(fromStr);

            Action<string, string, object> setWhere = (@where, name, value) =>
            {
                dataCount.AndWhere(where).Parameter(name, value);
                dataList.AndWhere(where).Parameter(name, value);
            };

            if (request.SourceType.HasValue)
            {
                setWhere("l.SourceType = @SourceType", "SourceType", request.SourceType.Value);
            }
            if (request.OutOperatorType.HasValue)
            {
                setWhere("l.OutOperatorType = @OutOperatorType", "OutOperatorType", request.OutOperatorType.Value);
            }
            if (request.InOperatorType.HasValue)
            {
                setWhere("l.InOperatorType = @InOperatorType", "InOperatorType", request.InOperatorType.Value);
            }
            if (!string.IsNullOrWhiteSpace(request.OutName))
            {
                setWhere("l.OutName LIKE CONCAT('%',@OutName,'%')", "OutName", request.OutName);
            }
            if (!string.IsNullOrWhiteSpace(request.InName))
            {
                setWhere("l.InName LIKE CONCAT('%',@InName,'%')", "InName", request.InName);
            }

            return new PagedList<TradeLogExt>
            {
                TData = dataList.Paging(request.CurrentPageIndex.GetHashCode(), request.PageSize.GetHashCode()).OrderBy("l.SysNo desc").QueryMany(),
                CurrentPageIndex = request.CurrentPageIndex.GetHashCode(),
                TotalCount = dataCount.QuerySingle()
            };
        }
    }
}
