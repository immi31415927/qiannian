using System.Transactions;

namespace EC.Libraries.Core.Transaction
{
    /// <summary>
    /// TransactionScope 适配器
    /// </summary>
    public class TransactionScopeAdapter : ITransaction
    {
        private TransactionScope tran = null;
        public TransactionScopeAdapter(System.Data.IsolationLevel level = System.Data.IsolationLevel.Serializable)
        {
            tran = new TransactionScope(); 
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public void Commit()
        {
            tran.Complete();
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public void Rollback()
        {
            tran.Dispose();
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public void Dispose()
        {
            tran.Dispose();
        }
    }
}
