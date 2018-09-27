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
        /// <summary>
        /// 新升级代码
        /// </summary>
        /// <returns></returns>
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
            var agencySales = gradeJson.ToObject<List<AgencySale>>();

            
            return result;
        }
    }
}
