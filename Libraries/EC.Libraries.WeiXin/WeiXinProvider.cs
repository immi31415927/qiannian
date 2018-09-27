using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Xml;
using EC.Libraries.Util;
using EC.Libraries.WeiXin.Impl;
using EC.Libraries.WeiXin.Model;

namespace EC.Libraries.WeiXin
{
    /// <summary>
    /// 微信
    /// </summary>
    public class WeiXinProvider
    {
        /// <summary>
        /// 授权
        /// </summary>
        /// <param name="appId">appId</param>
        /// <param name="redirect_uri">redirect_uri</param>
        /// <param name="code">code</param>
        public string GetAuthorize(string appId, string redirect_uri, string code)
        {
            var requestUrl = "https://open.weixin.qq.com/connect/oauth2/authorize?";

            Dictionary<String, String> dc = new Dictionary<string, string>();
            dc.Add("appid", appId);
            dc.Add("redirect_uri", redirect_uri);
            dc.Add("response_type", "code");
            dc.Add("scope", "snsapi_base");
            dc.Add("state", "STATE" + "#wechat_redirect");

            return WebUtil.buildParamStr(requestUrl, dc);
        }

        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <param name="appId">AppId</param>
        /// <param name="appSecret">appSecret</param>
        public AccessTokenResponse GetAccessToken(string appId, string appSecret)
        {
            var requestUrl = "https://api.weixin.qq.com/cgi-bin/token";

            var data = new HttpUtils().Get(requestUrl, new Dictionary<string, string>()
            {
                {"grant_type", "client_credential"},
                {"appid", appId},
                {"secret", appSecret}
            }, Encoding.GetEncoding("utf-8"));

            var accessTokenResponse = data.ToObject<AccessTokenResponse>();

            if (!string.IsNullOrWhiteSpace(accessTokenResponse.errcode))
            {
                accessTokenResponse.return_code = return_code.FAIL.ToString();

                accessTokenResponse.result_code = result_code.FAIL.ToString();
                accessTokenResponse.err_code = accessTokenResponse.errcode;

                return accessTokenResponse;
            }

            accessTokenResponse.return_code = return_code.SUCCESS.ToString();
            accessTokenResponse.result_code = result_code.SUCCESS.ToString();

            return accessTokenResponse;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="accessTokenOrAppId">AccessToken或AppId（推荐使用AppId，需要先注册）</param>
        /// <param name="openId">openId</param>
        public UserInfoResponse GetUserInfo(string accessTokenOrAppId, string openId)
        {
            var requestUrl = "https://api.weixin.qq.com/cgi-bin/user/info";

            var userInfoResponse = new HttpUtils().Get(requestUrl, new Dictionary<string, string>()
            {
                {"access_token", accessTokenOrAppId},
                {"openid", openId}
            }, Encoding.GetEncoding("utf-8"));

            return userInfoResponse.ToObject<UserInfoResponse>();
        }

        /// <summary>
        /// 获取微信jsapi_ticket
        /// </summary>
        public JSApiTicketResponse GetTicket(string appId, string accessToken, string url)
        {
            var requestUrl = "https://api.weixin.qq.com/cgi-bin/ticket/getticket";

            var data = new HttpUtils().Get(requestUrl, new Dictionary<string, string>()
            {
                {"access_token", accessToken},
                {"type", "jsapi"}
            }, Encoding.GetEncoding("utf-8"));

            var jsapiTicketResponse = data.ToObject<JSApiTicketResponse>();

            if (!jsapiTicketResponse.errcode.Equals("0"))
            {
                jsapiTicketResponse.return_code = return_code.FAIL.ToString();

                jsapiTicketResponse.result_code = result_code.FAIL.ToString();
                jsapiTicketResponse.err_code = jsapiTicketResponse.errcode;
                return jsapiTicketResponse;
            }

            jsapiTicketResponse.return_code = return_code.SUCCESS.ToString();
            jsapiTicketResponse.result_code = result_code.SUCCESS.ToString();

            if (jsapiTicketResponse.return_code.Equals(return_code.SUCCESS.ToString()) &&jsapiTicketResponse.result_code.Equals(result_code.SUCCESS.ToString()))
            {
                string nonceStr = Guid.NewGuid().ToString().Replace("-", "");
                string timestamp = WebUtil.GetTimeStamp();
                string signature = WebUtil.SHA1(string.Format("jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}", jsapiTicketResponse.ticket, nonceStr, timestamp, url));

                jsapiTicketResponse.appId = appId;
                jsapiTicketResponse.url = url;
                jsapiTicketResponse.jsapi_ticket = jsapiTicketResponse.ticket;
                jsapiTicketResponse.noncestr = nonceStr;
                jsapiTicketResponse.timestamp = timestamp;
                jsapiTicketResponse.signature = signature;
            }
            return jsapiTicketResponse;
        }

        /// <summary>
        /// 创建菜单(https://mp.weixin.qq.com/wiki?t=resource/res_main&id=mp1421141013)
        /// </summary>
        public CreateMenuResponse CreateMenu(string appId, string accessToken)
        {
            var response = new CreateMenuResponse()
            {
                //return_code = "FAI"
            };

            string requestUrl = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token=ACCESS_TOKEN";

            return response;
        }


        /// <summary>
        /// 统一下单 https://pay.weixin.qq.com/wiki/doc/api/jsapi.php?chapter=9_1
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public UnifiedOrderResponse UnifiedOrder(UnifiedOrderRequest request)
        {
            var response = new UnifiedOrderResponse()
            {
                return_code = "FAI"
            };

            string requestUrl = "https://api.mch.weixin.qq.com/pay/unifiedorder";

            PayImpl pay = new PayImpl()
            {
                apiKey = request.apiKey
            };

            pay.Add("appid", request.appid);           //公众账号ID
            pay.Add("mch_id", request.mch_id);         //商户号
            pay.Add("nonce_str", Util.GetNoncestr());  //随机字符串
            pay.Add("body", request.body);             //订单信息
            pay.Add("out_trade_no", request.order_no); //订单号
            pay.Add("total_fee", Convert.ToInt32(request.total_fee * 100).ToString());//商品金额,以分为单位(money * 100).ToString()
            pay.Add("spbill_create_ip", request.spbill_create_ip); //用户的公网ip，不是商户服务器IP
            pay.Add("notify_url", request.notify_url); //接收财付通通知的URL
            pay.Add("trade_type", request.trade_type); //交易类型
            pay.Add("openid", request.open_id);        //用户的openId
            var sign = pay.CreateMd5Sign();
            pay.Add("sign", sign);                     //签名

            var data = new HttpUtils().DoPost(requestUrl, pay.ParseXML(), false);

            var doc = new XmlDocument();
            doc.LoadXml(data);

            XmlNode xmlNode = doc.DocumentElement;
            if (xmlNode != null)
            {
                response.return_code = xmlNode["return_code"].InnerText;
                if (response.return_code.ToUpper() != "SUCCESS")
                {
                    response.return_msg = xmlNode["return_msg"].InnerText;
                    return response;
                }

                response.result_code = xmlNode["result_code"].InnerText;
                if (response.result_code.ToUpper() == "FAIL")
                {
                    var err_code_des = xmlNode["err_code_des"].InnerText;
                    if (err_code_des.Contains("订单号重复"))
                    {
                        response.err_code_des = "订单号重复";
                        return response;
                    }
                    response.err_code = "错误码:" + xmlNode["err_code"].InnerText;
                    response.err_code_des = "错误描述:" + xmlNode["err_code_des"].InnerText;
                    return response;
                }

                response.prepay_id = xmlNode["prepay_id"].InnerText;
                response.sign = xmlNode["sign"].InnerText;
                if (request.trade_type.Equals(trade_type.MWEB.ToString()))
                {
                    response.mweb_url = xmlNode["mweb_url"].InnerText;
                }
            }
            return response;
        }

        /// <summary>
        /// 申请退款(https://pay.weixin.qq.com/wiki/doc/api/jsapi.php?chapter=9_4)
        /// </summary>
        public RefundReponse Refund(RefundRequest request)
        {
            var response = new RefundReponse()
            {
                return_code = "FAI"
            };

            string requestUrl = "https://api.mch.weixin.qq.com/secapi/pay/refund";

            RefundImpl refund = new RefundImpl()
            {
                apiKey = request.apiKey
            };

            refund.Add("appid", request.appid);
            refund.Add("mch_id", request.mch_id);
            refund.Add("nonce_str", request.nonce_str);
            refund.Add("out_refund_no", request.out_refund_no);
            //refund.Add("out_trade_no", request.out_trade_no);
            refund.Add("refund_fee", Convert.ToInt32(request.refund_fee * 100).ToString());
            refund.Add("total_fee", Convert.ToInt32(request.total_fee * 100).ToString());
            refund.Add("transaction_id", request.transaction_id);
            var sign = refund.CreateMd5Sign();
            refund.Add("sign", sign);

            var data = new HttpUtils().DoPost(requestUrl, refund.ParseXML(), request.isUseCert, request.cert, request.password);
            var doc = new XmlDocument();
            doc.LoadXml(data);

            XmlNode xmlNode = doc.DocumentElement;
            if (xmlNode != null)
            {
                response.return_code = xmlNode["return_code"].InnerText;
                if (response.return_code.ToUpper() != "SUCCESS")
                {
                    response.return_msg = xmlNode["return_msg"].InnerText;
                    return response;
                }

                response.result_code = xmlNode["result_code"].InnerText;
                if (response.result_code.ToUpper() == "FAIL")
                {
                    response.err_code = "错误码:" + xmlNode["err_code"].InnerText;
                    response.err_code_des = "错误描述:" + xmlNode["err_code_des"].InnerText;
                    return response;
                }
            }

            return response;
        }

        /// <summary>
        /// 企业向微信用户个人付款https://pay.weixin.qq.com/wiki/doc/api/tools/mch_pay.php?chapter=14_2
        /// </summary>
        public TransfersResponse Transfers(TransfersRequest request)
        {
            var response = new TransfersResponse()
            {
                return_code = "FAI"
            };

            string requestUrl = "https://api.mch.weixin.qq.com/mmpaymkttransfers/promotion/transfers";

            TransfersImpl wxTransfers = new TransfersImpl()
            {
                apiKey = request.apiKey
            };

            wxTransfers.Add("mch_appid", request.mch_appid);
            wxTransfers.Add("mchid", request.mchid);
            wxTransfers.Add("nonce_str", request.nonce_str);
            wxTransfers.Add("partner_trade_no", request.partner_trade_no);
            wxTransfers.Add("openid", request.openid);
            wxTransfers.Add("check_name", "NO_CHECK");
            wxTransfers.Add("amount", Convert.ToInt32(request.amount * 100).ToString());
            wxTransfers.Add("desc", request.desc);
            wxTransfers.Add("spbill_create_ip", request.spbill_create_ip);
            var sign = wxTransfers.CreateMd5Sign();
            wxTransfers.Add("sign", sign);

            var xmlData = wxTransfers.ParseXML();

            var data = new HttpUtils().DoPost(requestUrl, xmlData, request.isUseCert, request.cert, request.password);
            var doc = new XmlDocument();
            doc.LoadXml(data);

            XmlNode xmlNode = doc.DocumentElement;
            if (xmlNode != null)
            {
                response.return_code = xmlNode["return_code"].InnerText;
                if (response.return_code.ToUpper() != "SUCCESS")
                {
                    response.return_msg = xmlNode["return_msg"].InnerText;
                    return response;
                }

                response.result_code = xmlNode["result_code"].InnerText;
                if (response.result_code.ToUpper() == "FAIL")
                {
                    response.err_code = "错误码:" + xmlNode["err_code"].InnerText;
                    response.err_code_des = "错误描述:" + xmlNode["err_code_des"].InnerText;
                    return response;
                }
            }
            return response;
        }

        /// <summary>
        /// 刷卡支付
        /// </summary>
        /// <param name="request"></param>
        public BarCodePayResponse BarCodePay(BarCodePayRequest request)
        {
            var response = new BarCodePayResponse()
            {
                return_code = "FAI"
            };

            string requestUrl = "https://api.mch.weixin.qq.com/pay/micropay";

            BarCodePayImpl barCodePay = new BarCodePayImpl()
            {
                apiKey = request.apiKey
            };

            barCodePay.Add("appid", request.appid);//公众账号ID
            barCodePay.Add("mch_id", request.mch_id);//商户号
            barCodePay.Add("nonce_str", Guid.NewGuid().ToString().Replace("-", ""));//随机字符串
            barCodePay.Add("body", request.body);                                   //商品描述
            barCodePay.Add("total_fee", Convert.ToInt32(request.total_fee * 100).ToString());      //总金额 （单位：分）
            barCodePay.Add("out_trade_no", request.out_trade_no);                   //商户订单号
            barCodePay.Add("spbill_create_ip", request.spbill_create_ip);           //终端IP
            barCodePay.Add("auth_code", request.auth_code);                         //授权码
            var sign = barCodePay.CreateMd5Sign();
            barCodePay.Add("sign", sign);//签名

            var data = new HttpUtils().DoPost(requestUrl, barCodePay.ParseXML(), false);
            var doc = new XmlDocument();
            doc.LoadXml(data);

            XmlNode xmlNode = doc.DocumentElement;
            if (xmlNode != null)
            {
                response.return_code = xmlNode["return_code"].InnerText;
                if (response.return_code.ToUpper() != "SUCCESS")
                {
                    response.return_msg = xmlNode["return_msg"].InnerText;
                    return response;
                }

                response.result_code = xmlNode["result_code"].InnerText;
                if (response.result_code.ToUpper() == "FAIL")
                {
                    response.err_code = "错误码:" + xmlNode["err_code"].InnerText;
                    response.err_code_des = "错误描述:" + xmlNode["err_code_des"].InnerText;
                    return response;
                }

                //商户订单号
                response.out_trade_no = xmlNode["out_trade_no"].InnerText;
                response.transaction_id = xmlNode["transaction_id"].InnerText;
                response.total_fee = Convert.ToInt32(xmlNode["total_fee"].InnerText);
            }

            return response;
        }

        /// <summary>
        /// 刷卡支付查询订单
        /// </summary>
        public OrderQueryResponse Orderquery(OrderQueryRequest request)
        {
            var response = new OrderQueryResponse()
            {
                return_code = "FAI"
            };

            string requestUrl = "https://api.mch.weixin.qq.com/pay/orderquery";

            OrderqueryImpl orderQuery = new OrderqueryImpl()
            {
                apiKey = request.apiKey
            };
            orderQuery.Add("appid", request.appid);//公众账号ID
            orderQuery.Add("mch_id", request.mch_id);//商户号
            orderQuery.Add("nonce_str", Guid.NewGuid().ToString().Replace("-", ""));//随机字符串
            orderQuery.Add("out_trade_no", request.out_trade_no);
            var sign = orderQuery.CreateMd5Sign();
            orderQuery.Add("sign", sign);//签名

            var data = new HttpUtils().DoPost(requestUrl, orderQuery.ParseXML(), false);
            var doc = new XmlDocument();
            doc.LoadXml(data);

            XmlNode xmlNode = doc.DocumentElement;
            if (xmlNode != null)
            {
                response.return_code = xmlNode["return_code"].InnerText;
                if (response.return_code.ToUpper() != "SUCCESS")
                {
                    response.return_msg = xmlNode["return_msg"].InnerText;
                    return response;
                }

                response.result_code = xmlNode["result_code"].InnerText;
                if (response.result_code.ToUpper() == "FAIL")
                {
                    response.err_code = "错误码:"+xmlNode["err_code"].InnerText;
                    response.err_code_des = "错误描述:"+xmlNode["err_code_des"].InnerText;
                    return response;
                }
                //商户订单号
                response.out_trade_no = xmlNode["out_trade_no"].InnerText;
                //微信支付订单号
                response.transaction_id = xmlNode["transaction_id"].InnerText;
                //订单金额
                response.total_fee = Convert.ToInt32(xmlNode["total_fee"].InnerText);
            }
            return response;
        }

        /// <summary>
        /// 支付结果通知 https://pay.weixin.qq.com/wiki/doc/api/jsapi.php?chapter=9_7
        /// </summary>
        public OrderNotifyResponse OrderNotify(NotifyRequest request)
        {
            var orderNotifyImpl = new OrderNotifyImpl()
            {
                apiKey = request.apiKey
            };

            var xml = @"
                    <xml>
                        <appid><![CDATA[wx9535283296d186c3]]></appid>
                        <bank_type><![CDATA[CFT]]></bank_type>
                        <cash_fee><![CDATA[1]]></cash_fee>
                        <fee_type><![CDATA[CNY]]></fee_type>
                        <is_subscribe><![CDATA[Y]]></is_subscribe>
                        <mch_id><![CDATA[1491518502]]></mch_id>
                        <nonce_str><![CDATA[5D078CEBD641FB4524960067EDE27592]]></nonce_str>
                        <openid><![CDATA[o121v0wIoFtd8yWZiaezCVTU_Pqg]]></openid>
                        <out_trade_no><![CDATA[4b3439afdf364d71b56d4b9594f9709d]]></out_trade_no>
                        <result_code><![CDATA[SUCCESS]]></result_code>
                        <return_code><![CDATA[SUCCESS]]></return_code>
                        <sign><![CDATA[7FA9A764835E91BFC909D021734B5F86]]></sign>
                        <time_end><![CDATA[20180114204939]]></time_end>
                        <total_fee>1</total_fee>
                        <trade_type><![CDATA[MWEB]]></trade_type>
                        <transaction_id><![CDATA[4200000093201801144137104667]]></transaction_id>
                    </xml>
                    ";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            XmlNode xmlNode = doc.SelectSingleNode("xml");
            XmlNodeList xmlNodeList = xmlNode.ChildNodes;

            foreach (XmlNode xnf in xmlNodeList)
            {
                //this.SetParameter(xnf.Name, xnf.InnerText);
                orderNotifyImpl.Add(xnf.Name, xnf.InnerText);
            }

            ///var orderNotifyResponse = new OrderNotifyResponse();


            //orderNotifyImpl.IsTenpaySign();

            //var orderNotify = new OrderNotifyRequest()
            //{
            //    //返回状态码 SUCCESS/FAIL 此字段是通信标识，非交易标识，交易是否成功需要查看 result_code来判断
            //    return_code = orderNotifyImpl.Get("return_code"),
            //    //返回信息，如非空为错误原因 1：签名失败 2：参数格式校验错误
            //    return_msg = orderNotifyImpl.Get("return_msg"),
            //    //业务结果
            //    result_code = orderNotifyImpl.Get("result_code"),
            //    //错误代码
            //    err_code = orderNotifyImpl.Get("err_code"),
            //    //错误代码描述
            //    err_code_des = orderNotifyImpl.Get("err_code_des"),

            //    #region 以下字段在return_code为SUCCESS的时候有返回
            //    //微信公众账号id
            //    appid = orderNotifyImpl.Get("appid"),
            //    //商户号
            //    mch_id = orderNotifyImpl.Get("mch_id"),
            //    //终端设备号
            //    device_info = orderNotifyImpl.Get("device_info"),
            //    //随机字符串
            //    nonce_str = orderNotifyImpl.Get("nonce_str"),
            //    //签名
            //    sign = orderNotifyImpl.Get("sign"),
            //    #endregion

            //    #region 以下字段在return_code和result_code都为SUCCESS的时候有返回
            //    //用户标识
            //    openid = orderNotifyImpl.Get("openid"),
            //    //是否关注公众账号
            //    is_subscribe = orderNotifyImpl.Get("is_subscribe"),
            //    //交易类型
            //    trade_type = orderNotifyImpl.Get("trade_type"),
            //    //付款银行
            //    bank_type = orderNotifyImpl.Get("bank_type"),
            //    //总金额
            //    total_fee = orderNotifyImpl.Get("total_fee"),
            //    //现金券金额
            //    coupon_fee = orderNotifyImpl.Get("coupon_fee"),
            //    //货币种类
            //    fee_type = orderNotifyImpl.Get("fee_type"),
            //    //微信支付订单号
            //    transaction_id = orderNotifyImpl.Get("transaction_id"),
            //    //商户订单号
            //    out_trade_no = orderNotifyImpl.Get("out_trade_no"),
            //    //商家数据包
            //    attach = orderNotifyImpl.Get("attach"),
            //    //支付完成时间
            //    time_end = orderNotifyImpl.Get("time_end")
            //    #endregion
            //};
            //


            //var requestStream = System.Web.HttpContext.Current.Request.InputStream;

            //byte[] requestByte = new byte[requestStream.Length];
            //requestStream.Read(requestByte, 0, (int) requestStream.Length);
            //string request = Encoding.UTF8.GetString(requestByte);


            return null;
        }

        /// <summary>
        /// 企业付款到银行卡https://pay.weixin.qq.com/wiki/doc/api/tools/mch_pay.php?chapter=24_2
        /// </summary>
        /// <param name="request"></param>
        public PayBankResponse PayBank(PayBankRequest request)
        {
            var response = new PayBankResponse()
            {
                return_code = "FAI"
            };

            string requestUrl = "https://api.mch.weixin.qq.com/mmpaysptrans/pay_bank";

            PayBankImpl payBank = new PayBankImpl()
            {
                apiKey = request.apiKey
            };

            payBank.Add("mch_id", request.mch_id);
            payBank.Add("nonce_str", request.desc);
            payBank.Add("amount", request.amount.ToString());
            payBank.Add("bank_code", request.bank_code);
            payBank.Add("account_type", request.account_type.ToString());
            payBank.Add("bank_note", request.bank_note);
            payBank.Add("desc", request.desc);
            payBank.Add("enc_bank_no", request.enc_bank_no);
            payBank.Add("enc_true_name", request.enc_true_name);
            payBank.Add("partner_trade_no", request.partner_trade_no);
            var sign = payBank.CreateMd5Sign();
            payBank.Add("sign", sign);

            var data = new HttpUtils().DoPost(requestUrl, payBank.ParseXML(), request.isUseCert, request.cert, request.password);
            var doc = new XmlDocument();
            doc.LoadXml(data);

            XmlNode xmlNode = doc.DocumentElement;
            if (xmlNode != null)
            {
                response.return_code = xmlNode["return_code"].InnerText;
                if (response.return_code.ToUpper() != "SUCCESS")
                {
                    response.return_msg = xmlNode["return_msg"].InnerText;
                    return response;
                }

                response.result_code = xmlNode["result_code"].InnerText;
                if (response.result_code.ToUpper() == "FAIL")
                {
                    response.err_code = xmlNode["err_code"].InnerText;
                    response.err_code_des = xmlNode["err_code_des"].InnerText;
                    return response;
                }
                response.return_code = "SUCCESS";
            }
            return response;
        }

        /// <summary>
        /// 加密码公钥
        /// </summary>
        /// <param name="request"></param>
        public PublickeyResponse GetPublickey(PublickeyRequest request)
        {
            var response = new PublickeyResponse()
            {
                return_code = "FAI"
            };

            string requestUrl = "https://fraud.mch.weixin.qq.com/risk/getpublickey";

            PublickeyImpl publickey = new PublickeyImpl()
            {
                apiKey = request.apiKey
            };

            publickey.Add("mch_id", request.mch_id);
            publickey.Add("nonce_str", request.nonce_str);
            publickey.Add("sign_type", "MD5");
            var sign = publickey.CreateMd5Sign();
            publickey.Add("sign", sign);

            var data = new HttpUtils().DoPost(requestUrl, publickey.ParseXML(), request.isUseCert, request.cert, request.password);
            var doc = new XmlDocument();
            doc.LoadXml(data);


            XmlNode xmlNode = doc.DocumentElement;
            if (xmlNode != null)
            {
                response.return_code = xmlNode["return_code"].InnerText;
                if (response.return_code.ToUpper() != "SUCCESS")
                {
                    response.return_msg = xmlNode["return_msg"].InnerText;
                    return response;
                }

                response.result_code = xmlNode["result_code"].InnerText;
                if (response.result_code.ToUpper() == "FAIL")
                {
                    response.err_code = xmlNode["err_code"].InnerText;
                    response.err_code_des = xmlNode["err_code_des"].InnerText;
                    return response;
                }
                response.mch_id = xmlNode["mch_id"].InnerText;
                response.pub_key = xmlNode["pub_key"].InnerText;
                response.return_code = "SUCCESS";
            }
            return response;
        }

        /// <summary>  
        /// 创建二维码ticket  
        /// </summary>  
        /// <param name="accessToken">accessToken</param>
        /// <param name="sceneStr">字符串类型的参数</param>
        /// <returns>二维码ticket</returns>  
        public static string CreateTicket(string accessToken, string sceneStr)
        {
            var response = "";

            
            var requestUrl = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=" + accessToken;

            var data = @"{""action_name"": ""QR_LIMIT_STR_SCENE"", ""action_info"": {""scene"": {""scene_str"":""" + sceneStr + "\"}}}";

            var webClient = new WebClient
            {
                Credentials = CredentialCache.DefaultCredentials
            };

            try
            {
                response = webClient.UploadString(requestUrl, "POST", data);

                var ticket = response.ToObject<Ticket>();
                if (!String.IsNullOrEmpty(ticket.errcode) && (ticket.errcode.Equals("40001") || ticket.errcode.Equals("41001")))
                {
                    response = ticket.errcode;
                }
                else
                {
                    response = ticket.ticket;
                }
            }
            catch (Exception ex)
            {
                response = "Error:" + ex.Message;
            }
            return response;
        }

        /// <summary>
        /// 使用微信模板发送固定消息https://mp.weixin.qq.com/wiki?t=resource/res_main&id=mp1421140543
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
        public static string SendTemplateMessage(string accessToken, string openId, string templateId, object data,string url = "")
        {
            return "";
        }
    }
}
