using System;
using System.Collections.Generic;
using EC.Entity.Parameter.Request.CRM;
using EC.Entity.Parameter.Request.Fn;
using EC.Entity.Parameter.Response.CRM;
using EC.Entity.Tables.CRM;
using EC.Entity.View.CRM;
using EC.Entity.View.CRM.Ext;
using EC.Libraries.Core.Pager;

namespace EC.DataAccess.CRM
{
    /// <summary>
    /// 会员扩展数据访问接口
    /// </summary>
    public interface ICrCustomerExt
    {
        ///// <summary>
        ///// 插入
        ///// </summary>
        ///// <param name="model">数据模型</param>
        //int Register(CrCustomerExt model);

        ///// <summary>
        ///// 修改
        ///// </summary>
        ///// <param name="model">数据模型</param>
        //int Update(CrCustomerExt model);

        ///// <summary>
        ///// 更新用户基本资料
        ///// </summary>
        ///// <param name="model">数据模型</param>
        ///// <returns>影响行数</returns>
        //int UpdateProfile(CrCustomerExt model);

        ///// <summary>
        ///// 更新会员信息
        ///// </summary>
        ///// <param name="customerSysNo">会员编号</param>
        ///// <param name="realName">真实姓名</param>
        ///// <param name="grade">会员等级</param>
        ///// <returns>影响行数</returns>
        //int UpdateInfo(int customerSysNo, string realName, int grade);

        ///// <summary>
        ///// 获取
        ///// </summary>
        ///// <param name="sysNo">系统编号</param>
        ///// <returns></returns>
        //CrCustomerExt Get(int sysNo);

        ///// <summary>
        ///// 根据代理编号获取扩展会员
        ///// </summary>
        ///// <param name="serialNumber">代理编号</param>
        ///// <returns>扩展会员信息</returns>
        //CrCustomerExt GetSerialNumber(string serialNumber);

        ///// <summary>
        ///// 根据会员编号获取扩展会员
        ///// </summary>
        ///// <param name="customerSysNo">会员编号</param>
        ///// <returns></returns>
        //CrCustomerExt GetByCustomerSysNo(int customerSysNo);

        ///// <summary>
        ///// 获取扩展会员通过OpenId
        ///// </summary>
        ///// <param name="openId">微信OpenId</param>
        ///// <returns>扩展会员信息</returns>
        //CrCustomerExt GetCustomerExtByOpenId(string openId);

        ///// <summary>
        ///// 获取会员扩展信息
        ///// </summary>
        ///// <param name="customerSysNo">会员编号</param>
        ///// <returns>会员扩展信息</returns>
        //CustomerExt GetCustomerExt(int customerSysNo);

        ///// <summary>
        ///// 获取会员层级列表
        ///// </summary>
        ///// <param name="levelCustomerSysNo">层级会员编号（用于查询层级推荐会员）</param>
        ///// <returns>会员层级列表</returns>
        //IList<CustomerLevelView> GetCustomerLevelCount(int levelCustomerSysNo);

        ///// <summary>
        ///// 获取会员团队数列表
        ///// </summary>
        ///// <param name="sysNoList">会员编号列表</param>
        ///// <returns>会员团队数列表</returns>
        //IList<CustomerLevelView> GetCustomerTeamCount(List<int> sysNoList);

        ///// <summary>
        ///// 获取平台参数
        ///// </summary>
        ///// <returns>平台参数</returns>
        //PlatformView GetPlatformValue();

        ///// <summary>
        ///// 获取会员扩展信息列表
        ///// </summary>
        ///// <param name="request">查询参数</param>
        ///// <returns>会员扩展信息列表</returns>
        //IList<CrCustomerExt> GetList(CustomerExtRequest request);

        ///// <summary>
        ///// 获取会员扩展信息分页列表
        ///// </summary>
        ///// <param name="request">查询参数</param>
        ///// <returns>会员扩展信息分页列表</returns>
        //PagedList<CrCustomerExt> GetPagerList(CustomerExtRequest request);

        ///// <summary>
        ///// 获取会员扩展信息分页列表
        ///// </summary>
        ///// <param name="request">查询参数</param>
        ///// <returns>会员扩展信息分页列表</returns>
        //PagedList<CustomerExt> GetExtPagerList(CustomerExtRequest request);

        ///// <summary>
        ///// 更新头像
        ///// </summary>
        //int UpdateHead(CrCustomerExt model);

        ///// <summary>
        ///// 获取未同步会员
        ///// </summary>
        ///// <returns>会员列表</returns>
        //IList<CrCustomer> GetCustomerNotSync();

        ///// <summary>
        ///// 批量更新团队人数
        ///// </summary>
        ///// <param name="ids">会员编号</param>
        //int UpdateTeamCount(List<int> ids);

        ///// <summary>
        ///// 批量更新层级
        ///// </summary>
        ///// <param name="ids">会员编号列表</param>
        ///// <param name="sysNo">会员编号</param>
        //int UpdateLevelStr(List<int> ids, int sysNo);

        ///// <summary>
        ///// 更新级别或推荐人
        ///// </summary>
        //int UpdateLevelAndReferrerSysNo(CrCustomerExt model);

        ///// <summary>
        ///// 更新过期时间
        ///// </summary>
        //int UpdateExpiresTime(UpdateExpiresTimeRequest request);

        ///// <summary>
        ///// 更新提现(钱包金额-提现金额)
        ///// </summary>
        //int UpdateWalletAmount(UpdateWalletAmountRequest request);

        ///// <summary>
        ///// 批量更新父级奖励
        ///// </summary>
        ///// <param name="batchUpgradeParentList">参数</param>
        ///// <returns>影响行数</returns>
        //int BatchUpdate(List<BatchUpgradeParent> batchUpgradeParentList);

        ///// <summary>
        ///// 更新级别
        ///// </summary>
        //int UpdateLevel(CrCustomerExt model);

        ///// <summary>
        ///// 减去升级基金
        ///// </summary>
        //int SubtractUpgradeFundAmount(CrCustomerExt model);

        ///// <summary>
        ///// 批量修改
        ///// </summary>
        ///// <param name="request">参数</param>
        ///// <returns>影响行数</returns>
        //int BatchUpdate(List<CustomerExtBatchRequest> request);

        ///// <summary>
        ///// 更新等级
        ///// </summary>
        //int UpdateGrade(CrCustomerExt model);

        ///// <summary>
        ///// 支付更新(升级机基金、历史收)
        ///// </summary>
        //int UpdatePayWalletAmount(UpdatePayWalletAmountRequest request);

        ///// <summary>
        ///// 更新OpenId
        ///// </summary>
        //int UpdateUserInfo(CrCustomerExt model);

        ///// <summary>
        ///// 根据OpenId获取会员扩展表
        ///// </summary>
        //LoginResponse GetByOpenId(string openId);
    }
}
