using System.Collections.Generic;
using EC.Entity.Parameter.Request.Auth;
using EC.Entity.Tables.Bs;

namespace EC.DataAccess.Bs
{
    using EC.Libraries.Core.Pager;

    /// <summary>
    /// 角色数据访问接口
    /// </summary>
    public interface IBsRole
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        BsRole Get(int sysNo);

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model">数据模型</param>
        int Insert(BsRole model);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">数据模型</param>
        int Update(BsRole model);

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="sysNo">编码</param>
        /// <param name="status">状态</param>
        /// <returns>影响行数</returns>
        int UpdateStatus(int sysNo, int status);

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>角色列表</returns>
        IList<BsRole> GetList(RoleQueryRequeest request);

        /// <summary>
        /// 获取角色列表
        /// </summary>
        PagedList<BsRole> GetPagingList(RoleQueryRequeest requeest);

        /// <summary>
        /// 获取角色列表
        /// </summary>
        PagedList<BsRole> GetPaging(RoleQueryRequeest requeest);
    }
}
