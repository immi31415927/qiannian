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
    /// 交易日志接口实现
    /// </summary>
    public interface IFnTradeLog
    {
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>系统编号</returns>
        int Insert(FnTradeLog model);

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="list">列表</param>
        /// <returns>影响行数</returns>
        int BatchInsert(List<FnTradeLog> list);

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>影响行数</returns>
        int Update(FnTradeLog model);

        /// <summary>
        /// 获取交易日志信息
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>交易日志信息</returns>
        FnTradeLog Get(int sysNo);

        /// <summary>
        /// 获取交易日志列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>交易日志列表</returns>
        IList<FnTradeLog> GetList(TradeLogRequest request);

        /// <summary>
        /// 获取交易日志分页列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>交易日志分页列表</returns>
        PagedList<TradeLogExt> GetPagerList(TradeLogRequest request);
    }
}
