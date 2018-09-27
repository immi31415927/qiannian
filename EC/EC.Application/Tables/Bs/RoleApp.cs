using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using EC.DataAccess.Bs;
using EC.Entity;
using EC.Entity.Enum;
using EC.Entity.Parameter.Request.Auth;
using EC.Entity.Parameter.Request.Bs;
using EC.Entity.Tables.Bs;
using EC.Entity.View.Bs;
using EC.Libraries.Util;

namespace EC.Application.Tables.Bs
{
    using EC.Libraries.Core;
    using EC.Libraries.Core.Pager;

    /// <summary>
    /// 角色业务层
    /// </summary>
    public class RoleApp : Base<RoleApp>
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        public BsRole Get(int sysNo)
        {
            var result = Using<IBsRole>().Get(sysNo);

            return result;
        }

        /// <summary>
        /// 获取角色菜单权限树
        /// </summary>
        /// <param name="roleSysNo">角色编号</param>
        /// <returns>角色菜单权限树列表</returns>
        public List<ZCheckTreeNode> GetMenuPermissionViewListByRoleSysNo(int roleSysNo)
        {
            //菜单列表
            var menuList = Using<IBsMenu>().GetMenuList();
            //菜单权限列表
            var menuPermissionViewList = Using<IBsMenuPermission>().GetMenuPermissionViewList();
            //获取已选择菜单权限列表
            var permissionList = Using<IBsAuthorize>().GetPermissionListRoleSysNo(roleSysNo);

            var zCheckTreeNodeList = new List<ZCheckTreeNode>();

            menuList.ForEach(item =>
            {
                //菜单
                var zCheckTreeNode = new ZCheckTreeNode();
                zCheckTreeNode.id = string.Format("m_{0}", item.SysNo);
                zCheckTreeNode.pId = string.Format("m_{0}", item.ParentSysNo);
                zCheckTreeNode.name = item.Name;
                zCheckTreeNode.nodetype = AuthorizeEnum.AuthorizeType.菜单.GetHashCode();
                zCheckTreeNode.@checked = permissionList.Any(authorizeMenuItem => authorizeMenuItem.AuthorizeType.Equals(AuthorizeEnum.AuthorizeType.菜单.GetHashCode()) && authorizeMenuItem.AuthorizeSysNo.Equals(item.SysNo));
                zCheckTreeNodeList.Add(zCheckTreeNode);
                //菜单权限
                var menuPermissionList = menuPermissionViewList.Where(menuItem => menuItem.MenuSysNo.Equals(item.SysNo)).ToList();
                if (menuPermissionList != null && menuPermissionList.Any())
                {
                    menuPermissionList.ForEach(p =>
                    {
                        var childZCheckTreeNode = new ZCheckTreeNode();
                        childZCheckTreeNode.id = string.Format("p_{0}", p.SysNo);
                        childZCheckTreeNode.pId = string.Format("m_{0}", item.SysNo);
                        childZCheckTreeNode.name = p.Name;
                        childZCheckTreeNode.nodetype = AuthorizeEnum.AuthorizeType.权限.GetHashCode();
                        childZCheckTreeNode.@checked = permissionList.Any(authorizePermissionItem => authorizePermissionItem.AuthorizeType.Equals(AuthorizeEnum.AuthorizeType.权限.GetHashCode()) && authorizePermissionItem.AuthorizeSysNo.Equals(p.SysNo));
                        zCheckTreeNodeList.Add(childZCheckTreeNode);
                    });
                }
            });
            return zCheckTreeNodeList;
        }

        /// <summary>
        /// 保存角色
        /// </summary>
        /// <param name="role">角色</param>
        /// <param name="authorize">菜单、权限列表</param>
        /// <returns>JResult</returns>
        public JResult SaveRole(RoleSaveRequest role, List<AuthorizeRequest> authorizeList)
        {
            var response = new JResult();

            if (role.SysNo > 0)
            {
                using (var tran = new TransactionScope())
                {
                    try
                    {
                        var model = new BsRole()
                        {
                            SysNo = role.SysNo,
                            Name = role.Name,
                            Description = role.Description,
                            Status = role.Status,
                            CreatedBy = 0,
                            CreatedDate = DateTime.Now
                        };
                        //删除角色授权信息
                        Using<IBsAuthorize>().DeleteByRoleSysNo(role.SysNo);
                        //批量插入授权信息
                        if (authorizeList != null && authorizeList.Any())
                        {
                            authorizeList.ForEach(item =>
                            {
                                Using<IBsAuthorize>().Insert(new BsAuthorize()
                                {
                                    RoleSysNo = item.RoleSysNo.Value,
                                    AuthorizeSysNo = item.AuthorizeSysNo.Value,
                                    AuthorizeType = item.AuthorizeType.Value
                                });
                            });
                        }
                        //修改角色
                        Using<IBsRole>().Update(model);

                        response.Status = true;
                        response.Message = "操作成功！";
                        //提交事务
                        tran.Complete();
                    }
                    catch (Exception ex)
                    {
                        response.Message = ex.Message;
                    }
                }
            }
            else
            {
                using (var tran = new TransactionScope())
                {
                    try
                    {
                        var model = new BsRole()
                        {
                            Name = role.Name,
                            Description = role.Description,
                            Status = role.Status
                        };
                        //添加角色
                        var roleSysNo = Using<IBsRole>().Insert(model);
                        if (roleSysNo > 0)
                        {
                            //批量插入授权信息
                            if (authorizeList != null && authorizeList.Any())
                            {
                                authorizeList.ForEach(item =>
                                {
                                    Using<IBsAuthorize>().Insert(new BsAuthorize()
                                    {
                                        RoleSysNo = roleSysNo,
                                        AuthorizeSysNo = item.AuthorizeSysNo.Value,
                                        AuthorizeType = item.AuthorizeType.Value
                                    });
                                });
                            }
                        }

                        response.Status = true;
                        response.Message = "操作成功！";
                        //提交事务
                        tran.Complete();
                    }
                    catch (Exception ex)
                    {
                        response.Message = ex.Message;
                    }
                }
            }
            return response;
        }

        /// <summary>
        /// 获取角色页面列表
        /// </summary>
        /// <param name="userSysNo">用户编号</param>
        /// <returns>角色页面列表</returns>
        public List<RoleView> GetRoleViewList(int? userSysNo)
        {
            var list = Using<IBsRole>().GetList(new RoleQueryRequeest()
            {
                Status = StatusEnum.启用.GetHashCode()
            });
            var selectedList = new List<BsUserRole>();

            if (userSysNo.HasValue)
            {
                selectedList = Using<IBsUserRole>().GetListByUserSysNo(userSysNo.Value).ToList();
            }

            var roleView = list.Select(item => new RoleView()
            {
                SysNo = item.SysNo, Name = item.Name, Selected = selectedList.Any(p => p.RoleSysNo == item.SysNo)
            }).ToList();

            return roleView;
        }

        /// <summary>
        /// 获取角色列表
        /// </summary>
        public PagedList<BsRole> GetPagingList(RoleQueryRequeest requeest)
        {
            var result = Using<IBsRole>().GetPagingList(requeest);

            return result;
        }

        /// <summary>
        /// 获取角色列表
        /// </summary>
        public PagedList<BsRole> GetPaging(RoleQueryRequeest requeest)
        {
            var result = Using<IBsRole>().GetPaging(requeest);

            return result;
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        public BsRole GetRole(int sysNo)
        {
            var result = Using<IBsRole>().Get(sysNo);

            return result;
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="sysNo">编码</param>
        /// <param name="status">状态</param>
        /// <returns>影响行数</returns>
        public JResult UpdateStatus(int sysNo, int status)
        {
            var result = new JResult();

            try
            {
                if (!EnumUtil.IsEnumExist<StatusEnum>(status))
                {
                    throw new Exception("数据不合法");
                }

                if (Using<IBsRole>().UpdateStatus(sysNo, status) <= 0)
                {
                    throw new Exception("修改数据失败");
                }

                result.Message = "操作成功！";
                result.Status = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }
    }
}
