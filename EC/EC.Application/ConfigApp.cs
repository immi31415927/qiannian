using EC.Entity.View.CRM.Ext;
using EC.Libraries.Core;
using EC.Libraries.Util;

namespace EC.Application
{
    /// <summary>
    ///  配置
    /// </summary>
    public class ConfigApp : Base<ConfigApp>
    {
        /// <summary>
        /// 获取银行
        /// </summary>
        /// <returns>银行信息</returns>
        public BanksConfig GetBanksConfig()
        {
            return ConfigUtil.GetConfig<BanksConfig>("Banks.config");
        }
    }
}
