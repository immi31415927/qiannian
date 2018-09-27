using System;
using System.Collections.Generic;
using System.Linq;
using EC.Application.Tables.CRM;
using EC.DataAccess.CRM;
using EC.DataAccess.Fn;
using EC.Entity;
using EC.Entity.Enum;
using EC.Entity.Parameter.Request.CRM;
using EC.Entity.Parameter.Request.Finance;
using EC.Entity.Tables.CRM;
using EC.Entity.Tables.Finance;
using EC.Libraries.Core.Transaction;

namespace EC.Application.Tables.Fn
{
    using EC.Libraries.Core;
    using EC.Libraries.Util;
    using System.Collections.Generic;
    using EC.Entity.Output.Fore;
    using EC.Application.Tables.WeiXin;
    using EC.Entity.Parameter.Request.WeiXin;
    using EC.Application.Tables.Bs;
    using EC.Entity.View.CRM;

    /// <summary>
    /// 充值日志业务层
    /// </summary>
    public class RechargeApp : Base<RechargeApp>
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        public FnRecharge Get(int sysNo)
        {
            return Using<IFnRecharge>().Get(sysNo);
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="orderNo">序列号</param>
        /// <returns></returns>
        public FnRecharge GetbyOrderNo(string orderNo)
        {
            return Using<IFnRecharge>().GetbyOrderNo(orderNo);
        }

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>系统编号</returns>
        public int Insert(FnRecharge model)
        {
            return Using<IFnRecharge>().Insert(model);
        }

        /// <summary>
        /// 更新订单状态
        /// </summary>
        /// <returns></returns>
        public JResult UpdatePayStatus(UpdatePayStatusRequest request)
        {
            //{"OrderSysNo":"de08b16c016d47f2b109eae46b11b545","Amount":0.1,"VoucherNo":"4200000083201803148591866779"}
            var response = new JResult()
            {
                Status = false
            };

            try
            {
                var model = Using<IFnRecharge>().GetbyOrderNo(request.OrderSysNo);
                //如果订单已经支付，则直接返回true
                if (model != null && model.Status == PayStatus.已支付.GetHashCode())
                {
                    response.Status = false;
                    response.Message = "订单已支付！";
                    return response;
                }

                if (Using<IFnRecharge>().UpdatePayStatus(request) <= 0)
                {
                    throw new Exception("更新支付状态失败！");
                }

                if (model.Type.Equals(FnEnum.RechargeType.充值.GetHashCode()))
                {
                    #region 充值
                    if (Using<ICrCustomer>().UpdatePayWalletAmount(new UpdatePayWalletAmountRequest()
                    {
                        CustomerSysNo = model.CustomerSysNo,
                        UpgradeFundAmount = model.Amount,
                        RechargeTotalAmount = model.Amount
                    }) <= 0)
                    {
                        throw new Exception("更新支付状态失败！");
                    }
                    #endregion
                }
                else if (model.Type.Equals(FnEnum.RechargeType.续费.GetHashCode()))
                {
                    #region 续费
                    var customer = Using<ICrCustomer>().Get(model.CustomerSysNo);
                    if (customer != null) {
                        //如果过期时间为空则添加一年有效期
                        if (string.IsNullOrWhiteSpace(customer.ExpiresTime))
                        {
                            customer.ExpiresTime = DateTime.Now.AddYears(1).ToString("yyyy-MM-dd");
                            if (CustomerApp.Instance.UpdateExpiresTime(customer) <= 0)
                            {
                                throw new Exception("更新过期时间失败!");
                            }
                        }
                        else
                        {
                            customer.ExpiresTime = Convert.ToDateTime(customer.ExpiresTime).AddYears(1).ToString("yyyy-MM-dd");
                            if (CustomerApp.Instance.UpdateExpiresTime(customer) <= 0)
                            {
                                throw new Exception("更新过期时间失败!");
                            }
                        }
                    }
                    #endregion
                }
                else if (model.Type.Equals(FnEnum.RechargeType.升级.GetHashCode()))
                {
                    #region 升级
                    if (Using<ICrCustomer>().UpdatePayWalletAmount(new UpdatePayWalletAmountRequest()
                    {
                        CustomerSysNo = model.CustomerSysNo,
                        UpgradeFundAmount = model.Amount,
                        RechargeTotalAmount = model.Amount
                    }) <= 0)
                    {
                        throw new Exception("更新支付状态失败！");
                    }

                    var customer = Using<ICrCustomer>().Get(model.CustomerSysNo);
                    if (customer != null)
                    {
                        //获取等级字符串
                        var code = CodeApp.Instance.GetByType(CodeEnum.CodeTypeEnum.会员等级信息.GetHashCode());
                        if (code == null)
                        {
                            throw new Exception("获取会员等级信息失败！");
                        }

                        var gradeList = JsonUtil.ToObject<List<GradeView>>(code.Value);
                        if (gradeList == null && !gradeList.Any())
                        {
                            throw new Exception("请设置会员等级信息！");
                        }

                        var grade = gradeList.FirstOrDefault(p => p.Type.Equals(customer.Grade + 10));
                        if (grade == null)
                        {
                            throw new Exception("请选择会员等级！");
                        }

                        var UpgradeResult = CustomerApp.Instance.Upgrade(new UpgradeRequest()
                        {
                            CustomerSysNo = customer.SysNo,
                            Amount = grade.Amount,
                            SelectGrade = customer.Grade + 10
                        });

                        if (!UpgradeResult.Status)
                        {
                            throw new Exception(UpgradeResult.Message);
                        }
                    }
                    #endregion
                }
                response.Status = true;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
