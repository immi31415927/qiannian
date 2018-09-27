using System;
using System.Collections.Generic;
using System.Linq;
using EC.DataAccess.Fn;
using EC.Entity;
using EC.Entity.Parameter.Request.Fn;
using EC.Entity.Tables.Finance;
using EC.Entity.View.Fn.Ext;
using EC.Libraries.Core;
using EC.Libraries.Core.Pager;

namespace EC.Application.Tables.Fn
{
    /// <summary>
    /// 交易日志业务层
    /// </summary>
    public class TradeLogApp : Base<TradeLogApp>
    {
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>系统编号</returns>
        public JResult<int> Insert(FnTradeLog model)
        {
            var result = new JResult<int>();

            try
            {
                var row = Using<IFnTradeLog>().Insert(model);
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
        /// 批量添加
        /// </summary>
        /// <param name="list">列表</param>
        /// <returns>影响行数</returns>
        public JResult<int> BatchInsert(List<FnTradeLog> list)
        {
            var result = new JResult<int>();

            try
            {
                var row = Using<IFnTradeLog>().BatchInsert(list);
                if (row <= 0)
                {
                    throw new Exception("添加数据失败");
                }

                result.Status = true;
                result.Data = row;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message; 
            }

            return result;
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>影响行数</returns>
        public JResult Update(FnTradeLog model)
        {
            var result = new JResult();

            try
            {
                var row = Using<IFnTradeLog>().Update(model);
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
        /// 获取交易日志信息
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>交易日志信息</returns>
        public FnTradeLog Get(int sysNo)
        {
            return Using<IFnTradeLog>().Get(sysNo);
        }

        /// <summary>
        /// 获取交易日志列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>交易日志列表</returns>
        public List<FnTradeLog> GetList(TradeLogRequest request)
        {
            var list = Using<IFnTradeLog>().GetList(request);

            return list.ToList();
        }

        /// <summary>
        /// 获取交易日志分页列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>交易日志分页列表</returns>
        public PagedList<TradeLogExt> GetPagerList(TradeLogRequest request)
        {
            return Using<IFnTradeLog>().GetPagerList(request);
        }
    }
}
