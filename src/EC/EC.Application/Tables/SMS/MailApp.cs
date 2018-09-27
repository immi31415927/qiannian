using System;
using EC.Entity;
using EC.Entity.Parameter.Request.SMS;
using EC.Entity.Parameter.Response.SMS;
using EC.Libraries.Core;
using EC.Libraries.Framework;
using EC.Libraries.Redis;

namespace EC.Application.Tables.SMS
{
    public class MailApp : Base<MailApp>
    {
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <returns>JResult</returns>
        /// <remarks>2017-03-31 苟治国 创建</remarks>
        public JResult SendVerifyCode(SendVerifyCodeRequest request)
        {
            var response = new JResult()
            {
                Status = false
            };
            

            var rand = new Random().Next(100000, 999999).ToString();
            string content = "";
            var verifyCodeCacheKey = request.Key + request.PhoneNo;
            var smsRequest = SendSms(new SmsRequest()
            {
                Mobile = request.PhoneNo, 
                Content = content, 
                Rand = rand, 
                VerifyCodeCacheKey = 
                verifyCodeCacheKey, 
                catchTime = 10
            });
            if (smsRequest != null)
            {
                var span = DateTime.Now.Subtract(smsRequest.SendTime);
                //秒数
                var seconds = (span.Minutes * 60) + span.Seconds;
                if (seconds > 60)
                {
                    var _redisProvider = ClientProxy.GetInstance<IRedisProvider>();
                    //删除缓存
                    _redisProvider.Remove(verifyCodeCacheKey);
                    //重新发送验证码
                    SendSms(new SmsRequest() { Mobile = request.PhoneNo, Content = content, Rand = rand, VerifyCodeCacheKey = verifyCodeCacheKey, catchTime = 10 });
                    response.StatusCode = "60";
                    response.Status = true;
                }
                else
                {
                    response.StatusCode = (60 - seconds).ToString();
                    response.Status = true;
                }
            }
            else
            {
                response.StatusCode = "手机号验证码发送失败！";
                return response;
            }
            return response;
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="request">参数</param>
        /// <remarks>2017-03-31 苟治国 创建</remarks>
        public SmsResponse SendSms(SmsRequest request)
        {
            //var _smsService = new ServiceProxy<ISmsService>();

            //var _cacheProvider = ClientProxy.GetInstance<IRedisProvider>();
            //return _cacheProvider.Get<SmsResponse>(request.VerifyCodeCacheKey, () =>
            //{
            //    var result = _smsService.Channel.Send(SmsType.手机维修.ToString(), request.Mobile, string.Format("{0};", request.Rand), null, true);
            //    if (!result.IsError)
            //    {
            //        return new SmsResponse() { Mobile = request.Mobile, Rand = request.Rand, SendTime = DateTime.Now };
            //    }
            //    return null;
            //}, request.catchTime);
            return null;
        }
    }
}
