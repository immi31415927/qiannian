using System;

namespace EC.Libraries.Core.Transaction
{
    /// <summary>
    /// 事务接口
    /// </summary>
    public interface ITransaction:IDisposable
    {
        /// <summary>
        /// 提交事务
        /// </summary>
        void Commit();

        /// <summary>
        /// 回滚事务
        /// </summary>
        void Rollback();
    }
}