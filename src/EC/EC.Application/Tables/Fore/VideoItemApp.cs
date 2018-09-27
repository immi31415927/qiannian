using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EC.DataAccess.Fore;
using EC.Entity;
using EC.Entity.Enum;
using EC.Entity.Parameter.Request.Video;
using EC.Entity.Tables.Fore;
using EC.Entity.View.Fore.Ext;
using EC.Libraries.Core.Transaction;

namespace EC.Application.Tables.Fore
{
    using EC.Libraries.Core;
    using EC.Libraries.Core.Pager;

    /// <summary>
    /// 视频业务层
    /// </summary>
    public class VideoItemApp : Base<VideoItemApp>
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        public FeVideoItem Get(int sysNo)
        {
            return Using<IVideoItem>().Get(sysNo);
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="videoSysNo">系统编号</param>
        public List<FeVideoItem> GetByVideoSysNo(int videoSysNo)
        {
            return Using<IVideoItem>().GetByVideoSysNo(videoSysNo);
        }

        /// <summary>
        /// 验证编码是否存在
        /// </summary>
        /// <param name="model">专题模型</param>
        public JResult GetByTitle(FeVideoItem model)
        {
            var result = new JResult()
            {
                Status = false
            };

            try
            {
                if (Using<IVideoItem>().GetByTitle(model) != null)
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
        /// <param name="videoItem">数据模型</param>
        /// <returns>影响行数</returns>
        public JResult Save(List<VideoItemRequest> videoItem)
        {
            var response = new JResult()
            {
                Status = false
            };
            using (var tran = new TransactionProvider())
            {
                try
                {
                    foreach (var item in videoItem)
                    {
                        var obj = new FeVideoItem();
                        if (item.SysNo > 0)
                        {
                            obj = Using<IVideoItem>().Get(item.SysNo);
                            obj.Name = item.Name;
                            obj.ImageUrl = item.ImageUrl;
                            obj.PlayUrl = item.PlayUrl;
                            obj.DisplayOrder = item.DisplayOrder;
                        }
                        else
                        {
                            obj.VideoSysNo = item.VideoSysNo;
                            obj.Name = item.Name;
                            obj.ImageUrl = item.ImageUrl;
                            obj.PlayUrl = item.PlayUrl;
                            obj.PlayNumber = 0;
                            obj.DisplayOrder = item.DisplayOrder;
                            obj.Status = StatusEnum.启用.GetHashCode();
                            obj.CreatedDate = DateTime.Now;
                        }

                        if (item.SysNo > 0)
                        {
                            if (Using<IVideoItem>().Update(obj) <= 0)
                            {
                                throw new Exception("修改失败："+item.Name);
                            }
                        }
                        else
                        {
                            if (Using<IVideoItem>().Insert(obj)<=0)
                            {
                                throw new Exception("添加失败："+item.Name);
                            }
                        }
                    }
                    //LogApp.Instance.Info(new LogRequest() { Source = LogSource.后台, Message = "新增视频" });
                    tran.Commit();
                    response.Status = true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    response.Message = ex.Message;
                }
                finally
                {
                    tran.Dispose();
                }
            }
            return response;
        }

        /// <summary>
        /// 删除视频地址
        /// </summary>
        /// <param name="videoSysNo">视频编号</param>
        /// <returns>影响行数</returns>
        public JResult Delete(int videoSysNo)
        {
            var result = new JResult()
            {
                Status = false
            };

            try
            {
                if (Using<IVideoItem>().Delete(videoSysNo) <= 0)
                {
                    throw new Exception("删除数据失败");
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
        /// 修改
        /// </summary>
        /// <param name="model">数据模型</param>
        /// <returns>影响行数</returns>
        public JResult Update(FeVideoItem model)
        {
            var result = new JResult()
            {
                Status = false
            };

            try
            {
                if (Using<IVideoItem>().Update(model) <= 0)
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
                if (Using<IVideoItem>().UpdateStatus(sysNo, status) <= 0)
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
        /// 更新播放次数
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>影响行数</returns>
        public JResult UpdatePlayNumber(int sysNo)
        {
            var result = new JResult();

            try
            {
                if (Using<IVideoItem>().UpdatePlayNumber(sysNo) <= 0)
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
        /// 视频项分页列表
        /// </summary>
        /// <param name="requset">查询参数</param>
        public PagedList<VideoItemExt> GetExtPaging(VideoItemQueryRequest requset)
        {
            return Using<IVideoItem>().GetExtPaging(requset);
        }
    }
}
