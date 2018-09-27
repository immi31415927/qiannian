namespace EC.Libraries.WeiXin
{
    /// <summary>
    /// 支付类型
    /// </summary>
    public enum trade_type
    {
        JSAPI = 0,
        NATIVE = 1,
        APP = 2,
        MWEB = 3,
    }

    /// <summary>
    /// 返回状态码
    /// </summary>
    public enum return_code
    {
        SUCCESS = 1,
        FAIL = 0,
    }

    /// <summary>
    /// 业务结果
    /// </summary>
    public enum result_code
    {
        SUCCESS = 1,
        FAIL = 0,
    }

    /// <summary>
    /// 银行编号
    /// </summary>
    public enum bankId
    {
        工商银行 = 1002,
        农业银行 = 1005,
        中国银行 = 1005,
        建设银行 = 1005,
        招商银行 = 1005,
        邮储银行 = 1005,
        交通银行 = 1005,
        浦发银行 = 1005,
        兴业银行 = 1005,
        平安银行 = 1005,
        中信银行 = 1005,
        华夏银行 = 1005,
        广发银行 = 1005,
        光大银行 = 1005,
        北京银行 = 1005,
        宁波银行 = 1005
    }
}
