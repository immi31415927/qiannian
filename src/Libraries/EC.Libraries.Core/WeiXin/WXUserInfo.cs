
namespace EC.Libraries.Core.WeiXin
{
    public class WXUserInfo
    {
        /// <summary>
        /// 错误编码
        /// </summary>
        public string errcode { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string errmsg { get; set; }

        // 摘要: 
        //     用户所在城市
        public string city { get; set; }
        //
        // 摘要: 
        //     用户所在国家
        public string country { get; set; }
        //
        // 摘要: 
        //     用户所在的分组ID
        public int groupid { get; set; }
        //
        // 摘要: 
        //     用户头像，最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像），用户没有头像时该项为空。若用户更换头像，原有头像URL将失效。
        //     示例：http://wx.qlogo.cn/mmopen/g3MonUZtNHkdmzicIlibx6iaFqAc56vxLSUfpb6n5WKSYVY0ChQKkiaJSgQ1dZuTOgvLLrhJbERQQ4eMsv84eavHiaiceqxibJxCfHe/0
        public string headimgurl { get; set; }
        //
        // 摘要: 
        //     用户的语言，zh-CN 简体，zh-TW 繁体，en 英语，默认为zh-CN
        public string language { get; set; }
        //
        // 摘要: 
        //     用户的昵称
        public string nickname { get; set; }
        //
        // 摘要: 
        //     普通用户的标识，对当前公众号唯一
        public string openid { get; set; }
        //
        // 摘要: 
        //     用户所在省份
        public string province { get; set; }
        //
        // 摘要: 
        //     公众号运营者对粉丝的备注，公众号运营者可在微信公众平台用户管理界面对粉丝添加备注
        public string remark { get; set; }
        //
        // 摘要: 
        //     用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        public int sex { get; set; }
        //
        // 摘要: 
        //     用户是否订阅该公众号标识，值为0时，拉取不到其余信息
        public int subscribe { get; set; }
        //
        // 摘要: 
        //     用户关注时间，为时间戳。如果用户曾多次关注，则取最后关注时间
        public long subscribe_time { get; set; }
        //
        // 摘要: 
        //     只有在用户将公众号绑定到微信开放平台帐号后，才会出现该字段
        public string unionid { get; set; }
    }
}
