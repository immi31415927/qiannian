using System;
using System.Collections.Generic;
using EC.DataAccess.Fore;
using EC.Entity.Parameter.Request.Video;
using EC.Entity.Tables.Fore;
using EC.Libraries.Core.Data;

namespace EC.DataAccess.MySql.Fore
{
    using EC.Libraries.Core.Pager;

    /// <summary>
    /// 视频数据访问接口
    /// </summary>
    public class VideoCategoryImpl : Base, IVideoCategory
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        public FeVideoCategory Get(int sysNo)
        {
            var strSql = "select * from fevideocategory where sysNo=@sysNo;";

            var result = DBContext.Sql(strSql)
                                .Parameter("sysNo", sysNo)
                                .QuerySingle<FeVideoCategory>();
            return result;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public List<FeVideoCategory> GetList()
        {
            var strSql = "select * from fevideocategory";

            var result = DBContext.Sql(strSql)
                                .QueryMany<FeVideoCategory>();
            return result;
        }

        /// <summary>
        /// 验证标题是否存在
        /// </summary>
        /// <param name="model">账号</param>
        public FeVideoCategory GetByTitle(FeVideoCategory model)
        {
            var sql = "select * from fevideocategory where subject=@subject and sysno!=@sysno";
            return DBContext.Sql(sql)
                    .Parameter("subject", model.Subject)
                    .Parameter("sysno", model.SysNo)
                    .QuerySingle<FeVideoCategory>();
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model">数据模型</param>
        /// <returns>影响行数</returns>
        public int Insert(FeVideoCategory model)
        {
            var result = DBContext.Insert<FeVideoCategory>("fevideocategory", model)
                                .AutoMap(x => x.SysNo)
                                .ExecuteReturnLastId<int>("sysNo");
            return result;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">数据模型</param>
        /// <returns>影响行数</returns>
        public int Update(FeVideoCategory model)
        {
            int rowsAffected = DBContext.Update<FeVideoCategory>("fevideocategory", model)
                                      .AutoMap(x => x.SysNo)
                                      .Where(x => x.SysNo)
                                      .Execute();
            return rowsAffected;
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="sysNo">编码</param>
        /// <param name="status">状态</param>
        /// <returns>影响行数</returns>
        public int UpdateStatus(int sysNo, int status)
        {
            return DBContext.Update("fevideocategory")
                            .Column("status", status)
                            .Where("sysNo", sysNo)
                            .Execute();
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public List<FeVideoCategory> GetForumList(VideoCategoryQueryRequeest request)
        {
            var dataList = DBContext.Select<FeVideoCategory>("*").From("fevideocategory");

            Action<string, string, object> setWhere = (@where, name, value) => dataList.AndWhere(@where).Parameter(name, value);

            if (request.Status.HasValue)
            {
                setWhere("Status = @Status", "Status", request.Status.Value);
            }

            return dataList.QueryMany();
        }

        /// <summary>
        /// 分页列表
        /// </summary>
        public PagedList<FeVideoCategory> GetPaging(VideoCategoryQueryRequeest requeest)
        {
            requeest.Tables = "fevideocategory fvc";
            requeest.Tablefields = "fvc.*";
            requeest.OrderBy = "fvc.DisplayOrder";

            var row = DBContext.Select<FeVideoCategory>(requeest.Tablefields).From(requeest.Tables);
            var count = DBContext.Select<int>("count(0)").From(requeest.Tables);

            //条件查询委托
            Action<string, string, object> setWhere = (@where, name, value) =>
            {
                row.AndWhere(where).Parameter(name, value);
                count.AndWhere(where).Parameter(name, value);
            };

            if (requeest.ParentSysNo.HasValue)
            {
                setWhere("fvc.ParentSysNo = @ParentSysNo", "ParentSysNo", requeest.ParentSysNo);
            }
            if (requeest.Status.HasValue)
            {
                setWhere("fvc.Status = @Status", "Status", requeest.Status);
            }

            var list = new PagedList<FeVideoCategory>
            {
                TData = row.Paging(requeest.CurrentPageIndex.GetHashCode(), requeest.PageSize.GetHashCode()).OrderBy(requeest.OrderBy).QueryMany(),
                CurrentPageIndex = requeest.CurrentPageIndex.GetHashCode(),
                TotalCount = count.QuerySingle(),
            };
            return list;
        }
    }
}
