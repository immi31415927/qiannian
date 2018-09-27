using System;
using System.Collections.Generic;
using EC.DataAccess.Fore;
using EC.Entity.Parameter.Request.Video;
using EC.Entity.Tables.Fore;
using EC.Entity.View.Fore.Ext;
using EC.Libraries.Core.Data;
using EC.Libraries.Core.Pager;

namespace EC.DataAccess.MySql.Fore
{
    /// <summary>
    /// 视频项数据访问接口
    /// </summary>
    public class VideoItemImpl : Base, IVideoItem
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        public FeVideoItem Get(int sysNo)
        {
            var strSql = "select * from fevideoitem where sysNo=@sysNo;";

            var result = DBContext.Sql(strSql)
                                .Parameter("sysNo", sysNo)
                                .QuerySingle<FeVideoItem>();
            return result;
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="videoSysNo">系统编号</param>
        public List<FeVideoItem> GetByVideoSysNo(int videoSysNo)
        {
            var strSql = "select * from fevideoitem where videoSysNo=@videoSysNo order by DisplayOrder desc;";

            var result = DBContext.Sql(strSql)
                                .Parameter("videoSysNo", videoSysNo)
                                .QueryMany<FeVideoItem>();
            return result;
        }

        /// <summary>
        /// 验证标题是否存在
        /// </summary>
        /// <param name="model">账号</param>
        public FeVideoItem GetByTitle(FeVideoItem model)
        {
            var sql = "select * from fevideoitem where name=@name and sysno!=@sysno";
            return DBContext.Sql(sql)
                    .Parameter("name", model.Name)
                    .Parameter("sysno", model.SysNo)
                    .QuerySingle<FeVideoItem>();
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model">数据模型</param>
        /// <returns>影响行数</returns>
        public int Insert(FeVideoItem model)
        {
            var result = DBContext.Insert<FeVideoItem>("fevideoitem", model)
                                .AutoMap(x => x.SysNo)
                                .ExecuteReturnLastId<int>("sysNo");
            return result;
        }

        /// <summary>
        /// 视频编号
        /// </summary>
        /// <param name="videoSysNo">视频编号</param>
        /// <returns>影响行数</returns>
        public int Delete(int videoSysNo)
        {

            return DBContext.Delete("fevideoitem")
                .Where("SysNo", videoSysNo)
                .Execute();

            //var strSql = "delete fevideoitem where sysNo=@videoSysNo";
            //var result = DBContext.Sql(strSql)
            //                      .Parameter("videoSysNo", videoSysNo)
            //                      .Execute();
            //return result;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">数据模型</param>
        /// <returns>影响行数</returns>
        public int Update(FeVideoItem model)
        {
            int rowsAffected = DBContext.Update<FeVideoItem>("fevideoitem", model)
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
            return DBContext.Update("fevideoitem")
                            .Column("status", status)
                            .Where("sysNo", sysNo)
                            .Execute();
        }

        /// <summary>
        /// 更新播放次数
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>影响行数</returns>
        public int UpdatePlayNumber(int sysNo)
        {
            return DBContext.Sql("update fevideoitem set PlayNumber=PlayNumber+1 where SysNo = @SysNo")
                            .Parameter("SysNo", sysNo)
                            .Execute();
        }

        /// <summary>
        /// 视频项分页列表
        /// </summary>
        /// <param name="requset">查询参数</param>
        public PagedList<VideoItemExt> GetExtPaging(VideoItemQueryRequest requset)
        {
            const string sqlFrom = "FeVideoItem i INNER JOIN FeVideo v ON i.VideoSysNo = v.SysNo";
            var dataList = DBContext.Select<VideoItemExt>("i.*,CONCAT(v.Title,' ',i.`Name`) as NameExt").From(sqlFrom);
            var dataCount = DBContext.Select<int>("count(0)").From(sqlFrom);

            //条件查询委托
            Action<string, string, object> setWhere = (@where, name, value) =>
            {
                dataList.AndWhere(where).Parameter(name, value);
                dataCount.AndWhere(where).Parameter(name, value);
            };

            if (requset.VideoStatus.HasValue)
            {
                setWhere("v.Status = @VideoStatus", "VideoStatus", requset.VideoStatus);
            }
            if (requset.VideoType.HasValue)
            {
                setWhere("v.Type = @VideoType", "VideoType", requset.VideoType);
            }
            if (requset.VideoItemStatus.HasValue)
            {
                setWhere("i.Status = @VideoItemStatus", "VideoItemStatus", requset.VideoItemStatus);
            }
            if (!string.IsNullOrEmpty(requset.VideoName))
            {
                setWhere("(v.Title like CONCAT('%',@VideoName,'%') or i.`Name` like CONCAT('%',@VideoName,'%'))", "VideoName", requset.VideoName);
            }
            if (requset.VideoSysNo.HasValue)
            {
                setWhere("i.VideoSysNo = @VideoSysNo", "VideoSysNo", requset.VideoSysNo);
            }

            var list = new PagedList<VideoItemExt>
            {
                TData = dataList.Paging(requset.CurrentPageIndex.GetHashCode(), requset.PageSize.GetHashCode()).OrderBy("v.DisplayOrder,i.DisplayOrder").QueryMany(),
                CurrentPageIndex = requset.CurrentPageIndex.GetHashCode(),
                TotalCount = dataCount.QuerySingle(),
            };

            return list;
        }
    }
}
