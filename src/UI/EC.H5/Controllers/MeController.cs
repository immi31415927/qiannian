using System;
using System.Linq;
using System.Web.Mvc;
using EC.Application.Tables.CRM;
using EC.Entity.Parameter.Request.NewCRM;
using EC.Application.Tables.Fn;
using EC.Application.Tables.WeiXin;
using EC.Entity;
using EC.Entity.Enum;
using EC.Entity.Parameter.Request.CRM;
using EC.Entity.Parameter.Request.Finance;
using EC.Entity.Parameter.Request.Fn;
using EC.Entity.Parameter.Request.WeiXin;
using EC.Entity.Parameter.Response.CRM;
using EC.Entity.Tables.CRM;
using EC.Entity.Tables.Finance;
using EC.Libraries.Core.Log;
using EC.Libraries.Core.Transaction;
using EC.Libraries.Core.Validator;
using EC.Libraries.Core.Validator.Rule;
using EC.Libraries.Util;
using EC.Libraries.WeiXin;
using EC.Application.Tables.Bs;
using EC.Entity.View.CRM;
using System.Collections.Generic;
using EC.Entity.Parameter.Request.Member;

namespace EC.H5.Controllers
{

    [AuthorizeFilter(true)]
    public class MeController : BaseController
    {
        /// <summary>
        /// 锁对象
        /// </summary>
        private static object _lock = new object();

        #region 首页
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //当前上下文
            var context = CustomerContext.Context;

            //会员扩展
            var customer = CustomerApp.Instance.Get(context.SysNo);
            if (customer == null)
            {
                return View("Error", new JResult() { Message = "扩展会员不存在﹗" });
            }

            var result = WeiXinApp.Instance.SendMessage(new SendMessageRequest()
            {
                WeiXinAppId = "wx9535283296d186c3",
                WeiXinAppSecret = "543e75c11909f438c02870ecbab85f5d",
                OpenId = "o121v0wIoFtd8yWZiaezCVTU_Pqg",
                TemplateId = "a8oNldLv7Nlvbkajt6vQbWUBOgmu7EWW2j08UzdQgBg",
                Data = new
                {
                    first = new { color = "#000000", value = "您有一笔新的分红奖金到账啦！" },
                    keyword1 = new { color = "#000000", value = "0.00元" },
                    keyword2 = new { color = "#000000", value = "0.00元" },
                    remark = new { color = "#000000", value = "会员：" + context.RealName }
                },
                Url = "#"
            });

            //计算总收益额
            int bonusSum = FnBonusLogApp.Instance.GetBonusSum(customer.SysNo);

            ViewBag.bonusSum = bonusSum.ToString("f");
            return View(customer);
        }
        #endregion

        #region 基金展示
        /// <summary>
        /// 基金展示
        /// </summary>
        /// <returns>分部视图</returns>
        public ActionResult FundShow()
        {
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

            ViewBag.WalletAmount = decimal.Round(customerExt.WalletAmount, 2);
            ViewBag.UpgradeFundAmount = decimal.Round(customerExt.UpgradeFundAmount, 2);
            ViewBag.BonusAmount = decimal.Round(customerExt.GeneralBonus + customerExt.AreaBonus + customerExt.GlobalBonus, 2);

            
            return PartialView();
        }
        #endregion

        #region 个人资料
        /// <summary>
        /// 个人资料
        /// </summary>
        /// <returns></returns>
        public ActionResult Info()
        {
            //当前上下文
            var context = CustomerContext.Context;
            if (context == null)
            {
                return View("Error", new JResult() { Message = "用户已超时、请重新登录﹗" });
            }

            ViewBag.bankTypeList = EnumUtil.ToDictionary(typeof(CustomerEnum.BankType));

            return View(context);
        }

        /// <summary>
        /// 个人资料
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <returns>视图</returns>
        [HttpPost]
        public JsonResult Profile(ProfileRequest request)
        {
            var response = new JResult()
            {
                Status = false
            };

            var result = VHelper.ValidatorRule(
                new Rule_NotAllowNull(request.BankNumber, "输入银行卡！")
            );

            if (!result.IsPass)
            {
                response.Status = false;
                response.Message = result.Message;
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            //当前上下文
            var context = CustomerContext.Context;
            if (context == null)
            {
                response.Message = "用户已超时、请重新登录﹗";
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            var model = new CrCustomer()
            {
                SysNo = context.SysNo,
                Bank = request.Bank,
                BankNumber = request.BankNumber,
            };
            int row = CustomerApp.Instance.UpdateBankInfo(model);
            if (row > 0)
            {
                response.Status = true;
                response.Message = "更新用户资料成功﹗";
            }
            else
            {
                response.Message = "更新用户资料失败﹗";
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 修改密码
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        public ActionResult Password()
        {
            return View();
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <returns>JResult</returns>
        [HttpPost]
        public JsonResult Password(PasswordRequest request)
        {
            var response = new JResult()
            {
                Status = false
            };

            var result = VHelper.ValidatorRule(
                new Rule_NotAllowNull(request.OldPassword, "请输入旧密码！"),
                new Rule_StringLenth(request.Password, min: 6, max: 20, message: "新密码长度在6-20位之间"),
                new Rule_StringLenth(request.ConfirmPassword, min: 6, max: 20, message: "新确认密码长度在6-20位之间")
            );

            if (!result.IsPass)
            {
                response.Status = false;
                response.Message = result.Message;
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            //新密码与确认密码验证
            if (!request.Password.Equals(request.ConfirmPassword))
            {
                response.Message = "两次输入密码不一致！";
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            //当前上下文
            var customer = CustomerContext.Context;

            //旧密码验证
            if (!EncryptionUtil.VerifyCiphetrextWithMd5AndSalt(request.OldPassword, customer.Password))
            {
                response.Status = false;
                response.Message = "原登录密码错误！";
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            try
            {
                customer.Password = EncryptionUtil.EncryptWithMd5AndSalt(request.Password);
                if ( CustomerApp.Instance.UpdatePassword(customer.SysNo,customer.Password).Status)
                {
                    response.Status = true;
                    response.Message = "修改密码成功﹗";
                }
                else
                {
                    response.Message = "修改密码失败﹗";
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 奖金记录
        /// <summary>
        /// 奖金记录
        /// </summary>
        /// <returns></returns>
        public ActionResult BonusLog()
        {
            //当前上下文
            var context = CustomerContext.Context;
            if (context == null)
            {
                return View("Error", new JResult() { Message = "用户已超时、请重新登录﹗" });
            }
            //会员
            var customer = CustomerApp.Instance.Get(context.SysNo);
            if (customer == null)
            {
                return View("Error", new JResult() { Message = "扩展会员不存在﹗" });
            }
            return View();
        }

        /// <summary>
        /// 奖金记录查询
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>分部视图</returns>
        public JsonResult BonusLogQuery(FnBonusLogQueryRequest request)
        {
            request.CustomerSysNo = CustomerContext.Context.SysNo;

            var list = FnBonusLogApp.Instance.GetPagerList(request);
            var data = new
            {
                Status = true,
                Data = list.TData.ToList(),
                Count = list.TotalCount
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 提现申请
        /// <summary>
        /// 提现申请
        /// </summary>
        /// <returns></returns>
        public ActionResult Cashout()
        {
            //当前上下文
            var context = CustomerContext.Context;
            if (context == null)
            {
                return View("Error", new JResult() { Message = "用户已超时、请重新登录﹗" });
            }

            //会员扩展
            var customerExt = CustomerApp.Instance.Get(context.SysNo);
            if (customerExt == null)
            {
                return View("Error", new JResult() { Message = "扩展会员不存在﹗" });
            }

            ViewBag.CashoutType = EnumUtil.ToDictionary(typeof(FnEnum.CashoutType));

            return View(customerExt);
        }

        /// <summary>
        /// 实际到账金额计算
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CashoutCalculate(CalculateRequest request)
        {
            var response = new JResult()
            {
                Status = false
            };

            //当前上下文
            var context = CustomerContext.Context;
            if (context == null)
            {
                response.Message = "用户已超时、请重新登录﹗";
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            //会员扩展
            var customerExt = CustomerApp.Instance.Get(context.SysNo);
            if (customerExt == null)
            {
                response.Message = "扩展会员不存在﹗";
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            if (request.Amount > customerExt.WalletAmount)
            {
                response.Message = "当前余额不足﹗";
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            if (request.Amount < 10)
            {
                response.Message = "提现金额必须为10元﹗";
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            //if (customerExt.Grade.Equals(CustomerEnum.Grade.股东.GetHashCode()))
            //{
            //    response.Status = true;
            //    response.StatusCode = request.Amount.ToString("F");
            //}
            //else
            //{
            //    response.Status = true;
            //    response.StatusCode = (request.Amount / 2).ToString("F");
            //}
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 提现申请
        /// </summary>
        /// <param name="request">参数</param>
        [HttpPost]
        public JsonResult Cashout(CashoutRequest request)
        {
            var response = new JResult()
            {
                Status = false
            };

            var result = VHelper.ValidatorRule(
                new Rule_Number(request.Amount.ToString(), "请输入提现金额！")
            );

            if (!result.IsPass)
            {
                response.Status = false;
                response.Message = result.Message;
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            lock (_lock)
            {
                Log4Helper.WriteInfoLog("EC.H5 提现" + result.ToJson2(), "Cashout");
                //当前上下文
                var context = CustomerContext.Context;
                if (context == null)
                {
                    response.Message = "用户已超时、请重新登录﹗";
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
                if (request.Amount <= 0)
                {
                    response.Message = "提现金额不能为零﹗";
                    return Json(response, JsonRequestBehavior.AllowGet);
                }

                var periodList = FnCashoutApp.Instance.GetPeriodTimes(new FnCashoutQueryRequest() {
                    CustomerSysNo = context.SysNo,
                    StartTime = DateTime.Now.ToString("yyyy-MM-dd 00:00:00"),
                    EndTime = DateTime.Now.ToString("yyyy-MM-dd 23:59:59")
                });
                if (periodList != null && periodList.Any()) {
                    response.Message = "每天只能提现一次﹗";
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
                //提现金额判断(100元的倍数)
                //var remainder = request.Amount % 100;
                //if (remainder != 0)
                //{
                //    response.Message = "提现金额必须为100的倍数﹗";
                //    return Json(response, JsonRequestBehavior.AllowGet);
                //}
                if (request.Amount < 10) {
                    response.Message = "提现金额必须为10元﹗";
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
                //验证是否填写银行卡
                if (request.CashoutType.Equals(FnEnum.CashoutType.银联.GetHashCode()))
                {
                    if (context.Bank <= 0 && context.BankNumber == "")
                    {
                        response.Message = "请设置银行卡";
                        return Json(response, JsonRequestBehavior.AllowGet);
                    }
                }
                if (context.WalletAmount >= request.Amount)
                {
                    WeiXinProvider wx = new WeiXinProvider();

                    var certPath = Server.MapPath(@"\cert\apiclient_cert.p12");

                    decimal upgradeFundAmount = 0;

                    if (!context.Grade.Equals(CustomerEnum.Grade.股东.GetHashCode()))
                    {
                        upgradeFundAmount = request.Amount / 2;
                    }

                    response = FnCashoutApp.Instance.Cashout(new CashoutRequest()
                    {
                        CustomerSysNo = context.SysNo,
                        WalletAmount = request.Amount,
                        UpgradeFundAmount = upgradeFundAmount,
                        Account = context.Account,
                        CashoutType = request.CashoutType,
                        CertPath = certPath
                    });
                }
                else
                {
                    response.Message = "当前余额不足﹗";
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 提现
        /// <summary>
        /// 提现申请记录
        /// </summary>
        /// <returns></returns>
        public ActionResult CashoutLog()
        {
            //当前上下文
            var context = CustomerContext.Context;
            if (context == null)
            {
                return View("Error", new JResult() { Message = "用户已超时、请重新登录﹗" });
            }

            //会员扩展
            var customer = CustomerApp.Instance.Get(context.SysNo);
            if (customer == null)
            {
                return View("Error", new JResult() { Message = "扩展会员不存在﹗" });
            }
            return View(customer);
        }

        /// <summary>
        /// 提现申请查询
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>分部视图</returns>
        public JsonResult CashoutLogQuery(FnCashoutQueryRequest request)
        {
            var list = FnCashoutApp.Instance.GetPagerList(new FnCashoutQueryRequest()
            {
                CustomerSysNo = CustomerContext.Context.SysNo
            });
            var data = new
            {
                Status = true,
                Data = list.TData.ToList(),
                Count = list.PageSize
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 代理升级
        /// <summary>
        /// 代理升级
        /// </summary>
        /// <returns>视图</returns>
        public ActionResult Upgrade()
        {
            //当前上下文
            var context = CustomerContext.Context;
            if (context == null)
            {
                return View("Error", new JResult() { Message = "用户已超时、请重新登录﹗" });
            }

            var customer = CustomerApp.Instance.Get(context.SysNo);
            if (customer == null)
            {
                return View("Error", new JResult() { Message = "扩展会员不存在﹗" });
            }

            var upgradeGrade = customer.Grade + Step.步长.GetHashCode();


            var nextGrade = (CustomerEnum.NewGrade)upgradeGrade;

            var code = CodeApp.Instance.GetByType(EC.Entity.Enum.CodeEnum.CodeTypeEnum.会员等级信息.GetHashCode());
            if (code != null) {
                var grades = code.Value.ToObject<List<Grade>>();

                var grade = grades.FirstOrDefault(p => p.Type == upgradeGrade);
                if (grade != null) {
                    ViewBag.UpgradeAmount = grade.Amount.ToString("#0.00");
                    if (customer.UpgradeFundAmount > grade.Amount)
                    {
                        ViewBag.PayAmount = 0.ToString("#0.00"); 
                    }
                    else {
                        ViewBag.PayAmount = (grade.Amount - customer.UpgradeFundAmount).ToString("#0.00"); 
                    }
                    
                }
            }

            ViewBag.NextGrade = nextGrade;

            return View(customer);
        }

        /// <summary>
        /// 代理升级
        /// </summary>
        /// <param name="request">参数</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Upgrade(UpgradeRequest request)
        {
            var response = new JResult()
            {
                Status = false
            };

            //当前上下文
            var context = CustomerContext.Context;
            if (context == null)
            {
                response.Message = "用户已超时、请重新登录﹗";
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            var customer = CustomerApp.Instance.Get(context.SysNo);
            if (customer == null)
            {
                response.Message = "扩展会员不存在﹗";
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            var upgradeGrade = customer.Grade + Step.步长.GetHashCode();

            var code = CodeApp.Instance.GetByType(EC.Entity.Enum.CodeEnum.CodeTypeEnum.会员等级信息.GetHashCode());
            if (code != null)
            {
                var grades = code.Value.ToObject<List<Grade>>();

                var grade = grades.FirstOrDefault(p => p.Type == upgradeGrade);
                if (grade != null)
                {
                    //验证升级基金
                    if (customer.UpgradeFundAmount < grade.Amount)
                    {
                        response.StatusCode = ErrorEnum.余额不足.GetHashCode().ToString();
                        response.Message = "升级基金不足!";
                        return Json(response, JsonRequestBehavior.AllowGet);
                    }
                }
            }

            response = NewCustomerApp.Instance.NewUpgrade(new EC.Entity.Parameter.Request.NewCRM.NewUpgradeRequest() { CustomerSysNo = customer.SysNo });
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 升级计算
        /// </summary>
        /// <param name="request">参数</param>
        [HttpPost]
        public JsonResult UpgradeCompute(UpgradeComputeRequest request)
        {
            var response = new JResult<UpgradeComputeResponse>()
            {
                Status = false
            };

            //当前上下文
            var context = CustomerContext.Context;
            if (context == null)
            {
                response.Message = "用户已超时、请重新登录﹗";
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            request.CustomerSysNo = CustomerContext.Context.SysNo;

            response = CustomerApp.Instance.UpgradeCompute(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 创建订单
        /// </summary>
        [HttpPost]
        public JsonResult UpgradeSaveRecharge(SaveRechargeRequest request)
        {
            var response = new JResult<string>()
            {
                Status = false
            };

            //当前上下文
            var context = CustomerContext.Context;
            if (context == null)
            {
                response.Message = "用户已超时、请重新登录﹗";
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            //获取会员
            var customer = CustomerApp.Instance.Get(context.SysNo);
            if (customer == null)
            {
                response.Message = "扩展会员不存在﹗";
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            var upgradeGrade = customer.Grade + Step.步长.GetHashCode();

            decimal PayAmount = 0;

            var code = CodeApp.Instance.GetByType(EC.Entity.Enum.CodeEnum.CodeTypeEnum.会员等级信息.GetHashCode());
            if (code != null)
            {
                var grades = code.Value.ToObject<List<Grade>>();

                var grade = grades.FirstOrDefault(p => p.Type == upgradeGrade);
                if (grade != null)
                {
                    if (customer.UpgradeFundAmount > grade.Amount)
                    {
                        ViewBag.PayAmount = 0.ToString("#0.00");
                    }
                    else
                    {
                        PayAmount = grade.Amount - customer.UpgradeFundAmount;
                    }
                    
                }
            }


            //支付订单编号
            var orderNo = Guid.NewGuid().ToString().Replace("-", "");

            var recharge = new FnRecharge();

            if (context.Account.Equals("15008228718"))
            {
                    recharge.OrderNo = orderNo;
                    recharge.CustomerSysNo = context.SysNo;
                    recharge.Amount = new decimal(0.01);
                    recharge.Remarks = string.Format("用户:{0},升级金额:{1}", context.RealName, new decimal(0.01));
                    recharge.Status = PayStatus.未支付.GetHashCode();
                    recharge.Type = FnEnum.RechargeType.升级.GetHashCode();
                    recharge.CreatedDate = DateTime.Now;
            }
            else {
                recharge.OrderNo = orderNo;
                recharge.CustomerSysNo = context.SysNo;
                recharge.Amount = PayAmount;
                recharge.Remarks = string.Format("用户:{0},升级金额:{1}", context.RealName, PayAmount);
                recharge.Status = PayStatus.未支付.GetHashCode();
                recharge.Type = FnEnum.RechargeType.升级.GetHashCode();
                recharge.CreatedDate = DateTime.Now;

            }

            if (RechargeApp.Instance.Insert(recharge) <= 0)
            {
                response.Message = "创建订单失败、联系管理员！";
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            response.Status = true;
            response.Data = orderNo;
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 充值
        /// <summary>
        /// 我的支付
        /// </summary>
        /// <returns></returns>
        public ActionResult Recharge()
        {
            return View();
        }

        /// <summary>
        /// 充值保存订单
        /// </summary>
        [HttpPost]
        public JsonResult SaveRecharge(SaveRechargeRequest request)
        {
            var response = new JResult<string>()
            {
                Status = false
            };
#if DEBUG
            //request.Amount = new decimal(0.01);
#else

#endif
            //当前上下文
            var context = CustomerContext.Context;
            if (context == null)
            {
                response.Message = "用户已超时、请重新登录﹗";
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            if (request.PayWay.Equals(10))
            {
                //支付订单编号
                var orderNo = Guid.NewGuid().ToString().Replace("-", "");

                var recharge = new FnRecharge()
                {
                    OrderNo = orderNo,
                    CustomerSysNo = context.SysNo,
                    Amount = request.Amount,
                    Remarks = string.Format("用户:{0},充值金额:{1}", context.RealName, request.Amount),
                    Status = PayStatus.未支付.GetHashCode(),
                    Type = FnEnum.RechargeType.充值.GetHashCode(),
                    CreatedDate = DateTime.Now
                };

                if (RechargeApp.Instance.Insert(recharge) <= 0)
                {
                    response.Message = "创建订单失败、联系管理员！";
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
                response.Status = true;
                response.Data = orderNo;
            }
            else if (request.PayWay.Equals(20))
            {
                lock (_lock)
                {
                    if (context.WalletAmount < request.Amount)
                    {
                        response.Message = "奖金余额不足！";
                        return Json(response, JsonRequestBehavior.AllowGet);
                    }
                    if (CustomerApp.Instance.UpdateBonusRecharge(new CrCustomer()
                    {
                        SysNo = context.SysNo,
                        WalletAmount = request.Amount,
                        UpgradeFundAmount = request.Amount,
                        RechargeTotalAmount = request.Amount
                    }) <= 0)
                    {
                        response.Message = "奖金充值失败、联系管理员！";
                        return Json(response, JsonRequestBehavior.AllowGet);
                    }
                    response.Status = true;
                }
            }
            else
            {
                response.Message = "参数错误！";
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 续费
        /// <summary>
        /// 帐户续费
        /// </summary>
        /// <returns></returns>
        public ActionResult Renew()
        {
            //当前上下文
            var context = CustomerContext.Context;
            if (context == null)
            {
                return View("Error", new JResult() { Message = "用户已超时、请重新登录﹗" });
            }
            return View(context);
        }

        /// <summary>
        /// 创建订单
        /// </summary>
        [HttpPost]
        public JsonResult RenewSaveRecharge(SaveRechargeRequest request)
        {
            var response = new JResult<string>()
            {
                Status = false
            };
#if DEBUG
            request.Amount = new decimal(0.01);
#else
            
#endif
            request.Amount = new decimal(200);
            //当前上下文
            var context = CustomerContext.Context;
            if (context == null)
            {
                response.Message = "用户已超时、请重新登录﹗";
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            if (request.PayWay.Equals(10))
            {
                //支付订单编号
                var orderNo = Guid.NewGuid().ToString().Replace("-", "");

                var recharge = new FnRecharge()
                {
                    OrderNo = orderNo,
                    CustomerSysNo = context.SysNo,
                    Amount = request.Amount,
                    Remarks = string.Format("用户:{0},续费充值金额:{1}", context.RealName, request.Amount),
                    Status = PayStatus.未支付.GetHashCode(),
                    Type = FnEnum.RechargeType.升级.GetHashCode(),
                    CreatedDate = DateTime.Now
                };

                if (RechargeApp.Instance.Insert(recharge) <= 0)
                {
                    response.Message = "创建订单失败、联系管理员！";
                    return Json(response, JsonRequestBehavior.AllowGet);
                }

                response.Status = true;
                response.Data = orderNo;
            }
            else if (request.PayWay.Equals(20))
            {
                lock (_lock)
                {
                    using (var tran = new TransactionProvider())
                    {
                        try
                        {
                            if (context.WalletAmount < request.Amount)
                            {
                                response.Message = "奖金余额不足！";
                                return Json(response, JsonRequestBehavior.AllowGet);
                            }
                            if (CustomerApp.Instance.UpdateWalletAmountRenew(new CrCustomer()
                            {
                                SysNo = context.SysNo,
                                WalletAmount = request.Amount                            
                            }) <= 0)
                            {
                                response.Message = "奖金充值失败、联系管理员！";
                                return Json(response, JsonRequestBehavior.AllowGet);
                            }

                            if (context.Grade.Equals(CustomerEnum.Grade.普通会员.GetHashCode()))
                            {
                                if (CustomerApp.Instance.UpdateGrade(new CrCustomer()
                                {
                                    SysNo = context.SysNo,
                                    Grade = CustomerEnum.Grade.普通代理.GetHashCode()
                                }) <= 0)
                                {
                                    throw new Exception("更新等级失败!");
                                }
                            }
                            //如果过期时间为空则添加一年有效期
                            if (string.IsNullOrWhiteSpace(context.ExpiresTime))
                            {
                                context.ExpiresTime = DateTime.Now.AddYears(1).ToString("yyyy-MM-dd");
                                if (CustomerApp.Instance.UpdateExpiresTime(context) <= 0)
                                {
                                    throw new Exception("更新过期时间失败!");
                                }
                            }
                            else
                            {
                                context.ExpiresTime = Convert.ToDateTime(context.ExpiresTime).AddYears(1).ToString("yyyy-MM-dd");
                                if (CustomerApp.Instance.UpdateExpiresTime(context) <= 0)
                                {
                                    throw new Exception("更新过期时间失败!");
                                }
                            }
                            tran.Commit();
                            response.Status = true;
                            response.Message = "续费成功！";
                        }
                        catch (Exception ex)
                        {
                            tran.Dispose();
                            response.Message = ex.Message;
                        }
                        finally
                        {
                            tran.Dispose();
                        }
                    }
                }
            }
            else
            {
                response.Message = "参数错误！";
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
