using System.Collections.Generic;
using EC.DataAccess.Bs;
using EC.Entity.Tables.Bs;

namespace EC.DataAccess.MySql.Bs
{
    using EC.Libraries.Core.Data;

    /// <summary>
    /// 菜单 数据访问接口实现
    /// </summary>
    public class BsMenuImpl : Base, IBsMenu
    {
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model">数据模型</param>
        public int Insert(BsMenu model)
        {
            return DBContext.Insert<BsMenu>("agent_bsmenu", model)
                            .AutoMap(x => x.SysNo)
                            .ExecuteReturnLastId<int>("sysNo");
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">数据模型</param>
        public int Update(BsMenu model)
        {
            return DBContext.Update<BsMenu>("agent_bsmenu", model)
                            .AutoMap(x => x.SysNo)
                            .Where(x => x.SysNo)
                            .Execute();
        }

        /// <summary>
        /// 获取菜单信息
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>菜单信息</returns>
        public BsMenu Get(int sysNo)
        {
            return DBContext.Sql("select * from agent_bsmenu where sysNo = @sysNo")
                            .Parameter("sysNo", sysNo)
                            .QuerySingle<BsMenu>();
        }

        /// <summary>
        /// 获取菜单列表
        /// </summary>
        public List<BsMenu> GetMenuList()
        {
            return DBContext.Sql("select * from agent_bsmenu")
                            .QueryMany<BsMenu>();
        }

        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <param name="sysNoList">系统编号列表</param>
        /// <returns>菜单列表</returns>
        public IList<BsMenu> GetListBySysNoList(List<int> sysNoList)
        {
            return DBContext.Sql("Select * From Agent_BsMenu Where SysNo in(@SysNos)")
                            .Parameter("SysNos", sysNoList.ToArray())
                            .QueryMany<BsMenu>();
        }
    }
}
