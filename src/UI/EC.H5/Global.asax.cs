using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using EC.Libraries.Core.Log;
using Senparc.Weixin.MP.TenPayLibV3;

namespace EC.H5
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
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            #region 初始化数据访问对象
            Libraries.Core.ServiceLocator.Initialization();
            #endregion

            var mchId = System.Configuration.ConfigurationManager.AppSettings["mchId"];
            var appId = System.Configuration.ConfigurationManager.AppSettings["appid"];
            var appSecret = System.Configuration.ConfigurationManager.AppSettings["appSecret"];
            var apiKey = System.Configuration.ConfigurationManager.AppSettings["apiKey"];
            var notifyUrl = System.Configuration.ConfigurationManager.AppSettings["notifyUrl"];

            var tenPayV3Info = new TenPayV3Info(appId, appSecret, mchId, apiKey, notifyUrl);
            TenPayV3InfoCollection.Register(tenPayV3Info);
        }

        /// <summary>
        /// 全局错误定位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Error(object sender, EventArgs e)
        {
            Log4Helper.WriteErrLog("EC.H5 项目启动异常", Server.GetLastError().GetBaseException());
        }
    }
}