using System;
using EC.DataAccess.Bs;
using EC.Entity.Parameter.Request.Bs;
using EC.Entity.Tables.Bs;
using EC.Libraries.Core.Pager;

namespace EC.DataAccess.MySql.Bs
{
    using EC.Libraries.Core.Data;

    /// <summary>
    /// 用户数据访问接口实现
    /// </summary>
    public class BsUserImpl : Base, IBsUser
    {
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>系统编号</returns>
        public int Insert(BsUser model)
        {
            return DBContext.Insert("Agent_BsUser", model)
                            .AutoMap(x => x.SysNo)
                            .ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>影响行数</returns>
        public int Update(BsUser model)
        {
            return DBContext.Update("Agent_BsUser", model)
                            .AutoMap(x => x.SysNo)
                            .Where(x => x.SysNo)
                            .Execute();
        }

        /// <summary>
        /// 修改局部数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>影响行数</returns>
        public int UpdateSection(BsUser model)
        {
            return DBContext.Update("Agent_BsUser")
                            .Column("Account", model.Account)
                            .Column("Name", model.Name)
                            .Column("PhoneNumber", model.PhoneNumber)
                            .Column("EMail", model.EMail)
                            .Column("Status", model.Status)
                            .Where("SysNo", model.SysNo)
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
            return DBContext.Update("Agent_BsUser")
                            .Column("Status", status)
                            .Where("SysNo", sysNo)
                            .Execute();
        }

        /// <summary>
        /// 账号是否重复
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <param name="account">账号</param>
        /// <returns>影响行数</returns>
        public int IsRepeatOfAccount(int sysNo, string account)
        {
            return DBContext.Sql("select count(0) from Agent_BsUser where Account = @Account and SysNo != @SysNo")
                            .Parameter("Account", account)
                            .Parameter("SysNo", sysNo)
                            .QuerySingle<int>();
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <param name="passWord">密码</param>
        /// <returns>影响行数</returns>
        public int ResetPassWord(int sysNo,string passWord)
        {
            return DBContext.Update("Agent_BsUser")
                            .Column("PassWord", passWord)
                            .Where("SysNo", sysNo)
                            .Execute();
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="account">用户账号</param>
        /// <returns>用户信息</returns>
        public BsUser GetUserByAccount(string account)
        {
            return DBContext.Select<BsUser>("*")
                            .From("Agent_BsUser")
                            .Where("Account = @Account")
                            .Parameter("Account", account)
                            .QuerySingle();
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>用户信息</returns>
        public BsUser Get(int sysNo)
        {
            return DBContext.Sql("select * from agent_bsuser where sysNo = @sysNo")
                            .Parameter("sysNo", sysNo)
                            .QuerySingle<BsUser>();
        }

        /// <summary>
        /// 获取角色分页列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>角色分页列表</returns>
        public PagedList<BsUser> GetPagerList(UserRequest request)
        {
            var dataCount = DBContext.Select<int>("count(0)").From("Agent_BsUser");
            var dataList = DBContext.Select<BsUser>("*").From("Agent_BsUser");

            //条件查询委托
            Action<string, string, object> setWhere = (@where, name, value) =>
            {
                dataCount.AndWhere(where).Parameter(name, value);
                dataList.AndWhere(where).Parameter(name, value);
            };


            return new PagedList<BsUser>
            {
                TData = dataList.Paging(request.CurrentPageIndex.GetHashCode(), request.PageSize.GetHashCode()).OrderBy("SysNo desc").QueryMany(),
                CurrentPageIndex = request.CurrentPageIndex.GetHashCode(),
                TotalCount = dataCount.QuerySingle()
            };
        }
    }
}
