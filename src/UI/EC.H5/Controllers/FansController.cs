using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EC.Application.Tables.CRM;
using EC.Application.Tables.WeiXin;
using EC.Entity;
using EC.Entity.Parameter.Request.CRM;
using EC.Entity.Parameter.Request.WeiXin;
using EC.Entity.View.CRM.Ext;

namespace EC.H5.Controllers
{
    public class FansController : BaseController
    {
        //
        // GET: /Fans/

        /// <summary>
        /// 我的粉丝
        /// </summary>
        /// <returns>视图</returns>
        public ActionResult MyFans()
        {
            return View();
        }

        /// <summary>
        /// 我的粉丝分页数据
        /// </summary>
        /// <param name="request">输入参数</param>
        /// <returns>Json</returns>
        public JsonResult MyFansQuery(RecommendExtRequest request)
        {
            request.ReferrerSysNo = CustomerContext.Context.SysNo;
            var pagerList = RecommendApp.Instance.GetExtPagerList(request);

            var data = new
            {
                Status = true,
                Data = pagerList.TData.ToList(),
                Count = pagerList.TotalCount
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 发送信息
        /// </summary>
        /// <returns>视图</returns>
        public ActionResult SendInfo(int sysNo)
        {
            var entity = RecommendApp.Instance.Get(sysNo);

            if (entity == null)
            {
                return View("Error");
            }

            //ViewBag.NickName = entity.Nickname;
            ViewBag.OpenId = entity.Openid;

            return View();
        }

        /// <summary>
        /// 发送信息响应
        /// </summary>
        /// <param name="openId">OpenId</param>
        /// <param name="message">消息信息</param>
        /// <returns>Json</returns>
        public JsonResult SendInfoResult(string openId, string message)
        {
            var result = new JResult();
            var appKey = ConfigurationManager.AppSettings["appKey"];
            var appSecret = ConfigurationManager.AppSettings["appSecret"];
            var sendInfoTemplate = ConfigurationManager.AppSettings["sendInfoTemplate"];

            var recommend = RecommendApp.Instance.GetByopenId(openId);

            if (recommend == null)
            {
                result = new JResult()
                {
                    Message = "发送用户未绑定粉丝"
                };
            }
            else
            {
                result = WeiXinApp.Instance.SendMessage(new SendMessageRequest()
                {
                    WeiXinAppId = appKey,
                    WeiXinAppSecret = appSecret,
                    OpenId = openId,
                    TemplateId = sendInfoTemplate,
                    Data = new
                    {
                        first = new { color = "#000000", value = "1000n消息信息" },
                        //keyword1 = new { color = "#000000", value = recommend.Nickname },
                        keyword2 = new { color = "#000000", value = DateTime.Now.ToString("yyyy年MM月dd日") },
                        remark = new { color = "#0066cc", value = message }
                    },
                    Url = "#"
                });
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
