using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Web;
using EC.Libraries.Core.Log;
using EC.Libraries.Util;
using Senparc.Weixin.Exceptions;
using Senparc.Weixin.MP.AdvancedAPIs;

namespace EC.Libraries.Core.WeiXin
{
    /// <summary>
    /// 微信帮助类
    /// </summary>
    public class WeiXinHelper
    {
        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <param name="weixinAppId">微信AppId</param>
        /// <param name="weixinAppSecret">微信AppSecret</param>
        /// <returns>AccessToken对象</returns>
        public static AccessToken GetAccessToken(string weixinAppId, string weixinAppSecret)
        {
            var content = WebUtil.Get("https://api.weixin.qq.com/cgi-bin/token", new Dictionary<string, string>()
            {
                {"grant_type", "client_credential"},
                {"appid", weixinAppId},
                {"secret", weixinAppSecret}
            }, Encoding.GetEncoding("utf-8"));
            var token = content.ToObject<AccessToken>();

            return new AccessToken()
            {
                access_token = token.access_token,
                expires_in = token.expires_in
            };
        }

        /// <summary>
        /// 用户信息接口
        /// </summary>
        /// <param name="accessTokenOrAppId">AccessToken或AppId（推荐使用AppId，需要先注册）</param>
        /// <param name="openId"></param>
        /// <returns></returns>
        public static WXUserInfo GetUserInfo(string accessTokenOrAppId, string openId)
        {
            var content = WebUtil.Get("https://api.weixin.qq.com/cgi-bin/user/info", new Dictionary<string, string>()
            {
                {"access_token", accessTokenOrAppId},
                {"openid", openId}
            }, Encoding.GetEncoding("utf-8"));

            var userinfo = content.ToObject<WXUserInfo>();

            return userinfo;
        }

        /// <summary>
        /// 创建二维码(参数为String)
        /// </summary>
        /// <param name="accessToken">微信AccessToken</param>
        /// <param name="sceneStr">二维码参数</param>
        /// <returns>二维码图片地址</returns>
        public static string GetTicketWidthSceneStr(string accessToken, string sceneStr)
        {
            var str = CreateTicket(accessToken, sceneStr);

            return str.Equals("40001") || str.Equals("41001") ? "" : GetTicketImage(str);
        }

        /// <summary>
        /// 使用微信模板发送固定消息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="openId">接收信息者的微信openId</param>
        /// <param name="templateId">选择模板</param>
        /// <param name="data">
        ///  new
        ///  {
        ///     first = new { color = "#f0f0f1", value = "first" },
        ///     reason = new { color = "#0fe123", value = "reason" },
        ///     refund = new { color = "#c321f0", value = 0.101 },
        ///     Remark = new { color = "#3f333", value = "备注" }
        ///  }
        /// </param>
        /// <param name="url">微信链接地址</param>
        /// <returns>返回参数</returns>
        public static WeiXinResult SendTemplateMessage(string accessToken, string openId, string templateId, object data, string url = "")
        {
            var result = new WeiXinResult();

            try
            {
                Log4Helper.WriteErrLog(String.Format("微信模板发送 WeiXin准备发送:openId:{0},templateId{1},data:{2}", openId, templateId, data.ToJson()), null);
                var template= TemplateApi.SendTemplateMessage(accessToken, openId, templateId, url, data);

                result.ErrCode = template.errcode.ToString();
                result.ErrMsg = template.errmsg;
            }
            catch (ErrorJsonResultException ex)
            {
                result.IsErr = true;
                result.ErrCode = ex.JsonResult.errcode.ToString();
                result.ErrMsg = ex.JsonResult.errmsg;

                Log4Helper.WriteErrLog("微信模板发送 发送异常", ex);
            }
            catch (Exception ex)
            {
                result.IsErr = true;
                result.ErrMsg = ex.Message;

                Log4Helper.WriteErrLog("微信模板发送 发送异常", ex);
            }

            return result;
        }

        /// <summary>  
        /// 创建二维码ticket  
        /// </summary>  
        /// <param name="accessToken">accessToken</param>
        /// <param name="sceneStr">字符串类型的参数</param>
        /// <returns>二维码ticket</returns>  
        private static string CreateTicket(string accessToken, string sceneStr)
        {
            var result = "";
            var strJson = @"{""action_name"": ""QR_LIMIT_STR_SCENE"", ""action_info"": {""scene"": {""scene_str"":""" + sceneStr + "\"}}}";
            var wxurl = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=" + accessToken;
            var myWebClient = new WebClient { Credentials = CredentialCache.DefaultCredentials };

            try
            {
                result = myWebClient.UploadString(wxurl, "POST", strJson);
                var mode = result.ToObject<Ticket>();

                if (!String.IsNullOrEmpty(mode.errcode) && (mode.errcode.Equals("40001") || mode.errcode.Equals("41001")))
                {
                    result = mode.errcode;
                }
                else
                {
                    result = mode.ticket;
                }
            }
            catch (Exception ex)
            {
                result = "Error:" + ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 使用Ticket换取二维码图片地址
        /// </summary>
        /// <param name="ticket">二维码ticket</param>
        /// <returns>二维码图片地址</returns>
        private static string GetTicketImage(string ticket)
        {
            string strpath;
            var stUrl = String.Format("https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket={0}", HttpUtility.UrlEncode(ticket));

            var req = (HttpWebRequest)WebRequest.Create(stUrl);
            req.Timeout = 30000;
            req.Method = "GET";

            using (var wr = req.GetResponse())
            {
                var myResponse = (HttpWebResponse)req.GetResponse();
                strpath = myResponse.ResponseUri.ToString();
            }

            return strpath;
        }
    }
}
