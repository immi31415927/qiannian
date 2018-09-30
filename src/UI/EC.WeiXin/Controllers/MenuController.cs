using System;
using System.Configuration;
using System.Web.Mvc;
using EC.Libraries.WeiXin;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Entities.Menu;

namespace EC.WeiXin.Controllers
{
    public class MenuController : Controller
    {
        //
        // GET: /Menu/

        /// <summary>
        /// 微信公众号WeixinAppId
        /// </summary>
        private readonly string _weixinAppId = ConfigurationManager.AppSettings["WeixinAppId"];

        /// <summary>
        /// 微信公众号AppSecret
        /// </summary>
        private readonly string _weixinAppSecret = ConfigurationManager.AppSettings["WeixinAppSecret"];

        /// <summary>
        /// 创建微信菜单（开发者模式使用）
        /// </summary>
        /// <returns>视图</returns>
        public ActionResult Index()
        {
            #region 菜单信息

            //var root1 = new MenuFull_RootButton { name = "千年大计" };

            //var root101 = new MenuFull_RootButton
            //{
            //    name = "公司介绍",
            //    type = "view",
            //    url = "http://www.1000n.com/about.html"
            //};
            //var root102 = new MenuFull_RootButton
            //{
            //    name = "软件下载",
            //    type = "view",
            //    url = "http://www.1000n.com"
            //};
            //var root103 = new MenuFull_RootButton
            //{
            //    name = "企业文化",
            //    type = "view",
            //    key = ""
            //};

            //root1.sub_button = new List<MenuFull_RootButton> { root101, root102, root103 };

            //var getMenuResultFull = new GetMenuResultFull
            //{
            //    menu = new MenuFull_ButtonGroup
            //    {
            //        button = new List<MenuFull_RootButton> { root1 }
            //    }
            //};

            ////获取微信 AccessToken
            //var accessToken = WeiXinHelper.GetAccessToken(_weixinAppId, _weixinAppSecret);


            //DeleteMenu(accessToken.access_token);
            //CreateMenu(accessToken.access_token, getMenuResultFull);

            #endregion
            WeiXinProvider wx = new WeiXinProvider();

            var accessTokenResponse = wx.GetAccessToken("wx9535283296d186c3", "543e75c11909f438c02870ecbab85f5d");
            if (accessTokenResponse.return_code.Equals(return_code.SUCCESS.ToString()) && accessTokenResponse.result_code.Equals(result_code.SUCCESS.ToString()))
            {
                var menu = @"
                {
                     ""button"":[
                      {
                           ""name"":""千年大计"",
                           ""sub_button"":[
                            {
                               ""type"":""view"",
                               ""name"":""公司介绍"",
                               ""url"":""http://www.1000n.com/about.html""
                            },
                            {
                               ""type"":""view"",
                               ""name"":""软件下载"",
                               ""url"":""http://www.1000n.com""
                            },
                            {
                               ""type"":""view"",
                               ""name"":""企业文化"",
                               ""key"":""http://www.1000n.com""
                            }]
                      }
                ]}";
                var createMenuResponse = wx.CreateMenu(accessTokenResponse.access_token, menu);
            }
            ViewBag.accessToken = accessTokenResponse.access_token;


            var accessToken = wx.GetAccessToken(_weixinAppId, _weixinAppSecret);

            ViewBag.accessToken = accessToken;
            return View();
        }

        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <param name="token">微信AccessToken</param>
        /// <param name="resultFull">菜单结构</param>
        private void CreateMenu(string token, GetMenuResultFull resultFull)
        {
            try
            {
                //重新整理按钮信息
                var bg = Senparc.Weixin.MP.CommonAPIs.CommonApi.GetMenuFromJsonResult(resultFull, new ButtonGroup()).menu;
                var result = Senparc.Weixin.MP.CommonAPIs.CommonApi.CreateMenu(token, bg);
            }
            catch (Exception ex)
            {

            }
        }

        private void DeleteMenu(string token)
        {
            try
            {
                var result = Senparc.Weixin.MP.CommonAPIs.CommonApi.DeleteMenu(token);
                var json = new
                {
                    Success = result.errmsg == "ok",
                    Message = result.errmsg
                };
            }
            catch (Exception ex)
            {
                var json = new { Success = false, Message = ex.Message };
            }
        }

        public ActionResult GetMenu(string token)
        {
            var result = Senparc.Weixin.MP.CommonAPIs.CommonApi.GetMenu(token);
            if (result == null)
            {
                return Json(new { error = "菜单不存在或验证失败！" }, JsonRequestBehavior.AllowGet);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
