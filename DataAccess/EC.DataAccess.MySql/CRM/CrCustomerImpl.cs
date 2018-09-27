using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EC.DataAccess.CRM;
using EC.Entity.Parameter.Request.CRM;
using EC.Entity.Parameter.Request.Fn;
using EC.Entity.Tables.CRM;
using EC.Entity.View.CRM;
using EC.Entity.View.CRM.Ext;
using EC.Libraries.Core.Data;
using EC.Libraries.Core.Pager;

namespace EC.DataAccess.MySql.CRM
{
    /// <summary>
    /// 会员 数据访问接口实现
    /// </summary>
    public class CrCustomerImpl : Base, ICrCustomer
    {
        #region 常量
        private readonly string strTableName = "crcustomer";
        #endregion

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="sysno">系统编号</param>
        /// <returns></returns>
        public CrCustomer Get(int sysno)
        {
            var sql = string.Format("select * from {0} where sysno=@sysno;", strTableName);

            var result = DBContext.Sql(sql)
                                .Parameter("sysno", sysno)
                                .QuerySingle<CrCustomer>();
            return result;
        }

        /// <summary>
        /// 根据手机号获取用户
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        public CrCustomer GetByPhoneNumber(string phoneNumber)
        {
            var sql = string.Format("select * from {0} where phoneNumber=@phoneNumber;", strTableName);

            var result = DBContext.Sql(sql)
                                .Parameter("phoneNumber", phoneNumber)
                                .QuerySingle<CrCustomer>();
            return result;
        }

        /// <summary>
        /// 根据账号获取用户
        /// </summary>
        /// <param name="account">账号</param>
        public CrCustomer GetByAccount(string account)
        {
            var sql = string.Format("select * from {0} where account=@account;", strTableName);

            var result = DBContext.Sql(sql)
                                .Parameter("account", account)
                                .QuerySingle<CrCustomer>();
            return result;
        }

        /// <summary>
        /// 根据openId获取用户
        /// </summary>
        /// <param name="openId">微信OpenId</param>
        public CrCustomer GetByOpenId(string openId)
        {
            var sql = string.Format("select * from {0} where openId=@openId;", strTableName);

            var result = DBContext.Sql(sql)
                                .Parameter("openId", openId)
                                .QuerySingle<CrCustomer>();
            return result;
        }

        /// <summary>
        /// 根据编码获取用户
        /// </summary>
        /// <param name="serialNumber">编码</param>
        public CrCustomer GetBySerialNumber(string serialNumber)
        {
            return DBContext.Sql(string.Format("Select * From {0} Where serialNumber = @serialNumber;", strTableName))
                            .Parameter("serialNumber", serialNumber)
                            .QuerySingle<CrCustomer>();
        }

        /// <summary>
        /// 获取会员层级列表
        /// </summary>
        /// <param name="levelCustomerSysNo">层级会员编号（用于查询层级推荐会员）</param>
        /// <returns>会员层级列表</returns>
        public IList<CustomerLevelView> GetCustomerLevelCount(int levelCustomerSysNo)
        {
            return DBContext.Sql(string.Format("Select `Level`,COUNT(0) as LevelTeamCount From {0} Where CONCAT(',',LevelCustomerStr,',') like '%,{1},%' GROUP BY `Level`  LIMIT 3", strTableName, levelCustomerSysNo))
                            .QueryMany<CustomerLevelView>();
        }

        public IList<CustomerLevelView> GetCustomerTeaCount(int levelCustomerSysNo)
        {
            return DBContext.Sql(string.Format("Select `Level`,COUNT(0) as LevelTeamCount From {0} Where CONCAT(',',LevelCustomerStr,',') like '%,{1},%' GROUP BY `Level`", strTableName, levelCustomerSysNo))
                            .QueryMany<CustomerLevelView>();
        }

        /// <summary>
        /// 获取会员团队数列表
        /// </summary>
        /// <param name="sysNoList">会员编号列表</param>
        /// <returns>会员团队数列表</returns>
        public IList<CustomerLevelView> GetCustomerTeamCount(List<int> sysNoList)
        {
            var sqlStr = new StringBuilder();

            for (var i = 0; i < sysNoList.Count; i++)
            {
                sqlStr.Append(string.Format("select {0} as SysNo,Count(0) as LevelTeamCount from Agent_CustomerExt where CONCAT(',',LevelCustomerStr,',') like '%,{1},%'", sysNoList[i], sysNoList[i]));

                if (i < sysNoList.Count - 1)
                {
                    sqlStr.Append(" UNION ");
                }
            }

            return DBContext.Sql(sqlStr.ToString())
                            .QueryMany<CustomerLevelView>();
        }

        /// <summary>
        /// 获取平台参数
        /// </summary>
        /// <returns>平台参数</returns>
        public PlatformView GetPlatformValue()
        {
            return DBContext.Sql(string.Format(@"select count(0) as RegisterNum,sum(CASE Grade WHEN 10 THEN 1 ELSE 0 END) as VipNum,
                                 sum(CASE Grade WHEN 20 THEN 1 ELSE 0 END) as CommonAgentNum,sum(CASE Grade WHEN 30 THEN 1 ELSE 0 END) as AreaAgentNum,
                                 sum(CASE Grade WHEN 40 THEN 1 ELSE 0 END) as CountryAgentNum,sum(CASE Grade WHEN 50 THEN 1 ELSE 0 END) as StockAgentNum,
                                 WithdrawalsTotalAmount,RechargeTotalAmount FROM {0}", strTableName))
                            .QuerySingle<PlatformView>();
        }

        /// <summary>
        /// 批量更新团队人数
        /// </summary>
        /// <param name="ids">会员编号</param>
        public int UpdateTeamCount(List<int> ids)
        {
            return DBContext.Sql(string.Format("update {0} set teamCount=teamCount+1 where SysNo IN({1})", strTableName, string.Join(",", ids))).Execute();
        }

        /// <summary>
        /// 批量更新层级
        /// </summary>
        /// <param name="ids">会员编号列表</param>
        /// <param name="sysNo">会员编号</param>
        public int UpdateLevelStr(List<int> ids, int sysNo)
        {
            var sql = string.Format("update crcustomer set LevelCustomerStr=if(LevelCustomerStr='',{0},concat(LevelCustomerStr,',',{0})) where SysNo IN({1})", sysNo, string.Join(",", ids));
            return DBContext.Sql(sql)
                            .Execute();
        }

        /// <summary>
        /// 更新提现(钱包金额-提现金额)(升级金额+提现金额/(如果是普通、区域、全国才除)2)
        /// </summary>
        public int UpdateWalletAmount(UpdateWalletAmountRequest request)
        {
            return DBContext.Sql("update crcustomer set WalletAmount=WalletAmount-@WalletAmount,UpgradeFundAmount=UpgradeFundAmount+@UpgradeFundAmount where SysNo=@SysNo")
                            .Parameter("WalletAmount", request.WalletAmount)
                            .Parameter("UpgradeFundAmount", request.UpgradeFundAmount)
                            .Parameter("SysNo", request.CustomerSysNo)
                            .Execute();
        }

        /// <summary>
        /// 批量更新父级奖励
        /// </summary>
        /// <param name="batchUpgradeParentList">参数</param>
        /// <returns>影响行数</returns>
        public int BatchUpdate(List<BatchUpgradeParent> batchUpgradeParentList)
        {
            var sqlStr = new StringBuilder();

            foreach (var item in batchUpgradeParentList)
            {
                sqlStr.Append(string.Format("update crcustomer set WalletAmount=WalletAmount+{0},HistoryWalletAmount=HistoryWalletAmount+{1},GeneralBonus=GeneralBonus+{2},AreaBonus=AreaBonus+{3},GlobalBonus=GlobalBonus+{4},SettledBonus10=SettledBonus10+{5},SettledBonus20=SettledBonus20+{6},SettledBonus30=SettledBonus30+{7},SettledBonus40=SettledBonus40+{8},SettledBonus50=SettledBonus50+{9},SettledBonus60=SettledBonus60+{10},SettledBonus70=SettledBonus70+{11},SettledBonus80=SettledBonus80+{12},SettledBonus90=SettledBonus90+{13} where SysNo={14};", item.WalletAmount, (item.WalletAmount > 0 ? item.WalletAmount : 0), item.GeneralBonus, item.AreaBonus, item.GlobalBonus, item.SettledBonus10, item.SettledBonus20, item.SettledBonus30, item.SettledBonus40, item.SettledBonus50, item.SettledBonus60, item.SettledBonus70, item.SettledBonus80, item.SettledBonus90, item.CustomerSysNo));
            }

            return DBContext.Sql(sqlStr.ToString())
                .Execute();
        }

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="request">参数</param>
        /// <returns>影响行数</returns>
        public int BatchUpdate(List<CustomerExtBatchRequest> request)
        {
            var sqlStr = new StringBuilder();
            sqlStr.Append(string.Format("UPDATE {0} SET ", strTableName));

            if (request.Any(p => p.Grade.HasValue))
            {
                sqlStr.Append(" Grade = CASE CustomerSysNo");
                foreach (var item in request)
                {
                    sqlStr.Append(item.Grade.HasValue
                        ? string.Format(" WHEN {0} THEN {1}", item.CustomerSysNo, item.Grade)
                        : string.Format(" WHEN {0} THEN Grade", item.CustomerSysNo));
                }
                sqlStr.Append(" END,");
            }
            if (request.Any(p => p.WalletAmount.HasValue))
            {
                sqlStr.Append(" WalletAmount = CASE CustomerSysNo");
                foreach (var item in request)
                {
                    sqlStr.Append(string.Format(" WHEN {0} THEN WalletAmount+{1}", item.CustomerSysNo, decimal.Round(Convert.ToDecimal(item.WalletAmount), 8)));
                }
                sqlStr.Append(" END,");
            }

            if (request.Any(p => p.HistoryWalletAmount.HasValue))
            {
                sqlStr.Append(" HistoryWalletAmount = CASE CustomerSysNo");
                foreach (var item in request)
                {
                    sqlStr.Append(string.Format(" WHEN {0} THEN HistoryWalletAmount+{1}", item.CustomerSysNo, (item.HistoryWalletAmount > 0 ? decimal.Round(Convert.ToDecimal(item.HistoryWalletAmount), 8) : 0)));
                }
                sqlStr.Append(" END,");
            }

            if (request.Any(p => p.UpgradeFundAmount.HasValue))
            {
                sqlStr.Append(" UpgradeFundAmount = CASE CustomerSysNo");
                foreach (var item in request)
                {
                    sqlStr.Append(string.Format(" WHEN {0} THEN UpgradeFundAmount+{1}", item.CustomerSysNo, decimal.Round(Convert.ToDecimal(item.UpgradeFundAmount), 8)));
                }
                sqlStr.Append(" END,");
            }

            if (request.Any(p => p.GeneralBonus.HasValue))
            {
                sqlStr.Append(" GeneralBonus = CASE CustomerSysNo");
                foreach (var item in request)
                {
                    sqlStr.Append(string.Format(" WHEN {0} THEN GeneralBonus+{1}", item.CustomerSysNo, decimal.Round(Convert.ToDecimal(item.GeneralBonus), 8)));
                }
                sqlStr.Append(" END,");
            }

            if (request.Any(p => p.AreaBonus.HasValue))
            {
                sqlStr.Append(" AreaBonus = CASE CustomerSysNo");
                foreach (var item in request)
                {
                    sqlStr.Append(string.Format(" WHEN {0} THEN AreaBonus+{1}", item.CustomerSysNo, decimal.Round(Convert.ToDecimal(item.AreaBonus), 8)));
                }
                sqlStr.Append(" END,");
            }

            if (request.Any(p => p.GlobalBonus.HasValue))
            {
                sqlStr.Append(" GlobalBonus = CASE CustomerSysNo");
                foreach (var item in request)
                {
                    sqlStr.Append(string.Format(" WHEN {0} THEN GlobalBonus+{1}", item.CustomerSysNo, decimal.Round(Convert.ToDecimal(item.GlobalBonus), 8)));
                }
                sqlStr.Append(" END,");
            }

            var sql = sqlStr.ToString();
            sql = sql.Substring(0, sql.Length - 1);

            return DBContext.Sql(string.Format("{0} WHERE CustomerSysNo IN({1})", sql, string.Join(",", request.Select(p => p.CustomerSysNo))))
                            .Execute();
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="model">数据模型</param>
        public int Register(CrCustomer model)
        {
            return DBContext.Insert(strTableName)
                            .Column("Account", model.Account)
                            .Column("Password", model.Password)
                            .Column("SerialNumber", model.SerialNumber)
                            .Column("OpenId", model.OpenId)
                            .Column("ReferrerSysNo", model.ReferrerSysNo)
                            .Column("PhoneNumber", model.PhoneNumber)
                            .Column("RealName", model.RealName)
                            .Column("Nickname", model.Nickname)
                            .Column("HeadImgUrl", model.HeadImgUrl)
                            .Column("IDNumber", model.IDNumber)
                            .Column("TeamCount", model.TeamCount)
                            .Column("Grade", model.Grade)
                            .Column("Level", model.Level)
                            .Column("LevelCustomerStr", model.LevelCustomerStr)
                            .Column("Bank", model.Bank)
                            .Column("BankNumber", model.BankNumber)
                            .Column("WalletAmount", model.WalletAmount)
                            .Column("HistoryWalletAmount", model.HistoryWalletAmount)
                            .Column("UpgradeFundAmount", model.UpgradeFundAmount)
                            .Column("RechargeTotalAmount", model.UpgradeFundAmount)
                            .Column("GeneralBonus", model.GeneralBonus)
                            .Column("AreaBonus", model.AreaBonus)
                            .Column("GlobalBonus", model.GlobalBonus)
                            .Column("ExpiresTime", model.ExpiresTime)
                            .Column("FollowDate", model.FollowDate)
                            .Column("RegisterIP", model.RegisterIP)
                            .Column("RegisterDate", model.RegisterDate)
                            .Column("LoginCount", model.LoginCount)
                            .Column("Status", model.Status)
                            .Column("CreatedDate", model.CreatedDate)
                            .ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// 获取会员扩展信息列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>会员扩展信息列表</returns>
        public IList<CrCustomer> GetList(CustomerExtRequest request)
        {
            var dataList = DBContext.Select<CrCustomer>("*").From(strTableName);

            Action<string, string, object> setWhere = (@where, name, value) => dataList.AndWhere(@where).Parameter(name, value);

            if (request.LevelCustomerSysNo.HasValue)
            {
                setWhere(string.Format("CONCAT(',',LevelCustomerStr,',') like '%,{0},%'", request.LevelCustomerSysNo.Value), "", "");
            }
            if (request.SelfLevelCustomerSysNo.HasValue)
            {
                setWhere(string.Format("CONCAT(',',SysNo,',',LevelCustomerStr,',') like '%,{0},%'", request.SelfLevelCustomerSysNo.Value), "", "");
            }
            if (request.Level.HasValue)
            {
                setWhere("Level = @Level", "Level", request.Level.Value);
            }
            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                setWhere("(RealName = @Keyword or SerialNumber = @Keyword)", "Keyword", request.Keyword);
            }
            if (request.Grade.HasValue)
            {
                setWhere("Grade = @Grade", "Grade", request.Grade.Value);
            }
            if (request.CustomerSysNoList != null && request.CustomerSysNoList.Any())
            {
                setWhere("CustomerSysNo in(@CustomerSysNoList)", "CustomerSysNoList", string.Join(",", request.CustomerSysNoList));
            }
            if (request.LevelList != null && request.LevelList.Any())
            {
                setWhere("Level in(@LevelList)", "LevelList", string.Join(",", request.LevelList));
            }

            return dataList.OrderBy("SysNo desc").QueryMany();
        }

        /// <summary>
        /// 更新用户登录信息
        /// </summary>
        public int UpdateLogin(CrCustomer model)
        {
            int rowsAffected = DBContext.Update(strTableName)
                        .Column("LastLoginIP", model.LastLoginIP)
                        .Column("LastLoginDate", model.LastLoginDate)
                        .Column("LoginCount", model.LoginCount)
                        .Where("sysNo", model.SysNo)
                        .Execute();
            return rowsAffected;
        }

        /// <summary>
        /// 更新用户基本资料
        /// </summary>
        /// <param name="model">数据模型</param>
        /// <returns>影响行数</returns>
        public int UpdateBankInfo(CrCustomer model)
        {
            return DBContext.Update(strTableName)
                            .Column("Bank", model.Bank)
                            .Column("BankNumber", model.BankNumber)
                            .Where("sysNo", model.SysNo)
                            .Execute();
        }

        /// <summary>
        /// 更新用户头像
        /// </summary>
        public int UpdateHead(int sysNo, string headImgUrl)
        {
            return DBContext.Update(strTableName)
                    .Column("HeadImgUrl", headImgUrl)
                    .Where("SysNo", sysNo)
                    .Execute();
        }

        /// <summary>
        /// 绑定账号更新用户信息
        /// </summary>
        public int MemberBindUpdate(CustomerExtRequest model)
        {
            int rowsAffected = DBContext.Update(strTableName)
                .Column("OpenId", model.OpenId)
                .Column("Nickname", model.Nickname)
                .Column("HeadImgUrl", model.HeadImgUrl)
                .Where("sysNo", model.SysNo)
                .Execute();

            return rowsAffected;
        }

        /// <summary>
        /// 减去升级基金
        /// </summary>
        public int SubtractUpgradeFundAmount(CrCustomer model)
        {
            return DBContext.Sql(string.Format("update {0} set UpgradeFundAmount=UpgradeFundAmount-{1} where sysNo={2}", strTableName, model.UpgradeFundAmount, model.SysNo)).Execute();
        }

        /// <summary>
        /// 更新用户等级
        /// </summary>
        public int UpdateGrade(CrCustomer model)
        {
            return DBContext.Sql(string.Format("update {0} set Grade={1} where sysNo={2}", strTableName, model.Grade, model.SysNo)).Execute();
        }

        /// <summary>
        /// 将VIP结算奖金写入钱包
        /// </summary>
        public int UpdateGeneralBonus(CrCustomer model)
        {
            return DBContext.Sql(string.Format("update {0} set WalletAmount=WalletAmount+GeneralBonus,GeneralBonus=0 where sysNo={1}", strTableName, model.SysNo)).Execute();
        }

        /// <summary>
        /// 区域待结算奖金写入钱包
        /// </summary>
        public int UpdateAreaBonus(CrCustomer model)
        {
            return DBContext.Sql(string.Format("update {0} set WalletAmount=WalletAmount+AreaBonus,AreaBonus=0 where sysNo={1}", strTableName, model.SysNo)).Execute();
        }

        /// <summary>
        /// 全国待结算奖金写入钱包
        /// </summary>
        public int UpdateGlobalBonus(CrCustomer model)
        {
            return DBContext.Sql(string.Format("update {0} set WalletAmount=WalletAmount+GlobalBonus,GlobalBonus=0 where sysNo={1}", strTableName, model.SysNo)).Execute();
        }

        /// <summary>
        /// 支付更新(升级机基金、历史收)
        /// </summary>
        public int UpdatePayWalletAmount(UpdatePayWalletAmountRequest request)
        {
            return DBContext.Sql("update crcustomer set UpgradeFundAmount=UpgradeFundAmount+@UpgradeFundAmount,RechargeTotalAmount=RechargeTotalAmount+@RechargeTotalAmount where sysNo=@sysNo")

                            .Parameter("UpgradeFundAmount", request.UpgradeFundAmount)
                            .Parameter("RechargeTotalAmount", request.RechargeTotalAmount)
                            .Parameter("sysNo", request.CustomerSysNo)
                            .Execute();
        }

        /// <summary>
        /// 续费【微信回调】
        /// </summary>
        /// <param name="model"></param>
        public int UpdateRenew(CrCustomer model)
        {
            return DBContext.Sql(string.Format("update {0} set ExpiresTime='{1}' where sysNo={2}", strTableName, model.ExpiresTime, model.SysNo)).Execute();
        }

        /// <summary>
        /// 更新用户密码
        /// </summary>
        /// <param name="model">会员模型</param>
        public int UpdatePassword(CrCustomer model)
        {
            return DBContext.Update(strTableName)
                            .Column("Password", model.Password)
                            .Where("SysNo", model.SysNo)
                            .Execute();
        }

        /// <summary>
        /// 更新用户基础信息
        /// </summary>
        /// <param name="model">用户信息</param>
        /// <returns>影响行数</returns>
        public int UpdateBaseInfo(CrCustomer model)
        {
            return DBContext.Update(strTableName)
                            .Column("PhoneNumber", model.PhoneNumber)
                            .Column("RealName", model.RealName)
                            .Column("Grade", model.Grade)
                            .Column("Status", model.Status)
                            .Where("SysNo", model.SysNo)
                            .Execute();
        }

        /// <summary>
        /// 更新奖金充值(基金充值[充值方式:奖金余额]、帐户续费[续费方式:奖金余额])
        /// </summary>
        /// <returns></returns>
        public int UpdateBonusRecharge(CrCustomer model)
        {
            return DBContext.Sql("update crcustomer set WalletAmount=WalletAmount-@WalletAmount,UpgradeFundAmount=UpgradeFundAmount+@UpgradeFundAmount,RechargeTotalAmount=RechargeTotalAmount+@RechargeTotalAmount where SysNo=@SysNo")
                            .Parameter("WalletAmount", model.WalletAmount)
                            .Parameter("UpgradeFundAmount", model.UpgradeFundAmount)
                            .Parameter("RechargeTotalAmount", model.RechargeTotalAmount)
                            .Parameter("SysNo", model.SysNo)
                            .Execute();
        }

        /// <summary>
        /// 续费更新钱包
        /// </summary>
        public int UpdateWalletAmountRenew(CrCustomer model)
        {
            return DBContext.Sql("update crcustomer set WalletAmount=WalletAmount-@WalletAmount where SysNo=@SysNo")
                .Parameter("WalletAmount", model.WalletAmount)
                .Parameter("SysNo", model.SysNo)
                .Execute();
        }

        /// <summary>
        /// 更新过期时间
        /// </summary>
        public int UpdateExpiresTime(CrCustomer model)
        {
            return DBContext.Sql("update crcustomer set ExpiresTime=@ExpiresTime where SysNo=@SysNo")
                .Parameter("ExpiresTime", model.ExpiresTime)
                .Parameter("SysNo", model.SysNo)
                .Execute();
        }

        /// <summary>
        /// 驳回
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Reject(CrCustomer model)
        {
            return DBContext.Sql("update crcustomer set WalletAmount=WalletAmount+@WalletAmount,UpgradeFundAmount=UpgradeFundAmount-@UpgradeFundAmount where SysNo=@SysNo")
                .Parameter("WalletAmount", model.WalletAmount)
                .Parameter("UpgradeFundAmount", model.UpgradeFundAmount)
                .Parameter("SysNo", model.SysNo)
                .Execute();
        }

        /// <summary>
        /// 获取会员扩展信息分页列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>会员扩展信息分页列表</returns>
        public PagedList<CustomerExt> GetExtPagerList(CustomerExtRequest request)
        {
            const string sqlForm = "crcustomer c left join crcustomer rc on c.ReferrerSysNo = rc.SysNo";
            var dataCount = DBContext.Select<int>("count(0)").From(sqlForm);
            var dataList = DBContext.Select<CustomerExt>("c.*,rc.RealName as ReferrerName").From(sqlForm);

            Action<string, string, object> setWhere = (@where, name, value) =>
            {
                dataCount.AndWhere(where).Parameter(name, value);
                dataList.AndWhere(where).Parameter(name, value);
            };

            if (request.ReferrerSysNo.HasValue)
            {
                setWhere("c.ReferrerSysNo = @ReferrerSysNo", "ReferrerSysNo", request.ReferrerSysNo.Value);
            }
            if (request.LevelCustomerSysNo.HasValue)
            {
                setWhere("CONCAT(',',c.LevelCustomerStr,',') like CONCAT('%',@LevelCustomerSysNo,'%')", "LevelCustomerSysNo", request.LevelCustomerSysNo.Value);
            }
            if (request.Level.HasValue)
            {
                setWhere("c.Level = @Level", "Level", request.Level.Value);
            }
            if (request.Grade.HasValue)
            {
                setWhere("c.Grade = @Grade", "Grade", request.Grade.Value);
            }
            if (!string.IsNullOrWhiteSpace(request.RealName))
            {
                setWhere("c.RealName LIKE CONCAT('%',@RealName,'%')", "RealName", request.RealName);
            }
            if (!string.IsNullOrWhiteSpace(request.RealName))
            {
                setWhere("c.Account LIKE CONCAT('%',@Account,'%')", "Account", request.RealName);
            }
            if (!string.IsNullOrWhiteSpace(request.ReferrerName))
            {
                setWhere("rc.RealName LIKE CONCAT('%',@ReferrerName,'%')", "ReferrerName", request.ReferrerName);
            }
            if (!string.IsNullOrWhiteSpace(request.Account))
            {
                setWhere("rc.Account LIKE CONCAT('%',@Account,'%')", "Account", request.Account);
            }

            return new PagedList<CustomerExt>
            {
                TData = dataList.Paging(request.CurrentPageIndex.GetHashCode(), request.PageSize.GetHashCode()).OrderBy("c.SysNo desc").QueryMany(),
                CurrentPageIndex = request.CurrentPageIndex.GetHashCode(),
                TotalCount = dataCount.QuerySingle()
            };
        }

        ///// <summary>
        ///// 更新用户密码
        ///// </summary>
        ///// <param name="id">用户ID</param>
        ///// <param name="password">密码</param>
        ///// <param name="code">密钥</param>
        ///// <returns>影响行数</returns>
        //public int UpdatePassword(int id, string password, string code)
        //{
        //    return DBContext.Update(strTableName)
        //                    .Column("pass", password)
        //                    .Column("code", code)
        //                    .Where("id", id)
        //                    .Execute();
        //}



        ///// <summary>
        ///// 更新用户基本资料
        ///// </summary>
        //public int UpdateProfile(CrCustomer model)
        //{
        //    int rowsAffected = DBContext.Update(strTableName)
        //        //.Column("Email",model.Email)
        //        //.Column("EmailStatus", model.EmailStatus)
        //        //.Column("Bank", model.Bank)
        //        //.Column("BankNumber", model.BankNumber)
        //            .Where("id", model.id)
        //            .Execute();
        //    return rowsAffected;
        //}



        ///// <summary>
        ///// 更新用户登录信息
        ///// </summary>
        //public int UpdateLogin(CrCustomer model)
        //{
        //    int rowsAffected = DBContext.Update(strTableName)
        //        .Column("logip", model.logip)
        //        .Column("logtime", model.logtime)
        //        .Column("lognum", model.lognum)
        //        .Where("id", model.id)
        //        .Execute();
        //    return rowsAffected;
        //}

        ///// <summary>
        ///// 更新用户状态
        ///// </summary>
        //public int UpdateStatus(CrCustomer model)
        //{
        //    return DBContext.Update("1000n_user")
        //                  .Column("sid", model.sid)
        //                  .Where("SysNo", model.id)
        //                  .Execute();
        //}

        ///// <summary>
        ///// 批量更新团队人数
        ///// </summary>
        ///// <param name="ids">会员编号</param>
        //public int BatchUpdateTeamCount(List<int> ids)
        //{
        //    var sql = string.Format("update crcustomer set teamCount=teamCount+1 where SysNo IN({0})", string.Join(",", ids));
        //    return DBContext.Sql(sql)
        //                    .Execute();
        //}

        //#region 创建会员
        ///// <summary>
        ///// 创建用户
        ///// </summary>
        ///// <param name="model">会员模型</param>
        //public int Register(CrCustomer model)
        //{
        //    int rowsAffected = DBContext.Insert(strTableName)
        //            .Column("name", model.name)
        //            .Column("uid", model.uid)
        //            .Column("tid", model.tid)
        //            .Column("sid", model.sid)
        //            .Column("yid", model.yid)
        //            .Column("zid", model.zid)
        //            .Column("rzid", model.rzid)
        //            .Column("suid", model.suid)
        //            .Column("xunum", model.xunum)
        //            .Column("pass", model.pass)
        //            .Column("code", model.code)
        //            .Column("logip", model.logip)
        //            .Column("lognum", model.lognum)
        //            .Column("logtime", model.logtime)
        //            .Column("addtime", model.addtime)
        //            .Column("zutime", model.zutime)
        //            .Column("qq", model.qq)
        //            .Column("tel", model.tel)
        //            .Column("sex", model.sex)
        //            .Column("city", model.city)
        //            .Column("email", model.email)
        //            .Column("logo", model.logo)
        //            .Column("nichen", model.nichen)
        //            .Column("cion", model.cion)
        //            .Column("scion", model.scion)
        //            .Column("rmb", model.rmb)
        //            .Column("vip", model.vip)
        //            .Column("viptime", model.viptime)
        //            .Column("qianm", model.qianm)
        //            .Column("zx", model.zx)
        //            .Column("logms", model.logms)
        //            .Column("qdts", model.qdts)
        //            .Column("qdtime", model.qdtime)
        //            .Column("level", model.level)
        //            .Column("jinyan", model.jinyan)
        //            .Column("hits", model.hits)
        //            .Column("yhits", model.yhits)
        //            .Column("zhits", model.zhits)
        //            .Column("rhits", model.rhits)
        //            .Column("zanhits", model.zanhits)
        //            .Column("addhits", model.addhits)
        //            .Column("regip", model.regip)
        //            .Column("skins", model.skins)
        //            .Column("bgpic", model.bgpic)
        //            .ExecuteReturnLastId<int>(strPKName);
        //    return rowsAffected;
        //}

        ///// <summary>
        ///// 获取未同步会员
        ///// </summary>
        ///// <returns>会员列表</returns>
        //public IList<CrCustomer> GetCustomerNotSync()
        //{
        //    return DBContext.Sql("call GetCustomerNotSync")
        //        .QueryMany<CrCustomer>();
        //}

        //#endregion

        ///// <summary>
        ///// 根据referrerSysNo获取用户信息
        ///// </summary>
        ///// <param name="referrerSysNo"></param>
        //public UserInfoResponse GetUserInfo(int referrerSysNo)
        //{
        //    var sql = "SELECT u.id as CustomerSysNo,u.tel as PhoneNumber,u.sid,u.code,u.pass,c.OpenId,c.Nickname,c.RealName,c.HeadImgUrl FROM 1000n_user u INNER JOIN agent_customerext c on u.id=c.customersysno where u.id=@id";

        //    var result = DBContext.Sql(sql)
        //            .Parameter("id", referrerSysNo)
        //            .QuerySingle<UserInfoResponse>();

        //    return result;
        //}
    }
}
