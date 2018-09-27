using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EC.Entity.Parameter.Request.Bs;
using EC.Entity.Tables.Bs;
using EC.Libraries.Core.Pager;

namespace EC.DataAccess.Bs
{
    public interface IBsNotice
    {
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>系统编号</returns>
        int Insert(BsNotice model);

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>影响行数</returns>
        int Update(BsNotice model);

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="sysNo">编号</param>
        /// <returns>影响行数</returns>
        int UpdateStatus(int status, int sysNo);

        /// <summary>
        /// 编辑公告
        /// </summary>
        /// <param name="model">公告信息</param>
        /// <returns>影响行数</returns>
        int EditNotice(BsNotice model);

        /// <summary>
        /// 获取公告信息
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>公告信息</returns>
        BsNotice Get(int sysNo);

        /// <summary>
        /// 获取公告列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>公告列表</returns>
        IList<BsNotice> GetList(NoticeRequest request);

        /// <summary>
        /// 获取公告分页列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>公告分页列表</returns>
        PagedList<BsNotice> GetPagerList(NoticeRequest request);
    }
}
