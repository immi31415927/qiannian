using System;
using System.Threading;
using EC.Libraries.Core.Data;

namespace EC.Libraries.Core.Transaction
{
    /// <summary>
    /// 本地自定义事务
    /// </summary>
    internal sealed class TransactionLocal : ITransaction
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private const string DbConnectionString = "MySqlConnectionString";

        /// <summary>
        /// 当前数据库上下文
        /// </summary>
        private static ThreadLocal<IDbContext> DbContex = new ThreadLocal<IDbContext>();

        /// <summary>
        /// 判断当前事务是否是最外层事务
        /// </summary>
        private static ThreadLocal<bool> IsParentTran = new ThreadLocal<bool>(() => false);

        /// <summary>
        /// 当前事务是否中止
        /// </summary>
        private static ThreadLocal<bool> IsAbortTran = new ThreadLocal<bool>(() => false);

        /// <summary>
        /// 当前事务ID
        /// </summary>
        private string TransactionId
        {
            get { return Guid.NewGuid().ToString(); }
        }

        /// <summary>
        /// 当前事务是否已经提交
        /// </summary>
        public bool IsCommited { get; private set; }

        /// <summary>
        /// 是否可以提交
        /// </summary>
        private bool _isCommit = true;

        /// <summary>
        /// 获取当前托管线程的唯一标识符
        /// </summary>
        public static string GetCurrentThreadId
        {
            get { return Thread.CurrentThread.ManagedThreadId.ToString(); }
        }

        /// <summary>
        /// 初始构造函数
        /// </summary>
        /// <param name="level">事务隔离级别</param>
        public TransactionLocal(System.Data.IsolationLevel level = System.Data.IsolationLevel.ReadCommitted)
        {
            if (IsAbortTran.Value)
            {
                throw new System.Transactions.TransactionAbortedException();
            }
            IsCommited = false;
            //判断当前事务是否可以提交
            if (IsParentTran.Value)
            {
                _isCommit = false;
            }
            else
            {
                Initialize(level);
                IsParentTran.Value = true;
            }
        }

        /// <summary>
        /// 创建当前线程的事务连接
        /// </summary>
        /// <returns></returns>
        public static IDbContext CreateDbContext()
        {
            IDbContext context = DbContex.Value;
            if (context == null)
            {
                context = new DbContext().ConnectionStringName(DbConnectionString);
                context.IgnoreIfAutoMapFails(true);
                DbContex.Value = context;
            }
            return context;
        }

        /// <summary>
        /// 初始化事务
        /// </summary>
        /// <param name="level"></param>
        private void Initialize(System.Data.IsolationLevel level)
        {
            IDbContext context = CreateDbContext();
            context.UseTransaction(true);
            context.Data.IsolationLevel = (IsolationLevel)level;
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public void Commit()
        {
            if (_isCommit && !IsAbortTran.Value)
            {
                DbContex.Value.Commit();
            }
            IsCommited = true;
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public void Rollback()
        {
            if (_isCommit)
                Dispose();
            else
            {
                IsAbortTran.Value = true;
            }
        }

        /// <summary>
        /// 资源释放
        /// </summary>
        public void Dispose()
        {
            if (!IsCommited)
                IsAbortTran.Value = true;
            if (_isCommit)
            {
                DbContex.Value.Dispose();
                DbContex.Value.UseTransaction(false);
                IsParentTran.Value = false;
                IsAbortTran.Value = false;
            }
        }
    }

    public class Db
    {
        /// <summary>
        /// 创建当前线程的事务连接
        /// </summary>
        /// <returns></returns>
        public static IDbContext CreateDbContext()
        {
            return TransactionLocal.CreateDbContext();
        }
    }
}
