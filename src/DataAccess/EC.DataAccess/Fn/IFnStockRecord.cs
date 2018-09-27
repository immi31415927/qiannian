using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EC.Entity.Parameter.Request.Fn;
using EC.Entity.Tables.Finance;
using EC.Libraries.Core.Pager;

namespace EC.DataAccess.Fn
{
    /// <summary>
    /// 股权记录接口实现
    /// </summary>
    public interface IFnStockRecord
    {
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>系统编号</returns>
        int Insert(FnStockRecord model);

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="list">列表</param>
        /// <returns>影响行数</returns>
        int BatchInsert(List<FnStockRecord> list);

        /// <summary>
        /// 批量更新挂售股权记录
        /// </summary>
        /// <param name="list">挂售股权列表</param>
        /// <returns>影响行数</returns>
        int BatchUpdateSaleRecord(List<FnStockRecord> list);

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>影响行数</returns>
        int Update(FnStockRecord model);

        /// <summary>
        /// 更新取消股权销售
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>影响行数</returns>
        int UpdateCancelSaleStock(FnStockRecord model);

        /// <summary>
        /// 获取挂售比率
        /// </summary>
        /// <returns>挂售比率</returns>
        decimal GetHangSaleRate();

        /// <summary>
        /// 获取股权记录信息
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>股权记录信息</returns>
        FnStockRecord Get(int sysNo);

        /// <summary>
        /// 获取股权记录列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>股权记录列表</returns>
        IList<FnStockRecord> GetList(StockRecordRequest request);

        /// <summary>
        /// 获取股权记录分页列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>股权记录分页列表</returns>
        PagedList<FnStockRecord> GetPagerList(StockRecordRequest request);
    }
}
