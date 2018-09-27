using System;
using System.Web;
using System.Web.Security;
using EC.DataAccess.CRM;
using EC.Entity.Tables.CRM;
using EC.Entity.Parameter.Request.Auth;

namespace EC.Application.Tables.CRM
{
    using EC.Libraries.Core;
    using EC.Libraries.Util;

    /// <summary>
    /// 身份验证 业务层
    /// </summary>
    public class AuthApp : Base<AuthApp>
    {

        /// <summary>
        /// 写用户认证
        /// </summary>
        /// <param name="ticketRequest">用户票据</param>
        /// <param name="createPersistentCookie">是否创建持久性Cookie</param>
        public void Ticket(TicketRequest ticketRequest, bool createPersistentCookie)
        {
            var ticket = new FormsAuthenticationTicket(
                1,
                ticketRequest.SysNo.ToString(),
                DateTime.Now,
                DateTime.Now.Add(FormsAuthentication.Timeout),
                createPersistentCookie,
                ticketRequest.ToJson(),
                FormsAuthentication.FormsCookiePath);

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket) { HttpOnly = true };
            if (ticket.IsPersistent)
            {
                cookie.Expires = ticket.Expiration;
            }
            cookie.Secure = FormsAuthentication.RequireSSL;
            cookie.Path = FormsAuthentication.FormsCookiePath;
            if (FormsAuthentication.CookieDomain != null)
            {
                cookie.Domain = FormsAuthentication.CookieDomain;
            }

            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 用户登出
        /// </summary>
        public void SignOut()
        {
            FormsAuthentication.SignOut();
            HttpContext.Current.Session.Abandon();
        }

        /// <summary>
        /// 获取当前登录用户票据信息
        /// </summary>
        /// <returns></returns>
        public TicketRequest GetAuthenticatedCustomer()
        {
#if TEST
            return new TicketRequest
            {
                SysNo =1,
                Account = "test@gmail.com"，
                RealName = "测试"
            };
#endif

            if (HttpContext.Current == null) return null;
            if (!HttpContext.Current.Request.IsAuthenticated || !(HttpContext.Current.User.Identity is FormsIdentity))
            {
                var cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                if (cookie == null) return null;
                else
                {
                    var ticket = FormsAuthentication.Decrypt(cookie.Value);
                    return ticket.UserData.ToObject<TicketRequest>();
                }
            }

            var formsIdentity = (FormsIdentity)HttpContext.Current.User.Identity;

            return formsIdentity.Ticket.UserData.ToObject<TicketRequest>();
        }

        /// <summary>
        /// 客户是否登录
        /// </summary>
        public bool IsLogin
        {
            get
            {
                return CustomerSysNo > 0;
            }
        }

        /// <summary>
        /// 客户编号
        /// </summary>
        private int CustomerSysNo
        {

            get
            {
                var cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];

                if (cookie != null)
                {
                    var ticketRequest = JsonUtil.ToObject<TicketRequest>(FormsAuthentication.Decrypt(cookie.Value).UserData);
                    if (ticketRequest != null)
                        return ticketRequest.SysNo;
                    else
                        return 0;
                }
                else
                    return 0;
            }
        }

        /// <summary>
        /// 当前客户
        /// </summary>
        public CrCustomer CurrentCustomer
        {
            get
            {
                return Using<ICrCustomer>().Get(CustomerSysNo);
            }
        }
    }
}
