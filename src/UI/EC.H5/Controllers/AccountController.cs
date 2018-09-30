using System;
using System.Linq;
using System.Web.Mvc;
using System.Configuration;
using EC.Entity;
using EC.Entity.Caching;
using EC.Entity.Parameter.Request.CRM;
using EC.Entity.Parameter.Request.SMS;
using EC.Entity.Tables.CRM;
using EC.Libraries.Core.Log;
using EC.Libraries.Core.Validator;
using EC.Libraries.Core.Validator.Rule;
using EC.Libraries.Framework;
using EC.Libraries.Redis;
using Senparc.Weixin.MP.AdvancedAPIs;

#region 自定义
using EC.Application.Tables.CRM;
using EC.Entity.Enum;
using EC.Libraries.Util;
#endregion

namespace EC.H5.Controllers
{
    using Senparc.Weixin;
    using Senparc.Weixin.Exceptions;
    using Senparc.Weixin.MP;

    /// <summary>
    /// 登录、注册
    /// </summary>
    public class AccountController : Controller
    {
        private static string appKey = ConfigurationManager.AppSettings["appid"];
        private static string appSecret = ConfigurationManager.AppSettings["appSecret"];

        #region 微信授权
        /// <summary>
        /// 微信授权
        /// </summary>
        public ActionResult Login()
        {
            var redirectUrl = ConfigurationManager.AppSettings["redirectUrl"];

            var url = OAuthApi.GetAuthorizeUrl(appKey, redirectUrl, WebUtil.Number(6, true), OAuthScope.snsapi_userinfo);

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

                    var customerExt = CustomerApp.Instance.GetByOpenId(openId);

                    Log4Helper.WriteInfoLog(string.Format("OpenId:{0}", openId), "Login");
                    Log4Helper.WriteInfoLog(string.Format("会员对象:{0}", customerExt.ToJson2()), "Login");
                    if (customerExt == null){
                        return RedirectToAction("Register", "Account");
                    }else{
                        //登录成功
                        var response = CustomerApp.Instance.OpenIdLogin(new LoginRequest()
                        {
                            OpenId = openId
                        });

                        Log4Helper.WriteInfoLog(string.Format("登录成功:{0}", response.ToJson()), "Login");

                        if (response.Status){
                            return RedirectToAction("Index", "Me");
                        }else{
                            return RedirectToAction("MemberBind", "Account");
                        }
                    }
                }else{
                    return Content("用户已授权，获取UserInfo失败");
                }
            }
            catch (ErrorJsonResultException ex)
            {
                return Content("用户已授权，授权Token：" + result);
            }
        }
        #endregion

        #region 绑定用户
        /// <summary>
        /// 绑定用户
        /// </summary>
        /// <returns>绑定用户视图</returns>
        [HttpGet]
        public ActionResult MemberBind()
        {
            var openId = CookieUtil.Get("OpenId");
#if DEBUG
            openId = Guid.NewGuid().ToString().Replace("-", "");
            openId = "30";
            //登录成功
            var response = CustomerApp.Instance.OpenIdLogin(new LoginRequest()
            {
                OpenId = openId
            });
            if (response.Status)
            {
                return RedirectToAction("Index", "Me");
            }
            CookieUtil.SetCookie("OpenId", openId);
#endif
            //openId = Guid.NewGuid().ToString().Replace("-", "");
            CookieUtil.SetCookie("OpenId", openId);
            ViewBag.openId = openId;

            ViewBag.returnUrl = "/me/index";
            return View();
        }

        /// <summary>
        /// 绑定账号
        /// </summary>
        /// <returns>JResult</returns>
        [HttpPost]
        public JsonResult MemberBind(MemberBindRequest request)
        {
            var response = new JResult()
            {
                Status = false
            };

            var result = VHelper.ValidatorRule(
                new Rule_NotAllowNull(request.OpenId, "微信授权失败！"),
                new Rule_NotAllowNull(request.Account, "账号不能为空！"),
                new Rule_Mobile(request.Account, "输入手机号！"),
                new Rule_StringLenth(request.Password, min: 6, max: 20, message: "新密码长度在6-20位之间")
            );

            if (!result.IsPass)
            {
                response.Status = result.IsPass;
                response.Message = result.Message;
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            var openId = CookieUtil.Get("OpenId");
            if (!request.OpenId.Equals(openId))
            {
                response.Status = false;
                response.Message = "请不要不暴力操作";
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            response = CustomerApp.Instance.MemberBind(new LoginRequest()
            {
                Account = request.Account,
                Password = request.Password,
                OpenId = request.OpenId,
                Nickname = CookieUtil.Get("Nickname"),
                HeadImgUrl = CookieUtil.Get("HeadImgUrl")
            });
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 用户注册
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <returns>用户注册视图</returns>
        [HttpGet]
        public ActionResult Register()
        {
            var openId = CookieUtil.Get("OpenId");
#if DEBUG
            openId = "openId002";
            CookieUtil.SetCookie("OpenId", openId);
#endif
            ViewBag.openId = openId;

            ViewBag.returnUrl = "/me/index";
            //注册支付类型
            ViewBag.RegisterPayType = EnumUtil.ToDictionary(typeof(CustomerEnum.RegisterPayType));

            var recommend = RecommendApp.Instance.GetByopenId(openId);
            if (recommend != null)
            {
                var customer = CustomerApp.Instance.Get(recommend.ReferrerSysNo);
                return View(customer);
            }
            return View(new CrCustomer(){});
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Register(RegisterRequest request)
        {
            var response = new JResult<string>()
            {
                Status = false
            };

            var result = VHelper.ValidatorRule(
                new Rule_NotAllowNull(request.OpenId, "微信授权失败！"),
                new Rule_NotAllowNull(request.RealName, "姓名不能为空！"),
                new Rule_NotAllowNull(request.PhoneNumber, "账号不能为空！"),
                new Rule_Mobile(request.PhoneNumber, "输入手机号！"),
                new Rule_LetterAndNumber(request.Password, "密码应由拉丁字母和数字组成！"),
                new Rule_StringLenth(request.Password, min: 6, max: 20, message: "新密码长度在6-20位之间"),
                new Rule_LetterAndNumber(request.ConfirmPassword, "确认密码应由拉丁字母和数字组成"),
                new Rule_StringLenth(request.ConfirmPassword, min: 6, max: 20, message: "新确认密码长度在6-20位之间"),
                new Rule_StringLenth(request.MobileVerifyCode, min: 6, max: 6, message: "手机验证码不能为空！")
            );

            if (!result.IsPass)
            {
                response.Status = result.IsPass;
                response.Message = result.Message;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            //短信验证
            var _cacheProvider = ClientProxy.GetInstance<IRedisProvider>();

            var cached = _cacheProvider.Get<SendSMSResponse>(CacheKeys.Items.SMS_ + request.PhoneNumber);
            if (cached != null)
            {
                if (!cached.Rand.Equals(request.MobileVerifyCode))
                {
                    response.Status = false;
                    response.Message = "手机验证错误！";
                    return Json(response);
                }
            }
            else
            {
                response.Status = false;
                response.Message = "手机验证错误！";
                return Json(response);
            }
            //OpenId验证
            var openId = CookieUtil.Get("OpenId");
            if (!request.OpenId.Equals(openId))
            {
                response.Status = false;
                response.Message = "请不要不暴力操作";
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            request.OpenId = openId;
            request.Nickname = CookieUtil.Get("Nickname");
            request.HeadImgUrl = CookieUtil.Get("HeadImgUrl");
            //验证手机号是否存在
            var phoneExist = CustomerApp.Instance.GetByPhoneNumber(request.PhoneNumber);
            if (phoneExist != null)
            {
                response.Status = false;
                response.Message = "手机号已存在！";
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            var accountExist = CustomerApp.Instance.GetByAccount(request.PhoneNumber);
            if (accountExist != null)
            {
                response.Status = false;
                response.Message = "手机号已存在！";
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            if (!string.IsNullOrEmpty(request.Referrer))
            {
                //查找推荐人
                var referrerExist = CustomerApp.Instance.GetByAccount(request.Referrer);
                if (referrerExist != null)
                {
                    request.ReferrerSysNo = referrerExist.SysNo;
                }
                else
                {
                    response.Status = false;
                    response.Message = "推荐人不存在！";
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
            }
            request.AutoLogin = true;
            response = CustomerApp.Instance.Register(request);
            if (response.Status)
            {
                var loginResonse = CustomerApp.Instance.AccountLogin(new LoginRequest()
                {
                    Account = request.PhoneNumber,
                    Password = request.Password
                });
                if (!loginResonse.Status)
                {
                    response.Status = false;
                    response.Message = "自动登录失败！";
                }
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion


        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns>登录页面</returns>
        [HttpGet]
        public ActionResult Logout()
        {
            AuthApp.Instance.SignOut();

            return View();
        }


        [HttpPost]
        public JsonResult sendSMS(string phoneNumber)
        {
            var response = new JResult()
            {
                Status = false
            };

            var result = VHelper.ValidatorRule(
                new Rule_NotAllowNull(phoneNumber, "账号不能为空！"),
                new Rule_Mobile(phoneNumber, "输入手机号！")
            );

            if (!result.IsPass)
            {
                response.Status = result.IsPass;
                response.Message = result.Message;
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            var customer = CustomerApp.Instance.GetByAccount(phoneNumber);
            if (customer != null)
            {
                response.Status = false;
                response.Message = "该手机号已注册！";
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            var list  = SmsApp.Instance.GetSendingTimes(new SMSQueryRequeest()
            {
                PhoneNumber = phoneNumber,
                StartTime = DateTime.Now.ToString("yyyy-MM-dd 00:00:00"),
                EndTime = DateTime.Now.ToString("yyyy-MM-dd 23:59:59")
            });

            if (list.Count > 20)
            {
                response.Status = false;
                response.Message = "您今天发20条短信明天在来吧！";
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            return Json(SmsApp.Instance.SMSVerify(phoneNumber),JsonRequestBehavior.AllowGet);
        }
    }
}
