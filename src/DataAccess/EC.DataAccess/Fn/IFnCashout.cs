using System;
using EC.Entity.Parameter.Request.Fn;
using EC.Entity.Tables.Finance;
using EC.Libraries.Core.Pager;
using System.Collections.Generic;

namespace EC.DataAccess.Fn
{
    /// <summary>
    /// 提现数据访问接口
    /// </summary>
    public interface IFnCashout
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        FnCashout Get(int sysNo);

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model">数据模型</param>
        int Insert(FnCashout model);

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="sysNo">编码</param>
        /// <param name="status">状态</param>
        /// <returns>影响行数</returns>
        int UpdateStatus(int sysNo, int status, string reject);

        /// <summary>
        /// 查询某个时间段提现记录
        /// </summary>
        /// <param name="requeest">参数</param>
        List<FnCashout> GetPeriodTimes(FnCashoutQueryRequest requeest);

        /// <summary>
        /// 获取会员扩展信息分页列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>会员扩展信息分页列表</returns>
        PagedList<FnCashout> GetPagerList(FnCashoutQueryRequest request);
    }
}
