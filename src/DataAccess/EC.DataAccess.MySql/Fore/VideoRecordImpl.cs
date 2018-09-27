using System;
using System.Collections;
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
    /// 视频收藏数据访问接口
    /// </summary>
    public class VideoRecordImpl : Base, IVideoRecord
    {
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model">数据模型</param>
        /// <returns>影响行数</returns>
        public int Insert(FeVideoRecord model)
        {
            return DBContext.Insert("FeVideoRecord", model)
                            .AutoMap(x => x.SysNo)
                            .ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">数据模型</param>
        /// <returns>影响行数</returns>
        public int Update(FeVideoRecord model)
        {
            return DBContext.Update("FeVideoRecord", model)
                            .AutoMap(x => x.SysNo)
                            .Where(x => x.SysNo)
                            .Execute();
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="sysNo">编码</param>
        /// <param name="status">状态</param>
        /// <returns>影响行数</returns>
        public int UpdateStatus(int sysNo, int status)
        {
            return DBContext.Update("FeVideoRecord")
                            .Column("Status", status)
                            .Where("SysNo", sysNo)
                            .Execute();
        }

        /// <summary>
        /// 删除会员收藏信息
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <param name="customerSysNo">会员编号</param>
        /// <returns>影响行数</returns>
        public int Delete(int sysNo, int customerSysNo)
        {
            return DBContext.Delete("FeVideoRecord")
                            .Where("SysNo", sysNo)
                            .Where("CustomerSysNo", customerSysNo)
                            .Execute();
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        public FeVideoRecord Get(int sysNo)
        {
            return DBContext.Sql("select * from FeVideoRecord where sysNo = @sysNo;")
                            .Parameter("sysNo", sysNo)
                            .QuerySingle<FeVideoRecord>();
        }

        /// <summary>
        /// 获取视频记录列表
        /// </summary>
        /// <param name="requset">查询参数</param>
        /// <returns>视频记录列表</returns>
        public IList<FeVideoRecord> GetList(VideoFavQueryRequest requset)
        {
            var dataList = DBContext.Select<FeVideoRecord>("*").From("FeVideoRecord");

            //条件查询委托
            Action<string, string, object> setWhere = (@where, name, value) => dataList.AndWhere(@where).Parameter(name, value);

            if (requset.VideoSysNo.HasValue)
            {
                setWhere("VideoSysNo = @VideoSysNo", "VideoSysNo", requset.VideoSysNo.Value);
            }
            if (requset.Type.HasValue)
            {
                setWhere("Type = @Type", "Type", requset.Type.Value);
            }
            if (requset.CustomerSysNo.HasValue)
            {
                setWhere("CustomerSysNo = @CustomerSysNo", "CustomerSysNo", requset.CustomerSysNo.Value);
            }

            return dataList.QueryMany();
        }

        /// <summary>
        /// 获取视频收藏信息分页列表
        /// </summary>
        /// <param name="requset">查询参数</param>
        /// <returns>视频收藏信息分页列表</returns>
        public PagedList<FeVideoRecord> GetPaging(VideoFavQueryRequest requset)
        {
            var dataList = DBContext.Select<FeVideoRecord>("*").From("FeVideoRecord");
            var dataCount = DBContext.Select<int>("count(0)").From("FeVideoRecord");

            //条件查询委托
            Action<string, string, object> setWhere = (@where, name, value) =>
            {
                dataList.AndWhere(where).Parameter(name, value);
                dataCount.AndWhere(where).Parameter(name, value);
            };

            if (requset.Type.HasValue)
            {
                setWhere("Type = @Type", "Type", requset.Type.Value);
            }
            if (requset.CustomerSysNo.HasValue)
            {
                setWhere("CustomerSysNo = @CustomerSysNo", "CustomerSysNo", requset.CustomerSysNo.Value);
            }

            var list = new PagedList<FeVideoRecord>
            {
                TData = dataList.Paging(requset.CurrentPageIndex.GetHashCode(), requset.PageSize.GetHashCode()).OrderBy("CreatedDate desc").QueryMany(),
                CurrentPageIndex = requset.CurrentPageIndex.GetHashCode(),
                TotalCount = dataCount.QuerySingle(),
            };

            return list;
        }
    }
}
