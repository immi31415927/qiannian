using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EC.DataAccess.CRM;
using EC.Entity.Parameter.Request.CRM;
using EC.Entity.Parameter.Request.Fn;
using EC.Entity.Parameter.Response.CRM;
using EC.Entity.Tables.CRM;
using EC.Entity.View.CRM;
using EC.Entity.View.CRM.Ext;
using EC.Libraries.Core.Data;
using EC.Libraries.Core.Pager;

namespace EC.DataAccess.MySql.CRM
{
    /// <summary>
    /// 会员扩展数据访问接口实现
    /// </summary>
    public class CrCustomerExtImpl : Base, ICrCustomerExt
    {
        #region 常量
        private const string StrTableName = "agent_customerext";
        #endregion

//        /// <summary>
//        /// 插入扩展会员
//        /// </summary>
//        /// <param name="model">数据模型</param>
//        /// <returns>系统编号</returns>
//        public int Register(CrCustomerExt model)
//        {
//            return DBContext.Insert(StrTableName)
//                            .Column("CustomerSysNo", model.CustomerSysNo)
//                            .Column("SerialNumber", model.SerialNumber)
//                            .Column("OpenId", model.OpenId)
//                            .Column("ReferrerSysNo", model.ReferrerSysNo)
//                            .Column("RealName", model.RealName)
//                            .Column("HeadImgUrl", model.HeadImgUrl)
//                //.Column("PhoneNumber", model.PhoneNumber)
//                            .Column("IDNumber", model.IDNumber)
//                            .Column("TeamCount", model.TeamCount)
//                            .Column("Grade", model.Grade)
//                            .Column("Level", model.Level)
//                            .Column("LevelCustomerStr", model.LevelCustomerStr)
//                            .Column("Bank", model.Bank)
//                            .Column("BankNumber", model.BankNumber)
//                            .Column("WalletAmount", model.WalletAmount)
//                            .Column("HistoryWalletAmount", model.HistoryWalletAmount)
//                            .Column("UpgradeFundAmount", model.UpgradeFundAmount)
//                            .Column("GeneralBonus", model.GeneralBonus)
//                            .Column("AreaBonus", model.AreaBonus)
//                            .Column("GlobalBonus", model.GlobalBonus)
//                            .Column("Expires", model.Expires)
//                            .Column("CreatedDate", model.CreatedDate)
//                            .ExecuteReturnLastId<int>("SysNo");
//        }

//        /// <summary>
//        /// 修改扩展会员
//        /// </summary>
//        /// <param name="model">数据模型</param>
//        /// <returns>影响行数</returns>
//        public int Update(CrCustomerExt model)
//        {
//            return DBContext.Update(StrTableName, model)
//                            .AutoMap(x => x.SysNo)
//                            .Where(x => x.SysNo)
//                            .Execute();
//        }

//        /// <summary>
//        /// 更新用户基本资料
//        /// </summary>
//        /// <param name="model">数据模型</param>
//        /// <returns>影响行数</returns>
//        public int UpdateProfile(CrCustomerExt model)
//        {
//            return DBContext.Update(StrTableName)
//                            .Column("HeadImgUrl", model.HeadImgUrl)
//                    .Column("RealName", model.RealName)
//                            .Column("Email", model.Email)
//                            .Column("IDNumber", model.IDNumber)
//                            .Column("Bank", model.Bank)
//                            .Column("BankNumber", model.BankNumber)
//                            .Where("CustomerSysNo", model.CustomerSysNo)
//                            .Execute();
//        }

//        /// <summary>
//        /// 更新会员信息
//        /// </summary>
//        /// <param name="customerSysNo">会员编号</param>
//        /// <param name="realName">真实姓名</param>
//        /// <param name="grade">会员等级</param>
//        /// <returns>影响行数</returns>
//        public int UpdateInfo(int customerSysNo, string realName, int grade)
//        {
//            return DBContext.Update(StrTableName)
//                            .Column("RealName", realName)
//                            .Column("Grade", grade)
//                            .Where("CustomerSysNo", customerSysNo)
//                            .Execute();
//        }

//        /// <summary>
//        /// 获取扩展会员
//        /// </summary>
//        /// <param name="sysNo">系统编号</param>
//        /// <returns>扩展会员信息</returns>
//        public CrCustomerExt Get(int sysNo)
//        {
//            return DBContext.Sql(string.Format("Select * From {0} Where sysNo = @sysNo;", StrTableName))
//                            .Parameter("sysNo", sysNo)
//                            .QuerySingle<CrCustomerExt>();
//        }

//        /// <summary>
//        /// 根据代理编号获取扩展会员
//        /// </summary>
//        /// <param name="serialNumber">代理编号</param>
//        /// <returns>扩展会员信息</returns>
//        public CrCustomerExt GetSerialNumber(string serialNumber)
//        {
//            return DBContext.Sql(string.Format("Select * From {0} Where serialNumber = @serialNumber;", StrTableName))
//                            .Parameter("serialNumber", serialNumber)
//                            .QuerySingle<CrCustomerExt>();
//        }

//        /// <summary>
//        /// 根据会员编号获取扩展会员
//        /// </summary>
//        /// <param name="customerSysNo">会员编号</param>
//        /// <returns>扩展会员信息</returns>
//        public CrCustomerExt GetByCustomerSysNo(int customerSysNo)
//        {
//            return DBContext.Sql(string.Format("Select * From {0} Where customerSysNo = @customerSysNo;", StrTableName))
//                            .Parameter("customerSysNo", customerSysNo)
//                            .QuerySingle<CrCustomerExt>();
//        }

//        /// <summary>
//        /// 获取扩展会员通过OpenId
//        /// </summary>
//        /// <param name="openId">微信OpenId</param>
//        /// <returns>扩展会员信息</returns>
//        public CrCustomerExt GetCustomerExtByOpenId(string openId)
//        {
//            return DBContext.Sql(string.Format("Select * From {0} Where OpenId = @OpenId;", StrTableName))
//                            .Parameter("OpenId", openId)
//                            .QuerySingle<CrCustomerExt>();
//        }

//        /// <summary>
//        /// 获取会员扩展信息
//        /// </summary>
//        /// <param name="customerSysNo">会员编号</param>
//        /// <returns>会员扩展信息</returns>
//        public CustomerExt GetCustomerExt(int customerSysNo)
//        {
//            return DBContext.Select<CustomerExt>("c.*,u.tel as TelNumber,u.zx as Status")
//                            .From(string.Format("{0} c inner join {1} u on u.id = c.CustomerSysNo", StrTableName, "1000n_user"))
//                            .Where("c.CustomerSysNo = @CustomerSysNo")
//                            .Parameter("CustomerSysNo", customerSysNo)
//                            .QuerySingle();
//        }

//        /// <summary>
//        /// 获取会员层级列表
//        /// </summary>
//        /// <param name="levelCustomerSysNo">层级会员编号（用于查询层级推荐会员）</param>
//        /// <returns>会员层级列表</returns>
//        public IList<CustomerLevelView> GetCustomerLevelCount(int levelCustomerSysNo)
//        {
//            return DBContext.Sql(string.Format("Select `Level`,COUNT(0) as LevelTeamCount From {0} Where CONCAT(',',LevelCustomerStr,',') like '%,{1},%' GROUP BY `Level`", StrTableName, levelCustomerSysNo))
//                            .QueryMany<CustomerLevelView>();
//        }

//        /// <summary>
//        /// 获取会员团队数列表
//        /// </summary>
//        /// <param name="sysNoList">会员编号列表</param>
//        /// <returns>会员团队数列表</returns>
//        public IList<CustomerLevelView> GetCustomerTeamCount(List<int> sysNoList)
//        {
//            var sqlStr = new StringBuilder();

//            for (var i = 0; i < sysNoList.Count; i++)
//            {
//                sqlStr.Append(string.Format("select {0} as SysNo,Count(0) as LevelTeamCount from Agent_CustomerExt where CONCAT(',',LevelCustomerStr,',') like '%,{1},%'", sysNoList[i], sysNoList[i]));

//                if (i < sysNoList.Count - 1)
//                {
//                    sqlStr.Append(" UNION ");
//                }
//            }

//            return DBContext.Sql(sqlStr.ToString())
//                            .QueryMany<CustomerLevelView>();
//        }

//        /// <summary>
//        /// 获取平台参数
//        /// </summary>
//        /// <returns>平台参数</returns>
//        public PlatformView GetPlatformValue()
//        {
//            return DBContext.Sql(string.Format(@"select count(0) as RegisterNum,sum(CASE Grade WHEN 10 THEN 1 ELSE 0 END) as VipNum,
//                                 sum(CASE Grade WHEN 20 THEN 1 ELSE 0 END) as CommonAgentNum,sum(CASE Grade WHEN 30 THEN 1 ELSE 0 END) as AreaAgentNum,
//                                 sum(CASE Grade WHEN 40 THEN 1 ELSE 0 END) as CountryAgentNum,sum(CASE Grade WHEN 50 THEN 1 ELSE 0 END) as StockAgentNum,
//                                 WithdrawalsTotalAmount,RechargeTotalAmount FROM {0}", StrTableName))
//                            .QuerySingle<PlatformView>();
//        }

//        /// <summary>
//        /// 获取会员扩展信息列表
//        /// </summary>
//        /// <param name="request">查询参数</param>
//        /// <returns>会员扩展信息列表</returns>
//        public IList<CrCustomerExt> GetList(CustomerExtRequest request)
//        {
//            var dataList = DBContext.Select<CrCustomerExt>("*").From(StrTableName);

//            Action<string, string, object> setWhere = (@where, name, value) => dataList.AndWhere(@where).Parameter(name, value);

//            if (request.LevelCustomerSysNo.HasValue)
//            {
//                setWhere(string.Format("CONCAT(',',LevelCustomerStr,',') like '%,{0},%'", request.LevelCustomerSysNo.Value), "", "");
//            }
//            if (request.SelfLevelCustomerSysNo.HasValue)
//            {
//                setWhere(string.Format("CONCAT(',',SysNo,',',LevelCustomerStr,',') like '%,{0},%'", request.SelfLevelCustomerSysNo.Value), "", "");
//            }
//            if (request.Level.HasValue)
//            {
//                setWhere("Level = @Level", "Level", request.Level.Value);
//            }
//            if (!string.IsNullOrWhiteSpace(request.Keyword))
//            {
//                setWhere("(RealName = @Keyword or SerialNumber = @Keyword)", "Keyword", request.Keyword);
//            }
//            if (request.Grade.HasValue)
//            {
//                setWhere("Grade = @Grade", "Grade", request.Grade.Value);
//            }
//            if (request.CustomerSysNoList != null && request.CustomerSysNoList.Any())
//            {
//                setWhere("CustomerSysNo in(@CustomerSysNoList)", "CustomerSysNoList", string.Join(",", request.CustomerSysNoList));
//            }
//            if (request.LevelList != null && request.LevelList.Any())
//            {
//                setWhere("Level in(@LevelList)", "LevelList", string.Join(",", request.LevelList));
//            }

//            return dataList.OrderBy("SysNo desc").QueryMany();
//        }

//        /// <summary>
//        /// 获取会员扩展信息分页列表
//        /// </summary>
//        /// <param name="request">查询参数</param>
//        /// <returns>会员扩展信息分页列表</returns>
//        public PagedList<CrCustomerExt> GetPagerList(CustomerExtRequest request)
//        {
//            var dataCount = DBContext.Select<int>("count(0)").From(StrTableName);
//            var dataList = DBContext.Select<CrCustomerExt>("*").From(StrTableName);

//            Action<string, string, object> setWhere = (@where, name, value) =>
//            {
//                dataCount.AndWhere(where).Parameter(name, value);
//                dataList.AndWhere(where).Parameter(name, value);
//            };

//            if (request.LevelCustomerSysNo.HasValue)
//            {
//                setWhere(string.Format("CONCAT(',',LevelCustomerStr,',') like '%,{0},%'", request.LevelCustomerSysNo.Value), "", "");
//            }
//            if (request.Level.HasValue)
//            {
//                setWhere("Level = @Level", "Level", request.Level.Value);
//            }
//            if (!string.IsNullOrWhiteSpace(request.Keyword))
//            {
//                setWhere("(RealName = @Keyword or SerialNumber = @Keyword)", "Keyword", request.Keyword);
//            }

//            return new PagedList<CrCustomerExt>
//            {
//                TData = dataList.Paging(request.CurrentPageIndex.GetHashCode(), request.PageSize.GetHashCode()).OrderBy("SysNo desc").QueryMany(),
//                CurrentPageIndex = request.CurrentPageIndex.GetHashCode(),
//                TotalCount = dataCount.QuerySingle()
//            };
//        }

//        /// <summary>
//        /// 获取会员扩展信息分页列表
//        /// </summary>
//        /// <param name="request">查询参数</param>
//        /// <returns>会员扩展信息分页列表</returns>
//        public PagedList<CustomerExt> GetExtPagerList(CustomerExtRequest request)
//        {
//            var sqlFrom = string.Format("{0} a inner join {1} e on a.ReferrerSysNo = e.CustomerSysNo inner join 1000n_user u on u.id = a.CustomerSysNo", StrTableName, StrTableName);
//            var sqlSelect = string.Format("a.*,e.RealName as ReferrerName,u.tel as TelNumber,(select count(0) from {0} b where b.ReferrerSysNo = a.CustomerSysNo) as ReferrerCount,u.zx as Status", StrTableName);
//            var dataCount = DBContext.Select<int>("count(0)").From(sqlFrom);
//            var dataList = DBContext.Select<CustomerExt>(sqlSelect).From(sqlFrom);

//            Action<string, string, object> setWhere = (@where, name, value) =>
//            {
//                dataCount.AndWhere(where).Parameter(name, value);
//                dataList.AndWhere(where).Parameter(name, value);
//            };

//            if (request.LevelCustomerSysNo.HasValue)
//            {
//                setWhere("CONCAT(',',a.LevelCustomerStr,',') like CONCAT('%',@LevelCustomerSysNo,'%')", "LevelCustomerSysNo", request.LevelCustomerSysNo.Value);
//            }
//            if (request.Level.HasValue)
//            {
//                setWhere("a.Level = @Level", "Level", request.Level.Value);
//            }
//            if (request.Grade.HasValue)
//            {
//                setWhere("a.Grade = @Grade", "Grade", request.Grade.Value);
//            }
//            if (!string.IsNullOrWhiteSpace(request.RealName))
//            {
//                setWhere("a.RealName LIKE CONCAT('%',@RealName,'%')", "RealName", request.RealName);
//            }
//            if (!string.IsNullOrWhiteSpace(request.ReferrerName))
//            {
//                setWhere("e.RealName LIKE CONCAT('%',@ReferrerName,'%')", "ReferrerName", request.ReferrerName);
//            }

//            return new PagedList<CustomerExt>
//            {
//                TData = dataList.Paging(request.CurrentPageIndex.GetHashCode(), request.PageSize.GetHashCode()).OrderBy("a.SysNo desc").QueryMany(),
//                CurrentPageIndex = request.CurrentPageIndex.GetHashCode(),
//                TotalCount = dataCount.QuerySingle()
//            };
//        }

//        /// <summary>
//        /// 更新头像
//        /// </summary>
//        public int UpdateHead(CrCustomerExt model)
//        {
//            int rowsAffected = DBContext.Update(StrTableName)
//                .Column("HeadImgUrl", model.HeadImgUrl)
//                .Where("CustomerSysNo", model.CustomerSysNo)
//                .Execute();

//            return rowsAffected;
//        }

//        /// <summary>
//        /// 获取未同步会员
//        /// </summary>
//        /// <returns>会员列表</returns>
//        public IList<CrCustomer> GetCustomerNotSync()
//        {
//            return DBContext.Sql("call GetCustomerNotSync")
//                .QueryMany<CrCustomer>();
//        }

//        /// <summary>
//        /// 批量更新团队人数
//        /// </summary>
//        /// <param name="ids">会员编号</param>
//        public int UpdateTeamCount(List<int> ids)
//        {
//            return DBContext.Sql(string.Format("update {0} set teamCount=teamCount+1 where customerSysNo IN({1})", StrTableName, string.Join(",", ids)))
//                            .Execute();
//        }

//        /// <summary>
//        /// 批量更新层级
//        /// </summary>
//        /// <param name="ids">会员编号列表</param>
//        /// <param name="sysNo">会员编号</param>
//        public int UpdateLevelStr(List<int> ids, int sysNo)
//        {
//            //var sql = string.Format("update agent_customerext set LevelCustomerStr=if(LevelCustomerStr='',{0},concat(LevelCustomerStr,',',{0})) where customerSysNo IN({1})", sysNo, string.Join(",", ids));
//            var sql = string.Format("update agent_customerext set LevelCustomerStr=if(LevelCustomerStr='',{0},concat(LevelCustomerStr,',',{0})) where customerSysNo IN({1})", sysNo, string.Join(",", ids));
//            return DBContext.Sql(sql)
//                            .Execute();
//        }

//        /// <summary>
//        /// 更新级别或推荐人
//        /// </summary>
//        public int UpdateLevelAndReferrerSysNo(CrCustomerExt model)
//        {
//            int rowsAffected = DBContext.Update(StrTableName)
//                .Column("ReferrerSysNo", model.ReferrerSysNo)
//                .Column("Level", model.Level)
//                .Where("CustomerSysNo", model.CustomerSysNo)
//                .Execute();

//            return rowsAffected;
//        }

//        /// <summary>
//        /// 更新过期时间
//        /// </summary>
//        public int UpdateExpiresTime(UpdateExpiresTimeRequest request)
//        {
//            int rowsAffected = DBContext.Update(StrTableName)
//                    .Column("ExpiresTime", request.ExpiresTime)
//                    .Where("CustomerSysNo", request.CustomerSysNo)
//                    .Execute();
//            return rowsAffected;
//        }

//        /// <summary>
//        /// 更新提现(钱包金额-提现金额)(升级金额+提现金额/(如果是普通、区域、全国才除)2)
//        /// </summary>
//        public int UpdateWalletAmount(UpdateWalletAmountRequest request)
//        {
//            return DBContext.Sql("update agent_customerext set WalletAmount=WalletAmount-@WalletAmount,WithdrawalsTotalAmount=WithdrawalsTotalAmount+@WithdrawalsTotalAmount,UpgradeFundAmount=UpgradeFundAmount+@UpgradeFundAmount where CustomerSysNo=@CustomerSysNo")
//                            .Parameter("WalletAmount", request.WalletAmount)
//                            .Parameter("WithdrawalsTotalAmount", request.WithdrawalsTotalAmount)
//                            .Parameter("UpgradeFundAmount", request.UpgradeFundAmount)
//                            .Parameter("CustomerSysNo", request.CustomerSysNo)
//                            .Execute();
//        }

//        /// <summary>
//        /// 批量更新父级奖励
//        /// </summary>
//        /// <param name="batchUpgradeParentList">参数</param>
//        /// <returns>影响行数</returns>
//        public int BatchUpdate(List<BatchUpgradeParent> batchUpgradeParentList)
//        {
//            var sqlStr = new StringBuilder();

//            foreach (var item in batchUpgradeParentList)
//            {
//                sqlStr.Append(string.Format("update agent_customerext set WalletAmount=WalletAmount+{0},HistoryWalletAmount=HistoryWalletAmount+{1},GeneralBonus=GeneralBonus+{2},AreaBonus=AreaBonus+{3},GlobalBonus=GlobalBonus+{4} where CustomerSysNo={5};",
//                    item.WalletAmount, (item.WalletAmount > 0 ? item.WalletAmount : 0), item.GeneralBonus, item.AreaBonus, item.GlobalBonus, item.CustomerSysNo));
//            }

//            return DBContext.Sql(sqlStr.ToString())
//                .Execute();
//        }

//        /// <summary>
//        /// 更新级别
//        /// </summary>
//        public int UpdateLevel(CrCustomerExt model)
//        {
//            int rowsAffected = DBContext.Update(StrTableName)
//                .Column("Level", model.Level)
//                .Where("CustomerSysNo", model.CustomerSysNo)
//                .Execute();

//            return rowsAffected;
//        }

//        /// <summary>
//        /// 减去升级基金
//        /// </summary>
//        public int SubtractUpgradeFundAmount(CrCustomerExt model)
//        {
//            return DBContext.Sql(string.Format("update {0} set UpgradeFundAmount=UpgradeFundAmount-{1} where customerSysNo={2}", StrTableName, model.UpgradeFundAmount, model.CustomerSysNo))
//                            .Execute();
//        }

//        /// <summary>
//        /// 批量修改
//        /// </summary>
//        /// <param name="request">参数</param>
//        /// <returns>影响行数</returns>
//        public int BatchUpdate(List<CustomerExtBatchRequest> request)
//        {
//            var sqlStr = new StringBuilder();
//            sqlStr.Append(string.Format("UPDATE {0} SET ", StrTableName));

//            if (request.Any(p => p.Grade.HasValue))
//            {
//                sqlStr.Append(" Grade = CASE CustomerSysNo");
//                foreach (var item in request)
//                {
//                    sqlStr.Append(item.Grade.HasValue
//                        ? string.Format(" WHEN {0} THEN {1}", item.CustomerSysNo, item.Grade)
//                        : string.Format(" WHEN {0} THEN Grade", item.CustomerSysNo));
//                }
//                sqlStr.Append(" END,");
//            }
//            if (request.Any(p => p.WalletAmount.HasValue))
//            {
//                sqlStr.Append(" WalletAmount = CASE CustomerSysNo");
//                foreach (var item in request)
//                {
//                    sqlStr.Append(string.Format(" WHEN {0} THEN WalletAmount+{1}", item.CustomerSysNo, decimal.Round(Convert.ToDecimal(item.WalletAmount), 8)));
//                }
//                sqlStr.Append(" END,");
//            }

//            if (request.Any(p => p.HistoryWalletAmount.HasValue))
//            {
//                sqlStr.Append(" HistoryWalletAmount = CASE CustomerSysNo");
//                foreach (var item in request)
//                {
//                    sqlStr.Append(string.Format(" WHEN {0} THEN HistoryWalletAmount+{1}", item.CustomerSysNo, (item.HistoryWalletAmount > 0 ? decimal.Round(Convert.ToDecimal(item.HistoryWalletAmount), 8) : 0)));
//                }
//                sqlStr.Append(" END,");
//            }

//            if (request.Any(p => p.UpgradeFundAmount.HasValue))
//            {
//                sqlStr.Append(" UpgradeFundAmount = CASE CustomerSysNo");
//                foreach (var item in request)
//                {
//                    sqlStr.Append(string.Format(" WHEN {0} THEN UpgradeFundAmount+{1}", item.CustomerSysNo, decimal.Round(Convert.ToDecimal(item.UpgradeFundAmount), 8)));
//                }
//                sqlStr.Append(" END,");
//            }

//            if (request.Any(p => p.GeneralBonus.HasValue))
//            {
//                sqlStr.Append(" GeneralBonus = CASE CustomerSysNo");
//                foreach (var item in request)
//                {
//                    sqlStr.Append(string.Format(" WHEN {0} THEN GeneralBonus+{1}", item.CustomerSysNo, decimal.Round(Convert.ToDecimal(item.GeneralBonus), 8)));
//                }
//                sqlStr.Append(" END,");
//            }

//            if (request.Any(p => p.AreaBonus.HasValue))
//            {
//                sqlStr.Append(" AreaBonus = CASE CustomerSysNo");
//                foreach (var item in request)
//                {
//                    sqlStr.Append(string.Format(" WHEN {0} THEN AreaBonus+{1}", item.CustomerSysNo, decimal.Round(Convert.ToDecimal(item.AreaBonus), 8)));
//                }
//                sqlStr.Append(" END,");
//            }

//            if (request.Any(p => p.GlobalBonus.HasValue))
//            {
//                sqlStr.Append(" GlobalBonus = CASE CustomerSysNo");
//                foreach (var item in request)
//                {
//                    sqlStr.Append(string.Format(" WHEN {0} THEN GlobalBonus+{1}", item.CustomerSysNo, decimal.Round(Convert.ToDecimal(item.GlobalBonus), 8)));
//                }
//                sqlStr.Append(" END,");
//            }

//            var sql = sqlStr.ToString();
//            sql = sql.Substring(0, sql.Length - 1);

//            return DBContext.Sql(string.Format("{0} WHERE CustomerSysNo IN({1})", sql, string.Join(",", request.Select(p => p.CustomerSysNo))))
//                            .Execute();
//        }

//        /// <summary>
//        /// 更新等级
//        /// </summary>
//        public int UpdateGrade(CrCustomerExt model)
//        {
//            int rowsAffected = DBContext.Update(StrTableName)
//                .Column("Grade", model.Grade)
//                .Where("CustomerSysNo", model.CustomerSysNo)
//                .Execute();

//            return rowsAffected;
//        }

//        /// <summary>
//        /// 支付更新(升级机基金、历史收)
//        /// </summary>
//        public int UpdatePayWalletAmount(UpdatePayWalletAmountRequest request)
//        {
//            return DBContext.Sql("update agent_customerext set UpgradeFundAmount=UpgradeFundAmount+@UpgradeFundAmount,RechargeTotalAmount=RechargeTotalAmount+@RechargeTotalAmount where CustomerSysNo=@CustomerSysNo")

//                            .Parameter("UpgradeFundAmount", request.UpgradeFundAmount)
//                            .Parameter("RechargeTotalAmount", request.RechargeTotalAmount)
//                            .Parameter("CustomerSysNo", request.CustomerSysNo)
//                            .Execute();
//        }

//        /// <summary>
//        /// 更新OpenId
//        /// </summary>
//        public int UpdateUserInfo(CrCustomerExt model)
//        {
//            int rowsAffected = DBContext.Update(StrTableName)
//                .Column("OpenId", model.OpenId)
//                .Column("Nickname", model.Nickname)
//                .Column("HeadImgUrl", model.HeadImgUrl)
//                .Where("CustomerSysNo", model.CustomerSysNo)
//                .Execute();

//            return rowsAffected;
//        }

//        /// <summary>
//        /// 根据OpenId获取会员扩展表
//        /// </summary>
//        public LoginResponse GetByOpenId(string openId)
//        {
//            var sql = string.Format("SELECT u.id,u.tel,u.sid,c.OpenId,c.Nickname,c.HeadImgUrl FROM 1000n_user u INNER JOIN agent_customerext c on u.id=c.customersysno where c.OpenId='{0}'",openId);

//            var result = DBContext.Sql(sql)
//                                .QuerySingle<LoginResponse>();
//            return result;
//        }
    }
}
