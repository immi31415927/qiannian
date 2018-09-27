using EC.DataAccess.Fn;
using EC.Entity;
using EC.Entity.Parameter.Request.Fn;
using EC.Entity.Tables.Finance;
using EC.Libraries.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using EC.Libraries.Core.Pager;

namespace EC.Application.Tables.Fn
{
    /// <summary>
    /// 股权记录业务层
    /// </summary>
    public class StockRecordApp : Base<StockRecordApp>
    {
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>系统编号</returns>
        public JResult<int> Insert(FnStockRecord model)
        {
            var result = new JResult<int>();

            try
            {
                var row = Using<IFnStockRecord>().Insert(model);
                if (row <= 0)
                {
                    throw new Exception("添加数据失败");
                }

                result.Status = true;
                result.Data = row;
            }
            catch (Exception ex) { result.Message = ex.Message; }

            return result;
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>影响行数</returns>
        public JResult Update(FnStockRecord model)
        {
            var result = new JResult();

            try
            {
                var row = Using<IFnStockRecord>().Update(model);
                if (row <= 0)
                {
                    throw new Exception("修改数据失败");
                }

                result.Status = true;
            }
            catch (Exception ex) { result.Message = ex.Message; }

            return result;
        }

        /// <summary>
        /// 获取挂售比率
        /// </summary>
        /// <returns>挂售比率</returns>
        public decimal GetHangSaleRate()
        {
            return Using<IFnStockRecord>().GetHangSaleRate();
        }

        /// <summary>
        /// 获取股权记录信息
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>股权记录信息</returns>
        public FnStockRecord Get(int sysNo)
        {
            return Using<IFnStockRecord>().Get(sysNo);
        }

        /// <summary>
        /// 获取股权记录列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>股权记录列表</returns>
        public List<FnStockRecord> GetList(StockRecordRequest request)
        {
            var list = Using<IFnStockRecord>().GetList(request);

            return list.ToList();
        }

        /// <summary>
        /// 获取股权记录分页列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>股权记录分页列表</returns>
        public PagedList<FnStockRecord> GetPagerList(StockRecordRequest request)
        {
            return Using<IFnStockRecord>().GetPagerList(request);
        }
    }
}
