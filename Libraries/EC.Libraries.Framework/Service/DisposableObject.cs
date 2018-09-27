using System;

namespace EC.Libraries.Framework.Service
{
    /// <summary>
    /// 表示派生类是一次性的对象
    /// </summary>
    /// <Remark>苟治国 创建</Remark>
    public abstract class DisposableObject : IDisposable
    {
        #region Finalization Constructs
        /// <summary>
        /// Finalizes the object.
        /// </summary>
        ~DisposableObject()
        {
            this.Dispose(false);
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Disposes the object.
        /// </summary>
        protected abstract void Dispose(bool disposing);

        /// <summary>
        /// 显示强制回收不被使用的对象
        /// </summary>
        protected void ExplicitDispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// 执行应用程序定义的任务相关联的释放,或非托管资源的重置
        /// </summary>
        public void Dispose()
        {
            this.ExplicitDispose();
        }
        #endregion
    }
}
