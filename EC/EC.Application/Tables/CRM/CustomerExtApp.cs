using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Transactions;
using EC.Application.Tables.Fn;
using EC.DataAccess.Bs;
using EC.DataAccess.CRM;
using EC.DataAccess.Fn;
using EC.Entity;
using EC.Entity.Enum;
using EC.Entity.Parameter.Request.CRM;
using EC.Entity.Parameter.Response.CRM;
using EC.Entity.Tables.CRM;
using EC.Entity.Tables.Finance;
using EC.Entity.View.CRM;
using EC.Entity.View.CRM.Ext;
using EC.Libraries.Core.Pager;


namespace EC.Application.Tables.CRM
{
    using EC.Libraries.Core;
    using EC.Libraries.Util;

    /// <summary>
    /// 扩展会员业务层
    /// </summary>
    public class CustomerExtApp : Base<CustomerExtApp>
    {
        ///// <summary>
        ///// 获取扩展会员
        ///// </summary>
        ///// <param name="sysNo">系统编号</param>
        ///// <returns></returns>
        //public CrCustomerExt Get(int sysNo)
        //{
        //    return Using<ICrCustomerExt>().Get(sysNo);
        //}

        ///// <summary>
        ///// 根据会员编号获取扩展会员
        ///// </summary>
        ///// <param name="customerSysNo">会员编号</param>
        ///// <returns></returns>
        //public CrCustomerExt GetByCustomerSysNo(int customerSysNo)
        //{
        //    return Using<ICrCustomerExt>().GetByCustomerSysNo(customerSysNo);
        //}

        ///// <summary>
        ///// 获取扩展会员通过OpenId
        ///// </summary>
        ///// <param name="openId">微信OpenId</param>
        ///// <returns>扩展会员信息</returns>
        //public CrCustomerExt GetCustomerExtByOpenId(string openId)
        //{
        //    return Using<ICrCustomerExt>().GetCustomerExtByOpenId(openId);
        //}

        ///// <summary>
        ///// 更新用户基本资料
        ///// </summary>
        //public int UpdateProfile(CrCustomerExt model)
        //{
        //    return Using<ICrCustomerExt>().UpdateProfile(model);
        //}

        ///// <summary>
        ///// 更新头像
        ///// </summary>
        //public int UpdateHead(CrCustomerExt model)
        //{
        //    return Using<ICrCustomerExt>().UpdateHead(model);
        //}

        ///// <summary>
        ///// 更新会员信息
        ///// </summary>
        ///// <param name="customer">会员信息</param>
        ///// <returns>影响行数</returns>
        //public JResult UpdateInfo(CustomerExt customer)
        //{
        //    var result = new JResult();

        //    using (var tran = new TransactionScope())
        //    {
        //        try
        //        {
        //            if (Using<ICrCustomer>().UpdateInfo(new CrCustomer()
        //            {
        //                tel = customer.TelNumber,
        //                zx = customer.Status,
        //                id = customer.CustomerSysNo
        //            }) < 0)
        //            {
        //                throw new Exception("更新用户信息失败");
        //            }

        //            if (Using<ICrCustomerExt>().UpdateInfo(customer.CustomerSysNo, customer.RealName, customer.Grade) < 0)
        //            {
        //                throw new Exception("更新用户信息失败");
        //            }

        //            result.Status = true;
        //            tran.Complete();
        //        }
        //        catch (Exception ex)
        //        {
        //            tran.Dispose();
        //            result.Message = ex.Message;
        //        }
        //    }

        //    return result;
        //}

        ///// <summary>
        ///// 注册扩展表
        ///// </summary>
        //public JResult Register(int customerSysNo)
        //{
        //    var response = new JResult()
        //    {
        //        Status = false
        //    };

        //    try
        //    {
        //        var customer = Using<ICrCustomer>().Get(customerSysNo);
        //        if (customer == null)
        //        {
        //            response.Message = "创建扩展注册失败！";
        //            return response;
        //        }

        //        var customerExt = new CrCustomerExt()
        //        {
        //            CustomerSysNo = customerSysNo,
        //            ReferrerSysNo = 0,
        //            RealName = "",
        //            Email = "",
        //            HeadImgUrl = "",
        //            //PhoneNumber = customer.name,
        //            IDNumber = "",
        //            TeamCount = 0,
        //            Grade = CustomerEnum.Grade.普通会员.GetHashCode(),
        //            Level = 0,
        //            LevelCustomerStr = "",
        //            Bank = 0,
        //            BankNumber = "",
        //            WalletAmount = 0,
        //            HistoryWalletAmount = 0,
        //            UpgradeFundAmount = 0,
        //            GeneralBonus = 0,
        //            AreaBonus = 0,
        //            GlobalBonus = 0,
        //            Expires = CustomerEnum.Expires.已过期.GetHashCode(),
        //            CreatedDate = DateTime.Now
        //        };

        //        if (customer.suid > 0)
        //        {
        //            customerExt.ReferrerSysNo = customer.suid;
        //        }

        //        var customerExtSysNo = Using<ICrCustomerExt>().Register(customerExt);
        //        if (customerExtSysNo <= 0)
        //        {
        //            throw new Exception("创建创建扩展注册失败、请联系管理员!");
        //        }

        //        response.Status = true;
        //        response.Message = "创建扩展注册成功！";
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Status = false;
        //        response.Message = "创建扩展注册失败！" + ex.Message;
        //    }

        //    return response;
        //}

        ///// <summary>
        ///// 获取团队数
        ///// </summary>
        ///// <param name="sysNo">用户编号</param>
        ///// <param name="list">用户列表</param>
        ///// <returns>用户列表</returns>
        //private static List<int> GetTeam(int sysNo, List<CrCustomerExt> list)
        //{
        //    var teamIds = new List<int>();

        //    foreach (var item in list.Where(p => p.ReferrerSysNo == sysNo).ToList())
        //    {
        //        teamIds.Add(item.SysNo);

        //        teamIds.AddRange(GetTeam(item.SysNo, list));
        //    }

        //    return teamIds;
        //}

        ///// <summary>
        ///// 获取推荐人
        ///// </summary>
        ///// <param name="customersList">所有会员</param>
        ///// <param name="ids">推荐人编号</param>
        ///// <param name="referrerSysNo">推荐编号</param>
        ///// <returns>List<int></returns>
        //public List<int> GetReferrer(IList<CrCustomerExt> customersList, List<int> ids, int referrerSysNo)
        //{
        //    var customer = customersList.FirstOrDefault(p => p.CustomerSysNo.Equals(referrerSysNo));
        //    if (customer != null)
        //    {
        //        ids.Add(customer.CustomerSysNo);
        //        return GetReferrer(customersList, ids, customer.ReferrerSysNo);
        //    }
        //    return ids;
        //}

        ///// <summary>
        ///// 获取升级推荐人列表
        ///// </summary>
        ///// <param name="customersList">所有会员</param>
        ///// <param name="parentIds">推荐人列表</param>
        ///// <param name="referrerSysNo">推荐编号</param>
        ///// <param name="layer">获取上级层数</param>
        ///// <param name="sort">排序</param>
        ///// <returns>List<ParentIds></returns>
        //public List<ParentIds> GetUpgradeReferrer(IList<CrCustomerExt> customersList, List<ParentIds> parentIds, int referrerSysNo, int layer, int sort = 1)
        //{
        //    var customer = customersList.FirstOrDefault(p => p.CustomerSysNo.Equals(referrerSysNo));
        //    if (customer != null)
        //    {
        //        parentIds.Add(new ParentIds()
        //        {
        //            CustomerSysNo = customer.CustomerSysNo,
        //            Grade = customer.Grade,
        //            Sort = sort
        //        });
        //        if (sort.Equals(layer))
        //            return parentIds;
        //        sort++;
        //        return GetUpgradeReferrer(customersList, parentIds, customer.ReferrerSysNo, layer, sort);
        //    }
        //    return parentIds;
        //}

        ///// <summary>
        ///// 获取会员层级列表
        ///// </summary>
        ///// <param name="levelCustomerSysNo">层级会员编号（用于查询层级推荐会员）</param>
        ///// <returns>会员层级列表</returns>
        //public List<CustomerLevelView> GetCustomerLevelCount(int levelCustomerSysNo)
        //{
        //    string[] letterArrStr = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

        //    var list = Using<ICrCustomerExt>().GetCustomerLevelCount(levelCustomerSysNo).ToList();

        //    if (!list.Any()) return list;
        //    var minLevel = list.Min(p => p.Level);
        //    foreach (var item in list.Where(item => item.Level - minLevel <= 25))
        //    {
        //        item.LevelLetter = letterArrStr[item.Level - minLevel];
        //    }

        //    return list.OrderBy(p => p.Level).ToList();
        //}

        ///// <summary>
        ///// 获取会员团队数列表
        ///// </summary>
        ///// <param name="sysNoList">会员编号列表</param>
        ///// <returns>会员团队数列表</returns>
        //public List<CustomerLevelView> GetCustomerTeamCount(List<int> sysNoList)
        //{
        //    if (sysNoList == null || !sysNoList.Any())
        //    {
        //        return null;
        //    }

        //    return Using<ICrCustomerExt>().GetCustomerTeamCount(sysNoList).ToList();
        //}

        ///// <summary>
        ///// 获取会员扩展信息
        ///// </summary>
        ///// <param name="customerSysNo">会员编号</param>
        ///// <returns>会员扩展信息</returns>
        //public CustomerExt GetCustomerExt(int customerSysNo)
        //{
        //    return Using<ICrCustomerExt>().GetCustomerExt(customerSysNo);
        //}

        ///// <summary>
        ///// 获取平台参数
        ///// </summary>
        ///// <returns>平台参数</returns>
        //public PlatformView GetPlatformValue()
        //{
        //    var platformValue = Using<ICrCustomerExt>().GetPlatformValue();
        //    var codeList = Using<IBsCode>().GetListByTypeList(new List<int>()
        //    {
        //         CodeEnum.CodeTypeEnum.股池资金.GetHashCode(),
        //         CodeEnum.CodeTypeEnum.股权数总额.GetHashCode(),
        //         CodeEnum.CodeTypeEnum.股权价格.GetHashCode()
        //    });

        //    var stockTotalAmountModel = codeList.FirstOrDefault(p => p.Type == CodeEnum.CodeTypeEnum.股池资金.GetHashCode());
        //    var stockNumModel = codeList.FirstOrDefault(p => p.Type == CodeEnum.CodeTypeEnum.股权数总额.GetHashCode());
        //    var stockPriceModel = codeList.FirstOrDefault(p => p.Type == CodeEnum.CodeTypeEnum.股权价格.GetHashCode());

        //    platformValue.StockTotalAmount = stockTotalAmountModel != null ? decimal.Round(Convert.ToDecimal(stockTotalAmountModel.Value), 3) : 0;
        //    platformValue.StockNum = stockNumModel != null ? Convert.ToInt32(stockNumModel.Value) : 0;
        //    platformValue.StockPrice = stockPriceModel != null ? decimal.Round(Convert.ToDecimal(stockPriceModel.Value), 3) : 0;
        //    platformValue.RechargeTotalAmount = decimal.Round(platformValue.RechargeTotalAmount, 3);
        //    platformValue.WithdrawalsTotalAmount = decimal.Round(platformValue.WithdrawalsTotalAmount, 3);

        //    return platformValue;
        //}

        ///// <summary>
        ///// 获取会员扩展信息列表
        ///// </summary>
        ///// <param name="request">查询参数</param>
        ///// <returns>会员扩展信息列表</returns>
        //public List<CrCustomerExt> GetList(CustomerExtRequest request)
        //{
        //    return Using<ICrCustomerExt>().GetList(request).ToList();
        //}

        ///// <summary>
        ///// 获取会员分页列表
        ///// </summary>
        ///// <param name="request">查询参数</param>
        ///// <returns>会员分页列表</returns>
        //public PagedList<CrCustomerExt> GetPagerList(CustomerExtRequest request)
        //{
        //    return Using<ICrCustomerExt>().GetPagerList(request);
        //}

        ///// <summary>
        ///// 获取会员扩展信息分页列表
        ///// </summary>
        ///// <param name="request">查询参数</param>
        ///// <returns>会员扩展信息分页列表</returns>
        //public PagedList<CustomerExt> GetExtPagerList(CustomerExtRequest request)
        //{
        //    return Using<ICrCustomerExt>().GetExtPagerList(request);
        //}

        ///// <summary>
        ///// 更新过期时间
        ///// </summary>
        //public int UpdateExpiresTime(UpdateExpiresTimeRequest request)
        //{
        //    return Using<ICrCustomerExt>().UpdateExpiresTime(request);
        //}

        ///// <summary>
        ///// 会员升级股东等级
        ///// </summary>
        ///// <param name="customerSysNo">会员编号</param>
        ///// <returns>JResult</returns>
        //public JResult UpStockholderGrade(int customerSysNo)
        //{
        //    var result = new JResult();

        //    try
        //    {
        //        var model = Using<ICrCustomerExt>().GetByCustomerSysNo(customerSysNo);

        //        if (model == null)
        //        {
        //            throw new Exception("用户信息不存在");
        //        }
        //        if (model.Grade == CustomerEnum.Grade.股东.GetHashCode())
        //        {
        //            throw new Exception("会员已为股东");
        //        }
        //        var codeList = Using<IBsCode>().GetListByTypeList(new List<int>()
        //        {
        //            CodeEnum.CodeTypeEnum.会员等级信息.GetHashCode(),
        //            CodeEnum.CodeTypeEnum.股权价格.GetHashCode(),
        //            CodeEnum.CodeTypeEnum.股权增值金额.GetHashCode(),
        //            CodeEnum.CodeTypeEnum.购股有效金额.GetHashCode(),
        //            CodeEnum.CodeTypeEnum.购股手续费率.GetHashCode(),
        //            CodeEnum.CodeTypeEnum.推荐奖励.GetHashCode()
        //        });
        //        //固定数据
        //        var stockPrice = Convert.ToDecimal(codeList.First(p => p.Type == CodeEnum.CodeTypeEnum.股权价格.GetHashCode()).Value);
        //        var addStockPoolAmount = Convert.ToDecimal(codeList.First(p => p.Type == CodeEnum.CodeTypeEnum.股权增值金额.GetHashCode()).Value);
        //        var usefulStockAmount = Convert.ToDecimal(codeList.First(p => p.Type == CodeEnum.CodeTypeEnum.购股有效金额.GetHashCode()).Value);
        //        var rate = Convert.ToDecimal(codeList.First(p => p.Type == CodeEnum.CodeTypeEnum.购股手续费率.GetHashCode()).Value);
        //        var recommendAmount = Convert.ToDecimal(codeList.First(p => p.Type == CodeEnum.CodeTypeEnum.推荐奖励.GetHashCode()).Value);
        //        //实际可获股权数
        //        var getStockNum = Convert.ToInt32(usefulStockAmount / stockPrice);
        //        //等级信息列表
        //        var gradeInfoList = codeList.First(p => p.Type == CodeEnum.CodeTypeEnum.会员等级信息.GetHashCode()).Value.ToObject<List<GradeView>>();
        //        //升级所需金额
        //        var upGradeAmount = gradeInfoList.Where(p => p.Type > model.Grade && p.Type <= CustomerEnum.Grade.股东.GetHashCode()).Sum(x => x.Amount);

        //        if (model.UpgradeFundAmount < upGradeAmount)
        //        {
        //            result.StatusCode = ErrorEnum.余额不足.GetHashCode().ToString(CultureInfo.InvariantCulture);
        //            throw new Exception(string.Format("用户升级基金{0}余额不足，请充值", model.UpgradeFundAmount));
        //        }

        //        //创建会员股权账户
        //        var stockAccount = new FnStockAccount()
        //        {
        //            CustomerSysNo = model.CustomerSysNo,
        //            StockNum = getStockNum,
        //            CreatedDate = DateTime.Now
        //        };
        //        //用户资金变动列表
        //        var customerChangeList = new List<CustomerExtBatchRequest>()
        //        {
        //            //会员升级股东时账户变动
        //            new CustomerExtBatchRequest()
        //            {
        //                CustomerSysNo = model.CustomerSysNo,
        //                Grade = CustomerEnum.Grade.股东.GetHashCode(),
        //                UpgradeFundAmount = -upGradeAmount
        //            }
        //        };
        //        //销售股权记录（用户股权先从挂售获取股权）
        //        var saleStockRecordList = StockAccountApp.Instance.GetUpdateHangSaleStock(model.CustomerSysNo, getStockNum, stockPrice);
        //        //购买方股权记录
        //        var buyStockRecord = new FnStockRecord()
        //        {
        //            Type = StockRecordEnum.TypeEnum.股权申购.GetHashCode(),
        //            OperatorType = OperatorTypeEnum.会员.GetHashCode(),
        //            OperatorSysNo = model.CustomerSysNo,
        //            StockNum = getStockNum,
        //            TradedStockNum = getStockNum,
        //            CurrentStockAmount = stockPrice,
        //            Status = StockRecordEnum.StatusEnum.已处理.GetHashCode(),
        //            Remarks = string.Format("会员升级股东获的股权，当前股权价格:{0}", stockPrice),
        //            CreatedDate = DateTime.Now
        //        };
        //        //新增股权池金额
        //        var newAddStockPoolAmount = decimal.Round(Convert.ToDecimal(0), 8);
        //        //获取新增股权池数
        //        var newAddStockPoolNum = getStockNum - saleStockRecordList.Sum(p => p.TradedStockNum);
        //        if (newAddStockPoolNum > 0)
        //        {
        //            if (newAddStockPoolNum == getStockNum)
        //            {
        //                newAddStockPoolAmount = addStockPoolAmount;
        //            }
        //            else
        //            {
        //                var stockAmount = decimal.Round(addStockPoolAmount / getStockNum, 8);
        //                newAddStockPoolAmount = decimal.Round(newAddStockPoolNum * stockAmount, 8);
        //            }
        //        }

        //        //销售用户销售金额
        //        customerChangeList.AddRange(StockAccountApp.Instance.GetSaleAmount(rate, saleStockRecordList));
        //        //用户股权账户变动列表
        //        var changeStockAccountList = StockAccountApp.Instance.GetSaleStockAccount(new List<FnStockAccount>(), saleStockRecordList);
        //        //直推会员推荐奖励
        //        FnBonusLog bonusLog = null;
        //        var referrerCustomer = Using<ICrCustomerExt>().GetByCustomerSysNo(model.ReferrerSysNo);
        //        if (referrerCustomer != null)
        //        {
        //            //当存在推荐人时
        //            var customerChange = customerChangeList.SingleOrDefault(p => p.CustomerSysNo == model.ReferrerSysNo);

        //            if (customerChange == null)
        //            {
        //                customerChangeList.Add(new CustomerExtBatchRequest()
        //                {
        //                    CustomerSysNo = model.ReferrerSysNo,
        //                    WalletAmount = recommendAmount,
        //                    HistoryWalletAmount = recommendAmount
        //                });
        //            }
        //            else
        //            {
        //                customerChange.WalletAmount += recommendAmount;
        //                customerChange.HistoryWalletAmount += recommendAmount;
        //            }

        //            //股东升级推荐奖励日志
        //            bonusLog = new FnBonusLog()
        //            {
        //                CustomerSysNo = referrerCustomer.CustomerSysNo,
        //                SourceSysNo = model.CustomerSysNo,
        //                SourceSerialNumber = model.SerialNumber.ToString(CultureInfo.InvariantCulture),
        //                Amount = recommendAmount,
        //                Type = FnEnum.BonusLogType.股东升级推荐奖励.GetHashCode(),
        //                CreatedDate = DateTime.Now
        //            };
        //        }


        //        using (var tran = new TransactionScope())
        //        {
        //            try
        //            {
        //                //扣除会员升级基金 以及 新增销售会员销售金额
        //                if (Using<ICrCustomerExt>().BatchUpdate(customerChangeList) <= 0)
        //                {
        //                    throw new Exception("金额写入失败");
        //                }
        //                //新增会员股权账户
        //                if (Using<IFnStockAccount>().Insert(stockAccount) <= 0)
        //                {
        //                    throw new Exception("新增股权账户失败");
        //                }
        //                //更新股权账户
        //                if (changeStockAccountList.Any() && Using<IFnStockAccount>().BatchUpdateSaleStockNum(changeStockAccountList) <= 0)
        //                {
        //                    throw new Exception("更新股权账户失败");
        //                }
        //                //新增申购用户股权记录
        //                buyStockRecord.SysNo = Using<IFnStockRecord>().Insert(buyStockRecord);
        //                if (buyStockRecord.SysNo <= 0)
        //                {
        //                    throw new Exception("新增股权记录失败");
        //                }
        //                //更新挂售用户股权记录
        //                if (saleStockRecordList.Any() && Using<IFnStockRecord>().BatchUpdateSaleRecord(saleStockRecordList) <= 0)
        //                {
        //                    throw new Exception("更新销售股权记录失败");
        //                }
        //                //获取交易日志
        //                var tradeLogList = StockAccountApp.Instance.GetSaleStockTradeLog(stockPrice, buyStockRecord, saleStockRecordList);
        //                if (Using<IFnTradeLog>().BatchInsert(tradeLogList) <= 0)
        //                {
        //                    throw new Exception("新增交易日志失败");
        //                }
        //                //股东见点奖励
        //                var seeBonusResult = StockAccountApp.Instance.SeeBonusPoints(model.CustomerSysNo);
        //                if (!seeBonusResult.Status)
        //                {
        //                    throw new Exception(seeBonusResult.Message);
        //                }
        //                //股东升级推荐奖励日志
        //                if (bonusLog != null && Using<IFnBonusLog>().Insert(bonusLog) <= 0)
        //                {
        //                    throw new Exception("奖金记录写入失败");
        //                }
        //                //股权池变动更新
        //                var changeStockPool = StockAccountApp.Instance.ChangeStock(newAddStockPoolNum, newAddStockPoolAmount);
        //                if (!changeStockPool.Status)
        //                {
        //                    throw new Exception(changeStockPool.Message);
        //                }

        //                result.Status = true;
        //                tran.Complete();
        //            }
        //            catch (Exception ex)
        //            {
        //                tran.Dispose();
        //                throw ex;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result.Message = ex.Message;
        //    }

        //    return result;
        //}

        ///// <summary>
        ///// 更新OpenId
        ///// </summary>
        //public int UpdateUserInfo(CrCustomerExt model)
        //{
        //    return Using<ICrCustomerExt>().UpdateUserInfo(model);
        //}

        ///// <summary>
        ///// 根据OpenId获取会员扩展表
        ///// </summary>
        //public LoginResponse GetByOpenId(string openId)
        //{
        //    return Using<ICrCustomerExt>().GetByOpenId(openId);
        //}
    }
}
