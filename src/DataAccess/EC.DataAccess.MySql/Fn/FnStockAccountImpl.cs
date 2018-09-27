using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EC.DataAccess.Fn;
using EC.Entity.Parameter.Request.Fn;
using EC.Entity.Tables.Finance;
using EC.Entity.View.Fn.Ext;
using EC.Libraries.Core.Data;
using EC.Libraries.Core.Pager;

namespace EC.DataAccess.MySql.Fn
{
    /// <summary>
    /// 股权账户接口实现
    /// </summary>
    public class FnStockAccountImpl : Base, IFnStockAccount
    {
        #region 常量

        private const string StrTableName = "Agent_FnStockAccount";

        #endregion

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>系统编号</returns>
        public int Insert(FnStockAccount model)
        {
            return DBContext.Insert(StrTableName, model)
                            .AutoMap(x => x.SysNo)
                            .ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>影响行数</returns>
        public int Update(FnStockAccount model)
        {
            return DBContext.Update(StrTableName, model)
                            .AutoMap(x => x.SysNo)
                            .Where(x => x.SysNo)
                            .Execute();
        }

        /// <summary>
        /// 批量更新销售股权数
        /// </summary>
        /// <param name="list">用户股权账户列表</param>
        /// <returns>影响行数</returns>
        public int BatchUpdateSaleStockNum(List<FnStockAccount> list)
        {
            var sqlStr = new StringBuilder();
            sqlStr.Append(string.Format("UPDATE {0} SET ", StrTableName));

            sqlStr.Append(" StockNum = CASE CustomerSysNo");
            foreach (var item in list)
            {
                sqlStr.Append(string.Format(" WHEN {0} THEN StockNum+{1}", item.CustomerSysNo, item.StockNum));
            }
            sqlStr.Append(" END,");

            sqlStr.Append(" StockForSale = CASE CustomerSysNo");
            foreach (var item in list)
            {
                sqlStr.Append(string.Format(" WHEN {0} THEN StockForSale+{1}", item.CustomerSysNo, item.StockForSale));
            }
            sqlStr.Append(" END,");

            sqlStr.Append(" StockSold = CASE CustomerSysNo");
            foreach (var item in list)
            {
                sqlStr.Append(string.Format(" WHEN {0} THEN StockSold+{1}", item.CustomerSysNo, item.StockSold));
            }
            sqlStr.Append(" END");

            return DBContext.Sql(string.Format("{0} WHERE CustomerSysNo IN({1})", sqlStr.ToString(), string.Join(",", list.Select(p => p.CustomerSysNo))))
                            .Execute();
        }

        /// <summary>
        /// 更新销售股权
        /// </summary>
        /// <param name="customerSysNo">会员编号</param>
        /// <param name="saleStockNum">销售股权数</param>
        /// <returns>影响行数</returns>
        public int UpdateSaleStock(int customerSysNo, int saleStockNum)
        {
            return DBContext.Sql(string.Format("update {0} set StockNum=StockNum-@SaleStockNum,StockForSale=StockForSale+@SaleStockNum where CustomerSysNo=@CustomerSysNo", StrTableName))
                            .Parameter("SaleStockNum", saleStockNum)
                            .Parameter("CustomerSysNo", customerSysNo)
                            .Execute();
        }

        /// <summary>
        /// 更新取消股权销售
        /// </summary>
        /// <param name="customerSysNo">会员编号</param>
        /// <param name="stockNum">股权数</param>
        /// <returns>影响行数</returns>
        public int UpdateCancelSaleStock(int customerSysNo, int stockNum)
        {
            return DBContext.Sql(string.Format("update {0} set StockNum=StockNum+@StockNum,StockForSale=StockForSale-@StockNum where CustomerSysNo=@CustomerSysNo", StrTableName))
                            .Parameter("StockNum", stockNum)
                            .Parameter("CustomerSysNo", customerSysNo)
                            .Execute();
        }

        /// <summary>
        /// 获取股权账户信息
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>股权账户信息</returns>
        public FnStockAccount Get(int sysNo)
        {
            return DBContext.Select<FnStockAccount>("*")
                            .From(StrTableName)
                            .Where("sysNo = @sysNo")
                            .Parameter("sysNo", sysNo)
                            .QuerySingle();
        }

        /// <summary>
        /// 获取股权账户信息通过会员编号
        /// </summary>
        /// <param name="customerSysNo">会员编号</param>
        /// <returns>股权账户信息</returns>
        public FnStockAccount GetByCustomerSysNo(int customerSysNo)
        {
            return DBContext.Select<FnStockAccount>("*")
                            .From(StrTableName)
                            .Where("CustomerSysNo = @CustomerSysNo")
                            .Parameter("CustomerSysNo", customerSysNo)
                            .QuerySingle();
        }

        /// <summary>
        /// 获取股权账户列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>股权账户列表</returns>
        public IList<FnStockAccount> GetList(StockAccountRequest request)
        {
            var dataList = DBContext.Select<FnStockAccount>("*").From(StrTableName);

            Action<string, string, object> setWhere = (@where, name, value) => dataList.AndWhere(@where).Parameter(name, value);

            return dataList.QueryMany();
        }

        /// <summary>
        /// 获取股权账户分页列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>股权账户分页列表</returns>
        public PagedList<StockAccountExt> GetPagerList(StockAccountRequest request)
        {
            const string fromStr = "Agent_FnStockAccount a INNER JOIN Agent_CustomerExt c ON a.CustomerSysNo=c.CustomerSysNo";

            var dataCount = DBContext.Select<int>("count(0)").From(fromStr);
            var dataList = DBContext.Select<StockAccountExt>("a.*,c.RealName as CustomerName,c.SerialNumber").From(fromStr);

            Action<string, string, object> setWhere = (@where, name, value) =>
            {
                dataCount.AndWhere(where).Parameter(name, value);
                dataList.AndWhere(where).Parameter(name, value);
            };

            if (!string.IsNullOrWhiteSpace(request.CustomerName))
            {
                setWhere("c.RealName LIKE CONCAT('%',@CustomerName,'%')", "CustomerName", request.CustomerName);
            }
            if (!string.IsNullOrWhiteSpace(request.SerialNumber))
            {
                setWhere("c.SerialNumber LIKE CONCAT('%',@SerialNumber,'%')", "SerialNumber", request.SerialNumber);
            }

            return new PagedList<StockAccountExt>
            {
                TData = dataList.Paging(request.CurrentPageIndex.GetHashCode(), request.PageSize.GetHashCode()).OrderBy("a.SysNo desc").QueryMany(),
                CurrentPageIndex = request.CurrentPageIndex.GetHashCode(),
                TotalCount = dataCount.QuerySingle()
            };
        }
    }
}
