using System;
using System.Collections.Generic;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Dysmsapi.Model.V20170525;
using EC.DataAccess.Bs;
using EC.Entity;
using EC.Entity.Caching;
using EC.Entity.Parameter.Request.CRM;
using EC.Entity.Parameter.Request.SMS;
using EC.Entity.Tables.Bs;
using EC.Libraries.Core.Pager;

namespace EC.Application.Tables.CRM
{
    using EC.Libraries.Util;
    using EC.Libraries.Core;
    using EC.Libraries.Framework;
    using EC.Libraries.Redis;

    /// <summary>
    /// 短信业务层
    /// </summary>
    public class SmsApp : Base<SmsApp>
    {
        /// <summary>
        /// 查询某个时间段发送短信数量
        /// </summary>
        /// <param name="requeest">参数</param>
        public List<BsSMS> GetSendingTimes(SMSQueryRequeest requeest)
        {
            return Using<ISMS>().GetSendingTimes(requeest);
        }

        /// <summary>
        /// 分页列表
        /// </summary>
        public PagedList<BsSMS> GetPaging(SMSQueryRequeest requeest)
        {
            return Using<ISMS>().GetPaging(requeest);
        }

        /// <summary>
        /// 注册验证码
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        public JResult SMSVerify(string phoneNumber)
        {
            var response = new JResult()
            {
                Status = false
            };

            var rand = WebUtil.Number(6, true);

            var content = string.Format("【XX】尊敬的用户，您的验证码为{0}，热线：888-88888888", rand);

            var smsCacheKey = CacheKeys.Items.SMS_ + phoneNumber;
            var smsCountCacheKey = CacheKeys.Items.SMSCount_ + phoneNumber;

            var sendSMSResponse = this.SendSMS(new SendSMSRequest() { PhoneNumber = phoneNumber, Content = content, Rand = rand, SMSCacheKey = smsCacheKey,SMSCountCacheKey=smsCountCacheKey,catchTime = 120 });

            if (sendSMSResponse != null)
            {
                var span = DateTime.Now.Subtract(sendSMSResponse.SendTime);
                //秒数
                var seconds = (span.Minutes*60) + span.Seconds;

                if (seconds > 120)
                {
                    var cacheProvider = ClientProxy.GetInstance<IRedisProvider>();
                    //TODO 验证发送数量
                    //移除缓存
                    cacheProvider.Remove(smsCacheKey);
                    //重新发送验证码
                    this.SendSMS(new SendSMSRequest()
                    {
                        PhoneNumber = phoneNumber, 
                        Content = content, 
                        Rand = rand, 
                        SMSCacheKey = smsCacheKey, 
                        SMSCountCacheKey = smsCountCacheKey, 
                        catchTime = 120
                    });
                    response.StatusCode = "120";
                    response.Status = true;
                }
                else{
                    response.StatusCode = (120 - seconds).ToString();
                    response.Status = true;
                }
            }
            else
            {
                response.Message = "手机号验证码发送失败！";
            }

            return response;
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="request"></param>
        public SendSMSResponse SendSMS(SendSMSRequest request)
        {
            string product = "Dysmsapi";//短信API产品名称
            string domain = "dysmsapi.aliyuncs.com";//短信API产品域名
            string accessKeyId = "LTAIOMPsjtzrkrgC";//你的accessKeyId
            string accessKeySecret = "y4umqrQMKR29NJWoUzMMxuOcCTaEK9";//你的accessKeySecret

            var cacheProvider = ClientProxy.GetInstance<IRedisProvider>();

            return cacheProvider.Get<SendSMSResponse>(request.SMSCacheKey, () =>{

                IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", accessKeyId, accessKeySecret);
                DefaultProfile.AddEndpoint("cn-hangzhou", "cn-hangzhou", product, domain);
                IAcsClient acsClient = new DefaultAcsClient(profile);
                SendSmsRequest sendSmsRequest = new SendSmsRequest();
                //必填:待发送手机号。支持以逗号分隔的形式进行批量调用，批量上限为20个手机号码,批量调用相对于单条调用及时性稍有延迟,验证码类型的短信推荐使用单条调用的方式
                sendSmsRequest.PhoneNumbers = request.PhoneNumber;
                //必填:短信签名-可在短信控制台中找到
                sendSmsRequest.SignName = "千年教育";
                //必填:短信模板-可在短信控制台中找到
                sendSmsRequest.TemplateCode = "SMS_112465567";
                //可选:模板中的变量替换JSON串,如模板内容为"亲爱的${name},您的验证码为${code}"时,此处的值为
                var templateParam = new
                {
                    code = request.Rand
                };
                sendSmsRequest.TemplateParam = templateParam.ToJson2();
                //可选:outId为提供给业务方扩展字段,最终在短信回执消息中将此值带回给调用者
                //request.OutId = "21212121211";
                //请求失败这里会抛ClientException异常
                SendSmsResponse sendSmsResponse = acsClient.GetAcsResponse(sendSmsRequest);
                if (sendSmsResponse.Code != null && sendSmsResponse.Code.Equals("OK"))
                {
                    Using<ISMS>().Insert(new BsSMS()
                    {
                        PhoneNumber = request.PhoneNumber,
                        Content = string.Format("您的短信验证码是{0}。若非本人发送，请忽略此短信。本条短信免费。",request.Rand),
                        Priority = 10,
                        FailureTimes= 0,
                        Status = 1,
                        CreatedBy = 1,
                        CreatedDate = DateTime.Now
                    });
                    return new SendSMSResponse()
                    {
                        PhoneNumber = request.PhoneNumber, 
                        Rand = request.Rand, 
                        SendTime = DateTime.Now 
                    };
                }
                else
                    return new SendSMSResponse();
            },request.catchTime);
        }

    }
}
