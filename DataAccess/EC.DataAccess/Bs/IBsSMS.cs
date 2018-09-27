using System.Collections.Generic;
using EC.Entity.Parameter.Request.SMS;
using EC.Entity.Tables.Bs;

namespace EC.DataAccess.Bs
{
    using EC.Libraries.Core.Pager;

    /// <summary>
    /// 短信数据访问接口
    /// </summary>
    public interface ISMS
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        BsSMS Get(int sysNo);

        /// <summary>
        /// 查询某个时间段发送短信数量
        /// </summary>
        /// <param name="requeest">参数</param>
        List<BsSMS> GetSendingTimes(SMSQueryRequeest requeest);

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model">数据模型</param>
        /// <returns>影响行数</returns>
        int Insert(BsSMS model);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">数据模型</param>
        /// <returns>影响行数</returns>
        int Update(BsSMS model);

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="sysNo">编码</param>
        /// <param name="status">状态</param>
        /// <returns>影响行数</returns>
        int UpdateStatus(int sysNo, int status);

        /// <summary>
        /// 获取角色列表
        /// </summary>
        PagedList<BsSMS> GetPaging(SMSQueryRequeest requeest);
    }
}
