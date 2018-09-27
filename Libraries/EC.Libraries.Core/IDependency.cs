namespace EC.Libraries.Core
{
    /// <summary>
    /// 可依赖注入的基础接口定义
    /// </summary>
    public interface IDependency
    {
    }

    /// <summary>
    /// 单例
    /// </summary>
    public interface ISingletonDependency : IDependency
    {
    }

    /// <summary>
    /// 每次使用的时候都实例化
    /// </summary>
    public interface ITransientDependency : IDependency
    {
    }

}
