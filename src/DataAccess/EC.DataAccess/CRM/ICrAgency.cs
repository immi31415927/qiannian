using System;
using System.Collections.Generic;
using EC.Entity.Parameter.Request.CRM;
using EC.Entity.Tables.CRM;
using EC.Libraries.Core.Pager;

namespace EC.DataAccess.CRM
{
    /// <summary>
    /// 代理审核数据访问接口
    /// </summary>
    public interface ICrAgency
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        CrAgency Get(int sysNo);

        /// <summary>
        /// 获取会员扩展信息分页列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>会员扩展信息分页列表</returns>
        PagedList<CrAgency> GetPagerList(CrAgencyQueryRequest request);
    }
}
