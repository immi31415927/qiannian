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
    using EC.Entity.Parameter.Request.NewCRM;
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
                var upgradeGrades =agencySales.Where(p => p.Grade == nestGrade).ToList();
                if (upgradeGrades.Count != grade.Top){
                    throw new Exception("代理销售等级错误");
                }

                //升级金额验证
                if (customer.UpgradeFundAmount < grade.Amount){
                    throw new Exception("升级基金不足");
                }


            }
            catch (Exception ex) { 
            
            }
            return result;
        }
    }
}
