using System.Collections.Generic;
using EC.Entity.Parameter.Request.Video;
using EC.Entity.Tables.Fore;
using EC.Entity.View.Fore.Ext;

namespace EC.DataAccess.Fore
{
    using EC.Libraries.Core.Pager;

    /// <summary>
    /// 视频数据访问接口
    /// </summary>
    public interface IVideo
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        FeVideo Get(int sysNo);

        /// <summary>
        /// 获取列表
        /// </summary>
        List<FeVideo> GetList();

        /// <summary>
        /// 验证标题是否存在
        /// </summary>
        /// <param name="model">专题模型</param>
        FeVideo GetByTitle(FeVideo model);

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model">数据模型</param>
        /// <returns>影响行数</returns>
        int Insert(FeVideo model);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">数据模型</param>
        /// <returns>影响行数</returns>
        int Update(FeVideo model);

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="sysNo">编码</param>
        /// <param name="status">状态</param>
        /// <returns>影响行数</returns>
        int UpdateStatus(int sysNo, int status);

        /// <summary>
        /// 更新点赞次数
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>影响行数</returns>
        int UpdateHots(int sysNo);

        /// <summary>
        /// 获取角色列表
        /// </summary>
        PagedList<FeVideo> GetPaging(VideoQueryRequeest requeest);

        /// <summary>
        /// 获取视频扩展信息分页列表
        /// </summary>
        /// <param name="requset">查询参数</param>
        /// <returns>视频扩展信息分页列表</returns>
        PagedList<VideoExt> GetExtPaging(VideoQueryRequeest requset);
    }
}
