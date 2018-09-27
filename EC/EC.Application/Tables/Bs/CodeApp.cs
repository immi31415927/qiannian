using System;
using System.Collections.Generic;
using EC.DataAccess.Bs;
using EC.Entity;
using EC.Entity.Tables.Bs;

namespace EC.Application.Tables.Bs
{
    using EC.Libraries.Core;

    /// <summary>
    /// 码表业务层
    /// </summary>
    public class CodeApp : Base<CodeApp>
    {
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="codeList">子码表列表</param>
        /// <returns>执行结果</returns>
        public JResult BatchUpdate(List<BsCode> codeList)
        {
            var result = new JResult();

            try
            {
                if (Using<IBsCode>().BatchUpdate(codeList) <= 0)
                {
                    throw new Exception("批量更新失败");
                }

                result.Status = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        public BsCode Get(int sysNo)
        {
            var result = Using<IBsCode>().Get(sysNo);

            return result;
        }

        /// <summary>
        /// 查询码表通过类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>码表列表</returns>
        public BsCode GetByType(int type)
        {
            var result = Using<IBsCode>().GetByType(type);

            return result;
        }

        /// <summary>
        /// 查询码表列表通过类型列表
        /// </summary>
        /// <param name="list">类型列表</param>
        /// <returns>码表列表</returns>
        public IList<BsCode> GetListByTypeList(List<int> list)
        {
            var result = Using<IBsCode>().GetListByTypeList(list);

            return result;
        }
    }
}
