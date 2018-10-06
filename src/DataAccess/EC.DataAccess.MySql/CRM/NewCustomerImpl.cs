using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.DataAccess.MySql.CRM
{
    using EC.DataAccess.CRM;
    using EC.Entity.Parameter.NewCustomer;
    using EC.Entity.Parameter.Request.CRM;
    using EC.Entity.Tables.CRM;
    using EC.Libraries.Core.Data;

    /// <summary>
    /// 新会员数据接口现实
    /// </summary>
    public class NewCustomerImpl : Base, INewCustomer
    {
        /// <summary>
        /// 获取会员
        /// </summary>
        /// <param name="sysno">系统编号</param>
        /// <returns></returns>
        public CrCustomer Get(int sysno)
        {
            var sql = "select * from crcustomer where sysno=@sysno;";

            var result = DBContext.Sql(sql)
                                .Parameter("sysno", sysno)
                                .QuerySingle<CrCustomer>();
            return result;
        }

        /// <summary>
        /// 获取会员列表
        /// </summary>
        /// <returns>会员扩展信息列表</returns>
        public IList<CrCustomer> GetAll()
        {
            var sql = "select * from crcustomer;";

            return DBContext.Sql(sql).QueryMany<CrCustomer>();
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
        /// 将待结算金额写入钱包
        /// </summary>
        public int UpdatePendingAmount(int sysNo, int grade)
        {
            var sql = "update crcustomer set ";
            switch (grade)
            {
                case 20:
                    sql += "WalletAmount=WalletAmount+SettledBonus20,SettledBonus20=0 ";
                    break;
                case 30:
                    sql += "WalletAmount=WalletAmount+SettledBonus30,SettledBonus30=0 ";
                    break;
                case 40:
                    sql += "WalletAmount=WalletAmount+SettledBonus40,SettledBonus40=0 ";
                    break;
                case 50:
                    sql += "WalletAmount=WalletAmount+SettledBonus50,SettledBonus50=0 ";
                    break;
                case 60:
                    sql += "WalletAmount=WalletAmount+SettledBonus60,SettledBonus60=0 ";
                    break;
                case 70:
                    sql += "WalletAmount=WalletAmount+SettledBonus70,SettledBonus70=0 ";
                    break;
                case 80:
                    sql += "WalletAmount=WalletAmount+SettledBonus80,SettledBonus80=0 ";
                    break;
                case 90:
                    sql += "WalletAmount=WalletAmount+SettledBonus90,SettledBonus90=0 ";
                    break;
            }
            sql += string.Format("where sysNo={1}",sysNo);
            return DBContext.Sql(sql).Execute();
        }

        /// <summary>
        /// 更新用户等级
        /// </summary>
        public int UpdateGrade(CrCustomer model)
        {
            return DBContext.Sql(string.Format("update crcustomer set Grade={1} where sysNo={2}", model.Grade, model.SysNo)).Execute();
        }
    }
}
