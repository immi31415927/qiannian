using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using EC.Libraries.Core.Log;

namespace EC.WeiXin
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            #region 初始化数据访问对象
            Libraries.Core.ServiceLocator.Initialization();
            #endregion
        }

        /// <summary>
        /// 全局错误定位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Error(object sender, EventArgs e)
        {
            Log4Helper.WriteErrLog("EC.WeiXin 项目启动异常", Server.GetLastError().GetBaseException());
        }

        /// <summary>
        /// EndRequest监听Response状态码
        /// </summary>
        protected void Application_EndReqeust()
        {
            var statusCode = Context.Response.StatusCode;
            var routingData = Context.Request.RequestContext.RouteData;
            {
                Response.Clear();
                Response.RedirectToRoute("Default", new { controller = "Error", action = "Path404" });
            }

        }
    }
}