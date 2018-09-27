using System;
using EC.DataAccess.CRM;
using EC.DataAccess.Fn;
using EC.Entity;
using EC.Entity.Enum;
using EC.Entity.Parameter.Request.Fn;
using EC.Entity.Tables.Finance;
using EC.Libraries.Core.Pager;
using EC.Libraries.Core.Transaction;

namespace EC.Application.Tables.Fn
{
    using EC.Libraries.Core;
    using EC.Libraries.Util;
    using EC.Entity.Tables.CRM;
    using EC.Libraries.WeiXin;
    using EC.Libraries.WeiXin.Model;
    using System.Collections.Generic;
    using EC.Libraries.Core.Log;

    /// <summary>
    /// 提现申请业务层
    /// </summary>
    public class FnCashoutApp : Base<FnCashoutApp>
    {
        #region 获取
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        public FnCashout Get(int sysNo)
        {
            var result = Using<IFnCashout>().Get(sysNo);

            return result;
        }
        #endregion

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="sysNo">编码</param>
        /// <param name="status">状态</param>
        /// <returns>影响行数</returns>
        public JResult UpdateStatus(int sysNo, int status)
        {
            var response = new JResult()
            {
                Status = false
            };

            if (!EnumUtil.IsEnumExist<FnEnum.CashoutStatus>(status))
            {
                throw new Exception("数据不合法");
            }

            if (Using<IFnCashout>().UpdateStatus(sysNo, status, "") <= 0)
            {
                throw new Exception("修改数据失败");
            }

            response.Message = "操作成功！";
            response.Status = true;

            return response;
        }

        /// <summary>
        /// 驳回
        /// </summary>
        /// <param name="sysNo">编码</param>
        /// <param name="status">状态</param>
        /// <returns>影响行数</returns>
        public JResult CashoutLogReject(int sysNo,string reject)
        {
            var response = new JResult()
            {
                Status = false
            };

            //if (!EnumUtil.IsEnumExist<FnEnum.CashoutStatus>(status))
            //{
            //    throw new Exception("数据不合法");
            //}

            using (var tran = new TransactionProvider())
            {
                try
                {
                    var result = Using<IFnCashout>().Get(sysNo);
                    if (result != null) {
                        if (Using<IFnCashout>().UpdateStatus(sysNo, FnEnum.CashoutStatus.驳回.GetHashCode(), reject) <= 0)
                        {
                            throw new Exception("修改数据失败");
                        }

                        var customer = Using<ICrCustomer>().Get(result.CustomerSysNo);

                        var walletAmount = result.Amount * 2;
                        var upgradeFundAmount = result.Amount;

                        if(customer.Grade==CustomerEnum.Grade.股东.GetHashCode()){
                            walletAmount = result.Amount;
                            upgradeFundAmount = 0;
                        }

                        var para = new CrCustomer() {
                            SysNo = result.CustomerSysNo,
                            WalletAmount = walletAmount,
                            UpgradeFundAmount = result.Amount
                        };

                        if (Using<ICrCustomer>().Reject(para) <= 0) {
                            throw new Exception("修改数据失败");
                        }

                        response.Message = "操作成功！";
                        response.Status = true;
                        tran.Commit();

                    }
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw new Exception(ex.Message);
                }
                finally
                {
                    tran.Dispose();
                }
            }



            return response;
        }


        /// <summary>
        /// 驳回
        /// </summary>
        /// <param name="sysNo">编码</param>
        /// <returns>影响行数</returns>
        public JResult CashoutWeiXin(int sysNo,string certPath)
        {
            var response = new JResult()
            {
                Status = false
            };

            var cashout = Using<IFnCashout>().Get(sysNo);
            if (cashout == null) {
                response.Message = "记录不存在！";
                return response;
            }

            if (cashout.Status != FnEnum.CashoutStatus.处理中.GetHashCode()) {
                response.Message = "当前状态不能提现！";
                return response;
            }

            var customer = Using<ICrCustomer>().Get(cashout.CustomerSysNo);
            if (customer == null)
            {
                response.Message = "会员不存在！";
                return response;
            }
            if (string.IsNullOrEmpty(customer.OpenId)) {
                response.Message = "会员openId为空！";
                return response;
            }

            //<xml>
            //<return_code><![CDATA[SUCCESS]]></return_code>
            //<return_msg><![CDATA[]]></return_msg>
            //<mch_appid><![CDATA[wx9535283296d186c3]]></mch_appid>
            //<mchid><![CDATA[1491518502]]></mchid>
            //<nonce_str><![CDATA[1213546623546]]></nonce_str>
            //<result_code><![CDATA[SUCCESS]]></result_code>
            //<partner_trade_no><![CDATA[1111111223]]></partner_trade_no>
            //<payment_no><![CDATA[1000018301201805187873493468]]></payment_no>
            //<payment_time><![CDATA[2018-05-18 22:49:13]]></payment_time>
            //</xml>

            WeiXinProvider wx = new WeiXinProvider();

            var result = wx.Transfers(new TransfersRequest()
            {
                mch_appid = "wx9535283296d186c3",
                mchid = "1491518502",
                nonce_str = "1213546623546",
                partner_trade_no = cashout.SysNo.ToString(),
                openid = customer.OpenId,
                check_name = customer.RealName,
                amount = cashout.Amount,
                desc = "代理商佣金提现",
                spbill_create_ip = WebUtil.GetUserIp(),
                apiKey = "0bad12ce2b775367347d0b3a4bacd698",
                isUseCert = true,
                cert = certPath,
                password = "1491518502"
            });

            if (result.return_code.ToUpper() == "SUCCESS" && result.result_code.ToUpper() == "SUCCESS")
            {
                if (Using<IFnCashout>().UpdateStatus(sysNo, FnEnum.CashoutStatus.完成.GetHashCode(), "") <= 0)
                {
                    throw new Exception("修改数据失败");
                }

                response.Message = "操作成功！";
                response.Status = true;
            }
            else
            {
                response.Message = result.return_msg + result.err_code_des;
            }

            return response;
        }

        /// <summary>
        /// 驳回
        /// </summary>
        /// <param name="sysNo">编码</param>
        /// <returns>影响行数</returns>
        public JResult H5CashoutWeiXin(int sysNo, string certPath)
        {
            var response = new JResult()
            {
                Status = false
            };

            var cashout = Using<IFnCashout>().Get(sysNo);
            if (cashout == null)
            {
                response.Message = "记录不存在！";
                return response;
            }

            var customer = Using<ICrCustomer>().Get(cashout.CustomerSysNo);
            if (customer == null)
            {
                response.Message = "会员不存在！";
                return response;
            }
            if (string.IsNullOrEmpty(customer.OpenId))
            {
                response.Message = "会员openId为空！";
                return response;
            }

            WeiXinProvider wx = new WeiXinProvider();

            var result = wx.Transfers(new TransfersRequest()
            {
                mch_appid = "wx9535283296d186c3",
                mchid = "1491518502",
                nonce_str = "1213546623546",
                partner_trade_no = cashout.SysNo.ToString(),
                openid = customer.OpenId,
                check_name = customer.RealName,
                amount = cashout.Amount,
                desc = "代理商佣金提现",
                spbill_create_ip = WebUtil.GetUserIp(),
                apiKey = "0bad12ce2b775367347d0b3a4bacd698",
                isUseCert = true,
                cert = certPath,
                password = "1491518502"
            });

            Log4Helper.WriteInfoLog("EC.H5 提现" + result.ToJson2(), "Cashout");

            if (result.return_code.ToUpper() == "SUCCESS" && result.result_code.ToUpper() == "SUCCESS")
            {
                response.Message = "操作成功！";
                response.Status = true;
            }
            else
            {
                response.Message = result.return_msg + result.err_code_des;
            }

            return response;
        }

        /// <summary>
        /// 查询某个时间段发送短信数量
        /// </summary>
        /// <param name="requeest">参数</param>
        public List<FnCashout> GetPeriodTimes(FnCashoutQueryRequest requeest)
        {
            return Using<IFnCashout>().GetPeriodTimes(requeest);
        }

        /// <summary>
        /// 提现申请
        /// </summary>
        /// <param name="request">参数</param>
        public JResult Cashout(CashoutRequest request)
        {
            var response = new JResult()
            {
                Status = false
            };

            using (var tran = new TransactionProvider())
            {
                try
                {
                    //更新会员钱包(钱包金额-提现金额)(升级金额+提现金额/(如果是普通、区域、全国才除)2)
                    if (Using<ICrCustomer>().UpdateWalletAmount(new UpdateWalletAmountRequest()
                    {
                        CustomerSysNo = request.CustomerSysNo,
                        WalletAmount = request.WalletAmount,
                        UpgradeFundAmount = request.UpgradeFundAmount
                    }) <= 0)
                    {
                        throw new Exception("更新会员钱包失败");
                    }
                    //添加提现记录
                    var amount = request.WalletAmount - request.UpgradeFundAmount;
                    var model = new FnCashout()
                    {
                        CustomerSysNo = request.CustomerSysNo,
                        RealName = request.Account,
                        CashoutType = request.CashoutType,
                        Amount = amount,
                        Remarks = string.Format("提现金额:{0} 实际到账金额:{1} 升级基金:{2}", request.WalletAmount, request.WalletAmount - request.UpgradeFundAmount, request.UpgradeFundAmount),
                        Status = amount < 200?FnEnum.CashoutStatus.完成.GetHashCode(): FnEnum.CashoutStatus.处理中.GetHashCode(),//FnEnum.CashoutStatus.处理中.GetHashCode()
                        CreatedDate = DateTime.Now
                    };

                    var cashoutSysNo = Using<IFnCashout>().Insert(model);
                    if (cashoutSysNo <= 0)
                    {
                        throw new Exception("添加提现记录失败");
                    }

                    if (amount < 200) {
                        var result = FnCashoutApp.Instance.H5CashoutWeiXin(cashoutSysNo, request.CertPath);

                        Log4Helper.WriteInfoLog("EC.H5 提现" + result.ToJson2(), "Cashout");

                        if (!result.Status) {
                            throw new Exception(result.Message);
                        }
                    }

                    tran.Commit();
                    response.Status = true;
                    response.Message = "提现申请成功、等待管理员审核！";
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    response.Message = ex.Message;
                }
                finally
                {
                    tran.Dispose();
                }
            }

            return response;
        }

        /// <summary>
        /// 获取提现申请分页列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>提现申请分页列表</returns>
        public PagedList<FnCashout> GetPagerList(FnCashoutQueryRequest request)
        {
            return Using<IFnCashout>().GetPagerList(request);
        }
    }
}
