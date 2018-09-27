using System;
using System.Collections.Generic;
using EC.Entity.Parameter.Request.Fn;
using EC.Entity.Tables.Finance;
using EC.Entity.View.Fn;
using EC.Entity.View.Fn.Ext;
using EC.Libraries.Core.Pager;

namespace EC.DataAccess.Fn
{
    /// <summary>
    /// 奖金记录数据访问接口
    /// </summary>
    public interface IFnBonusLog
    {
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>系统编号</returns>
        int Insert(FnBonusLog model);

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        FnBonusLog Get(int sysNo);

        /// <summary>
        /// 获取会员扩展信息分页列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>会员扩展信息分页列表</returns>
        PagedList<FnBonusLog> GetPagerList(FnBonusLogQueryRequest request);

        /// <summary>
        /// 获取奖金扩展记录分页列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>奖金扩展记录分页列表</returns>
        PagedList<BonusLogExt> GetPagerExtList(FnBonusLogQueryRequest request);

        /// <summary>
        /// 后台获取奖金记录查询
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>后台获取奖金分页列表</returns>
        PagedList<BonusLogResponse> GetAdminPager(BonusLogAdminQueryRequest request);
        
        /// <summary>
        /// 批量添加奖金日志
        /// </summary>
        /// <param name="batchInsertList">参数</param>
        /// <returns>影响行数</returns>
        int BatchInsert(List<FnBonusLog> batchInsertList);

        /// <summary>
        /// 获取奖金总额
        /// </summary>
        /// <param name="customerSysNo"></param>
        /// <returns></returns>
        int GetBonusSum(int customerSysNo);
    }
}
