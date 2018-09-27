using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Application.Tables.CRM
{
    using EC.DataAccess.Bs;
    using EC.DataAccess.CRM;
    using EC.Entity;
    using EC.Entity.Enum;
    using EC.Entity.Output.Fore;
    using EC.Entity.Parameter.Request.CRM;
    using EC.Entity.Parameter.Request.NewCRM;
    using EC.Entity.Tables.CRM;
    using EC.Entity.Tables.Finance;
    using EC.Libraries.Core;
    using EC.Libraries.Util;

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
            return grade + interval;
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

            try
            {
                //升级级别
				var nestGrade = NextGrade(customer.Grade);
                //升级等级
                var grade = grades.FirstOrDefault(p => p.Type == nestGrade);
                if (grade == null) {
                    throw new Exception("升级等级错误");
                }

                //代理销售验证
                var upgradeGrades =agencySales.Where(p => p.Grade == nestGrade).OrderBy(p=>p.Sort).ToList();
                if (upgradeGrades.Count != grade.Top){
                    throw new Exception("代理销售等级错误");
                }

                //升级金额验证
                if (customer.UpgradeFundAmount < grade.Amount){
                    throw new Exception("升级基金不足");
                }

                //获取所有推荐人
                var referrerTop = GetReferrer(customers, new List<ParentIds>(), customer.ReferrerSysNo, grade.Top,1);
                //批量更新上层
                var batchUpgradeParentList = new List<BatchUpgradeParent>();
                //批量添加奖金日志对象
                var batchInertBonusLogList = new List<FnBonusLog>();
                //微信提示
                var weiXinTipList = new List<WeiXinTip>();

                foreach (var item in referrerTop)
                { 
                    var referrer = upgradeGrades.FirstOrDefault(p=>p.Sort == item.Sort);

                    //钱包
                    var walletAmount = item.Grade >= grade.Type ? referrer.Amount : referrer.Amount / 2;


                    if (item.Grade < grade.Type)
                    {
                        var thatReferrerAll =GetReferrer(customers, new List<ParentIds>(), item.CustomerSysNo, 1);
                        if (thatReferrerAll.Count > 0)
                        {
                            this.Find(batchUpgradeParentList, batchInertBonusLogList,weiXinTipList, thatReferrerAll, grade, item.Sort);
                        }
                        //设置待结算
                        //SettledBonus(batchUpgradeParent, grade.Type, walletAmount);
                        //batchUpgradeParentList.Add(batchUpgradeParent);
                    }
                    else {
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
                    }
                }
            }
            catch (Exception ex) {
                result.Message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="batchUpgradeParent"></param>
        /// <param name="bonusLog"></param>
        /// <param name="weixinTip"></param>
        /// <param name="thatReferrerAll"></param>
        /// <param name="grade"></param>
        /// <param name="sort"></param>
        private void Find(List<BatchUpgradeParent> batchUpgradeParent,List<FnBonusLog> bonusLog,List<WeiXinTip> weixinTip, List<ParentIds> thatReferrerAll,Grade grade, int sort)
        {
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
