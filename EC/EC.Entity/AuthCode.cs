using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Entity
{
    /// <summary>
    /// 菜单权限
    /// </summary>
    public enum AuthCode
    {
        #region 空授权码

        /// <summary>
        /// 空授权码
        /// 使用此授权码时不进行登录和权限检查
        /// </summary>
        None,

        /// <summary>
        /// 忽略权限
        /// </summary>
        EmptyCode,

        #endregion

        #region 权限管理授权码

        /// <summary>
        /// 查看权限列表
        /// </summary>
        PE0001,

        /// <summary>
        /// 编辑权限信息
        /// </summary>
        PE0002,

        /// <summary>
        /// 权限管理启用/禁用
        /// </summary>
        PE0003,

        /// <summary>
        /// 权限管理删除
        /// </summary>
        PE0004,

        #endregion

        #region 用户管理授权码

        /// <summary>
        /// 查看用户信息列表
        /// </summary>
        US0001,

        /// <summary>
        /// 编辑用户信息
        /// </summary>
        US0002,

        /// <summary>
        /// 启用/禁用
        /// </summary>
        US0003,

        /// <summary>
        /// 用户管理重置密码
        /// </summary>
        US0004,

        #endregion

        #region 公告管理授权码

        /// <summary>
        /// 查看公告信息列表
        /// </summary>
        NT0001,

        /// <summary>
        /// 编辑公告信息
        /// </summary>
        NT0002,

        /// <summary>
        /// 启用/禁用
        /// </summary>
        NT0003,

        #endregion

        #region 码表管理授权码

        /// <summary>
        /// 查看码表信息列表
        /// </summary>
        CO0001,

        /// <summary>
        /// 编辑码表信息
        /// </summary>
        CO0002,

        #endregion

        #region 视频管理授权码
        /// <summary>
        /// 视频查看
        /// </summary>
        Video0001,
        /// <summary>
        /// 视频修改
        /// </summary>
        Video0002,
        /// <summary>
        /// 视频状态
        /// </summary>
        Video0003,
        #endregion

        #region 视频分类授权码
        /// <summary>
        /// 视频分类查看
        /// </summary>
        VideoCategory0001,
        /// <summary>
        /// 视频分类修改
        /// </summary>
        VideoCategory0002,
        /// <summary>
        /// 视频分类状态
        /// </summary>
        VideoCategory0003,
        #endregion
    }
}
