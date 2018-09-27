using System;

namespace EC.Libraries.Core.Transaction
{
    /// <summary>
    /// 事务提供器
    /// </summary>
    public class TransactionProvider:IDisposable
    {
        private ITransaction tran = null;

        /// <summary>
        /// 初始化事务提供器
        /// </summary>
        /// <param name="level">事务隔离级别</param>
        public TransactionProvider(System.Data.IsolationLevel level = System.Data.IsolationLevel.ReadCommitted)
        {
            // 默认事务类型为TransactionLocal
            var type = TransactionType.TransactionLocal;
            switch (type)
            {
                case TransactionType.TransactionScope :
                    tran = new TransactionScopeAdapter(level);
                    break;
                case TransactionType.TransactionLocal:
                    tran = new TransactionLocal(level);
                    break; 
                default:
                    tran = new TransactionLocal(level);
                    break;
            }
           
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public void Commit()
        {
            tran.Commit();
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public void Rollback()
        {
            tran.Rollback();
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public void Dispose()
        {
            tran.Dispose();
        }
    }

    /// <summary>
    /// 事务类型
    /// </summary>
    public enum TransactionType
    {
        /// <summary>
        /// windows事务
        /// </summary>
        TransactionScope,

        /// <summary>
        /// 本地自定义事务
        /// </summary>
        TransactionLocal
    }
}
