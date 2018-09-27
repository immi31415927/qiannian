using System;
using EC.Entity;
using EC.Entity.Parameter.Request.WeiXin;
using EC.Libraries.Core;
using EC.Libraries.Core.WeiXin;

namespace EC.Application.Tables.WeiXin
{
    /// <summary>
    /// 微信业务逻辑
    /// </summary>
    public class WeiXinApp : Base<WeiXinApp>
    {
        /// <summary>
        /// 获取微信AccessToken
        /// </summary>
        /// <param name="weixinAppId">微信AppId</param>
        /// <param name="weixinAppSecret">微信AppSecret</param>
        /// <returns>结果</returns>
        public JResult<string> GetAccessToken(string weixinAppId, string weixinAppSecret)
        {
            var result = new JResult<string>();

            try
            {
                var accessToken = WeiXinHelper.GetAccessToken(weixinAppId, weixinAppSecret);
                //可进行缓存

                if (string.IsNullOrEmpty(accessToken.access_token))
                {
                    throw new Exception("微信AccessToken获取失败");
                }

                result.Status = true;
                result.Data = accessToken.access_token;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 用户信息接口
        /// </summary>
        /// <param name="weixinAppId">微信AppId</param>
        /// <param name="weixinAppSecret">微信AppSecret</param>
        /// <param name="openId"></param>
        /// <returns></returns>
        public WXUserInfo GetUserInfo(string weixinAppId, string weixinAppSecret, string openId)
        {
            var response = GetAccessToken(weixinAppId, weixinAppSecret);
            if (response.Status)
            {
                return WeiXinHelper.GetUserInfo(response.Data, openId);
            }
            return null;
        }

        /// <summary>
        /// 创建二维码(参数为Int)
        /// </summary>
        /// <param name="weixinAppId">微信AppId</param>
        /// <param name="weixinAppSecret">微信AppSecret</param>
        /// <param name="sceneStr">二维码参数</param>
        /// <returns>二维码图片地址</returns>
        public string GetTicketWidthSceneStr(string weixinAppId, string weixinAppSecret, string sceneStr)
        {
            var accessToken = GetAccessToken(weixinAppId, weixinAppSecret);

            return !accessToken.Status ? "" : WeiXinHelper.GetTicketWidthSceneStr(accessToken.Data, sceneStr);
        }

        /// <summary>
        /// 使用微信模板发送固定消息
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <returns>结果</returns>
        public JResult SendMessage(SendMessageRequest request)
        {
            var result = new JResult();

            try
            {
                var strToken = GetAccessToken(request.WeiXinAppId, request.WeiXinAppSecret);

                if (!strToken.Status)
                {
                    throw new Exception(strToken.Message);
                }

                var wResult = WeiXinHelper.SendTemplateMessage(strToken.Data, request.OpenId, request.TemplateId, request.Data, request.Url);

                if (wResult.IsErr)
                {
                    throw new Exception(wResult.ErrMsg);
                }

                result.Status = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }
    }
}
