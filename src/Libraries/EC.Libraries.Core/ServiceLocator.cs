namespace EC.Libraries.Core
{
    using Autofac;
    using System.Reflection;

    /// <summary>
    /// 启动程序
    /// </summary>
    public class ServiceLocator
    {
        /// <summary>
        /// IoC容器
        /// </summary>
        internal static IContainer WorkerContainer = null;

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Initialization()
        {
            if (WorkerContainer == null)
            {
                var builder = new ContainerBuilder();
                var mysqlImpl = Assembly.Load("EC.DataAccess.MySql");

                builder.RegisterAssemblyTypes(mysqlImpl)
                    .Where(m => m.Name.EndsWith("Impl"))
                    .AsImplementedInterfaces()
                    .SingleInstance();

                WorkerContainer = builder.Build();
            }
        }
    }
}
