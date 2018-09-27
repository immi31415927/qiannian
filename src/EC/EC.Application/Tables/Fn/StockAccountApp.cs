using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using EC.DataAccess.Bs;
using EC.DataAccess.CRM;
using EC.DataAccess.Fn;
using EC.Entity;
using EC.Entity.Enum;
using EC.Entity.Parameter.Request.CRM;
using EC.Entity.Parameter.Request.Fn;
using EC.Entity.Tables.Bs;
using EC.Entity.Tables.CRM;
using EC.Entity.Tables.Finance;
using EC.Entity.View.CRM;
using EC.Entity.View.Fn.Ext;
using EC.Libraries.Core;
using EC.Libraries.Core.Pager;
using EC.Libraries.Core.Transaction;
using EC.Libraries.Util;

namespace EC.Application.Tables.Fn
{
    /// <summary>
    /// 股权账户业务层
    /// </summary>
    public class StockAccountApp : Base<StockAccountApp>
    {
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>系统编号</returns>
        public JResult<int> Insert(FnStockAccount model)
        {
            var result = new JResult<int>();

            try
            {
                model.CreatedDate = DateTime.Now;
                var row = Using<IFnStockAccount>().Insert(model);

                if (row <= 0)
                {
                    throw new Exception("添加数据失败");
                }

                result.Status = true;
                result.Data = row;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>影响行数</returns>
        public JResult Update(FnStockAccount model)
        {
            var result = new JResult();

            try
            {
                var row = Using<IFnStockAccount>().Update(model);

                if (row <= 0)
                {
                    throw new Exception("修改数据失败");
                }

                result.Status = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 获取股权账户信息
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>股权账户信息</returns>
        public FnStockAccount Get(int sysNo)
        {
            return Using<IFnStockAccount>().Get(sysNo);
        }

        /// <summary>
        /// 获取股权账户信息通过会员编号
        /// </summary>
        /// <param name="customerSysNo">会员编号</param>
        /// <returns>股权账户信息</returns>
        public FnStockAccount GetByCustomerSysNo(int customerSysNo)
        {
            return Using<IFnStockAccount>().GetByCustomerSysNo(customerSysNo);
        }

        /// <summary>
        /// 获取股权账户列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>股权账户列表</returns>
        public List<FnStockAccount> GetList(StockAccountRequest request)
        {
            var list = Using<IFnStockAccount>().GetList(request);

            return list.ToList();
        }

        /// <summary>
        /// 获取股权账户分页列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>股权账户分页列表</returns>
        public PagedList<StockAccountExt> GetPagerList(StockAccountRequest request)
        {
            return Using<IFnStockAccount>().GetPagerList(request);
        }

        #region 扩展方法

        /// <summary>
        /// 挂售股权
        /// </summary>
        /// <param name="saleCustomerSysNo">销售会员编号</param>
        /// <param name="stockNum">股权数</param>
        /// <returns>销售结果</returns>
        public JResult HangSaleStock(int saleCustomerSysNo, int stockNum)
        {
            var result = new JResult();

            try
            {
                var stockAccount = Using<IFnStockAccount>().GetByCustomerSysNo(saleCustomerSysNo);

                if (stockNum <= 0)
                {
                    throw new Exception("请输入挂售股权数");
                }
                if (stockAccount == null)
                {
                    throw new Exception("股权账户不存在");
                }
                if (stockAccount.StockNum < stockNum)
                {
                    throw new Exception("销售股权数不足");
                }

                var code = Using<IBsCode>().GetByType(CodeEnum.CodeTypeEnum.股权价格.GetHashCode());

                if (code == null)
                {
                    throw new Exception("股权价格参数不存在，请联系管理员");
                }

                using (var tran = new TransactionScope())
                {
                    try
                    {
                        if (Using<IFnStockAccount>().UpdateSaleStock(saleCustomerSysNo, stockNum) <= 0)
                        {
                            throw new Exception("更新销售股权失败");
                        }
                        if (Using<IFnStockRecord>().Insert(new FnStockRecord()
                        {
                            Type = StockRecordEnum.TypeEnum.股权挂售.GetHashCode(),
                            OperatorType = OperatorTypeEnum.会员.GetHashCode(),
                            OperatorSysNo = saleCustomerSysNo,
                            StockNum = stockNum,
                            TradedStockNum = 0,
                            CurrentStockAmount = Convert.ToDecimal(code.Value),
                            Status = StockRecordEnum.StatusEnum.待处理.GetHashCode(),
                            Remarks = "",
                            CreatedDate = DateTime.Now
                        }) <= 0)
                        {
                            throw new Exception("新增股权记录失败");
                        }

                        result.Status = true;
                        tran.Complete();
                    }
                    catch (Exception ex)
                    {
                        tran.Dispose();
                        throw ex;
                    }
                }
            }
            catch (Exception exx)
            {
                result.Message = exx.Message;
            }

            return result;
        }

        /// <summary>
        /// 申购股权
        /// </summary>
        /// <param name="buyCustomerSysNo">申购会员编号</param>
        /// <returns>申购结果</returns>
        public JResult ApplyBuyStock(int buyCustomerSysNo)
        {
            var result = new JResult();

            try
            {
                var codeList = Using<IBsCode>().GetListByTypeList(new List<int>()
                {
                    CodeEnum.CodeTypeEnum.股权价格.GetHashCode(),
                    CodeEnum.CodeTypeEnum.固定购股金额.GetHashCode(),
                    CodeEnum.CodeTypeEnum.股权增值金额.GetHashCode(),
                    CodeEnum.CodeTypeEnum.购股有效金额.GetHashCode(),
                    CodeEnum.CodeTypeEnum.购股手续费率.GetHashCode(),
                });
                //固定数据
                var stockPrice = Convert.ToDecimal(codeList.First(p => p.Type == CodeEnum.CodeTypeEnum.股权价格.GetHashCode()).Value);
                var fixedStockAmount = Convert.ToDecimal(codeList.First(p => p.Type == CodeEnum.CodeTypeEnum.固定购股金额.GetHashCode()).Value);
                var addStockPoolAmount = Convert.ToDecimal(codeList.First(p => p.Type == CodeEnum.CodeTypeEnum.股权增值金额.GetHashCode()).Value);
                var usefulStockAmount = Convert.ToDecimal(codeList.First(p => p.Type == CodeEnum.CodeTypeEnum.购股有效金额.GetHashCode()).Value);
                var rate = Convert.ToDecimal(codeList.First(p => p.Type == CodeEnum.CodeTypeEnum.购股手续费率.GetHashCode()).Value);

                //获取用户信息
                var customer = Using<ICrCustomer>().Get(buyCustomerSysNo);

                if (customer == null)
                {
                    throw new Exception("用户信息不存在");
                }
                if (customer.UpgradeFundAmount < fixedStockAmount)
                {
                    result.StatusCode = ErrorEnum.余额不足.GetHashCode().ToString(CultureInfo.InvariantCulture);
                    throw new Exception(string.Format("用户升级基金{0}余额不足，请充值", customer.UpgradeFundAmount));
                }

                //用户资金变动列表
                var batchRequestList = new List<CustomerExtBatchRequest>();
                //用户股权账户变动列表
                var batchStockAccountList = new List<FnStockAccount>();
                //实际可获股权数
                var getStockNum = Convert.ToInt32(usefulStockAmount / stockPrice);

                //用户购买金额从升级基金扣除
                batchRequestList.Add(new CustomerExtBatchRequest()
                {
                    CustomerSysNo = buyCustomerSysNo,
                    UpgradeFundAmount = -fixedStockAmount
                });
                //用户获取的股权数
                batchStockAccountList.Add(new FnStockAccount()
                {
                    CustomerSysNo = buyCustomerSysNo,
                    StockNum = getStockNum
                });

                //销售股权记录
                var saleStockRecordList = GetUpdateHangSaleStock(buyCustomerSysNo, getStockNum, stockPrice);
                //购买方股权记录
                var buyStockRecord = new FnStockRecord()
                {
                    Type = StockRecordEnum.TypeEnum.股权申购.GetHashCode(),
                    OperatorType = OperatorTypeEnum.会员.GetHashCode(),
                    OperatorSysNo = buyCustomerSysNo,
                    StockNum = getStockNum,
                    TradedStockNum = getStockNum,
                    CurrentStockAmount = stockPrice,
                    Status = StockRecordEnum.StatusEnum.已处理.GetHashCode(),
                    Remarks = string.Format("当前股权价格:{0}", stockPrice),
                    CreatedDate = DateTime.Now
                };
                //新增股权池金额
                var newAddStockPoolAmount = decimal.Round(Convert.ToDecimal(0), 8);
                //获取新增股权池数
                var newAddStockPoolNum = getStockNum - saleStockRecordList.Sum(p => p.TradedStockNum);
                if (newAddStockPoolNum > 0)
                {
                    if (newAddStockPoolNum == getStockNum)
                    {
                        newAddStockPoolAmount = addStockPoolAmount;
                    }
                    else
                    {
                        var stockAmount = decimal.Round(addStockPoolAmount / getStockNum, 8);
                        newAddStockPoolAmount = decimal.Round(newAddStockPoolNum * stockAmount, 8);
                    }
                }
                //销售用户销售金额
                batchRequestList.AddRange(GetSaleAmount(rate, saleStockRecordList));
                //用户股权账户变动列表
                batchStockAccountList = GetSaleStockAccount(batchStockAccountList, saleStockRecordList);

                using (var tran = new TransactionScope())
                {
                    try
                    {
                        if (Using<ICrCustomer>().BatchUpdate(batchRequestList) <= 0)
                        {
                            throw new Exception("金额写入失败");
                        }
                        //更新股权账户
                        if (Using<IFnStockAccount>().BatchUpdateSaleStockNum(batchStockAccountList) <= 0)
                        {
                            throw new Exception("更新股权账户失败");
                        }
                        //新增申购用户股权记录
                        buyStockRecord.SysNo = Using<IFnStockRecord>().Insert(buyStockRecord);
                        if (buyStockRecord.SysNo <= 0)
                        {
                            throw new Exception("新增股权记录失败");
                        }
                        //更新挂售用户股权记录
                        if (saleStockRecordList.Any() && Using<IFnStockRecord>().BatchUpdateSaleRecord(saleStockRecordList) <= 0)
                        {
                            throw new Exception("更新销售股权记录失败");
                        }

                        //获取交易日志
                        var tradeLogList = GetSaleStockTradeLog(stockPrice, buyStockRecord, saleStockRecordList);
                        if (Using<IFnTradeLog>().BatchInsert(tradeLogList) <= 0)
                        {
                            throw new Exception("新增交易日志失败");
                        }
                        //股东见点奖励
                        var seeBonusResult = SeeBonusPoints(buyCustomerSysNo);
                        if (!seeBonusResult.Status)
                        {
                            throw new Exception(seeBonusResult.Message);
                        }
                        //股权池变动更新
                        var changeStockPool = ChangeStock(newAddStockPoolNum, newAddStockPoolAmount);
                        if (!changeStockPool.Status)
                        {
                            throw new Exception(changeStockPool.Message);
                        }

                        result.Status = true;
                        tran.Complete();
                    }
                    catch (Exception ex)
                    {
                        tran.Dispose();
                        throw ex;
                    }
                }
            }
            catch (Exception exx)
            {
                result.Message = exx.Message;
            }

            return result;
        }

        /// <summary>
        /// 取消挂售股权
        /// </summary>
        /// <param name="stockRecordSysNo">股权记录编号</param>
        /// <param name="customerSysNo">会员编号</param>
        /// <returns>JResult</returns>
        public JResult CancelSaleStock(int stockRecordSysNo, int customerSysNo)
        {
            var result = new JResult();

            try
            {
                var record = Using<IFnStockRecord>().Get(stockRecordSysNo);

                if (record == null)
                {
                    throw new Exception("数据不存在");
                }
                if (record.OperatorSysNo != customerSysNo)
                {
                    throw new Exception("该用户不合法");
                }

                using (var tran = new TransactionScope())
                {
                    try
                    {
                        var surplusStockNum = record.StockNum - record.TradedStockNum;

                        if (Using<IFnStockAccount>().UpdateCancelSaleStock(customerSysNo, surplusStockNum) <= 0)
                        {
                            throw new Exception("更新股权账户失败");
                        }
                        if (Using<IFnStockRecord>().UpdateCancelSaleStock(new FnStockRecord()
                        {
                            SysNo = record.SysNo,
                            StockNum = surplusStockNum,
                            Status = StockRecordEnum.StatusEnum.已处理.GetHashCode(),
                            Remarks = string.Format("撤销股权{0};", surplusStockNum)
                        }) <= 0)
                        {
                            throw new Exception("新增股权记录失败");
                        }

                        result.Status = true;
                        tran.Complete();
                    }
                    catch (Exception ex)
                    {
                        tran.Dispose();
                        throw ex;
                    }
                }
            }
            catch (Exception exx)
            {
                result.Message = exx.Message;
            }

            return result;
        }

        /// <summary>
        /// 平台购股权
        /// </summary>
        /// <param name="userSysNo">用户编号</param>
        /// <returns>JResult</returns>
        public JResult PlatformBuyStock(int userSysNo)
        {
            var result = new JResult();

            try
            {
                var codeList = Using<IBsCode>().GetListByTypeList(new List<int>()
                {
                    CodeEnum.CodeTypeEnum.股权价格.GetHashCode(),
                    CodeEnum.CodeTypeEnum.购股手续费率.GetHashCode(),
                });
                //固定数据
                var stockPrice = Convert.ToDecimal(codeList.First(p => p.Type == CodeEnum.CodeTypeEnum.股权价格.GetHashCode()).Value);
                var rate = Convert.ToDecimal(codeList.First(p => p.Type == CodeEnum.CodeTypeEnum.购股手续费率.GetHashCode()).Value);

                var stockRecordList = GetUpdateHangSaleStock(userSysNo, int.MaxValue, stockPrice);

                if (!stockRecordList.Any())
                {
                    throw new Exception("无挂售信息");
                }

                var getStockNum = stockRecordList.Sum(p => p.TradedStockNum);
                var saleAmountList = GetSaleAmount(rate, stockRecordList);
                //购买方股权记录
                var buyStockRecord = new FnStockRecord()
                {
                    Type = StockRecordEnum.TypeEnum.股权申购.GetHashCode(),
                    OperatorType = OperatorTypeEnum.管理员.GetHashCode(),
                    OperatorSysNo = userSysNo,
                    StockNum = getStockNum,
                    TradedStockNum = getStockNum,
                    CurrentStockAmount = stockPrice,
                    Status = StockRecordEnum.StatusEnum.已处理.GetHashCode(),
                    Remarks = "管理员进行平台收购",
                    CreatedDate = DateTime.Now
                };

                var changeStockAccountList = GetSaleStockAccount(new List<FnStockAccount>(), stockRecordList);


                using (var tran = new TransactionScope())
                {
                    try
                    {
                        if (Using<ICrCustomer>().BatchUpdate(saleAmountList) <= 0)
                        {
                            throw new Exception("金额写入失败");
                        }
                        //更新股权账户
                        if (Using<IFnStockAccount>().BatchUpdateSaleStockNum(changeStockAccountList) <= 0)
                        {
                            throw new Exception("更新股权账户失败");
                        }
                        //新增申购用户股权记录
                        buyStockRecord.SysNo = Using<IFnStockRecord>().Insert(buyStockRecord);
                        if (buyStockRecord.SysNo <= 0)
                        {
                            throw new Exception("新增股权记录失败");
                        }
                        //更新挂售用户股权记录
                        if (Using<IFnStockRecord>().BatchUpdateSaleRecord(stockRecordList) <= 0)
                        {
                            throw new Exception("更新销售股权记录失败");
                        }
                        //获取交易日志
                        var tradeLogList = GetSaleStockTradeLog(stockPrice, buyStockRecord, stockRecordList);
                        if (Using<IFnTradeLog>().BatchInsert(tradeLogList) <= 0)
                        {
                            throw new Exception("新增交易日志失败");
                        }

                        result.Status = true;
                        tran.Complete();
                    }
                    catch (Exception ex)
                    {
                        tran.Dispose();
                        throw ex;
                    }
                }
            }
            catch (Exception exx)
            {
                result.Message = exx.Message;
            }

            return result;
        }

        /// <summary>
        /// 获取需更新股权挂售记录
        /// </summary>
        /// <param name="buyCustomerSysNo">购买会员编码</param>
        /// <param name="getStockNum">购买股权数</param>
        /// <param name="nowStockAmount">当前股权金额</param>
        /// <returns>需更新股权挂售记录列表</returns>
        public List<FnStockRecord> GetUpdateHangSaleStock(int buyCustomerSysNo, int getStockNum, decimal nowStockAmount)
        {
            var stockRecordList = new List<FnStockRecord>();
            //获取未处理的挂售股权
            var saleStockRecordList = Using<IFnStockRecord>().GetList(new StockRecordRequest()
            {
                Type = StockRecordEnum.TypeEnum.股权挂售.GetHashCode(),
                StatusList = new List<int>() { StockRecordEnum.StatusEnum.待处理.GetHashCode(), StockRecordEnum.StatusEnum.部分处理.GetHashCode() }
            }).OrderBy(p => p.CreatedDate);

            //进行挂售的股权处理
            foreach (var item in saleStockRecordList)
            {
                FnStockRecord stockRecord;
                if (getStockNum <= 0)
                {
                    break;
                }
                if (getStockNum >= item.StockNum)
                {
                    stockRecord = new FnStockRecord()
                    {
                        SysNo = item.SysNo,
                        OperatorSysNo = item.OperatorSysNo,
                        TradedStockNum = item.StockNum - item.TradedStockNum,
                        CurrentStockAmount = item.CurrentStockAmount,
                        Status = StockRecordEnum.StatusEnum.已处理.GetHashCode(),
                        Remarks = string.Format("买入方[{0}]以当前股权价格:{1}，股权数：{2}买入；", buyCustomerSysNo, nowStockAmount, item.StockNum - item.TradedStockNum)
                    };

                    getStockNum -= item.StockNum;
                }
                else
                {
                    stockRecord = new FnStockRecord()
                    {
                        SysNo = item.SysNo,
                        OperatorSysNo = item.OperatorSysNo,
                        TradedStockNum = getStockNum,
                        CurrentStockAmount = item.CurrentStockAmount,
                        Status = StockRecordEnum.StatusEnum.部分处理.GetHashCode(),
                        Remarks = string.Format("买入方[{0}]以当前股权价格:{1}，股权数：{2}买入部分；", buyCustomerSysNo, nowStockAmount, getStockNum)
                    };

                    getStockNum = 0;
                }

                stockRecordList.Add(stockRecord);
            }

            return stockRecordList;
        }

        /// <summary>
        /// 获取销售股权交易日志列表
        /// </summary>
        /// <param name="nowStockAmount">当前股权金额</param>
        /// <param name="buyStockRecord">购买方股权记录</param>
        /// <param name="saleStockRecordList">销售股权记录列表</param>
        /// <returns>销售股权交易日志列表</returns>
        public List<FnTradeLog> GetSaleStockTradeLog(decimal nowStockAmount, FnStockRecord buyStockRecord, List<FnStockRecord> saleStockRecordList)
        {
            var tradeLogList = new List<FnTradeLog>();

            foreach (var item in saleStockRecordList)
            {
                //销售方交易记录
                tradeLogList.Add(new FnTradeLog()
                {
                    SourceType = TradeLogEnum.SourceTypeEnum.股权记录.GetHashCode(),
                    SourceSysNo = item.SysNo,
                    OutOperatorType = OperatorTypeEnum.会员.GetHashCode(),
                    OutOperatorSysNo = item.OperatorSysNo,
                    InOperatorType = buyStockRecord.OperatorType,
                    InOperatorSysNo = buyStockRecord.OperatorSysNo,
                    TradeAmount = item.TradedStockNum * item.CurrentStockAmount,
                    CurrentStockAmount = item.CurrentStockAmount,
                    Remarks = string.Format("卖家卖出{0}股", item.TradedStockNum),
                    CreatedDate = DateTime.Now
                });
                //购买方交易记录
                tradeLogList.Add(new FnTradeLog()
                {
                    SourceType = TradeLogEnum.SourceTypeEnum.股权记录.GetHashCode(),
                    SourceSysNo = buyStockRecord.SysNo,
                    OutOperatorType = OperatorTypeEnum.会员.GetHashCode(),
                    OutOperatorSysNo = item.OperatorSysNo,
                    InOperatorType = buyStockRecord.OperatorType,
                    InOperatorSysNo = buyStockRecord.OperatorSysNo,
                    TradeAmount = item.TradedStockNum * nowStockAmount,
                    CurrentStockAmount = nowStockAmount,
                    Remarks = string.Format("买家买入{0}股", item.TradedStockNum),
                    CreatedDate = DateTime.Now
                });
            }

            var surplusTradedStockNum = buyStockRecord.TradedStockNum - saleStockRecordList.Sum(p => p.TradedStockNum);
            if (surplusTradedStockNum > 0)
            {
                //购买方交易记录
                tradeLogList.Add(new FnTradeLog()
                {
                    SourceType = TradeLogEnum.SourceTypeEnum.股权记录.GetHashCode(),
                    SourceSysNo = buyStockRecord.SysNo,
                    OutOperatorType = OperatorTypeEnum.管理员.GetHashCode(),
                    OutOperatorSysNo = 0,
                    InOperatorType = buyStockRecord.OperatorType,
                    InOperatorSysNo = buyStockRecord.OperatorSysNo,
                    TradeAmount = surplusTradedStockNum * nowStockAmount,
                    CurrentStockAmount = nowStockAmount,
                    Remarks = string.Format("买家系统买入{0}股", surplusTradedStockNum),
                    CreatedDate = DateTime.Now
                });
            }

            return tradeLogList;
        }

        /// <summary>
        /// 获取用户销售金额
        /// </summary>
        /// <param name="rate">手续费率</param>
        /// <param name="saleStockRecordList">销售股权记录列表</param>
        /// <returns>用户获得的销售金额</returns>
        public List<CustomerExtBatchRequest> GetSaleAmount(decimal rate, IEnumerable<FnStockRecord> saleStockRecordList)
        {
            var list = new List<CustomerExtBatchRequest>();

            foreach (var item in saleStockRecordList)
            {
                var saleAmount = decimal.Round(item.TradedStockNum * item.CurrentStockAmount * (1 - rate), 8);
                var model = list.FirstOrDefault(p => p.CustomerSysNo == item.OperatorSysNo);

                if (model == null)
                {
                    list.Add(new CustomerExtBatchRequest()
                    {
                        CustomerSysNo = item.OperatorSysNo,
                        WalletAmount = saleAmount,
                        HistoryWalletAmount = saleAmount
                    });
                }
                else
                {
                    model.WalletAmount += saleAmount;
                    model.HistoryWalletAmount += saleAmount;
                }
            }

            return list;
        }

        /// <summary>
        /// 获取销售用户股权账户变动列表
        /// </summary>
        /// <param name="batchStockAccountList">用户股权账户变动列表</param>
        /// <param name="saleStockRecordList">销售股权记录列表</param>
        /// <returns>用户股权账户变动列表</returns>
        public List<FnStockAccount> GetSaleStockAccount(List<FnStockAccount> batchStockAccountList, IEnumerable<FnStockRecord> saleStockRecordList)
        {
            //销售用户股权账户变动
            foreach (var item in saleStockRecordList)
            {
                var stockAccount = batchStockAccountList.FirstOrDefault(p => p.CustomerSysNo == item.OperatorSysNo);

                if (stockAccount == null)
                {
                    batchStockAccountList.Add(new FnStockAccount()
                    {
                        CustomerSysNo = item.OperatorSysNo,
                        StockForSale = -item.TradedStockNum,
                        StockSold = item.TradedStockNum
                    });
                }
                else
                {
                    stockAccount.StockForSale = stockAccount.StockForSale - item.TradedStockNum;
                    stockAccount.StockSold = stockAccount.StockSold + item.TradedStockNum;
                }
            }

            return batchStockAccountList;
        }

        /// <summary>
        /// 锁对象
        /// </summary>
        private static readonly object _lock = new object();

        /// <summary>
        /// 股权变动
        /// </summary>
        /// <param name="changeStockNum">股权数变动（未变动时0）</param>
        /// <param name="changeStockAmount">股权金额变动（未变动时0）</param>
        /// <returns>操作结果</returns>
        public JResult ChangeStock(int changeStockNum, decimal changeStockAmount)
        {
            var result = new JResult();

            try
            {
                if (changeStockNum == 0 && changeStockAmount == 0)
                {
                    return new JResult() { Status = true };
                }

                lock (_lock)
                {
                    var codeList = Using<IBsCode>().GetListByTypeList(new List<int>()
                    {
                        CodeEnum.CodeTypeEnum.股池资金.GetHashCode(),
                        CodeEnum.CodeTypeEnum.股权数总额.GetHashCode()
                    });

                    var stockNum = codeList.First(p => p.Type == CodeEnum.CodeTypeEnum.股权数总额.GetHashCode());
                    var stockTotalAmount = codeList.First(p => p.Type == CodeEnum.CodeTypeEnum.股池资金.GetHashCode());

                    var newStockNum = decimal.Round(Convert.ToInt32(stockNum.Value) + changeStockNum, 8);
                    var newStockTotalAmount = decimal.Round(Convert.ToDecimal(stockTotalAmount.Value) + changeStockAmount, 8);
                    var newStockAmount = decimal.Round(newStockTotalAmount / newStockNum, 8);

                    var row = Using<IBsCode>().BatchUpdate(new List<BsCode>()
                    {
                        new BsCode()
                        {
                            Type = CodeEnum.CodeTypeEnum.股池资金.GetHashCode(),
                            Value = newStockTotalAmount.ToString(CultureInfo.InvariantCulture)
                        },
                        new BsCode()
                        {
                            Type = CodeEnum.CodeTypeEnum.股权数总额.GetHashCode(),
                            Value = newStockNum.ToString(CultureInfo.InvariantCulture)
                        },
                        new BsCode()
                        {
                            Type = CodeEnum.CodeTypeEnum.股权价格.GetHashCode(),
                            Value = newStockAmount.ToString(CultureInfo.InvariantCulture)
                        }
                    });

                    if (row <= 0)
                    {
                        throw new Exception("修改数据失败");
                    }

                    result.Status = true;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 会员股东见点奖励
        /// </summary>
        /// <param name="customerSysNo">会员编号</param>
        /// <returns>操作结果</returns>
        public JResult SeeBonusPoints(int customerSysNo)
        {
            var result = new JResult();

            try
            {
                var model = Using<ICrCustomer>().Get(customerSysNo);
                if (model == null)
                {
                    throw new Exception("会员不存在");
                }

                var code = Using<IBsCode>().GetByType(CodeEnum.CodeTypeEnum.股东见点奖.GetHashCode());
                if (code == null)
                {
                    throw new Exception("股东见点奖配置不存在");
                }
                var levelConfigList = code.Value.ToObject<List<LevelConfig>>();

                //判断可分奖励层级会员
                var customerSysNoList = !string.IsNullOrWhiteSpace(model.LevelCustomerStr)
                    ? model.LevelCustomerStr.Split(',').Select(p => Convert.ToInt32(p)).ToList()
                    : new List<int>();
                var levelList = new List<int>();
                for (var i = 1; i <= levelConfigList.Count; i++)
                {
                    levelList.Add(model.Level - i);
                }

                //可得见点奖股东
                var customerList = new List<CrCustomer>();
                if (customerSysNoList.Any())
                {
                    customerList = Using<ICrCustomer>().GetList(new CustomerExtRequest()
                    {
                        Grade = CustomerEnum.Grade.股东.GetHashCode(),
                        CustomerSysNoList = customerSysNoList,
                        LevelList = levelList
                    }).ToList();
                }

                var bonusLogList = new List<FnBonusLog>();
                var customerAmountList = new List<CustomerExtBatchRequest>();

                if (customerList.Any())
                {
                    customerList.ForEach(p =>
                    {
                        var amount = levelConfigList.First(x => x.Level == model.Level - p.Level).Amount;

                        customerAmountList.Add(new CustomerExtBatchRequest()
                        {
                            CustomerSysNo = p.SysNo,
                            WalletAmount = amount,
                            HistoryWalletAmount = amount
                        });
                        bonusLogList.Add(new FnBonusLog()
                        {
                            CustomerSysNo = p.SysNo,
                            SourceSysNo = model.SysNo,
                            SourceSerialNumber = model.SerialNumber.ToString(CultureInfo.InvariantCulture),
                            Amount = amount,
                            Type = FnEnum.BonusLogType.股东见点奖励.GetHashCode(),
                            CreatedDate = DateTime.Now
                        });
                    });
                }

                if (!customerAmountList.Any())
                {
                    result.Status = true;
                    throw new Exception("该会员没有获得见点奖的股东");
                }

                using (var tran = new TransactionScope())
                {
                    try
                    {
                        if (Using<ICrCustomer>().BatchUpdate(customerAmountList) <= 0)
                        {
                            throw new Exception("见点奖金写入失败");
                        }
                        if (Using<IFnBonusLog>().BatchInsert(bonusLogList) <= 0)
                        {
                            throw new Exception("奖金记录写入失败");
                        }

                        result.Status = true;
                        tran.Complete();
                    }
                    catch (Exception ex)
                    {
                        tran.Dispose();
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }

        #endregion
    }
}
