using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.DataAccess.CRM
{
    using EC.Entity.Parameter.NewCustomer;
    using EC.Entity.Parameter.Request.CRM;
    using EC.Entity.Tables.CRM;

    /// <summary>
    /// 新会员数据接口
    /// </summary>
    public interface INewCustomer
    {
        /// <summary>
        /// 获取会员
        /// </summary>
        /// <param name="sysno">系统编号</param>
        /// <returns></returns>
        CrCustomer Get(int sysno);

        /// <summary>
        /// 获取会员列表
        /// </summary>
        /// <returns>会员扩展信息列表</returns>
        IList<CrCustomer> GetAll();

                /// <summary>
        /// 批量更新父级奖励
        /// </summary>
        /// <param name="batchUpgradeParentList">参数</param>
        /// <returns>影响行数</returns>
        int BatchUpdate(List<BatchUpgradeParent> batchUpgradeParentList);

        /// <summary>
        /// 将待结算金额写入钱包
        /// </summary>
        int UpdatePendingAmount(int sysNo, int grade);

        /// <summary>
        /// 更新用户等级
        /// </summary>
        int UpdateGrade(CrCustomer model);
    }
}
