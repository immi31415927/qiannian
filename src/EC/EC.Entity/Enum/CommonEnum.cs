using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Entity.Enum
{
    /// <summary>
    /// 步长
    /// </summary>
    public enum Step
    {
        步长 = 10
    }

    /// <summary>
    /// 系统状态枚举
    /// </summary>
    public enum StatusEnum
    {
        禁用 = 0,
        启用 = 1,
    }

    /// <summary>
    /// 性别枚举
    /// </summary>
    public enum GenderEnum
    {
        未知 = 0,
        男 = 1,
        女 = 2,
    }

    /// <summary>
    /// 操作人类型枚举
    /// </summary>
    public enum OperatorTypeEnum
    {
        会员 = 10,
        管理员 = 20
    }

    /// <summary>
    /// 错误枚举
    /// </summary>
    public enum ErrorEnum
    {
        参数为空 = 1,
        余额不足 = 2
    }

    /// <summary>
    /// 销售单支付状态
    /// 字段:PayStatus
    /// </summary>
    public enum PayStatus
    {
        未支付 = 10,
        已支付 = 20,
        支付异常 = 30
    }
}
