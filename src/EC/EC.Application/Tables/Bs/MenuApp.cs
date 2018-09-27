using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using EC.DataAccess.Bs;
using EC.Entity;
using EC.Entity.Parameter.Request.Auth;
using EC.Entity.Tables.Bs;

namespace EC.Application.Tables.Bs
{
    using EC.Libraries.Core;

    /// <summary>
    /// 菜单业务层
    /// </summary>
    public class MenuApp : Base<MenuApp>
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        public BsMenu Get(int sysNo)
        {
            var result = Using<IBsMenu>().Get(sysNo);

            return result;
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="request">菜单</param>
        /// <param name="privileges">权限</param>
        public JResult Save(SaveOrUpdateMenuRequest request, List<int> privileges)
        {
            var response = new JResult()
            {
                Status = false,
                Message = "添加失败！"
            };

            using (var tran = new TransactionScope())
            {
                try
                {
                    var menu = new BsMenu()
                    {
                        SysNo = request.SysNo,
                        Name = request.Name,
                        ParentSysNo = request.ParentSysNo,
                        IsNav = request.IsNav,
                        Url = request.URL,
                        DisplayOrder = request.DisplayOrder,
                        Description = request.Description,
                        Status = request.Status
                    };

                    if (request.SysNo > 0)
                    {
                        //删除菜单权限
                        Using<IBsMenuPermission>().DeleteByMenuSysNo(request.SysNo);

                        if (privileges != null && privileges.Any())
                        {
                            privileges.ForEach(item =>
                            {
                                if (Using<IBsMenuPermission>().Insert(new BsMenuPermission()
                                {
                                    PermissionSysNo = item,
                                    MenuSysNo = request.SysNo,
                                }) <= 0)
                                {
                                    throw new Exception("添加菜单权限失败！");
                                }
                            });
                        }

                        var row = Using<IBsMenu>().Update(menu);
                        if (row > 0)
                        {
                            tran.Complete();
                            response.Status = true;
                            response.Message = "添加成功！";
                        }
                        else
                        {
                            throw new Exception("添加失败！");
                        }
                    }
                    else
                    {
                        var menuSysNo = Using<IBsMenu>().Insert(menu);
                        if (menuSysNo > 0)
                        {
                            if (privileges != null && privileges.Any())
                            {
                                privileges.ForEach(item =>
                                {
                                    if (Using<IBsMenuPermission>().Insert(new BsMenuPermission()
                                    {
                                        PermissionSysNo = item,
                                        MenuSysNo = menuSysNo,
                                    }) <= 0)
                                    {
                                        throw new Exception("添加菜单权限失败！");
                                    }
                                });
                            }

                            tran.Complete();
                            response.Status = true;
                            response.Message = "更新成功！";
                        }
                        else
                        {
                            throw new Exception("更新成功！");
                        }
                    }
                }
                catch (Exception ex)
                {
                    response.Message = ex.Message;
                }
                
            }
            

            return response;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">数据模型</param>
        public JResult Update(BsMenu model)
        {
            var response = new JResult()
            {
                Status = false,
                Message = "更新失败！"
            };

            try
            {
                var row = Using<IBsMenu>().Update(model);
                if (row > 0)
                {
                    response.Status = true;
                    response.Message = "更新成功！";
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }

        /// <summary>
        /// 获取菜单列表
        /// </summary>
        public List<BsMenu> GetMenuList()
        {
            var result = Using<IBsMenu>().GetMenuList();

            return result;
        }

        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <param name="sysNoList">系统编号列表</param>
        /// <returns>菜单列表</returns>
        public List<BsMenu> GetListBySysNoList(List<int> sysNoList)
        {
            var list = Using<IBsMenu>().GetListBySysNoList(sysNoList);

            return list.ToList();
        }
    }
}
