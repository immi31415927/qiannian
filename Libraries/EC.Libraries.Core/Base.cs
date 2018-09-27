namespace EC.Libraries.Core
{
	using Autofac;
	using System;

	/// <summary>
    /// Base基类
	/// </summary>
    public class Base<T> where T : Base<T>, new()
	{
		/// <summary>
		/// Application实例
		/// </summary>
		private static T _instance = new T();

		/// <summary>
		/// 返回当前业务逻辑实例
		/// </summary>
		public static T Instance
		{
			get { return _instance; }
		}

		/// <summary>
		/// 使用工作单元
		/// </summary>
		/// <typeparam name="TUnitWork">工作单元类型</typeparam>
		/// <returns></returns>
		protected internal static TUnitWork Using<TUnitWork>()
		{
            return ServiceLocator.WorkerContainer.Resolve<TUnitWork>();
		}

		/// <summary>
		/// 使用工作单元
		/// </summary>
		/// <typeparam name="TUnitWork">工作单元类型</typeparam>
		/// <param name="func">操作</param>
		protected internal static void Using<TUnitWork>(Action<TUnitWork> func)
		{
			func(Using<TUnitWork>());
		}

		/// <summary>
		/// 使用工作单元
		/// </summary>
		/// <typeparam name="TUnitWork">工作单元类型</typeparam>
		/// <typeparam name="TResult">返回类型</typeparam>
		/// <param name="func">操作</param>
		protected internal static TResult Using<TUnitWork, TResult>(Func<TUnitWork, TResult> func)
		{
			return func(Using<TUnitWork>());
		}

		/// <summary>
		/// 使用工作单元
		/// </summary>
		/// <typeparam name="T1UnitWork">工作单元类型</typeparam>
		/// <typeparam name="T2UnitWork">工作单元类型</typeparam>
		/// <typeparam name="TResult">返回类型</typeparam>
		/// <param name="func">操作</param>
		protected internal static TResult Using<T1UnitWork, T2UnitWork, TResult>(Func<T1UnitWork, T2UnitWork, TResult> func)
		{
			return func(Using<T1UnitWork>(), Using<T2UnitWork>());
		}

		/// <summary>
		/// 使用工作单元
		/// </summary>
		/// <typeparam name="T1UnitWork">工作单元类型</typeparam>
		/// <typeparam name="T2UnitWork">工作单元类型</typeparam>
		/// <param name="func">操作</param>
		protected internal static void Using<T1UnitWork, T2UnitWork>(Action<T1UnitWork, T2UnitWork> func)
		{
			func(Using<T1UnitWork>(), Using<T2UnitWork>());
		}
	}
}
