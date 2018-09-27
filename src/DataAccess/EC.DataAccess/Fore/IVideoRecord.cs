using System.Collections.Generic;
using EC.Entity.Parameter.Request.Video;
using EC.Entity.Tables.Fore;
using EC.Libraries.Core.Pager;

namespace EC.DataAccess.Fore
{
    /// <summary>
    /// 视频收藏数据访问接口
    /// </summary>
    public interface IVideoRecord
    {
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model">数据模型</param>
        /// <returns>影响行数</returns>
        int Insert(FeVideoRecord model);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">数据模型</param>
        /// <returns>影响行数</returns>
        int Update(FeVideoRecord model);

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="sysNo">编码</param>
        /// <param name="status">状态</param>
        /// <returns>影响行数</returns>
        int UpdateStatus(int sysNo, int status);

        /// <summary>
        /// 删除会员收藏信息
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <param name="customerSysNo">会员编号</param>
        /// <returns>影响行数</returns>
        int Delete(int sysNo, int customerSysNo);

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        FeVideoRecord Get(int sysNo);

        /// <summary>
        /// 获取视频记录列表
        /// </summary>
        /// <param name="requset">查询参数</param>
        /// <returns>视频记录列表</returns>
        IList<FeVideoRecord> GetList(VideoFavQueryRequest requset);

        /// <summary>
        /// 获取视频收藏信息分页列表
        /// </summary>
        /// <param name="requset">查询参数</param>
        /// <returns>视频收藏信息分页列表</returns>
        PagedList<FeVideoRecord> GetPaging(VideoFavQueryRequest requset);
    }
}
