using System;
using System.Collections.Generic;
using EC.DataAccess.Bs;
using EC.Entity.Parameter.Request.SMS;
using EC.Entity.Tables.Bs;
using EC.Libraries.Core.Pager;


namespace EC.DataAccess.MySql.Bs
{
    using EC.Libraries.Core.Data;

    /// <summary>
    /// 短信数据访问接口
    /// </summary>
    public class SMSImpl : Base, ISMS
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        public BsSMS Get(int sysNo)
        {
            var strSql = "select * from bssms where sysNo=@sysNo;";

            var result = DBContext.Sql(strSql)
                                .Parameter("sysNo", sysNo)
                                .QuerySingle<BsSMS>();
            return result;
        }

        /// <summary>
        /// 查询某个时间段发送短信数量
        /// </summary>
        /// <param name="requeest">参数</param>
        public List<BsSMS> GetSendingTimes(SMSQueryRequeest requeest)
        {
            //测试SQL:SELECT * FROM bssms WHERE PhoneNumber='15008228718' AND CreatedDate>='2017-11-22 00:00:00' AND CreatedDate<='2017-11-22 23:59:59'
            var strSql = "SELECT * FROM bssms WHERE PhoneNumber=@PhoneNumber AND CreatedDate>='@StartTime' AND CreatedDate<='@EndTime'";

            var result = DBContext.Sql(strSql)
                                .Parameter("PhoneNumber", requeest.PhoneNumber)
                                .Parameter("StartTime", requeest.StartTime)
                                .Parameter("EndTime", requeest.EndTime)
                                .QueryMany<BsSMS>();
            return result;
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model">数据模型</param>
        /// <returns>影响行数</returns>
        public int Insert(BsSMS model)
        {
            var result = DBContext.Insert<BsSMS>("bssms", model)
                                .AutoMap(x => x.SysNo)
                                .ExecuteReturnLastId<int>("sysNo");
            return result;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">数据模型</param>
        /// <returns>影响行数</returns>
        public int Update(BsSMS model)
        {
            int rowsAffected = DBContext.Update<BsSMS>("bssms", model)
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
            return DBContext.Update("bssms")
                            .Column("status", status)
                            .Where("sysNo", sysNo)
                            .Execute();
        }

        /// <summary>
        /// 分页列表
        /// </summary>
        public PagedList<BsSMS> GetPaging(SMSQueryRequeest requeest)
        {
            requeest.Tables = "bssms bs";
            requeest.Tablefields = "bs.*";
            requeest.OrderBy = "bs.SysNo desc";

            var row = DBContext.Select<BsSMS>(requeest.Tablefields).From(requeest.Tables);
            var count = DBContext.Select<int>("count(0)").From(requeest.Tables);

            //条件查询委托
            Action<string, string, object> setWhere = (@where, name, value) =>
            {
                row.AndWhere(where).Parameter(name, value);
                count.AndWhere(where).Parameter(name, value);
            };

            if (requeest.Status.HasValue)
                setWhere("bs.Status=@Status", "Status", requeest.Status);

            var list = new PagedList<BsSMS>
            {
                TData = row.Paging(requeest.CurrentPageIndex.GetHashCode(), requeest.PageSize.GetHashCode()).OrderBy(requeest.OrderBy).QueryMany(),
                CurrentPageIndex = requeest.CurrentPageIndex.GetHashCode(),
                TotalCount = count.QuerySingle(),
            };
            return list;
        }
    }
}
