using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Application.Tables.CRM
{
    using EC.Application.Tables.WeiXin;
    using EC.DataAccess.Bs;
    using EC.DataAccess.CRM;
    using EC.DataAccess.Fn;
    using EC.Entity;
    using EC.Entity.Enum;
    using EC.Entity.Output.Fore;
    using EC.Entity.Parameter.Request.CRM;
    using EC.Entity.Parameter.Request.NewCRM;
    using EC.Entity.Parameter.Request.WeiXin;
    using EC.Entity.Tables.CRM;
    using EC.Entity.Tables.Finance;
    using EC.Libraries.Core;
    using EC.Libraries.Util;
    using System.Transactions;

    /// <summary>
    /// 会员业务层
    /// </summary>
    public class NewCustomerApp : Base<NewCustomerApp>
    {
		private const int interval = 10;

		// <summary>
        /// 下一个等级
        /// </summary>
		private int NextGrade(int grade)
		{
            var nextGrade = grade + interval;
            if (nextGrade > EC.Entity.Enum.CustomerEnum.NewGrade.八星代理.GetHashCode()) {
                throw new Exception("你已是最高级别了");
            }
            return nextGrade;
		}
		
        /// <summary>
        /// 新升级代码
        /// </summary>
        public JResult NewUpgrade(NewUpgradeRequest request)
        {
            var result = new JResult()
            {
                Status = false
            };

            //当前会员
            var customer = Using<INewCustomer>().Get(request.CustomerSysNo);
            if (customer == null)
            {
                result.Message = "无会员信息！";
            }
            //获取所有会员
            var customers = Using<INewCustomer>().GetAll();
            //码表查询参数
            var codeQuery = new List<int> 
            { 
                CodeEnum.CodeTypeEnum.会员等级信息.GetHashCode(), 
                CodeEnum.CodeTypeEnum.代理销售奖金.GetHashCode() 
            };

            var codes = Using<IBsCode>().GetListByTypeList(codeQuery);

            //会员等级
            var gradeJson = codes.First(p => p.Type == CodeEnum.CodeTypeEnum.会员等级信息.GetHashCode()).Value;
            var grades = gradeJson.ToObject<List<Grade>>();
            //代理销售
            var agencySaleJson = codes.First(p => p.Type == CodeEnum.CodeTypeEnum.代理销售奖金.GetHashCode()).Value;
            var agencySales = agencySaleJson.ToObject<List<AgencySale>>();

            using (var tran = new TransactionScope())
            {
                try
                {
                    //升级级别
                    var nestGrade = NextGrade(customer.Grade);
                    //升级等级
                    var grade = grades.FirstOrDefault(p => p.Type == nestGrade);
                    if (grade == null)
                    {
                        throw new Exception("升级等级错误");
                    }

                    //代理销售验证
                    var upgradeGrades = agencySales.Where(p => p.Grade == nestGrade).OrderBy(p => p.Sort).ToList();
                    if (upgradeGrades.Count != grade.Top)
                    {
                        throw new Exception("代理销售等级错误");
                    }

                    //升级金额验证
                    if (customer.UpgradeFundAmount < grade.Amount)
                    {
                        throw new Exception("升级基金不足");
                    }

                    //获取所有推荐人
                    var referrerTop = GetReferrer(customers, new List<ParentIds>(), customer.ReferrerSysNo, grade.Top, 1);
                    //批量更新上层
                    var batchUpgradeParentList = new List<BatchUpgradeParent>();
                    //批量添加奖金日志对象
                    var batchInertBonusLogList = new List<FnBonusLog>();
                    //微信提示
                    var weiXinTipList = new List<WeiXinTip>();

                    #region
                    foreach (var item in referrerTop)
                    {
                        var referrer = upgradeGrades.FirstOrDefault(p => p.Sort == item.Sort);

                        //钱包
                        var walletAmount = item.Grade >= grade.Type ? referrer.Amount : referrer.Amount / 2;
                        if (item.Grade < grade.Type)
                        {
                            #region 不满足跳级查找所得50%
                            var thatReferrerAll = GetReferrer(customers, new List<ParentIds>(), item.ReferrerSysNo, 1);
                            if (thatReferrerAll.Count > 0)
                            {
                                //batchUpgradeParentList, batchInertBonusLogList,weiXinTipList, 
                                var findCustomer = this.Find(thatReferrerAll, grade, item, item.Sort, item.Sort, 1);
                                if (findCustomer != null)
                                {
                                    //奖金人
                                    var batchUpgradeParent = new BatchUpgradeParent()
                                    {
                                        CustomerSysNo = findCustomer.CustomerSysNo,
                                        WalletAmount = walletAmount,
                                        SettledBonus20 = 0,
                                        SettledBonus30 = 0,
                                        SettledBonus40 = 0,
                                        SettledBonus50 = 0,
                                        SettledBonus60 = 0,
                                        SettledBonus70 = 0,
                                        SettledBonus80 = 0,
                                        SettledBonus90 = 0
                                    };
                                    batchUpgradeParentList.Add(batchUpgradeParent);
                                    //奖金日志
                                    var bonusLog = new FnBonusLog()
                                    {
                                        CustomerSysNo = findCustomer.CustomerSysNo,
                                        SourceSysNo = customer.SysNo,
                                        SourceSerialNumber = "",
                                        Amount = walletAmount,
                                        Type = FnEnum.BonusLogType.代理奖励.GetHashCode(),
                                        Remarks = string.Format("{0}贡献,升级类型：普通代理,钱包：{1}，升级基金：{2}", customer.RealName, customer.WalletAmount, customer.UpgradeFundAmount),
                                        Status = FnEnum.BonusLogStatus.撤销.GetHashCode(),
                                        CreatedDate = DateTime.Now
                                    };
                                    batchInertBonusLogList.Add(bonusLog);
                                    //微信提示
                                    var model = customers.FirstOrDefault(p => p.SysNo == item.CustomerSysNo);
                                    if (model != null)
                                    {
                                        if (!string.IsNullOrWhiteSpace(model.OpenId))
                                        {
                                            weiXinTipList.Add(new WeiXinTip()
                                            {
                                                OpenId = model.OpenId,
                                                Keyword1 = walletAmount.ToString() + "元",
                                                Keyword2 = walletAmount.ToString() + "元",
                                                Remark = string.Format("业绩来源：{0}", customer.RealName)
                                            });
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region 当前会员所得50%
                            //奖金人
                            var _batchUpgradeParent = new BatchUpgradeParent()
                            {
                                CustomerSysNo = item.CustomerSysNo,
                                WalletAmount = 0,
                                SettledBonus20 = 0,
                                SettledBonus30 = 0,
                                SettledBonus40 = 0,
                                SettledBonus50 = 0,
                                SettledBonus60 = 0,
                                SettledBonus70 = 0,
                                SettledBonus80 = 0,
                                SettledBonus90 = 0
                            };

                            SettledBonus(_batchUpgradeParent, nestGrade, walletAmount);

                            batchUpgradeParentList.Add(_batchUpgradeParent);
                            //奖金日志
                            var _bonusLog = new FnBonusLog()
                            {
                                CustomerSysNo = item.CustomerSysNo,
                                SourceSysNo = customer.SysNo,
                                SourceSerialNumber = "",
                                Amount = walletAmount,
                                Type = FnEnum.BonusLogType.代理奖励.GetHashCode(),
                                Remarks = string.Format("{0}贡献,升级类型：普通代理,钱包：{1}，升级基金：{2}", customer.RealName, customer.WalletAmount, customer.UpgradeFundAmount),
                                Status = FnEnum.BonusLogStatus.撤销.GetHashCode(),
                                CreatedDate = DateTime.Now
                            };
                            batchInertBonusLogList.Add(_bonusLog);
                            //微信提示
                            var _model = customers.FirstOrDefault(p => p.SysNo == item.CustomerSysNo);
                            if (_model != null)
                            {
                                if (!string.IsNullOrWhiteSpace(_model.OpenId))
                                {
                                    weiXinTipList.Add(new WeiXinTip()
                                    {
                                        OpenId = _model.OpenId,
                                        Keyword1 = walletAmount.ToString() + "元",
                                        Keyword2 = walletAmount.ToString() + "元",
                                        Remark = string.Format("业绩来源：{0}", item.RealName)
                                    });
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            #region 满足所有100%
                            //奖金人
                            var batchUpgradeParent = new BatchUpgradeParent()
                            {
                                CustomerSysNo = item.CustomerSysNo,
                                WalletAmount = walletAmount,
                                SettledBonus20 = 0,
                                SettledBonus30 = 0,
                                SettledBonus40 = 0,
                                SettledBonus50 = 0,
                                SettledBonus60 = 0,
                                SettledBonus70 = 0,
                                SettledBonus80 = 0,
                                SettledBonus90 = 0
                            };
                            batchUpgradeParentList.Add(batchUpgradeParent);
                            //奖金日志
                            var bonusLog = new FnBonusLog()
                            {
                                CustomerSysNo = item.CustomerSysNo,
                                SourceSysNo = customer.SysNo,
                                SourceSerialNumber = "",
                                Amount = walletAmount,
                                Type = FnEnum.BonusLogType.代理奖励.GetHashCode(),
                                Remarks = string.Format("{0}贡献,升级类型：普通代理,钱包：{1}，升级基金：{2}", customer.RealName, customer.WalletAmount, customer.UpgradeFundAmount),
                                Status = FnEnum.BonusLogStatus.撤销.GetHashCode(),
                                CreatedDate = DateTime.Now
                            };
                            batchInertBonusLogList.Add(bonusLog);
                            //微信提示
                            var model = customers.FirstOrDefault(p => p.SysNo == item.CustomerSysNo);
                            if (model != null)
                            {
                                if (!string.IsNullOrWhiteSpace(model.OpenId))
                                {
                                    weiXinTipList.Add(new WeiXinTip()
                                    {
                                        OpenId = model.OpenId,
                                        Keyword1 = walletAmount.ToString() + "元",
                                        Keyword2 = walletAmount.ToString() + "元",
                                        Remark = string.Format("业绩来源：{0}", customer.RealName)
                                    });
                                }
                            }
                            #endregion
                        }
                    }
                    #endregion

                    //批量更新奖金
                    if (batchUpgradeParentList != null && batchUpgradeParentList.Any())
                    {
                        if (Using<INewCustomer>().BatchUpdate(batchUpgradeParentList) <= 0)
                        {
                            throw new Exception("批量更新推荐人奖金失败!");
                        }
                    }
                    //批量更新奖金日志
                    if (batchInertBonusLogList != null && batchInertBonusLogList.Any())
                    {
                        if (Using<IFnBonusLog>().BatchInsert(batchInertBonusLogList) <= 0)
                        {
                            throw new Exception("批量添加推荐人奖金日志失败!");
                        }
                    }

                    //将结算奖金写入钱包
                    if (Using<INewCustomer>().UpdatePendingAmount(customer.SysNo, nestGrade) <= 0)
                    {
                        throw new Exception("更新结算奖金失败!");
                    }
                    //更新等级和减去升级金额
                    if (Using<INewCustomer>().UpdateGradeAndUpgradeFundAmount(new CrCustomer()
                    {
                        SysNo = customer.SysNo,
                        Grade = nestGrade,
                        UpgradeFundAmount = customer.UpgradeFundAmount - grade.Amount
                    }) <= 0)
                    {
                        throw new Exception("更新级别失败!");
                    }
                    //微信提示
                    foreach (var p in weiXinTipList)
                    {
                        WeiXinApp.Instance.SendMessage(new SendMessageRequest()
                        {
                            WeiXinAppId = "wx9535283296d186c3",
                            WeiXinAppSecret = "543e75c11909f438c02870ecbab85f5d",
                            OpenId = p.OpenId,
                            TemplateId = "a8oNldLv7Nlvbkajt6vQbWUBOgmu7EWW2j08UzdQgBg",
                            Data = new
                            {
                                first = new { color = "#000000", value = "您有一笔新的分红奖金到账啦！" },
                                keyword1 = new { color = "#000000", value = p.Keyword1 },
                                keyword2 = new { color = "#000000", value = p.Keyword2 },
                                remark = new { color = "#000000", value = p.Remark }
                            },
                            Url = "#"
                        });
                    }
                    result.Message = "升级成功！";
                    result.Status = true;
                    tran.Complete();
                }
                catch (Exception ex)
                {
                    tran.Dispose();
                    result.Message = ex.Message;
                }
            }
            return result;
        }

        /// <summary>
        /// 循环查找
        /// </summary>
        /// <param name="referrerAll"></param>
        /// <param name="grade"></param>
        /// <param name="item"></param>
        /// <param name="increment">增量</param>
        /// <param name="cycleTotal">循环总数</param>
        /// <param name="cycles">次数</param>
        private ParentIds Find(List<ParentIds> referrerAll, Grade grade, ParentIds item, int increment, int cycleTotal, int cycles = 0)
        {
            var referrer = referrerAll.FirstOrDefault(p => p.CustomerSysNo == item.ReferrerSysNo);
            if (referrer != null)
            {
                if (cycleTotal == cycles)
                {
                    if (referrer.Grade >= grade.Type)
                    {
                        return referrer;
                    }
                    else
                    {
                        cycles++;
                        cycleTotal = cycleTotal + increment;
                        return this.Find(referrerAll, grade, referrer, increment, cycleTotal, cycles);
                    }
                }
                else
                {
                    cycles++;
                    return this.Find(referrerAll, grade, referrer, increment, cycleTotal, cycles);
                }
            }
            else {
                return null;
            }
        }

        /// <summary>
        /// 待结算字段设置
        /// </summary>
        /// <param name="batchUpgradeParent"></param>
        /// <param name="grade">升级级别</param>
        /// <param name="bonus">奖金</param>
        /// <param name="batchUpgradeParent"></param>
        private void SettledBonus(BatchUpgradeParent batchUpgradeParent, int grade, decimal bonus)
        {
            switch (grade)
            {
                case 10:
                    batchUpgradeParent.SettledBonus10 = bonus;
                    break;
                case 20:
                    batchUpgradeParent.SettledBonus20 = bonus;
                    break;
                case 30:
                    batchUpgradeParent.SettledBonus30 = bonus;
                    break;
                case 40:
                    batchUpgradeParent.SettledBonus40 = bonus;
                    break;
                case 50:
                    batchUpgradeParent.SettledBonus50 = bonus;
                    break;
                case 60:
                    batchUpgradeParent.SettledBonus60 = bonus;
                    break;
                case 70:
                    batchUpgradeParent.SettledBonus70 = bonus;
                    break;
                case 80:
                    batchUpgradeParent.SettledBonus80 = bonus;
                    break;
                case 90:
                    batchUpgradeParent.SettledBonus90 = bonus;
                    break;
            }
        }

        /// <summary>
        /// 获取升级推荐人列表
        /// </summary>
        /// <param name="customers">所有会员</param>
        /// <param name="parentIds">推荐人列表</param>
        /// <param name="referrerSysNo">推荐编号</param>
        /// <param name="sort">排序</param>
        public List<ParentIds> GetReferrer(IList<CrCustomer> customers, List<ParentIds> parentIds, int referrerSysNo,int sort = 1)
        {
            var customer = customers.FirstOrDefault(p => p.SysNo.Equals(referrerSysNo));
            if (customer != null)
            {
                parentIds.Add(new ParentIds()
                {
                    CustomerSysNo = customer.SysNo,
                    RealName = customer.RealName,
                    Grade = customer.Grade,
                    Sort = sort,
                    ReferrerSysNo = customer.ReferrerSysNo,
                    OpenId = customer.OpenId,
                    WalletAmount = customer.WalletAmount,
                    UpgradeFundAmount = customer.UpgradeFundAmount,
                    GeneralBonus = customer.GeneralBonus,
                    AreaBonus = customer.AreaBonus,
                    GlobalBonus = customer.GlobalBonus
                });
                sort++;
                return GetReferrer(customers, parentIds, customer.ReferrerSysNo, sort);
            }
            return parentIds;
        }

        /// <summary>
        /// 获取升级推荐人列表
        /// </summary>
        /// <param name="customersList">所有会员</param>
        /// <param name="parentIds">推荐人列表</param>
        /// <param name="referrerSysNo">推荐编号</param>
        /// <param name="layer">获取上级层数</param>
        /// <param name="sort">排序</param>
        /// <returns>List<ParentIds></returns>
        public List<ParentIds> GetReferrer(IList<CrCustomer> customersList, List<ParentIds> parentIds, int referrerSysNo, int layer, int sort = 1)
        {
            var customer = customersList.FirstOrDefault(p => p.SysNo.Equals(referrerSysNo));
            if (customer != null)
            {
                parentIds.Add(new ParentIds()
                {
                    CustomerSysNo = customer.SysNo,
                    RealName = customer.RealName,
                    Grade = customer.Grade,
                    Sort = sort,
                    ReferrerSysNo = customer.ReferrerSysNo,
                    OpenId = customer.OpenId,
                    WalletAmount = customer.WalletAmount,
                    UpgradeFundAmount = customer.UpgradeFundAmount,
                    GeneralBonus = customer.GeneralBonus,
                    AreaBonus = customer.AreaBonus,
                    GlobalBonus = customer.GlobalBonus
                });
                if (sort.Equals(layer))
                    return parentIds;
                sort++;
                return GetReferrer(customersList, parentIds, customer.ReferrerSysNo, layer, sort);
            }
            return parentIds;
        }
    }
}
