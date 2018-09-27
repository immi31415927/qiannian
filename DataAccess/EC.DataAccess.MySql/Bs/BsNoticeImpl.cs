using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EC.DataAccess.Bs;
using EC.Entity.Parameter.Request.Bs;
using EC.Entity.Tables.Bs;
using EC.Libraries.Core.Data;
using EC.Libraries.Core.Pager;

namespace EC.DataAccess.MySql.Bs
{
    public class BsNoticeImpl : Base, IBsNotice
    {
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>系统编号</returns>
        public int Insert(BsNotice model)
        {
            return DBContext.Insert("Agent_BsNotice", model)
                            .AutoMap(x => x.SysNo)
                            .ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>影响行数</returns>
        public int Update(BsNotice model)
        {
            return DBContext.Update("Agent_BsNotice", model)
                            .AutoMap(x => x.SysNo)
                            .Where(x => x.SysNo).Execute();
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="sysNo">编号</param>
        /// <returns>影响行数</returns>
        public int UpdateStatus(int status, int sysNo)
        {
            return DBContext.Update("Agent_BsNotice")
                            .Column("Status", status)
                            .Where("SysNo", sysNo)
                            .Execute();
        }

        /// <summary>
        /// 编辑公告
        /// </summary>
        /// <param name="model">公告信息</param>
        /// <returns>影响行数</returns>
        public int EditNotice(BsNotice model)
        {
            return DBContext.Update("Agent_BsNotice")
                            .Column("Type", model.Type)
                            .Column("Title", model.Title)
                            .Column("Synopsis", model.Synopsis)
                            .Column("ImageUrl", model.ImageUrl)
                            .Column("Content", model.Content)
                            .Column("DisplayOrder", model.DisplayOrder)
                            .Column("Operator", model.Operator)
                            .Column("CreatedDate", model.CreatedDate)
                            .Where("SysNo", model.SysNo)
                            .Execute();
        }

        /// <summary>
        /// 获取公告信息
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>公告信息</returns>
        public BsNotice Get(int sysNo)
        {
            return DBContext.Select<BsNotice>("*")
                            .From("Agent_BsNotice")
                            .Where("sysNo = @sysNo")
                            .Parameter("sysNo", sysNo)
                            .QuerySingle();
        }

        /// <summary>
        /// 获取公告列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>公告列表</returns>
        public IList<BsNotice> GetList(NoticeRequest request)
        {
            var dataList = DBContext.Select<BsNotice>("*").From("Agent_BsNotice");

            Action<string, string, object> setWhere = (@where, name, value) => dataList.AndWhere(@where).Parameter(name, value);

            return dataList.QueryMany();
        }

        /// <summary>
        /// 获取公告分页列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>公告分页列表</returns>
        public PagedList<BsNotice> GetPagerList(NoticeRequest request)
        {
            var dataCount = DBContext.Select<int>("count(0)").From("Agent_BsNotice");
            var dataList = DBContext.Select<BsNotice>("*").From("Agent_BsNotice");

            Action<string, string, object> setWhere = (@where, name, value) =>
            {
                dataCount.AndWhere(where).Parameter(name, value);
                dataList.AndWhere(where).Parameter(name, value);
            };

            return new PagedList<BsNotice>
            {
                TData = dataList.Paging(request.CurrentPageIndex.GetHashCode(), request.PageSize.GetHashCode()).OrderBy("SysNo desc").QueryMany(),
                CurrentPageIndex = request.CurrentPageIndex.GetHashCode(),
                TotalCount = dataCount.QuerySingle()
            };
        }
    }
}
