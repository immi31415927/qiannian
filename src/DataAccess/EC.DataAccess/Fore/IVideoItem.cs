using System.Collections.Generic;
using EC.Entity.Parameter.Request.Video;
using EC.Entity.Tables.Fore;
using EC.Entity.View.Fore.Ext;
using EC.Libraries.Core.Pager;

namespace EC.DataAccess.Fore
{
    /// <summary>
    /// 视频项数据访问接口
    /// </summary>
    public interface IVideoItem
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        FeVideoItem Get(int sysNo);

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="videoSysNo">系统编号</param>
        List<FeVideoItem> GetByVideoSysNo(int videoSysNo);

        /// <summary>
        /// 验证标题是否存在
        /// </summary>
        /// <param name="model">专题模型</param>
        FeVideoItem GetByTitle(FeVideoItem model);

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model">数据模型</param>
        /// <returns>影响行数</returns>
        int Insert(FeVideoItem model);

        /// <summary>
        /// 视频编号
        /// </summary>
        /// <param name="videoSysNo">视频编号</param>
        /// <returns>影响行数</returns>
        int Delete(int videoSysNo);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">数据模型</param>
        /// <returns>影响行数</returns>
        int Update(FeVideoItem model);

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="sysNo">编码</param>
        /// <param name="status">状态</param>
        /// <returns>影响行数</returns>
        int UpdateStatus(int sysNo, int status);

        /// <summary>
        /// 更新播放次数
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>影响行数</returns>
        int UpdatePlayNumber(int sysNo);

        /// <summary>
        /// 视频项分页列表
        /// </summary>
        /// <param name="requset">查询参数</param>
        PagedList<VideoItemExt> GetExtPaging(VideoItemQueryRequest requset);
    }
}
