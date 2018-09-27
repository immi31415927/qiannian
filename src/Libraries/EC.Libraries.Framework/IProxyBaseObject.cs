using System;

namespace EC.Libraries.Framework
{
    /// <summary>
    /// 所有需要使用ClientProxy调用的类库都必须要继承的基础接口
    /// </summary>
    public interface IProxyBaseObject<T> : IDisposable
    {
        /// <summary>
        /// 获取所需的基础调用实体
        /// </summary>
        /// <returns></returns>
        T Instance{ get; }

        /// <summary>
        /// 实现初始化配置工作，如载入配置数据
        /// </summary>
        void Initialize(BaseConfig config = null);
    }
}
