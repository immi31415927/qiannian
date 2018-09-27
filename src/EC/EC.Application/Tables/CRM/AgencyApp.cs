using System;
using EC.DataAccess.CRM;
using EC.Entity.Parameter.Request.CRM;
using EC.Entity.Tables.CRM;
using EC.Libraries.Core.Pager;

namespace EC.Application.Tables.CRM
{
    using EC.Libraries.Core;
    using EC.Libraries.Util;

    /// <summary>
    /// 代理审核 业务层
    /// </summary>
    public class AgencyApp : Base<AgencyApp>
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        public CrAgency Get(int sysNo)
        {
            return Using<ICrAgency>().Get(sysNo);
        }

        /// <summary>
        /// 获取奖金记录分页列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>奖金记录分页列表</returns>
        public PagedList<CrAgency> GetPagerList(CrAgencyQueryRequest request)
        {
            return Using<ICrAgency>().GetPagerList(request);
        }
    }
}
