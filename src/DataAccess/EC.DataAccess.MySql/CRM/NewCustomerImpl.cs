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
                sqlStr.Append(string.Format("update crcustomer set WalletAmount=WalletAmount+{0},HistoryWalletAmount=HistoryWalletAmount+{1},SettledBonus20=SettledBonus20+{2},SettledBonus30=SettledBonus30+{3},SettledBonus40=SettledBonus40+{4},SettledBonus50=SettledBonus50+{5},SettledBonus60=SettledBonus60+{6},SettledBonus70=SettledBonus70+{7},SettledBonus80=SettledBonus80+{8},SettledBonus90=SettledBonus90+{9} where SysNo={10};", item.WalletAmount, (item.WalletAmount > 0 ? item.WalletAmount : 0),item.SettledBonus20, item.SettledBonus30, item.SettledBonus40, item.SettledBonus50, item.SettledBonus60, item.SettledBonus70, item.SettledBonus80, item.SettledBonus90, item.CustomerSysNo));
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
            sql += string.Format("where sysNo={0}",sysNo);
            return DBContext.Sql(sql).Execute();
        }

        /// <summary>
        /// 更新等级和减去升级金额
        /// </summary>
        public int UpdateGradeAndUpgradeFundAmount(CrCustomer model)
        {
            return DBContext.Sql(string.Format("update crcustomer set Grade={0},UpgradeFundAmount={1} where sysNo={2}", model.Grade,model.UpgradeFundAmount, model.SysNo)).Execute();
        }
    }
}
