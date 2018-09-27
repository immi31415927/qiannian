using System;
using System.Collections.Generic;
using System.Linq;
using EC.DataAccess.Fore;
using EC.Entity;
using EC.Entity.Output.Fore;
using EC.Entity.Parameter.Request.Video;
using EC.Entity.Tables.Fore;
using EC.Entity.View.Fore.Ext;

namespace EC.Application.Tables.Fore
{
    using EC.Libraries.Core;
    using EC.Libraries.Core.Pager;

    /// <summary>
    /// 视频业务层
    /// </summary>
    public class VideoApp : Base<VideoApp>
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        public FeVideo Get(int sysNo)
        {
            return Using<IVideo>().Get(sysNo);
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        public VideoOutput GetById(int sysNo)
        {
            var model = new VideoOutput();

            var video = Using<IVideo>().Get(sysNo);
            if (video != null)
            {
                model.SysNo = video.SysNo;
                model.Title = video.Title;
                model.ImageUrl = video.ImageUrl;
                model.Content = video.Content;
                model.Type = video.Type;
                model.Views = video.Views;
                model.Hots = video.Hots;
                model.Shares = video.Shares;
                model.Sign = video.Sign;
                model.Status = video.Status;
                var videoItem = new List<VideoItemOutput>();

                var videoItemList = Using<IVideoItem>().GetByVideoSysNo(video.SysNo);

                videoItemList.ForEach(item =>
                {
                    videoItem.Add(new VideoItemOutput()
                    {
                        SysNo = item.SysNo,
                        Name = item.Name,
                        ImageUrl = item.Name,
                        PlayUrl = item.PlayUrl,
                        DisplayOrder = item.DisplayOrder,
                        Status = item.Status
                    });
                });

                if (videoItem != null && videoItem.Any())
                {
                    model.VideoItem = videoItem.OrderBy(p => p.DisplayOrder).ToList();
                }
            }

            return model;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public List<FeVideo> GetList()
        {
            return Using<IVideo>().GetList();
        }

        /// <summary>
        /// 验证编码是否存在
        /// </summary>
        /// <param name="model">专题模型</param>
        public JResult GetByTitle(FeVideo model)
        {
            var result = new JResult()
            {
                Status = false
            };

            try
            {
                if (Using<IVideo>().GetByTitle(model) != null)
                {
                    result.Message = "名称已存在！";
                    result.Status = true;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model">数据模型</param>
        /// <returns>影响行数</returns>
        public JResult<int> Insert(FeVideo model)
        {
            var result = new JResult<int>()
            {
                Status = false
            };

            try
            {
                var sysNo = Using<IVideo>().Insert(model);
                if ( sysNo <= 0)
                {
                    throw new Exception("添加数据失败");
                }

                //LogApp.Instance.Info(new LogRequest() { Source = LogSource.后台, Message = "新增视频" });
                result.Data = sysNo;
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
        public JResult Update(FeVideo model)
        {
            var result = new JResult()
            {
                Status = false
            };

            try
            {
                if (Using<IVideo>().Update(model) <= 0)
                {
                    throw new Exception("修改数据失败");
                }

                //LogApp.Instance.Info(new LogRequest() { Source = LogSource.后台, Message = "修改视频" });

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
                if (Using<IVideo>().UpdateStatus(sysNo, status) <= 0)
                {
                    throw new Exception("更新数据失败");
                }

                //LogApp.Instance.Info(new LogRequest() { Source = LogSource.后台, Message = "修改视频状态" });

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
        /// 更新点赞次数
        /// </summary>
        /// <param name="sysNo">编码</param>
        /// <returns>返回结果</returns>
        public JResult UpdateHots(int sysNo)
        {
            var result = new JResult();

            try
            {
                if (Using<IVideo>().UpdateHots(sysNo) <= 0)
                {
                    throw new Exception("更新数据失败");
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
        /// 分页列表
        /// </summary>
        public PagedList<FeVideo> GetPaging(VideoQueryRequeest requeest)
        {
            return Using<IVideo>().GetPaging(requeest);
        }

        /// <summary>
        /// 获取视频扩展信息分页列表
        /// </summary>
        /// <param name="requset">查询参数</param>
        /// <returns>视频扩展信息分页列表</returns>
        public PagedList<VideoExt> GetExtPaging(VideoQueryRequeest requset)
        {
            return Using<IVideo>().GetExtPaging(requset);
        }
    }
}
