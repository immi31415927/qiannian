using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.ConsoleApp
{
    using EC.Application.Tables.CRM;
    using EC.Entity.Parameter.Request.NewCRM;

    class Program
    {
        static void Main(string[] args)
        {
            Libraries.Core.ServiceLocator.Initialization();

            var request = NewCustomerApp.Instance.NewUpgrade(new NewUpgradeRequest()
            {
                CustomerSysNo = 30
            });
        }
    }
}
