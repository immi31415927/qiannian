using System.Collections.Generic;
using EC.Entity.Tables.Bs;

namespace EC.DataAccess.Bs
{
    /// <summary>
    /// 码表数据访问接口
    /// </summary>
    public interface IBsCode
    {
        /// <summary>
        /// 批量更新子码表值
        /// </summary>
        /// <param name="codeList">子码表列表</param>
        /// <returns>影响行数</returns>
        int BatchUpdate(List<BsCode> codeList);

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        BsCode Get(int sysNo);

        /// <summary>
        /// 查询码表通过类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>码表列表</returns>
        BsCode GetByType(int type);

        /// <summary>
        /// 查询码表列表通过类型列表
        /// </summary>
        /// <param name="list">类型列表</param>
        /// <returns>码表列表</returns>
        IList<BsCode> GetListByTypeList(List<int> list);
    }
}
