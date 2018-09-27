using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EC.Application.Tables.CRM;
using EC.Application.Tables.Fn;
using EC.Entity.Parameter.Request.CRM;
using EC.Entity.Parameter.Request.Finance;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EC.Entity.Parameter.Request.Member;
namespace EC.Application.Tables.CRM.Tests
{
    [TestClass()]
    public class SmsAppTests
    {
        [TestMethod()]
        public void SMSVerifyTest()
        {
            SmsApp.Instance.SMSVerify("15008228718");
            Assert.Fail();
        }

        [TestMethod()]
        public void UpdatePayTest()
        {
            Libraries.Core.ServiceLocator.Initialization();

            var request = new UpdatePayStatusRequest()
            {
                Amount = new decimal(200),
                VoucherNo = "4200000045201711277352608869",
                OrderSysNo = "9aa6f97bf7404fac92ddd5e7a07889ff"
            };

            var result = RechargeApp.Instance.UpdatePayStatus(request);
            Assert.IsTrue(result.Status);
        }

        [TestMethod()]
        public void UpdateTest()
        {
            Libraries.Core.ServiceLocator.Initialization();

            var request = CustomerApp.Instance.Upgrade(new UpgradeRequest()
            {
                CustomerSysNo = 68,
                Amount = 200,
                SelectGrade = 20
            });

            Assert.IsTrue(request.Status);
        }

        [TestMethod()]
        public void NewUpdateTest()
        {
            Libraries.Core.ServiceLocator.Initialization();

            var param = new NewUpgradeRequest() 
            { 
            };
            //var request = NewCustomerApp.Instance.NewUpgrade(param);

            //Assert.IsTrue(request.Status);
        }
    }
}
