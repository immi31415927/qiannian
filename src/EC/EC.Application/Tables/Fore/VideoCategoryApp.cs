using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EC.DataAccess.Fore;
using EC.Entity;
using EC.Entity.Parameter.Request.Video;
using EC.Entity.Tables.Fore;
using EC.Libraries.Core;
using EC.Libraries.Core.Pager;
using EC.Libraries.Core.Transaction;

namespace EC.Application.Tables.Fore
{
    /// <summary>
    /// 视频业务层
    /// </summary>
    public class VideoCategoryApp : Base<VideoCategoryApp>
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        public FeVideoCategory Get(int sysNo)
        {
            return Using<IVideoCategory>().Get(sysNo);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public List<FeVideoCategory> GetList()
        {
            return Using<IVideoCategory>().GetList();
        }

        /// <summary>
        /// 验证编码是否存在
        /// </summary>
        /// <param name="model">专题模型</param>
        public JResult GetByTitle(FeVideoCategory model)
        {
            var result = new JResult()
            {
                Status = false
            };

            try
            {
                if (Using<IVideoCategory>().GetByTitle(model) != null)
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
        public JResult Insert(FeVideoCategory model)
        {
            var result = new JResult()
            {
                Status = false
            };

            try
            {
                if (Using<IVideoCategory>().Insert(model) <= 0)
                {
                    throw new Exception("添加数据失败");
                }

                //LogApp.Instance.Info(new LogRequest() { Source = LogSource.后台, Message = "新增视频" });

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
        public JResult Update(FeVideoCategory model)
        {
            var result = new JResult()
            {
                Status = false
            };

            try
            {
                if (Using<IVideoCategory>().Update(model) <= 0)
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
        /// 插入
        /// </summary>
        /// <param name="request">菜单</param>
        public JResult Save(SaveVideoCategoryRequest request)
        {
            var response = new JResult()
            {
                Status = false,
                Message = "添加失败！"
            };

            using (var tran = new TransactionProvider())
            {
                try
                {
                    var forum = new FeVideoCategory()
                    {
                        SysNo = request.SysNo,
                        Subject = request.Subject,
                        ImageUrl = request.ImageUrl,
                        ParentSysNo = request.ParentSysNo,
                        DisplayOrder = request.DisplayOrder,
                        Status = request.Status
                    };

                    if (request.SysNo > 0)
                    {
                        var row = Using<IVideoCategory>().Update(forum);
                        if (row <= 0)
                        {
                            throw new Exception("添加失败！");
                        }
                        tran.Commit();
                        response.Status = true;
                        response.Message = "更新成功！";
                    }
                    else
                    {
                        var menuSysNo = Using<IVideoCategory>().Insert(forum);
                        if (menuSysNo <= 0)
                        {
                            throw new Exception("添加版块失败！");
                        }
                        tran.Commit();
                        response.Status = true;
                        response.Message = "添加成功！";
                    }
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    response.Message = ex.Message;
                }
            }
            return response;
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
                if (Using<IVideoCategory>().UpdateStatus(sysNo, status) <= 0)
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
        /// 获取列表
        /// </summary>
        public List<FeVideoCategory> GetForumList(VideoCategoryQueryRequeest request)
        {
            return Using<IVideoCategory>().GetForumList(request);
        }

        /// <summary>
        /// 分页列表
        /// </summary>
        public PagedList<FeVideoCategory> GetPaging(VideoCategoryQueryRequeest requeest)
        {
            return Using<IVideoCategory>().GetPaging(requeest);
        }
    }
}
