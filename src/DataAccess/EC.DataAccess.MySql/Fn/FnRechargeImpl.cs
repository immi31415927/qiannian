using EC.DataAccess.Fn;
using EC.Entity.Enum;
using EC.Entity.Parameter.Request.Finance;
using EC.Entity.Tables.Finance;
using EC.Libraries.Core.Data;

namespace EC.DataAccess.MySql.Fn
{
    using EC.Libraries.Core;

    /// <summary>
    /// 充值日志数据访问接口实现
    /// </summary>
    public class FnRechargeImpl : Base, IFnRecharge
    {
        #region 常量
        private const string StrTableName = "agent_fnrecharge";
        #endregion

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        public FnRecharge Get(int sysNo)
        {
            var sql = "select * from agent_fnrecharge where sysNo=@sysNo;";

            var result = DBContext.Sql(sql)
                                .Parameter("sysNo", sysNo)
                                .QuerySingle<FnRecharge>();
            return result;
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="orderNo">序列号</param>
        /// <returns></returns>
        public FnRecharge GetbyOrderNo(string orderNo)
        {
            var sql = "select * from agent_fnrecharge where orderNo=@orderNo;";

            var result = DBContext.Sql(sql)
                                .Parameter("orderNo", orderNo)
                                .QuerySingle<FnRecharge>();
            return result;
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>系统编号</returns>
        public int Insert(FnRecharge model)
        {
            return DBContext.Insert(StrTableName, model)
                            .AutoMap(x => x.SysNo)
                            .ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// 更新订单状态
        /// </summary>
        /// <returns></returns>
        public int UpdatePayStatus(UpdatePayStatusRequest request)
        {
            return DBContext.Sql(string.Format("update agent_fnrecharge set Status={0} where OrderNo='{1}'", PayStatus.已支付.GetHashCode(), request.OrderSysNo)).Execute();
        }
    }
}
