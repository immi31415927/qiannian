using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Entity.Enum
{
    public class StockRecordEnum
    {
        /// <summary>
        /// 投资类型枚举
        /// </summary>
        public enum TypeEnum
        {
            股权挂售 = 10,
            平台回购 = 20,
            股权申购 = 30
        }

        /// <summary>
        /// 股权记录状态枚举
        /// </summary>
        public enum StatusEnum
        {
            待处理 = 10, 
            部分处理 = 20, 
            已处理 = 30
        }
    }
}
