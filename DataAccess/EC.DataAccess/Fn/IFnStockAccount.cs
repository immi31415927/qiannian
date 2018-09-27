using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EC.Entity.Parameter.Request.Fn;
using EC.Entity.Tables.Finance;
using EC.Entity.View.Fn.Ext;
using EC.Libraries.Core.Pager;

namespace EC.DataAccess.Fn
{
    /// <summary>
    /// 股权账户接口实现接口
    /// </summary>
    public interface IFnStockAccount
    {
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>系统编号</returns>
        int Insert(FnStockAccount model);

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>影响行数</returns>
        int Update(FnStockAccount model);

        /// <summary>
        /// 批量更新销售股权数
        /// </summary>
        /// <param name="list">用户股权账户列表</param>
        /// <returns>影响行数</returns>
        int BatchUpdateSaleStockNum(List<FnStockAccount> list);

        /// <summary>
        /// 更新销售股权
        /// </summary>
        /// <param name="customerSysNo">会员编号</param>
        /// <param name="saleStockNum">销售股权数</param>
        /// <returns>影响行数</returns>
        int UpdateSaleStock(int customerSysNo, int saleStockNum);

        /// <summary>
        /// 更新取消股权销售
        /// </summary>
        /// <param name="customerSysNo">会员编号</param>
        /// <param name="stockNum">股权数</param>
        /// <returns>影响行数</returns>
        int UpdateCancelSaleStock(int customerSysNo, int stockNum);

        /// <summary>
        /// 获取股权账户信息
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>股权账户信息</returns>
        FnStockAccount Get(int sysNo);

        /// <summary>
        /// 获取股权账户信息通过会员编号
        /// </summary>
        /// <param name="customerSysNo">会员编号</param>
        /// <returns>股权账户信息</returns>
        FnStockAccount GetByCustomerSysNo(int customerSysNo);

        /// <summary>
        /// 获取股权账户列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>股权账户列表</returns>
        IList<FnStockAccount> GetList(StockAccountRequest request);

        /// <summary>
        /// 获取股权账户分页列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>股权账户分页列表</returns>
        PagedList<StockAccountExt> GetPagerList(StockAccountRequest request);
    }
}
