using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using EC.Application.Tables.CRM;
using EC.Entity.Parameter.Request.CRM;

namespace EC.H5.Controllers
{
    public class TeamController : BaseController
    {
        //
        // GET: /Team/

        /// <summary>
        /// 我的团队
        /// </summary>
        /// <returns>视图</returns>
        public ActionResult MyTeam()
        {
            ViewBag.teamCount = 0;
            var teamCount = CustomerApp.Instance.GetCustomerTeaCount(CustomerContext.Context.SysNo);
            ViewBag.teamCount = teamCount;
            ViewBag.TeamLevel = CustomerApp.Instance.GetCustomerLevelCount(CustomerContext.Context.SysNo);
            ViewBag.ReferrerEntity = CustomerApp.Instance.Get(CustomerContext.Context.ReferrerSysNo);
            ViewBag.ImageDomain = ConfigurationManager.AppSettings["ImageDomain"];

            return View();
        }

        /// <summary>
        /// 我的团队分页查询
        /// </summary>
        /// <param name="request">输入参数</param>
        /// <returns>Json</returns>
        public JsonResult MyTeamQuery(CustomerExtRequest request)
        {
            request.LevelCustomerSysNo = CustomerContext.Context.SysNo;
            var pagerList = CustomerApp.Instance.GetExtPagerList(request);

            var data = new
            {
                Status = true,
                Data = pagerList.TData.ToList(),
                Count = pagerList.TotalCount
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}
