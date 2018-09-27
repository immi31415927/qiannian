using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Tests
{
    using EC.Application.Tables.CRM;
    using EC.Entity.Parameter.Request.NewCRM;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public class NewCustomerAppTest
    {
        [TestMethod]
        public void NewUpgradeTest()
        {
            Libraries.Core.ServiceLocator.Initialization();

            var request = NewCustomerApp.Instance.NewUpgrade(new NewUpgradeRequest()
            {
                CustomerSysNo = 30
            });

            //Assert.IsTrue(request.Status);
        }
    }
}
