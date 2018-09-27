using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using EC.DataAccess.Bs;
using EC.Entity;
using EC.Entity.Enum;
using EC.Entity.Parameter.Request.Bs;
using EC.Entity.Tables.Bs;
using EC.Entity.View.Bs.Ext;
using EC.Libraries.Core;
using EC.Libraries.Core.Pager;
using EC.Libraries.Core.Transaction;
using EC.Libraries.Util;

namespace EC.Application.Tables.Bs
{
    /// <summary>
    /// 用户业务层
    /// </summary>
    public class UserApp : Base<UserApp>
    {
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>系统编号</returns>
        public JResult<int> Insert(BsUser model)
        {
            var result = new JResult<int>();

            try
            {
                model.PassWord = EncryptionUtil.EncryptWithMd5AndSalt(model.PassWord);
                model.CreatedDate = DateTime.Now;
                var row = Using<IBsUser>().Insert(model);

                if (row <= 0)
                {
                    throw new Exception("添加数据失败");
                }

                result.Status = true;
                result.Data = row;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>影响行数</returns>
        public JResult Update(BsUser model)
        {
            var result = new JResult();

            try
            {
                var row = Using<IBsUser>().Update(model);

                if (row <= 0)
                {
                    throw new Exception("修改数据失败");
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

                if (Using<IBsUser>().UpdateStatus(sysNo, status) <= 0)
                {
                    throw new Exception("修改数据失败");
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
        /// 重置密码
        /// </summary>
        /// <param name="sysNo">系统编码</param>
        /// <param name="passWord">密码</param>
        /// <returns>影响行数</returns>
        public JResult<string> ResetPassWord(int sysNo, string passWord)
        {
            var result = new JResult<string>();

            try
            {
                if (Using<IBsUser>().ResetPassWord(sysNo, EncryptionUtil.EncryptWithMd5AndSalt(passWord)) <= 0)
                {
                    throw new Exception("重置密码失败");
                }

                result.Data = passWord;
                result.Status = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 用户信息以及角色编辑
        /// </summary>
        /// <param name="model">用户扩展信息</param>
        /// <returns>返回参数</returns>
        public JResult UserEdit(UserExt model)
        {
            var result = new JResult();

            try
            {
                if (!EnumUtil.IsEnumExist<StatusEnum>(model.Status))
                {
                    throw new Exception("数据不合法");
                }

                if (Using<IBsUser>().IsRepeatOfAccount(model.SysNo, model.Account) > 0)
                {
                    throw new Exception("用户账号已存在");
                }

                var user = Mapper.Map<UserExt, BsUser>(model);

                using (var tran = new TransactionScope())
                {
                    try
                    {
                        if (user.SysNo <= 0)
                        {
                            var data = this.Insert(user);

                            if (!data.Status)
                            {
                                throw new Exception(data.Message);
                            }

                            user.SysNo = data.Data;
                        }
                        else
                        {
                            if (Using<IBsUser>().UpdateSection(user) <= 0)
                            {
                                throw new Exception("修改数据失败");
                            }
                        }

                        Using<IBsUserRole>().DeleteByUserSysNo(user.SysNo);

                        var userRoleList = new List<string>();

                        if (!string.IsNullOrWhiteSpace(model.RoleSelectedList))
                        {
                            userRoleList = model.RoleSelectedList.Split(',').ToList();
                        }

                        if (userRoleList.Any())
                        {
                            var list = userRoleList.Select(item => new BsUserRole()
                            {
                                UserSysNo = user.SysNo,
                                RoleSysNo = Convert.ToInt32(item),
                                CreatedBy = 0,
                                CreatedDate = DateTime.Now
                            }).ToList();

                            if (Using<IBsUserRole>().BatchInsert(list) <= 0)
                            {
                                throw new Exception("添加用户角色失败");
                            }
                        }

                        tran.Complete();
                        result.Status = true;
                    }
                    catch (Exception e)
                    {
                        tran.Dispose();
                        throw e;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 获取功能权限信息
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>功能权限信息</returns>
        public BsUser Get(int sysNo)
        {
            return Using<IBsUser>().Get(sysNo);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="account">用户账号</param>
        /// <returns>用户信息</returns>
        public BsUser GetUserByAccount(string account)
        {
            return Using<IBsUser>().GetUserByAccount(account);
        }

        /// <summary>
        /// 获取功能权限分页列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>功能权限分页列表</returns>
        public PagedList<BsUser> GetPagerList(UserRequest request)
        {
            return Using<IBsUser>().GetPagerList(request);
        }
    }
}
