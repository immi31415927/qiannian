using System;
using System.Configuration;
using System.IO;
using EC.Application.Tables.CRM;
using EC.Application.Tables.WeiXin;
using EC.Entity.Parameter.Request.WeiXin;
using EC.Entity.Tables.CRM;
using Senparc.Weixin.Context;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MessageHandlers;
using EC.Libraries.Util;

namespace EC.WeiXin.WeiXin
{
    /// <summary>
    /// 自定义MessageHandler
    /// 把MessageHandler作为基类，重写对应请求的处理方法
    /// </summary>
    public class CustomMessageHandler : MessageHandler<CustomMessageContext>
    {
        private readonly Article _article = null;

        public CustomMessageHandler(Stream inputStream, PostModel postModel, int maxRecordCount = 0): base(inputStream, postModel, maxRecordCount)
        {
            WeixinContext.ExpireMinutes = 3;
        }

        public sealed override WeixinContext<CustomMessageContext, IRequestMessageBase, IResponseMessageBase> WeixinContext
        {
            get { return base.WeixinContext; }
        }

        public override void OnExecuting()
        {
            if (CurrentMessageContext.StorageData == null)
            {
                CurrentMessageContext.StorageData = 0;
            }
            base.OnExecuting();
        }

        public override void OnExecuted()
        {
            base.OnExecuted();
            CurrentMessageContext.StorageData = ((int)CurrentMessageContext.StorageData) + 1;
        }

        /// <summary>
        /// 处理文字请求
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnTextRequest(RequestMessageText requestMessage)
        {
            return this.CreateResponseMessage<ResponseMessageTransfer_Customer_Service>();
        }

        //
        // 摘要: 
        //     图片类型请求
        public override IResponseMessageBase OnImageRequest(RequestMessageImage requestMessage)
        {
            return this.CreateResponseMessage<ResponseMessageTransfer_Customer_Service>();
        }

        //
        // 摘要: 
        //     链接消息类型请求
        public override IResponseMessageBase OnLinkRequest(RequestMessageLink requestMessage)
        {
            return this.CreateResponseMessage<ResponseMessageTransfer_Customer_Service>();
        }

        //
        // 摘要: 
        //     位置类型请求
        public override IResponseMessageBase OnLocationRequest(RequestMessageLocation requestMessage)
        {
            return this.CreateResponseMessage<ResponseMessageTransfer_Customer_Service>();
        }

        //
        // 摘要: 
        //     链接消息类型请求
        public override IResponseMessageBase OnShortVideoRequest(RequestMessageShortVideo requestMessage)
        {
            return this.CreateResponseMessage<ResponseMessageTransfer_Customer_Service>();
        }

        //
        // 摘要: 
        //     视频类型请求
        public override IResponseMessageBase OnVideoRequest(RequestMessageVideo requestMessage)
        {
            return this.CreateResponseMessage<ResponseMessageTransfer_Customer_Service>();
        }

        //
        // 摘要: 
        //     语音类型请求
        public override IResponseMessageBase OnVoiceRequest(RequestMessageVoice requestMessage)
        {
            return this.CreateResponseMessage<ResponseMessageTransfer_Customer_Service>();
        }

        /// <summary>
        /// 默认返回信息
        /// </summary>
        /// <param name="requestMessage">类型请求</param>
        public override IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage)
        {
            var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "这条消息来自DefaultResponseMessage。";
            return responseMessage;
        }

        /// <summary>
        /// 扫码事件
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_ScanRequest(RequestMessageEvent_Scan requestMessage)
        {
            string weixinAppId = ConfigurationManager.AppSettings["WeixinAppId"];
            string weixinAppSecret = ConfigurationManager.AppSettings["WeixinAppSecret"];
            var sendInfoTemplate = ConfigurationManager.AppSettings["sendInfoTemplate"];

            var eventKey = requestMessage.EventKey.Replace("qrscene_", "");


            var responseMessage = this.CreateResponseMessage<ResponseMessageText>();

            var model = RecommendApp.Instance.GetByopenId(requestMessage.FromUserName);
            if (model != null)
            {
                //老用户
                responseMessage.Content = "您好！感谢关注千年文化传播官方微信";
            }
            else
            {
                var userinfo = WeiXinApp.Instance.GetUserInfo(weixinAppId, weixinAppSecret, requestMessage.FromUserName);
                if (userinfo != null)
                {
                    var recommend = new CrRecommend()
                    {
                        Openid = requestMessage.FromUserName,
                        HeadImgUrl = userinfo.headimgurl,
                        Nickname = userinfo.nickname,
                        ReferrerSysNo = Convert.ToInt32(eventKey),
                        CreatedDate = DateTime.Now
                    };

                    if (RecommendApp.Instance.Insert(recommend).Status)
                    {
                        var customer = CustomerApp.Instance.Get(Convert.ToInt32(eventKey));
                        if (customer != null)
                        {
                            WeiXinApp.Instance.SendMessage(new SendMessageRequest()
                            {
                                WeiXinAppId = weixinAppId,
                                WeiXinAppSecret = weixinAppSecret,
                                OpenId = customer.OpenId,
                                TemplateId = sendInfoTemplate,
                                Data = new
                                {
                                    //first = new { color = "#000000", value = "1000n消息信息" },
                                    keyword1 = new { color = "#000000", value = userinfo.nickname },
                                    keyword2 = new { color = "#000000", value = DateTime.Now.ToString("yyyy年MM月dd日") },
                                    remark = new { color = "#000000", value = string.Format("{0}通过您的二维码关注了本公众号、成为您的粉丝", userinfo.nickname) }
                                },
                                Url = "#"
                            });
                        }
                    }



                    responseMessage.Content = "您好！感谢关注千年文化传播官方微信aa";
                }
            }
            return responseMessage;
        }

        /// <summary>
        /// 订阅（关注）事件
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_SubscribeRequest(RequestMessageEvent_Subscribe requestMessage)
        {
            string weixinAppId = ConfigurationManager.AppSettings["WeixinAppId"];
            string weixinAppSecret = ConfigurationManager.AppSettings["WeixinAppSecret"];
            var sendInfoTemplate = ConfigurationManager.AppSettings["sendInfoTemplate"];

            var eventKey = requestMessage.EventKey.Replace("qrscene_", "");

            var responseMessage = this.CreateResponseMessage<ResponseMessageText>();

            var model = RecommendApp.Instance.GetByopenId(requestMessage.FromUserName);
            if (model != null)
            {
                //老用户
                responseMessage.Content = "您好！感谢关注千年文化传播官方微信";
            }
            else
            {
                var userinfo = WeiXinApp.Instance.GetUserInfo(weixinAppId, weixinAppSecret, requestMessage.FromUserName);
                if (userinfo != null)
                {
                    var recommend = new CrRecommend()
                    {
                        Openid = requestMessage.FromUserName,
                        HeadImgUrl = userinfo.headimgurl,
                        Nickname = userinfo.nickname,
                        ReferrerSysNo = Convert.ToInt32(eventKey),
                        CreatedDate = DateTime.Now
                    };

                    if (RecommendApp.Instance.Insert(recommend).Status)
                    {
                        var customer = CustomerApp.Instance.Get(Convert.ToInt32(eventKey));
                        if (customer != null)
                        {
                            WeiXinApp.Instance.SendMessage(new SendMessageRequest()
                            {
                                WeiXinAppId = weixinAppId,
                                WeiXinAppSecret = weixinAppSecret,
                                OpenId = customer.OpenId,
                                TemplateId = sendInfoTemplate,
                                Data = new
                                {
                                    //first = new { color = "#000000", value = "1000n消息信息" },
                                    keyword1 = new { color = "#000000", value = userinfo.nickname },
                                    keyword2 = new { color = "#000000", value = DateTime.Now.ToString("yyyy年MM月dd日") },
                                    remark = new { color = "#000000", value = string.Format("{0}通过您的二维码关注了本公众号、成为您的粉丝", userinfo.nickname) }
                                },
                                Url = "#"
                            });
                        }
                    }
                    responseMessage.Content = "您好！感谢关注千年文化传播官方微信";
                }
            }

            return responseMessage;
        }

        /// <summary>
        /// 退订
        /// 实际上用户无法收到非订阅账号的消息，所以这里可以随便写。
        /// unsubscribe事件的意义在于及时删除网站应用中已经记录的OpenID绑定，消除冗余数据。并且关注用户流失的情况。
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_UnsubscribeRequest(RequestMessageEvent_Unsubscribe requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = requestMessage.FromUserName;
            return responseMessage;
        }

        /// <summary>
        /// Event事件类型请求之CLICK
        /// </summary>
        public override IResponseMessageBase OnEvent_ClickRequest(RequestMessageEvent_Click requestMessage)
        {
            var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "这条消息来自OnEvent_ClickRequest。";
            return responseMessage;
        }
    }
}