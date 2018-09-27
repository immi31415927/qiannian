using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EC.DataAccess.CRM;
using EC.Entity.Parameter.Request.CRM;
using EC.Entity.Tables.CRM;
using EC.Entity.View.CRM.Ext;
using EC.Libraries.Core.Pager;

namespace EC.DataAccess.MySql.CRM
{
    using EC.Libraries.Core.Data;

    /// <summary>
    /// 推荐数据访问接口实现
    /// </summary>
    public class CrRecommendImpl : Base, ICrRecommend
    {
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model">数据模型</param>
        public int Insert(CrRecommend model)
        {
            var result = DBContext.Insert<CrRecommend>("agent_crrecommend", model)
                                .AutoMap(x => x.SysNo)
                                .ExecuteReturnLastId<int>("sysNo");
            return result;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">数据模型</param>
        public int Update(CrRecommend model)
        {
            var rowsAffected = DBContext.Update<CrRecommend>("agent_crrecommend", model)
                                      .AutoMap(x => x.SysNo)
                                      .Where(x => x.SysNo)
                                      .Execute();
            return rowsAffected;
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        public CrRecommend Get(int sysNo)
        {
            var sql = "select * from agent_crrecommend where sysNo=@sysNo;";

            var result = DBContext.Sql(sql)
                                .Parameter("sysNo", sysNo)
                                .QuerySingle<CrRecommend>();
            return result;
        }
        
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="openId">openId</param>
        public CrRecommend GetByopenId(string openId)
        {
            return DBContext.Sql("select * from agent_crrecommend where openId=@openId;")
                            .Parameter("openId", openId)
                            .QuerySingle<CrRecommend>();
        }

        /// <summary>
        /// 获取关注扩展信息分页列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>关注扩展信息分页列表</returns>
        public PagedList<RecommendExt> GetExtPagerList(RecommendExtRequest request)
        {
            const string sqlFrom = "agent_crrecommend r LEFT JOIN crcustomer c on r.Openid = c.OpenId";

            var dataCount = DBContext.Select<int>("count(0)").From(sqlFrom);
            var dataList = DBContext.Select<RecommendExt>("r.*,c.Grade,c.PhoneNumber as TelNumber,CASE WHEN c.SysNo IS NULL THEN 0 ELSE 1 END as IsRegister").From(sqlFrom);

            Action<string, string, object> setWhere = (@where, name, value) =>
            {
                dataCount.AndWhere(where).Parameter(name, value);
                dataList.AndWhere(where).Parameter(name, value);
            };

            if (request.ReferrerSysNo.HasValue)
            {
                setWhere("r.ReferrerSysNo = @ReferrerSysNo", "ReferrerSysNo", request.ReferrerSysNo.Value);
            }

            if (!string.IsNullOrEmpty(request.TelNumber))
            {
                setWhere("u.tel = @tel", "tel", request.TelNumber);
            }

            return new PagedList<RecommendExt>
            {
                TData = dataList.Paging(request.CurrentPageIndex.GetHashCode(), request.PageSize.GetHashCode()).OrderBy("r.CreatedDate desc").QueryMany(),
                CurrentPageIndex = request.CurrentPageIndex.GetHashCode(),
                TotalCount = dataCount.QuerySingle()
            };
        }
    }
}
