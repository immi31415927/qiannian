using System.Collections.Generic;
using EC.Libraries.Core.Transaction;

namespace EC.Libraries.Core.Data
{
    public class Base
    {
        /// <summary>
        /// 数据访问上下文
        /// </summary>
        public IDbContext DBContext
        {
            get { return Db.CreateDbContext(); }
        }

        ///<summary>
        ///设置查询条件参数
        ///</summary>
        protected void SetWhereParameter<T>(ISelectBuilder<T> select, Dictionary<string, object> paras)
        {
            if (paras != null)
            {
                foreach (string key in paras.Keys)
                {
                    select.Parameter(key, paras[key]);
                }
            }
        }
    
    }
}