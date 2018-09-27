using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EC.Entity.Parameter.Request.CRM;
using EC.Entity.Tables.CRM;
using EC.Entity.View.CRM.Ext;
using EC.Libraries.Core.Pager;

namespace EC.DataAccess.CRM
{
    /// <summary>
    /// 推荐数据访问接口
    /// </summary>
    public interface ICrRecommend
    {
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model">数据模型</param>
        int Insert(CrRecommend model);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">数据模型</param>
        int Update(CrRecommend model);

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        CrRecommend Get(int sysNo);

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="openId">openId</param>
        CrRecommend GetByopenId(string openId);

        /// <summary>
        /// 获取关注扩展信息分页列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>关注扩展信息分页列表</returns>
        PagedList<RecommendExt> GetExtPagerList(RecommendExtRequest request);
    }
}
