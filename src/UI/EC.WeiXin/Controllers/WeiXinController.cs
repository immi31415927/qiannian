using System;
using System.Configuration;
using System.IO;
using System.Web.Mvc;
using EC.Libraries.Core.Log;
using EC.WeiXin.WeiXin;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Entities.Request;
using EC.Libraries.Util;

namespace EC.WeiXin.Controllers
{
    public class WeiXinController : Controller
    {
        //
        // GET: /WeiXin/

        #region 字段
        /// <summary>
        /// 与微信公众账号后台的AppId设置保持一致，区分大小写。
        /// </summary>
        public static readonly string AppId = ConfigurationManager.AppSettings["WeixinAppId"];

        /// <summary>
        /// 与微信公众账号后台的Token设置保持一致，区分大小写
        /// </summary>
        public static readonly string Token = ConfigurationManager.AppSettings["WeixinToken"];

        /// <summary>
        /// 微信公众账号后台的EncodingAESKey设置保持一致，区分大小写
        /// </summary>
        public static readonly string EncodingAesKey = ConfigurationManager.AppSettings["WeixinEncodingAESKey"];

        /// <summary>
        /// 微信公众号AppSecret
        /// </summary>
        public static readonly string WeixinAppSecret = ConfigurationManager.AppSettings["WeixinAppSecret"];

        #endregion 字段

        /// <summary>
        /// 接入微信
        /// 微信后台验证地址（使用Get），微信后台的“接口配置信息”的Url填写如：http://weixin.senparc.com/weixin
        /// </summary>
        [HttpGet]
        [ActionName("Index")]
        public ActionResult Get(PostModel postModel, string echostr)
        {
            Log4Helper.WriteInfoLog(String.Format("Get:{0}", DateTime.Now.ToString()), "api");

            return CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, Token) ? Content(echostr) : Content("failed:" + postModel.Signature + "," + CheckSignature.GetSignature(postModel.Timestamp, postModel.Nonce, Token) + "。" + "如果你在浏览器中看到这句话，说明此地址可以被作为微信公众账号后台的Url，请注意保持Token一致。");
        }

        /// <summary>
        /// 接入微信
        /// 用户发送消息后，微信平台自动Post一个请求到这里，并等待响应XML。
        /// PS：此方法为简化方法，效果与OldPost一致。
        /// v0.8之后的版本可以结合Senparc.Weixin.MP.MvcExtension扩展包，使用WeixinResult，见MiniPost方法。
        /// </summary>
        [HttpPost]
        [ActionName("Index")]
        public ActionResult Post(PostModel postModel)
        {
            Log4Helper.WriteInfoLog(String.Format("Post:{0}", DateTime.Now.ToString()), "api");

            if (!CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, Token))
            {
                return Content("参数错误！");
            }

            postModel.Token = Token;
            postModel.EncodingAESKey = EncodingAesKey;//根据自己后台的设置保持一致
            postModel.AppId = AppId;//根据自己后台的设置保持一致

            const int maxRecordCount = 10;
            var messageHandler = new CustomMessageHandler(Request.InputStream, postModel, maxRecordCount);
            try
            {
                messageHandler.OmitRepeatedMessage = true;
                //执行微信处理过程
                messageHandler.Execute();

                return new FixWeixinBugWeixinResult(messageHandler);//为了解决官方微信5.0软件换行bug暂时添加的方法，平时用下面一个方法即可
            }
            catch (Exception ex)
            {
                Log4Helper.WriteInfoLog(ex.Message, "wx");
            }
            return Content("");
        }
    }
}
