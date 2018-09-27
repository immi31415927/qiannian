using System.Collections.Generic;
using EC.Entity.Tables.Bs;

namespace EC.DataAccess.Bs
{
    /// <summary>
    /// 菜单数据访问接口
    /// </summary>
    public interface IBsMenu
    {
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model">数据模型</param>
        int Insert(BsMenu model);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">数据模型</param>
        int Update(BsMenu model);

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        BsMenu Get(int sysNo);

        /// <summary>
        /// 获取菜单列表
        /// </summary>
        List<BsMenu> GetMenuList();

        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <param name="sysNoList">系统编号列表</param>
        /// <returns>菜单列表</returns>
        IList<BsMenu> GetListBySysNoList(List<int> sysNoList);
    }
}
