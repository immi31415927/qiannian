using System;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Routing;
using EC.Application.Tables.Bs;
using EC.Application.Tables.CRM;
using EC.Entity;
using EC.Entity.Enum;
using EC.Entity.Parameter.Request.Bs;
using EC.Entity.Tables.CRM;
using EC.Libraries.Core.Log;

namespace EC.H5.Controllers
{
    #region 基础控制器
    /// <summary>
    /// 基础控制器
    /// </summary>
    [AuthorizeFilter(true)]
    public class BaseController : Controller
    {
        /// <summary>
        /// 接管系统异常,自定义处理程序
        /// </summary>
        /// <param name="exceptionContext">异常内容</param>
        protected override void OnException(ExceptionContext exceptionContext)
        {
            exceptionContext.ExceptionHandled = true;

            var result = new JResult()
            {
                Status = false
            };

            var ex = exceptionContext.Exception;

            //记录日志
            try
            {
                LogApp.Instance.Error(new LogRequest()
                {
                    Source = LogEnum.Source.前台,
                    Message = "系统日志",
                    Exception = ex
                });
            }
            catch (Exception e)
            {
                Log4Helper.WriteErrLog("BaseController->OnException日志记录失败", e);
            }
        }

        /// <summary>
        /// 重写action执行方法
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!CustomerContext.IsLogon)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new JsonResult
                    {
                        Data = new
                        {
                            IsLogout = true,
                            Message = "登录超时，请重新登录",
                        },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
                else
                {
                    filterContext.Result = new ViewResult { ViewName = "ErrorLogin" };
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
    #endregion

    #region 用户上下文信息
    /// <summary>
    /// 用户上下文信息
    /// </summary>
    public static class CustomerContext
    {
        /// <summary>
        /// 用户是否登录
        /// </summary>
        public static bool IsLogon
        {
            get
            {
                return AuthApp.Instance.IsLogin;
            }
        }

        /// <summary>
        /// 登录用户信息
        /// </summary>
        public static CrCustomer Context
        {
            get
            {
                return AuthApp.Instance.CurrentCustomer;
            }
        }
    }
    #endregion

    #region 认证用户筛选
    /// <summary>
    /// 认证用户筛选
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class AuthorizeFilter : ActionFilterAttribute
    {
        /// <summary>  
        ///是否登录
        /// </summary>
        public bool IsFilter { get; set; }

        ///// <summary>
        ///// 是否检查登录
        ///// </summary>
        ///// <param name="isFilter">false:不检查true:检查</param>
        public AuthorizeFilter(bool isFilter = true)
        {
            this.IsFilter = isFilter;
        }

        /// <summary>
        /// 拦截未登录的用户
        /// 使其跳转到登录页面
        /// </summary>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            #region 不需要验证登录
            if (!IsFilter)
            {
                base.OnActionExecuting(filterContext);
                return;
            }
            #endregion

            if (!Controllers.CustomerContext.IsLogon)
            {
                filterContext.Result = new RedirectResult("/account/login?returnUrl=" + HttpUtility.UrlEncode(HttpContext.Current.Request.RawUrl));
            }
            else
            {
                base.OnActionExecuting(filterContext);
            }
        }
    }
    #endregion
}
