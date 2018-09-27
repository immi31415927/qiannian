using System;
using System.Collections.Generic;
using EC.DataAccess.Fore;
using EC.Entity.Parameter.Request.Video;
using EC.Entity.Tables.Fore;
using EC.Entity.View.Fore.Ext;
using EC.Libraries.Core.Data;

namespace EC.DataAccess.MySql.Fore
{
    using EC.Libraries.Core.Pager;

    /// <summary>
    /// 视频数据访问接口
    /// </summary>
    public class VideoImpl : Base, IVideo
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        public FeVideo Get(int sysNo)
        {
            var strSql = "select * from fevideo where sysNo=@sysNo;";

            var result = DBContext.Sql(strSql)
                                .Parameter("sysNo", sysNo)
                                .QuerySingle<FeVideo>();
            return result;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public List<FeVideo> GetList()
        {
            var sql = "select * from fevideo";
            return DBContext.Sql(sql)
                    .QueryMany<FeVideo>();
        }

        /// <summary>
        /// 验证标题是否存在
        /// </summary>
        /// <param name="model">账号</param>
        public FeVideo GetByTitle(FeVideo model)
        {
            var sql = "select * from fevideo where title=@title and sysno!=@sysno";
            return DBContext.Sql(sql)
                    .Parameter("title", model.Title)
                    .Parameter("sysno", model.SysNo)
                    .QuerySingle<FeVideo>();
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model">数据模型</param>
        /// <returns>影响行数</returns>
        public int Insert(FeVideo model)
        {
            var result = DBContext.Insert<FeVideo>("fevideo", model)
                                .AutoMap(x => x.SysNo)
                                .ExecuteReturnLastId<int>("sysNo");
            return result;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">数据模型</param>
        /// <returns>影响行数</returns>
        public int Update(FeVideo model)
        {
            int rowsAffected = DBContext.Update<FeVideo>("fevideo", model)
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
            return DBContext.Update("fevideo")
                            .Column("status", status)
                            .Where("sysNo", sysNo)
                            .Execute();
        }

        /// <summary>
        /// 更新点赞次数
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>影响行数</returns>
        public int UpdateHots(int sysNo)
        {
            return DBContext.Sql("update fevideo set Hots=Hots+1 where SysNo = @SysNo")
                            .Parameter("SysNo", sysNo)
                            .Execute();
        }

        /// <summary>
        /// 分页列表
        /// </summary>
        public PagedList<FeVideo> GetPaging(VideoQueryRequeest requeest)
        {
            requeest.Tables = "fevideo fv";
            requeest.Tablefields = "fv.*";
            requeest.OrderBy = "fv.CreatedDate desc";

            var row = DBContext.Select<FeVideo>(requeest.Tablefields).From(requeest.Tables);
            var count = DBContext.Select<int>("count(0)").From(requeest.Tables);

            //条件查询委托
            Action<string, string, object> setWhere = (@where, name, value) =>
            {
                row.AndWhere(where).Parameter(name, value);
                count.AndWhere(where).Parameter(name, value);
            };

            if (requeest.Status.HasValue)
            {
                setWhere("fv.Status=@Status", "Status", requeest.Status);
            }
            if (requeest.Type.HasValue)
            {
                setWhere("fv.Type=@Type", "Type", requeest.Type);
            }
            if (requeest.Sign.HasValue)
            {
                setWhere("fv.Sign=@Sign", "Sign", requeest.Sign);
            }
            if (!string.IsNullOrWhiteSpace(requeest.Title))
            {
                setWhere("fv.Title=@Title", "Title", requeest.Title);
            }

            var list = new PagedList<FeVideo>
            {
                TData = row.Paging(requeest.CurrentPageIndex.GetHashCode(), requeest.PageSize.GetHashCode()).OrderBy(requeest.OrderBy).QueryMany(),
                CurrentPageIndex = requeest.CurrentPageIndex.GetHashCode(),
                TotalCount = count.QuerySingle(),
            };

            return list;
        }

        /// <summary>
        /// 获取视频扩展信息分页列表
        /// </summary>
        /// <param name="requset">查询参数</param>
        /// <returns>视频扩展信息分页列表</returns>
        public PagedList<VideoExt> GetExtPaging(VideoQueryRequeest requset)
        {
            var dataList = DBContext.Select<VideoExt>("v.*,(SELECT SUM(i.PlayNumber) FROM FeVideoItem i WHERE i.VideoSysNo = v.SysNo) as PlayNumber").From("FeVideo v");
            var dataCount = DBContext.Select<int>("count(0)").From("FeVideo v");

            //条件查询委托
            Action<string, string, object> setWhere = (@where, name, value) =>
            {
                dataList.AndWhere(where).Parameter(name, value);
                dataCount.AndWhere(where).Parameter(name, value);
            };

            if (requset.Status.HasValue)
            {
                setWhere("v.Status = @Status", "Status", requset.Status);
            }
            if (requset.Type.HasValue)
            {
                setWhere("v.Type = @Type", "Type", requset.Type);
            }
            if (requset.Sign.HasValue)
            {
                setWhere("v.Sign = @Sign", "Sign", requset.Sign);
            }
            if (requset.VideoCategorySysNo.HasValue)
            {
                setWhere("v.VideoCategorySysNo = @VideoCategorySysNo", "VideoCategorySysNo", requset.VideoCategorySysNo);
            }
            if (!string.IsNullOrEmpty(requset.VideoName))
            {
                setWhere("v.Title like CONCAT('%',@VideoName,'%')", "VideoName", requset.VideoName);
            }

            var list = new PagedList<VideoExt>
            {
                TData = dataList.Paging(requset.CurrentPageIndex.GetHashCode(), requset.PageSize.GetHashCode()).OrderBy("v.DisplayOrder").QueryMany(),
                CurrentPageIndex = requset.CurrentPageIndex.GetHashCode(),
                TotalCount = dataCount.QuerySingle(),
            };

            return list;
        }
    }
}
