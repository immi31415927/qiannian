using System;
using System.Collections.Generic;
using System.Linq;
using EC.DataAccess.Fore;
using EC.Entity;
using EC.Entity.Parameter.Request.Video;
using EC.Entity.Tables.Fore;
using EC.Libraries.Core.Pager;

namespace EC.Application.Tables.Fore
{
    using EC.Libraries.Core;

    /// <summary>
    /// 视频收藏业务层
    /// </summary>
    public class VideoRecordApp : Base<VideoRecordApp>
    {
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model">数据模型</param>
        /// <returns>影响行数</returns>
        public JResult Insert(FeVideoRecord model)
        {
            var result = new JResult();

            try
            {
                if (Using<IVideoRecord>().Insert(model) <= 0)
                {
                    throw new Exception("添加数据失败");
                }

                result.Status = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">数据模型</param>
        /// <returns>影响行数</returns>
        public JResult Update(FeVideoRecord model)
        {
            var result = new JResult();

            try
            {
                if (Using<IVideoRecord>().Update(model) <= 0)
                {
                    throw new Exception("修改数据失败");
                }

                result.Message = "操作成功！";
                result.Status = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="sysNo">编码</param>
        /// <param name="status">状态</param>
        /// <returns>影响行数</returns>
        public JResult UpdateStatus(int sysNo, int status)
        {
            var result = new JResult();

            try
            {
                if (Using<IVideoRecord>().UpdateStatus(sysNo, status) <= 0)
                {
                    throw new Exception("更新数据失败");
                }

                result.Message = "操作成功！";
                result.Status = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 删除会员收藏信息
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <param name="customerSysNo">会员编号</param>
        /// <returns>影响行数</returns>
        public JResult Delete(int sysNo, int customerSysNo)
        {
            var result = new JResult();

            try
            {
                if (Using<IVideoRecord>().Delete(sysNo, customerSysNo) <= 0)
                {
                    throw new Exception("操作数据失败");
                }

                result.Status = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        public FeVideoRecord Get(int sysNo)
        {
            return Using<IVideoRecord>().Get(sysNo);
        }

        /// <summary>
        /// 获取视频记录列表
        /// </summary>
        /// <param name="requset">查询参数</param>
        /// <returns>视频记录列表</returns>
        public List<FeVideoRecord> GetList(VideoFavQueryRequest requset)
        {
            return Using<IVideoRecord>().GetList(requset).ToList();
        }

        /// <summary>
        /// 获取视频收藏信息分页列表
        /// </summary>
        /// <param name="requset">查询参数</param>
        /// <returns>视频收藏信息分页列表</returns>
        public PagedList<FeVideoRecord> GetPaging(VideoFavQueryRequest requset)
        {
            return Using<IVideoRecord>().GetPaging(requset);
        }
    }
}
