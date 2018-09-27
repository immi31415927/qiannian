using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EC.DataAccess.Fn;
using EC.Entity.Parameter.Request.Fn;
using EC.Entity.Tables.Finance;
using EC.Entity.View.Fn;
using EC.Entity.View.Fn.Ext;
using EC.Libraries.Core.Pager;

namespace EC.Application.Tables.Fn
{
    using EC.Libraries.Core;
    using EC.Libraries.Util;

    /// <summary>
    /// 奖金记录业务层
    /// </summary>
    public class FnBonusLogApp : Base<FnBonusLogApp>
    {
        /// <summary>
        /// 获取奖金记录分页列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>奖金记录分页列表</returns>
        public PagedList<FnBonusLog> GetPagerList(FnBonusLogQueryRequest request)
        {
            return Using<IFnBonusLog>().GetPagerList(request);
        }

        /// <summary>
        /// 获取奖金扩展记录分页列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>奖金扩展记录分页列表</returns>
        public PagedList<BonusLogExt> GetPagerExtList(FnBonusLogQueryRequest request)
        {
            string[] letterArrStr = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            var pagedList = Using<IFnBonusLog>().GetPagerExtList(request);

            if (pagedList.TData == null || !pagedList.TData.Any()) return pagedList;

            foreach (var item in pagedList.TData.Where(item => item.SourceSysNo <= 25))
            {
                var level = item.SourceLevel - 1;
                item.SourceLevelName = level >= 0 ? letterArrStr[level] : "";
            }

            return pagedList;
        }

        /// <summary>
        /// 后台获取奖金记录查询
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>后台获取奖金分页列表</returns>
        public PagedList<BonusLogResponse> GetAdminPager(BonusLogAdminQueryRequest request)
        {
            return Using<IFnBonusLog>().GetAdminPager(request);
        }

        /// <summary>
        /// 获取奖金总额
        /// </summary>
        /// <param name="customerSysNo"></param>
        /// <returns></returns>
        public int GetBonusSum(int customerSysNo)
        {
            return Using<IFnBonusLog>().GetBonusSum(customerSysNo);
        }
    }
}
