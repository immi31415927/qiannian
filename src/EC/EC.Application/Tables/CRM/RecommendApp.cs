using System;
using EC.DataAccess.Bs;
using EC.DataAccess.CRM;
using EC.Entity;
using EC.Entity.Parameter.Request.CRM;
using EC.Entity.Tables.Bs;
using EC.Entity.Tables.CRM;
using EC.Entity.View.CRM.Ext;
using EC.Libraries.Core.Pager;
using EC.Libraries.Util;

namespace EC.Application.Tables.CRM
{
    using EC.Libraries.Core;

    /// <summary>
    /// 日志业务层
    /// </summary>
    public class RecommendApp : Base<RecommendApp>
    {
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>系统编号</returns>
        public JResult<int> Insert(CrRecommend model)
        {
            var result = new JResult<int>();

            try
            {
                model.CreatedDate = DateTime.Now;
                var row = Using<ICrRecommend>().Insert(model);

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
        /// 获取
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        public CrRecommend Get(int sysNo)
        {
            var result = Using<ICrRecommend>().Get(sysNo);

            return result;
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="openId">openId</param>
        public CrRecommend GetByopenId(string openId)
        {
            var result = Using<ICrRecommend>().GetByopenId(openId);
            return result;
        }

        /// <summary>
        /// 获取关注扩展信息分页列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>关注扩展信息分页列表</returns>
        public PagedList<RecommendExt> GetExtPagerList(RecommendExtRequest request)
        {
            return Using<ICrRecommend>().GetExtPagerList(request);
        }
    }
}
