using EC.Entity.Parameter.Request.Bs;
using EC.Entity.Tables.Bs;
using EC.Libraries.Core.Pager;

namespace EC.DataAccess.Bs
{
    /// <summary>
    /// 用户数据访问接口
    /// </summary>
    public interface IBsUser
    {
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>系统编号</returns>
        int Insert(BsUser model);

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>影响行数</returns>
        int Update(BsUser model);

        /// <summary>
        /// 修改局部数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>影响行数</returns>
        int UpdateSection(BsUser model);

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="sysNo">编码</param>
        /// <param name="status">状态</param>
        /// <returns>影响行数</returns>
        int UpdateStatus(int sysNo, int status);

        /// <summary>
        /// 账号是否重复
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <param name="account">账号</param>
        /// <returns>影响行数</returns>
        int IsRepeatOfAccount(int sysNo, string account);

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <param name="passWord">密码</param>
        /// <returns>影响行数</returns>
        int ResetPassWord(int sysNo, string passWord);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="account">用户账号</param>
        /// <returns>用户信息</returns>
        BsUser GetUserByAccount(string account);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>用户信息</returns>
        BsUser Get(int sysNo);

        /// <summary>
        /// 获取角色分页列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>角色分页列表</returns>
        PagedList<BsUser> GetPagerList(UserRequest request);
    }
}
