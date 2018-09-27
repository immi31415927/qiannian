using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EC.DataAccess.Fn;
using EC.Entity.Enum;
using EC.Entity.Parameter.Request.Fn;
using EC.Entity.Tables.Finance;
using EC.Libraries.Core.Data;
using EC.Libraries.Core.Pager;

namespace EC.DataAccess.MySql.Fn
{
    /// <summary>
    /// 股权记录接口实现
    /// </summary>
    public class FnStockRecordImpl : Base, IFnStockRecord
    {
        #region 常量

        private const string StrTableName = "Agent_FnStockRecord";

        #endregion

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>系统编号</returns>
        public int Insert(FnStockRecord model)
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
        public int BatchInsert(List<FnStockRecord> list)
        {
            var strSql = new StringBuilder();
            strSql.Append(string.Format("INSERT INTO {0}(Type,OperatorType,OperatorSysNo,StockNum,Status,Remarks,CreatedDate) VALUES", StrTableName));

            for (var i = 0; i < list.Count; i++)
            {
                var item = list[i];
                strSql.Append(string.Format("({0},{1},{2},{3},{4},'{5}',DATE_FORMAT('{6}', '%Y-%m-%d %h:%i:%s'))",
                         item.Type, item.OperatorType, item.OperatorSysNo, item.StockNum, item.Status, item.Remarks, item.CreatedDate));
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
        /// 批量更新挂售股权记录
        /// </summary>
        /// <param name="list">挂售股权列表</param>
        /// <returns>影响行数</returns>
        public int BatchUpdateSaleRecord(List<FnStockRecord> list)
        {
            var sqlStr = new StringBuilder();
            sqlStr.Append(string.Format("UPDATE {0} SET ", StrTableName));

            sqlStr.Append(" TradedStockNum = CASE SysNo");
            foreach (var item in list)
            {
                sqlStr.Append(string.Format(" WHEN {0} THEN TradedStockNum+{1}", item.SysNo, item.TradedStockNum));
            }
            sqlStr.Append(" END,");

            sqlStr.Append(" Status = CASE SysNo");
            foreach (var item in list)
            {
                sqlStr.Append(string.Format(" WHEN {0} THEN {1}", item.SysNo, item.Status));
            }
            sqlStr.Append(" END,");

            sqlStr.Append(" Remarks = CASE SysNo");
            foreach (var item in list)
            {
                sqlStr.Append(string.Format(" WHEN {0} THEN CONCAT(Remarks,'{1}')", item.SysNo, item.Remarks));
            }
            sqlStr.Append(" END");

            return DBContext.Sql(string.Format("{0} WHERE SysNo IN({1})", sqlStr.ToString(), string.Join(",", list.Select(p => p.SysNo))))
                            .Execute();
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>影响行数</returns>
        public int Update(FnStockRecord model)
        {
            return DBContext.Update(StrTableName, model)
                            .AutoMap(x => x.SysNo)
                            .Where(x => x.SysNo)
                            .Execute();
        }

        /// <summary>
        /// 更新取消股权销售
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>影响行数</returns>
        public int UpdateCancelSaleStock(FnStockRecord model)
        {
            return DBContext.Sql(string.Format("update {0} set StockNum=StockNum-@StockNum,Status=@Status,Remarks=CONCAT(Remarks,@Remarks) where SysNo=@SysNo", StrTableName))
                           .Parameter("StockNum", model.StockNum)
                           .Parameter("Status", model.Status)
                           .Parameter("Remarks", model.Remarks)
                           .Parameter("SysNo", model.SysNo)
                           .Execute();
        }

        /// <summary>
        /// 获取挂售比率
        /// </summary>
        /// <returns>挂售比率</returns>
        public decimal GetHangSaleRate()
        {
            return DBContext.Sql(string.Format("select SUM(s.TradedStockNum)/SUM(s.StockNum) from {0} s where s.Type = @Type", StrTableName))
                    .Parameter("Type", StockRecordEnum.TypeEnum.股权挂售.GetHashCode())
                    .QuerySingle<decimal>();
        }

        /// <summary>
        /// 获取股权记录信息
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>股权记录信息</returns>
        public FnStockRecord Get(int sysNo)
        {
            return DBContext.Select<FnStockRecord>("*")
                            .From(StrTableName)
                            .Where("sysNo = @sysNo")
                            .Parameter("sysNo", sysNo)
                            .QuerySingle();
        }

        /// <summary>
        /// 获取股权记录列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>股权记录列表</returns>
        public IList<FnStockRecord> GetList(StockRecordRequest request)
        {
            var dataList = DBContext.Select<FnStockRecord>("*").From(StrTableName);

            Action<string, string, object> setWhere = (@where, name, value) => dataList.AndWhere(@where).Parameter(name, value);

            if (request.Type.HasValue)
            {
                setWhere("Type = @Type", "Type", request.Type.Value);
            }
            if (request.OperatorType.HasValue)
            {
                setWhere("OperatorType = @OperatorType", "OperatorType", request.OperatorType.Value);
            }
            if (request.OperatorSysNo.HasValue)
            {
                setWhere("OperatorSysNo = @OperatorSysNo", "OperatorSysNo", request.OperatorSysNo.Value);
            }
            if (request.StatusList != null && request.StatusList.Any())
            {
                setWhere(string.Format("Status in({0})", string.Join(",", request.StatusList)), "", "");
            }
            if (request.StartDate.HasValue)
            {
                setWhere("CreatedDate >= @StartDate", "StartDate", request.StartDate.ToString());
            }
            if (request.EndDate.HasValue)
            {
                setWhere("CreatedDate <= @EndDate", "EndDate", request.EndDate.ToString());
            }

            if (request.TopNum.HasValue)
            {
                dataList.OrderBy("CreatedDate desc").Paging(1, request.TopNum.Value);
            }

            return dataList.QueryMany();
        }

        /// <summary>
        /// 获取股权记录分页列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>股权记录分页列表</returns>
        public PagedList<FnStockRecord> GetPagerList(StockRecordRequest request)
        {
            var dataCount = DBContext.Select<int>("count(0)").From(StrTableName);
            var dataList = DBContext.Select<FnStockRecord>("*").From(StrTableName);

            Action<string, string, object> setWhere = (@where, name, value) =>
            {
                dataCount.AndWhere(where).Parameter(name, value);
                dataList.AndWhere(where).Parameter(name, value);
            };

            if (request.Type.HasValue)
            {
                setWhere("Type = @Type", "Type", request.Type.Value);
            }
            if (request.OperatorType.HasValue)
            {
                setWhere("OperatorType = @OperatorType", "OperatorType", request.OperatorType.Value);
            }
            if (request.OperatorSysNo.HasValue)
            {
                setWhere("OperatorSysNo = @OperatorSysNo", "OperatorSysNo", request.OperatorSysNo.Value);
            }
            if (request.StatusList != null && request.StatusList.Any())
            {
                setWhere(string.Format("Status in({0})", string.Join(",", request.StatusList)), "", "");
            }

            return new PagedList<FnStockRecord>
            {
                TData = dataList.Paging(request.CurrentPageIndex.GetHashCode(), request.PageSize.GetHashCode()).OrderBy("SysNo desc").QueryMany(),
                CurrentPageIndex = request.CurrentPageIndex.GetHashCode(),
                TotalCount = dataCount.QuerySingle()
            };
        }
    }
}
