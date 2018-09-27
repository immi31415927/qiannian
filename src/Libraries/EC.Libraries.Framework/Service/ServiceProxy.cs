using System.ServiceModel;

namespace EC.Libraries.Framework.Service
{
    /// <summary>
    /// 表示用于调用WCF服务的会员端服务代理类型。
    /// </summary>
    /// <typeparam name="T">需要调用的服务契约类型。</typeparam>
    /// <remarks>苟治国 创建</remarks>
    public sealed class ServiceProxy<T> : DisposableObject where T : class
    {
        #region Private Fields
        private T client = null;
        private static readonly object sync = new object();
        #endregion

        #region Protected Methods
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                lock (sync)
                {
                    Close();
                }
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// 获取调用WCF服务的通道。
        /// </summary>
        public T Channel
        {
            get
            {
                lock (sync)
                {
                    if (client != null)
                    {
                        var state = (client as IClientChannel).State;
                        if (state == CommunicationState.Closed)
                            client = null;
                        else
                            return client;
                    }
                    var factory = ChannelFactoryManager.Instance.GetFactory<T>();
                    client = factory.CreateChannel();
                    (client as IClientChannel).Open();
                    return client;
                }
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// 关闭并断开会员端通道（Client Channel）。
        /// </summary>
        /// <remarks>
        /// 如果使用using语句对ServiceProxy进行了包裹，那么当程序执行点离开using的
        /// 覆盖范围时，Close方法会被自动调用，此时会员端无需显式调用Close方法。反之
        /// 如果没有使用using语句，那么则需要显式调用Close方法。
        /// </remarks>
        public void Close()
        {
            if (client != null)
                ((IClientChannel)client).Close();
        }
        #endregion
    }
}
