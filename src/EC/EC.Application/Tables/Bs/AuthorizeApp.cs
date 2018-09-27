using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EC.DataAccess.Bs;
using EC.Entity.Parameter.Request.Bs;
using EC.Entity.Tables.Bs;
using EC.Libraries.Core;

namespace EC.Application.Tables.Bs
{
    /// <summary>
    /// 功能权限业务层
    /// </summary>
    public class AuthorizeApp : Base<AuthorizeApp>
    {
        /// <summary>
        /// 获取授权列表
        /// </summary>
        /// <param name="request">输入参数</param>
        /// <returns>授权列表</returns>
        public List<BsAuthorize> GetList(AuthorizeRequest request)
        {
            var list = Using<IBsAuthorize>().GetList(request);

            return list.ToList();
        }
    }
}
