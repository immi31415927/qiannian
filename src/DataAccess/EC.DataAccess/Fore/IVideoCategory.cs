using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EC.Entity.Parameter.Request.Video;
using EC.Entity.Tables.Fore;

namespace EC.DataAccess.Fore
{
    using EC.Libraries.Core.Pager;

    /// <summary>
    /// 视频分类数据访问接口
    /// </summary>
    public interface IVideoCategory
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        FeVideoCategory Get(int sysNo);

        /// <summary>
        /// 获取列表
        /// </summary>
        List<FeVideoCategory> GetList();

        /// <summary>
        /// 验证标题是否存在
        /// </summary>
        /// <param name="model">专题模型</param>
        FeVideoCategory GetByTitle(FeVideoCategory model);

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model">数据模型</param>
        /// <returns>影响行数</returns>
        int Insert(FeVideoCategory model);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">数据模型</param>
        /// <returns>影响行数</returns>
        int Update(FeVideoCategory model);

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="sysNo">编码</param>
        /// <param name="status">状态</param>
        /// <returns>影响行数</returns>
        int UpdateStatus(int sysNo, int status);

        /// <summary>
        /// 获取列表
        /// </summary>
        List<FeVideoCategory> GetForumList(VideoCategoryQueryRequeest request);

        /// <summary>
        /// 获取角色列表
        /// </summary>
        PagedList<FeVideoCategory> GetPaging(VideoCategoryQueryRequeest requeest);
    }
}
