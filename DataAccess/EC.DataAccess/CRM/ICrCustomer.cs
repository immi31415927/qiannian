using System.Collections.Generic;
using EC.Entity.Parameter.Request.CRM;
using EC.Entity.Parameter.Request.Fn;
using EC.Entity.Tables.CRM;
using EC.Entity.View.CRM;
using EC.Entity.View.CRM.Ext;
using EC.Libraries.Core.Pager;

namespace EC.DataAccess.CRM
{
    /// <summary>
    /// 用户数据访问接口
    /// </summary>
    public interface ICrCustomer
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="sysno">系统编号</param>
        /// <returns></returns>
        CrCustomer Get(int sysno);

        /// <summary>
        /// 根据手机号获取用户
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        CrCustomer GetByPhoneNumber(string phoneNumber);

        /// <summary>
        /// 根据账号获取用户
        /// </summary>
        /// <param name="account">账号</param>
        CrCustomer GetByAccount(string account);

        /// <summary>
        /// 根据openId获取用户
        /// </summary>
        /// <param name="openId">微信OpenId</param>
        CrCustomer GetByOpenId(string openId);

        /// <summary>
        /// 根据编码获取用户
        /// </summary>
        /// <param name="serialNumber">编码</param>
        CrCustomer GetBySerialNumber(string serialNumber);

        /// <summary>
        /// 获取会员层级列表
        /// </summary>
        /// <param name="levelCustomerSysNo">层级会员编号（用于查询层级推荐会员）</param>
        /// <returns>会员层级列表</returns>
        IList<CustomerLevelView> GetCustomerLevelCount(int levelCustomerSysNo);

        /// <summary>
        /// 获取会员层级列表
        /// </summary>
        /// <param name="levelCustomerSysNo">层级会员编号（用于查询层级推荐会员）</param>
        /// <returns>会员层级列表</returns>
        IList<CustomerLevelView> GetCustomerTeaCount(int levelCustomerSysNo);

        /// <summary>
        /// 获取会员团队数列表
        /// </summary>
        /// <param name="sysNoList">会员编号列表</param>
        /// <returns>会员团队数列表</returns>
        IList<CustomerLevelView> GetCustomerTeamCount(List<int> sysNoList);

        /// <summary>
        /// 获取平台参数
        /// </summary>
        /// <returns>平台参数</returns>
        PlatformView GetPlatformValue();

        /// <summary>
        /// 批量更新团队人数
        /// </summary>
        /// <param name="ids">会员编号</param>
        int UpdateTeamCount(List<int> ids);

        /// <summary>
        /// 批量更新层级
        /// </summary>
        /// <param name="ids">会员编号列表</param>
        /// <param name="sysNo">会员编号</param>
        int UpdateLevelStr(List<int> ids, int sysNo);

        /// <summary>
        /// 更新提现(钱包金额-提现金额)(升级金额+提现金额/(如果是普通、区域、全国才除)2)
        /// </summary>
        int UpdateWalletAmount(UpdateWalletAmountRequest request);

        /// <summary>
        /// 批量更新父级奖励
        /// </summary>
        /// <param name="batchUpgradeParentList">参数</param>
        /// <returns>影响行数</returns>
        int BatchUpdate(List<BatchUpgradeParent> batchUpgradeParentList);

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="request">参数</param>
        /// <returns>影响行数</returns>
        int BatchUpdate(List<CustomerExtBatchRequest> request);

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="model">数据模型</param>
        int Register(CrCustomer model);

        /// <summary>
        /// 获取会员扩展信息列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>会员扩展信息列表</returns>
        IList<CrCustomer> GetList(CustomerExtRequest request);

        /// <summary>
        /// 更新用户登录信息
        /// </summary>
        int UpdateLogin(CrCustomer model);

        /// <summary>
        /// 更新用户基本资料
        /// </summary>
        /// <param name="model">数据模型</param>
        /// <returns>影响行数</returns>
        int UpdateBankInfo(CrCustomer model);

        /// <summary>
        /// 更新用户头像
        /// </summary>
        int UpdateHead(int sysNo, string headImgUrl);

        /// <summary>
        /// 绑定账号更新用户信息
        /// </summary>
        int MemberBindUpdate(CustomerExtRequest model);

        /// <summary>
        /// 减去升级基金
        /// </summary>
        int SubtractUpgradeFundAmount(CrCustomer model);

        /// <summary>
        /// 更新用户等级
        /// </summary>
        int UpdateGrade(CrCustomer model);

        /// <summary>
        /// 将VIP结算奖金写入钱包
        /// </summary>
        int UpdateGeneralBonus(CrCustomer model);

        /// <summary>
        /// 区域待结算奖金写入钱包
        /// </summary>
        int UpdateAreaBonus(CrCustomer model);

        /// <summary>
        /// 全国待结算奖金写入钱包
        /// </summary>
        int UpdateGlobalBonus(CrCustomer model);

        /// <summary>
        /// 支付更新(升级机基金、历史收)
        /// </summary>
        int UpdatePayWalletAmount(UpdatePayWalletAmountRequest request);

        /// <summary>
        /// 续费【微信回调】
        /// </summary>
        /// <param name="model"></param>
        int UpdateRenew(CrCustomer model);

        /// <summary>
        /// 更新用户密码
        /// </summary>
        /// <param name="model">会员模型</param>
        int UpdatePassword(CrCustomer model);

        /// <summary>
        /// 更新用户基础信息
        /// </summary>
        /// <param name="model">用户信息</param>
        /// <returns>影响行数</returns>
        int UpdateBaseInfo(CrCustomer model);

        /// <summary>
        /// 更新奖金充值(基金充值[充值方式:奖金余额]、帐户续费[续费方式:奖金余额])
        /// </summary>
        /// <returns></returns>
        int UpdateBonusRecharge(CrCustomer model);

        /// <summary>
        /// 续费更新钱包
        /// </summary>
        int UpdateWalletAmountRenew(CrCustomer model);

        /// <summary>
        /// 更新过期时间
        /// </summary>
        int UpdateExpiresTime(CrCustomer model);

        /// <summary>
        /// 驳回
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Reject(CrCustomer model);

        /// <summary>
        /// 获取会员扩展信息分页列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>会员扩展信息分页列表</returns>
        PagedList<CustomerExt> GetExtPagerList(CustomerExtRequest request);
    }
}
