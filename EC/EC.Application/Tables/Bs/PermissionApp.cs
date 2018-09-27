using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EC.DataAccess.Bs;
using EC.Entity;
using EC.Entity.Enum;
using EC.Entity.Parameter.Request.Bs;
using EC.Entity.Tables.Bs;
using EC.Libraries.Core;
using EC.Libraries.Core.Pager;
using EC.Libraries.Util;

namespace EC.Application.Tables.Bs
{
    /// <summary>
    /// 功能权限业务层
    /// </summary>
    public class PermissionApp : Base<PermissionApp>
    {
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>系统编号</returns>
        public JResult<int> Insert(BsPermission model)
        {
            var result = new JResult<int>();

            try
            {
                model.CreatedDate = DateTime.Now;
                var row = Using<IBsPermission>().Insert(model);

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
        public JResult Update(BsPermission model)
        {
            var result = new JResult();

            try
            {
                var row = Using<IBsPermission>().Update(model);

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
        /// 修改部分数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>影响行数</returns>
        public JResult UpdateSection(BsPermission model)
        {
            var result = new JResult();

            try
            {
                var row = Using<IBsPermission>().UpdateSection(model);

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

                if (Using<IBsPermission>().UpdateStatus(sysNo, status) <= 0)
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
        /// 删除
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>影响行数</returns>
        public JResult Delete(int sysNo)
        {
            var result = new JResult();

            try
            {
                if (Using<IBsPermission>().Delete(sysNo) <= 0)
                {
                    throw new Exception("删除数据失败");
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
        /// 获取功能权限信息
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>功能权限信息</returns>
        public BsPermission Get(int sysNo)
        {
            return Using<IBsPermission>().Get(sysNo);
        }

        /// <summary>
        /// 权限菜单编号获取菜单权限列表
        /// </summary>
        /// <param name="menuSysNo">菜单编号</param>
        /// <returns>列表</returns>
        public List<BsPermission> GetByMenuSysNo(int menuSysNo)
        {
            return Using<IBsPermission>().GetByMenuSysNo(menuSysNo);
        }

        /// <summary>
        /// 获取功能权限列表
        /// </summary>
        /// <param name="sysNoList">系统编号列表</param>
        /// <returns>功能权限列表</returns>
        public List<BsPermission> GetListBySysNoList(List<int> sysNoList)
        {
            var list = Using<IBsPermission>().GetListBySysNoList(sysNoList);

            return list.ToList();
        }

        /// <summary>
        /// 菜单功能权限分页列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>功能权限分页列表</returns>
        public PagedList<BsPermission> GetMenuPermissionPagerList(PermissionRequest request)
        {
            return Using<IBsPermission>().GetMenuPermissionPagerList(request);
        }

        /// <summary>
        /// 获取功能权限分页列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>功能权限分页列表</returns>
        public PagedList<BsPermission> GetPagerList(PermissionRequest request)
        {
            return Using<IBsPermission>().GetPagerList(request);
        }
    }
}
