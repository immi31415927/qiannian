using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EC.DataAccess.Bs;
using EC.Entity;
using EC.Entity.Parameter.Request.Bs;
using EC.Entity.Tables.Bs;
using EC.Libraries.Core;
using EC.Libraries.Core.Pager;

namespace EC.Application.Tables.Bs
{
    /// <summary>
    /// 公告业务层
    /// </summary>
    public class NoticeApp : Base<NoticeApp>
    {
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>系统编号</returns>
        public JResult<int> Insert(BsNotice model)
        {
            var result = new JResult<int>();

            try
            {
                var row = Using<IBsNotice>().Insert(model);
                if (row <= 0)
                {
                    throw new Exception("添加数据失败");
                }

                result.Status = true;
                result.Data = row;
            }
            catch (Exception ex) { result.Message = ex.Message; }

            return result;
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>影响行数</returns>
        public JResult Update(BsNotice model)
        {
            var result = new JResult();

            try
            {
                var row = Using<IBsNotice>().Update(model);
                if (row <= 0)
                {
                    throw new Exception("修改数据失败");
                }

                result.Status = true;
            }
            catch (Exception ex) { result.Message = ex.Message; }

            return result;
        }

        /// <summary>
        /// 获取公告信息
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>公告信息</returns>
        public BsNotice Get(int sysNo)
        {
            return Using<IBsNotice>().Get(sysNo);
        }

        /// <summary>
        /// 获取公告列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>公告列表</returns>
        public List<BsNotice> GetList(NoticeRequest request)
        {
            var list = Using<IBsNotice>().GetList(request);

            return list.ToList();
        }

        /// <summary>
        /// 获取公告分页列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>公告分页列表</returns>
        public PagedList<BsNotice> GetPagerList(NoticeRequest request)
        {
            return Using<IBsNotice>().GetPagerList(request);
        }

        #region 扩展

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="sysNo">编号</param>
        /// <returns>影响行数</returns>
        public JResult UpdateStatus(int status, int sysNo)
        {
            var result = new JResult();

            try
            {
                var row = Using<IBsNotice>().UpdateStatus(status, sysNo);
                if (row <= 0)
                {
                    throw new Exception("修改数据失败");
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
        /// 编辑公告
        /// </summary>
        /// <param name="model">公告信息</param>
        /// <returns>影响行数</returns>
        public JResult EditNotice(BsNotice model)
        {
            var result = new JResult();

            try
            {
                var row = Using<IBsNotice>().EditNotice(model);
                if (row <= 0)
                {
                    throw new Exception("修改数据失败");
                }

                result.Status = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }

        #endregion
    }
}
