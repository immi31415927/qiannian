using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using System.Configuration;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using EC.Application.Tables.CRM;
using EC.Application.Tables.Fn;
using EC.Entity;
using EC.Entity.Parameter.Request.Finance;
using EC.Entity.Parameter.Request.WeiXin;
using EC.Libraries.WeiXin;

namespace EC.H5.Controllers
{
    using EC.Libraries.Core.Log;
    using EC.Libraries.Util;

    using Senparc.Weixin.MP;
    using Senparc.Weixin.MP.TenPayLibV3;
    using EC.Application.Tables.Bs;
    using EC.Entity.Parameter.Request.Bs;
    using EC.Entity.Enum;

    /// <summary>
    /// 支付
    /// </summary>
    public class PayController : Controller
    {
        /// <summary>
        /// 请求的参数
        /// </summary>
        protected Hashtable parameters;

        private static TenPayV3Info _tenPayV3Info;

        public static TenPayV3Info TenPayV3Info
        {
            get
            {
                if (_tenPayV3Info == null)
                {
                    _tenPayV3Info = TenPayV3InfoCollection.Data[ConfigurationManager.AppSettings["mchId"]];
                }
                return _tenPayV3Info;
            }
        }

        #region 微信支付
        /// <summary>
        /// 微信支付
        /// </summary>
        /// <param name="id">订单编号</param>
        /// <returns>视图</returns>
        public ActionResult GoPayWeiXin(string id)
        {
            Log4Helper.WriteInfoLog(id, "pay");

            //当前上下文
            var context = CustomerContext.Context;
            if (context == null)
            {
                return View("Error", new JResult() { Message = "用户已超时、请重新登录﹗" });
            }

            var customerExt = CustomerApp.Instance.Get(context.SysNo);
            if (customerExt == null)
            {
                return View("Error", new JResult() { Message = "扩展会员不存在﹗" });
            }

            //获取订单
            var recharge = RechargeApp.Instance.GetbyOrderNo(id);
            if (recharge == null)
            {
                return View("Error", new JResult() { Message = "订单号不存在﹗" });
            }

            ViewBag.SourceUrl = WebUtil.GetSource();

            #region JsApi支付

            try
            {
                //创建支付应答对象
                var packageReqHandler = new RequestHandler(null);
                //初始化
                packageReqHandler.Init();

                var timeStamp = TenPayV3Util.GetTimestamp();
                var nonceStr = TenPayV3Util.GetNoncestr();

                //设置package订单参数
                packageReqHandler.SetParameter("appid", TenPayV3Info.AppId); //公众账号ID
                packageReqHandler.SetParameter("mch_id", TenPayV3Info.MchId); //商户号
                packageReqHandler.SetParameter("nonce_str", nonceStr); //随机字符串
                packageReqHandler.SetParameter("body", "千年教育充值订单"); //商品信息
                packageReqHandler.SetParameter("out_trade_no", recharge.OrderNo); //订单号
                packageReqHandler.SetParameter("total_fee", Convert.ToInt32(recharge.Amount * 100).ToString());
                //商品金额,以分为单位(money * 100).ToString()
                packageReqHandler.SetParameter("spbill_create_ip", Request.UserHostAddress); //用户的公网ip，不是商户服务器IP
                packageReqHandler.SetParameter("notify_url", TenPayV3Info.TenPayV3Notify); //接收财付通通知的URL
                packageReqHandler.SetParameter("trade_type", TenPayV3Type.JSAPI.ToString()); //交易类型
                packageReqHandler.SetParameter("openid", customerExt.OpenId); //用户的openId

                var sign = packageReqHandler.CreateMd5Sign("key", TenPayV3Info.Key);
                packageReqHandler.SetParameter("sign", sign); //签名

                var data = packageReqHandler.ParseXML();
                Log4Helper.WriteInfoLog("data:" + data, "pay");
                var result = TenPayV3.Unifiedorder(data);
                Log4Helper.WriteInfoLog("统一支付接口:"+result, "pay");

                var res = XDocument.Parse(result);

                var return_code = res.Element("xml").Element("return_code").Value;
                var result_code = res.Element("xml").Element("result_code").Value;
                if (return_code.ToUpper() != "SUCCESS")
                {
                    return View("Error", new JResult { Message = "通信故障，请稍后再试！" });
                }
                if (result_code.ToUpper() == "FAIL")
                {
                    var err_code_des = res.Element("xml").Element("err_code_des").Value;
                    if (err_code_des.Contains("订单号重复"))
                    {
                        return View("Error", new JResult { Message = "订单号重复" });
                    }
                    return View("Error", new JResult { Message = err_code_des });
                }

                var prepayId = res.Element("xml").Element("prepay_id").Value;

                //设置支付参数
                var paySignReqHandler = new RequestHandler(null);
                paySignReqHandler.SetParameter("appId", TenPayV3Info.AppId);
                paySignReqHandler.SetParameter("timeStamp", timeStamp);
                paySignReqHandler.SetParameter("nonceStr", nonceStr);
                paySignReqHandler.SetParameter("package", string.Format("prepay_id={0}", prepayId));
                paySignReqHandler.SetParameter("signType", "MD5");
                var paySign = paySignReqHandler.CreateMd5Sign("key", TenPayV3Info.Key);

                ViewData["appId"] = TenPayV3Info.AppId;
                ViewData["timeStamp"] = timeStamp;
                ViewData["nonceStr"] = nonceStr;
                ViewData["package"] = string.Format("prepay_id={0}", prepayId);
                ViewData["paySign"] = paySign;
            }
            catch (Exception ex)
            {
                Log4Helper.WriteInfoLog(ex.Message, "pay");
            }
            #endregion

            return View(customerExt);
        }
        #endregion

        #region 微信通知
        /// <summary>
        /// 获取微信支付回调请求对象
        /// </summary>
        /// <param name="resHandler">获取页面提交参数对象</param>
        /// <returns>微信支付回调请求对象</returns>
        /// <remarks>2015-09-24 苟治国 创建</remarks>
        private WeixinRequest GetParaWeixin(ResponseHandler resHandler)
        {
            if (resHandler == null) resHandler = new ResponseHandler(null);
            var paraWeixin = new WeixinRequest()
            {
                //返回状态码 SUCCESS/FAIL 此字段是通信标识，非交易标识，交易是否成功需要查看 result_code来判断
                return_code = resHandler.GetParameter("return_code"),
                //返回信息，如非空为错误原因 1：签名失败 2：参数格式校验错误
                return_msg = resHandler.GetParameter("return_msg"),

                #region 以下字段在return_code为SUCCESS的时候有返回
                //微信公众账号id
                appid = resHandler.GetParameter("appid"),
                //商户号
                mch_id = resHandler.GetParameter("mch_id"),
                //终端设备号
                device_info = resHandler.GetParameter("device_info"),
                //随机字符串
                nonce_str = resHandler.GetParameter("nonce_str"),
                //签名
                sign = resHandler.GetParameter("sign"),
                //业务结果
                result_code = resHandler.GetParameter("result_code"),
                //错误代码
                err_code = resHandler.GetParameter("err_code"),
                //错误代码描述
                err_code_des = resHandler.GetParameter("err_code_des"),
                #endregion

                #region 以下字段在return_code和result_code都为SUCCESS的时候有返回
                //用户标识
                openid = resHandler.GetParameter("openid"),
                //是否关注公众账号
                is_subscribe = resHandler.GetParameter("is_subscribe"),
                //交易类型
                trade_type = resHandler.GetParameter("trade_type"),
                //付款银行
                bank_type = resHandler.GetParameter("bank_type"),
                //总金额
                total_fee = resHandler.GetParameter("total_fee"),
                //现金券金额
                coupon_fee = resHandler.GetParameter("coupon_fee"),
                //货币种类
                fee_type = resHandler.GetParameter("fee_type"),
                //微信支付订单号
                transaction_id = resHandler.GetParameter("transaction_id"),
                //商户订单号
                out_trade_no = resHandler.GetParameter("out_trade_no"),
                //商家数据包
                attach = resHandler.GetParameter("attach"),
                //支付完成时间
                time_end = resHandler.GetParameter("time_end")
                #endregion
            };
            return paraWeixin;
        }


        /// <summary>
        /// 微信通知
        /// </summary>
        /// <example>页面只能显示success/failed 不能有任何html标签</example>
        /// <returns>成功返回success 失败返回其它任意对象</returns>
        public ActionResult PayNotifyUrlTest()
        {
            var orderNotify = new OrderNotifyImpl()
            {
                apiKey = ConfigurationManager.AppSettings["apiKey"]
            };

            var param = new UpdatePayStatusRequest()
            {
                Amount = new decimal(200),
                VoucherNo = "4200000083201803148591866779",
                OrderSysNo = "3535d53a9cd34816a532b80a1bfd7f3d"
            };

            WriteWeixinLog("更新参数：\r\n" + param.ToJson2(), null);

            var result = RechargeApp.Instance.UpdatePayStatus(param);

            return Content("", "text/xml");
        }

        /// <summary>
        /// 微信通知
        /// </summary>
        /// <example>页面只能显示success/failed 不能有任何html标签</example>
        /// <returns>成功返回success 失败返回其它任意对象</returns>
        public ActionResult PayNotifyUrl()
        {
            //返回状态码SUCCESS/FAIL,SUCCESS 表示商户接收通知成功并校验成功
            var returnCode = "FAIL";
            //返回信息 非空为错误原因:1、签名失败 2、参数格式校验错误
            var returnMsg = string.Empty;


            try
            {
                WriteWeixinLog("测试开始==========================\r\n", null);

                OrderNotifyImpl orderNotifyImpl = new OrderNotifyImpl()
                {
                    apiKey = ConfigurationManager.AppSettings["apiKey"]
                };

                WriteWeixinLog("PayNotifyUrl", null);

                var requestStream = System.Web.HttpContext.Current.Request.InputStream;

                byte[] requestByte = new byte[requestStream.Length];
                requestStream.Read(requestByte, 0, (int)requestStream.Length);
                string xmlStr = Encoding.UTF8.GetString(requestByte);

                if (!string.IsNullOrWhiteSpace(xmlStr))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.XmlResolver = null;
                    doc.LoadXml(xmlStr);

                    XmlNode xmlNode = doc.SelectSingleNode("xml");
                    XmlNodeList xmlNodeList = xmlNode.ChildNodes;
                    foreach (XmlNode xnf in xmlNodeList)
                    {
                        orderNotifyImpl.Add(xnf.Name, xnf.InnerText);
                    }

                    #region 参数
                    var model = new OrderNotifyResponse()
                    {
                        //返回状态码 SUCCESS/FAIL 此字段是通信标识，非交易标识，交易是否成功需要查看 result_code来判断
                        return_code = orderNotifyImpl.Get("return_code"),
                        //返回信息，如非空为错误原因 1：签名失败 2：参数格式校验错误
                        return_msg = orderNotifyImpl.Get("return_msg"),

                        #region 以下字段在return_code为SUCCESS的时候有返回
                        //微信公众账号id
                        appid = orderNotifyImpl.Get("appid"),
                        //商户号
                        mch_id = orderNotifyImpl.Get("mch_id"),
                        //终端设备号
                        device_info = orderNotifyImpl.Get("device_info"),
                        //随机字符串
                        nonce_str = orderNotifyImpl.Get("nonce_str"),
                        //签名
                        sign = orderNotifyImpl.Get("sign"),
                        //业务结果
                        result_code = orderNotifyImpl.Get("result_code"),
                        //错误代码
                        err_code = orderNotifyImpl.Get("err_code"),
                        //错误代码描述
                        err_code_des = orderNotifyImpl.Get("err_code_des"),
                        #endregion

                        #region 以下字段在return_code和result_code都为SUCCESS的时候有返回
                        //用户标识
                        openid = orderNotifyImpl.Get("openid"),
                        //是否关注公众账号
                        is_subscribe = orderNotifyImpl.Get("is_subscribe"),
                        //交易类型
                        trade_type = orderNotifyImpl.Get("trade_type"),
                        //付款银行
                        bank_type = orderNotifyImpl.Get("bank_type"),
                        //总金额
                        total_fee = orderNotifyImpl.Get("total_fee"),
                        //现金券金额
                        coupon_fee = orderNotifyImpl.Get("coupon_fee"),
                        //货币种类
                        fee_type = orderNotifyImpl.Get("fee_type"),
                        //微信支付订单号
                        transaction_id = orderNotifyImpl.Get("transaction_id"),
                        //商户订单号
                        out_trade_no = orderNotifyImpl.Get("out_trade_no"),
                        //商家数据包
                        attach = orderNotifyImpl.Get("attach"),
                        //支付完成时间
                        time_end = orderNotifyImpl.Get("time_end")
                        #endregion
                    };
                    #endregion

                    WriteWeixinLog("model:" + model.ToJson(), null);

                    if (model.return_code != "SUCCESS" || model.result_code != "SUCCESS")
                    {
                        WriteWeixinLog(string.Format("通信或业务结果失败，return_code:{0},result_code：{1},err_code:{2},err_code_des:{3}", model.return_code, model.result_code, model.err_code, model.err_code_des), null);
                    }

                    if (orderNotifyImpl.IsTenpaySign())
                    {
                        if (model.return_code == "SUCCESS" && model.result_code == "SUCCESS")
                        {
                            WriteWeixinLog("签名成功：\r\n", null);

                            var param = new UpdatePayStatusRequest()
                            {
                                Amount = decimal.Parse(model.total_fee) / 100,
                                VoucherNo = model.transaction_id,
                                OrderSysNo = model.out_trade_no
                            };

                            WriteWeixinLog("更新参数：\r\n" + param.ToJson2(), null);

                            var result = RechargeApp.Instance.UpdatePayStatus(param);

                            WriteWeixinLog(string.Format("更新订单:{0},支付状态{1}：\r\n", model.out_trade_no, result.ToJson()), null);

                            if (!result.Status)
                            {
                                returnCode = "FAIL";
                                returnMsg = result.Message;
                            }
                            returnCode = "SUCCESS";
                        }
                    }
                    else
                    {
                        returnMsg = "签名失败";
                        WriteWeixinLog("签名失败:" + model.ToJson(), null);
                    }
                }
                
            }
            catch (Exception ex )
            {
                WriteWeixinLog("exception:" + ex.Message, null);
            }
            string xml = string.Format(@"<xml>
                                            <return_code><![CDATA[{0}]]></return_code>
                                            <return_msg><![CDATA[{1}]]></return_msg>
                                         </xml>", returnCode, returnMsg);

            return Content(xml, "text/xml");
        }
        #endregion

        #region 操作日志
        /// <summary>
        /// 网银在线异步回调异常日志
        /// </summary>
        /// <param name="content">请求对象</param>
        /// <param name="folder">目录</param>
        /// <returns>void</returns>
        /// <remarks>2015-09-09 苟治国 创建</remarks>
        private void WriteLog(string content, string folder)
        {
            var sb = new StringBuilder("--------------------------------------------------------\r\n");
            sb.AppendLine("order：" + Request["out_trade_no"] == "" ? Request["v_oid"] : Request["out_trade_no"]);
            sb.AppendLine("logtime:" + DateTime.Now);
            try
            {
                sb.AppendLine("content:" + content);
            }
            catch { }
            Log4Helper.WriteInfoLog(sb.ToString(), folder);
        }

        /// <summary>
        /// 网银在线异步回调异常日志
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <param name="folder">目录</param>
        /// <returns>void</returns>
        /// <remarks>2015-09-09 苟治国 创建</remarks>
        private void WriteRequestLog(HttpRequestBase request, string folder)
        {
            var sb = new StringBuilder("--------------------------------------------------------\r\n");
            sb.AppendLine("order：" + Request["out_trade_no"] == "" ? Request["v_oid"] : Request["out_trade_no"]);
            sb.AppendLine("logtime:" + DateTime.Now);
            try
            {
                foreach (var key in request.Form.AllKeys)
                {
                    sb.AppendLine(key + ":" + Request.Form[key]);
                }
                foreach (var key in request.QueryString.AllKeys)
                {
                    sb.AppendLine(key + ":" + Request.QueryString[key]);
                }
                sb.AppendLine("REMOTE_ADDR:" + request.ServerVariables["REMOTE_ADDR"]);
                sb.AppendLine("REMOTE_HOST:" + request.ServerVariables["REMOTE_HOST"]);
                sb.AppendLine("REMOTE_PORT:" + request.ServerVariables["REMOTE_PORT"]);
                sb.AppendLine("REQUEST_METHOD:" + request.ServerVariables["REQUEST_METHOD"]);
            }
            catch { }
            Log4Helper.WriteInfoLog(sb.ToString(), folder);
        }

        /// <summary>
        /// 支付宝异步回调异常日志
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <param name="ex">异常对象</param>
        /// <param name="folder">目录</param>
        /// <returns>void</returns>
        /// <remarks>2015-09-09 苟治国 创建</remarks>
        private void WriteAlipaySyncExceptionLog(HttpRequestBase request, Exception ex, string folder)
        {
            var sb = new StringBuilder("--------------------------------------------------------\r\n");
            sb.AppendLine("order：" + Request["out_trade_no"] == "" ? Request["v_oid"] : Request["out_trade_no"]);
            sb.AppendLine("logtime:" + DateTime.Now);
            sb.AppendLine("message:" + ex.Message);
            Log4Helper.WriteInfoLog(sb.ToString(), folder);
        }

        /// <summary>
        /// 网银在线异步回调异常日志
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <param name="ex">异常对象</param>
        /// <param name="folder">目录</param>
        /// <returns>void</returns>
        private void WriteChinaBankSyncExceptionLog(HttpRequestBase request, Exception ex, string folder)
        {
            var sb = new StringBuilder("--------------------------------------------------------\r\n");
            var orderSysNo = Request["v_oid"].Split('-');
            sb.AppendLine("order：" + (orderSysNo.Length > 3 ? orderSysNo[2] : Request["v_oid"]));
            sb.AppendLine("logtime:" + DateTime.Now);
            sb.AppendLine("message:" + ex.Message);
            Log4Helper.WriteInfoLog(sb.ToString(), folder);

        }

        /// <summary>
        /// 微信回调普通日志
        /// </summary>
        /// <param name="content">日志内容</param>
        /// <param name="param">微信支付回调请求对象</param>
        /// <returns>void</returns>
        private void WriteWeixinLog(string content, WeixinRequest param)
        {
            if (param == null) param = new WeixinRequest();
            var sb = new StringBuilder("--------------------------------------------------------\r\n");
            sb.AppendLine("order：" + param.out_trade_no);
            sb.AppendLine("logtime:" + DateTime.Now);
            try
            {

                sb.AppendLine("content:" + content);
                Log4Helper.WriteInfoLog(sb.ToString(), "PayNotify");
            }
            catch { }

            LogApp.Instance.Info(new LogRequest()
            {
                Source = LogEnum.Source.前台,
                Message = sb.ToString(),
                Exception = null
            });
            
        }

        /// <summary>
        /// 微信回调异常日志
        /// </summary>
        /// <param name="ex">异常对象</param>
        /// <param name="param">微信支付回调请求对象</param>
        /// <returns>void</returns>
        private void WriteWeixinErrorLog(Exception ex, WeixinRequest param)
        {
            if (param == null) param = new WeixinRequest();
            var sb = new StringBuilder("--------------------------------------------------------\r\n");
            sb.AppendLine("order：" + param.out_trade_no);
            sb.AppendLine("logtime:" + DateTime.Now);
            sb.AppendLine("message:" + ex.Message);
            Log4Helper.WriteInfoLog(sb.ToString(), "PayNotify");
        }

        #endregion
    }
}
