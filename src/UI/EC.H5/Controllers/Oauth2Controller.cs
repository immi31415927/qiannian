using EC.Libraries.Core.Log;
using EC.Libraries.Util;
using Senparc.Weixin;
using Senparc.Weixin.Exceptions;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EC.H5.Controllers
{
    public class Oauth2Controller : Controller
    {
        private string appKey = ConfigurationManager.AppSettings["appid"];
        private string appSecret = ConfigurationManager.AppSettings["appSecret"];

        /// <summary>
        /// 微信授权
        /// </summary>
        /// <returns></returns>
        /// <param name="account">账号</param>
        public ActionResult Login(string account)
        {
            var redirectUrl = ConfigurationManager.AppSettings["redirectUrl"];

            var url = OAuthApi.GetAuthorizeUrl(appKey, redirectUrl, account, OAuthScope.snsapi_userinfo);
            return Redirect(url);
        }

        /// <summary>
        /// OAuthScope.snsapi_userinfo方式回调
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns>视图</returns>
        public ActionResult Index(string code, string state)
        {
            Log4Helper.WriteInfoLog("账号：" + code, "Oauth2");

            if (string.IsNullOrEmpty(code))
            {
                return Content("您拒绝了授权！");
            }
            if (string.IsNullOrEmpty(state))
            {
                return Content("验证失败！请从正规途径进入！");
            }
            var result = OAuthApi.GetAccessToken(appKey, appSecret, code);
            if (result.errcode != ReturnCode.请求成功)
            {
                return Content("错误：" + result.errmsg);
            }

            try
            {
                //因为这里还不确定用户是否关注本微信，所以只能试探性地获取一下
                var userInfo = OAuthApi.GetUserInfo(result.access_token, result.openid);
                if (userInfo != null)
                {
                    //用户信息
                    var openId = userInfo.openid;
                    var nickname = userInfo.nickname;
                    var headimgurl = userInfo.headimgurl;
                    //保存用户信息
                    CookieUtil.SetCookie("OpenId", openId);
                    CookieUtil.SetCookie("HeadImgUrl", headimgurl);
                    CookieUtil.SetCookie("Nickname", nickname);

                    Log4Helper.WriteInfoLog(string.Format("OpenId:{0}", userInfo.ToJson2()), "Login");

                    var url = "http://app.1000n.com/me/vip";

                    return Redirect(url);
                }
                else
                {
                    return Content("用户已授权，获取UserInfo失败");
                }
            }
            catch (ErrorJsonResultException ex)
            {
                return Content("用户已授权，授权Token：" + result);
            }
        }

    }
}
