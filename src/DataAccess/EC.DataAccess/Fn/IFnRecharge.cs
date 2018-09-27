using EC.Entity.Parameter.Request.Finance;
using EC.Entity.Tables.Finance;

namespace EC.DataAccess.Fn
{
    /// <summary>
    /// 充值日志数据访问接口
    /// </summary>
    public interface IFnRecharge
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        FnRecharge Get(int sysNo);

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="orderNo">序列号</param>
        /// <returns></returns>
        FnRecharge GetbyOrderNo(string orderNo);

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>系统编号</returns>
        int Insert(FnRecharge model);

        /// <summary>
        /// 更新订单状态
        /// </summary>
        /// <returns></returns>
        int UpdatePayStatus(UpdatePayStatusRequest request);
    }
}
