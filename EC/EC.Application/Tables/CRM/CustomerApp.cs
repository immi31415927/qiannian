using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using AutoMapper;
using EC.Application.Tables.Bs;
using EC.Application.Tables.WeiXin;
using EC.DataAccess.Bs;
using EC.DataAccess.CRM;
using EC.DataAccess.Fn;
using EC.Entity;
using EC.Entity.Enum;
using EC.Entity.Output.Fore;
using EC.Entity.Parameter.Request.Fn;
using EC.Entity.Parameter.Request.WeiXin;
using EC.Entity.Parameter.Response.CRM;
using EC.Entity.Tables.CRM;
using EC.Entity.Parameter.Request.CRM;
using EC.Entity.Parameter.Request.Auth;
using EC.Entity.Tables.Finance;
using EC.Entity.View.CRM;
using EC.Entity.View.CRM.Ext;
using EC.Libraries.Core.Extension;
using EC.Libraries.Core.Pager;
using EC.Libraries.Core.Transaction;

namespace EC.Application.Tables.CRM
{
    using EC.Libraries.Core;
    using EC.Libraries.Util;
    using EC.Application.Tables.Fn;

    /// <summary>
    /// 会员 业务层
    /// </summary>
    public class CustomerApp : Base<CustomerApp>
    {
        #region 获取
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        public CrCustomer Get(int sysNo)
        {
            var result = Using<ICrCustomer>().Get(sysNo);

            return result;
        }

        /// <summary>
        /// 根据账号获取用户对象
        /// </summary>
        public CrCustomer GetByAccount(string account)
        {
            return Using<ICrCustomer>().GetByAccount(account);
        }

        /// <summary>
        /// 根据手机号获取用户对象
        /// </summary>
        public CrCustomer GetByPhoneNumber(string phoneNumber)
        {
            return Using<ICrCustomer>().GetByPhoneNumber(phoneNumber);
        }

        /// <summary>
        /// 根据openId获取用户
        /// </summary>
        /// <param name="openId">微信OpenId</param>
        public CrCustomer GetByOpenId(string openId)
        {
            return Using<ICrCustomer>().GetByOpenId(openId);
        }

        /// <summary>
        /// 获取用户视图实体
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>用户视图实体</returns>
        public CustomerView GetCustomerView(int sysNo)
        {
            CustomerView entity = null;

            var model = Using<ICrCustomer>().Get(sysNo);

            if (model != null)
            {
                entity = model.MapTo<CustomerView>();
            }

            return entity;
        }
        #endregion

        #region 用户注册

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="request">注册请求参数</param>
        public JResult<string> Register(RegisterRequest request)
        {
            var response = new JResult<string>()
            {
                Status = false,
                Message = "注册失败,请稍后重试！"
            };
            using (var tran = new TransactionProvider())
            {
                try
                {
                    //推荐人
                    var referrerSysNo = request.ReferrerSysNo > 0 ? request.ReferrerSysNo : 0;

                    var model = new CrCustomer()
                    {
                        Account = request.PhoneNumber,
                        Password = EncryptionUtil.EncryptWithMd5AndSalt(request.Password),
                        SerialNumber = GetRand(),
                        OpenId = request.OpenId,
                        ReferrerSysNo = referrerSysNo,
                        PhoneNumber = request.PhoneNumber,
                        RealName = request.RealName,
                        Nickname = request.Nickname,
                        HeadImgUrl = request.HeadImgUrl,
                        IDNumber = "",
                        TeamCount = 0,
                        Grade = CustomerEnum.Grade.普通会员.GetHashCode(),
                        Level = 0,
                        LevelCustomerStr = "",
                        Bank = 0,
                        BankNumber = "",
                        WalletAmount = 0,
                        HistoryWalletAmount = 0,
                        UpgradeFundAmount = 0,
                        RechargeTotalAmount = 0,
                        GeneralBonus = 0,
                        AreaBonus = 0,
                        GlobalBonus = 0,
                        ExpiresTime = "",
                        FollowDate = "",
                        RegisterIP = WebUtil.GetUserIp(),
                        Status = StatusEnum.启用.GetHashCode(),
                        RegisterDate = DateTime.Now,
                        CreatedDate = DateTime.Now
                    };

                    if (referrerSysNo > 0)
                    {
                        var customerExtParent = Using<ICrCustomer>().Get(referrerSysNo);
                        if (customerExtParent == null)
                        {
                            throw new Exception("推荐人不存在!");
                        }
                        //层级
                        model.Level = customerExtParent.Level + 1;

                        //会员扩展列表
                        var customerExtList = Using<ICrCustomer>().GetList(new CustomerExtRequest() { });
                        //获取当前会员上层所有推荐编号
                        var referrerIds = GetReferrer(customerExtList, new List<int>(), referrerSysNo);
                        if (referrerIds.Count <= 0)
                        {
                            throw new Exception("推荐人数计算错误!");
                        }
                        //更新团队人数
                        if (Using<ICrCustomer>().UpdateTeamCount(referrerIds) < 0)
                        {
                            throw new Exception("更新更新团队人数失败!");
                        }
                        //更新所有推荐的层级字符串
                        model.LevelCustomerStr = string.Join(",", referrerIds.OrderBy(p => p).ToList());
                    }
                    //关注
                    var recommend = RecommendApp.Instance.GetByopenId(request.OpenId);
                    if (recommend != null)
                    {
                        model.FollowDate = recommend.CreatedDate.ToString("yyyy-MM-dd");
                    }
                    var customerExtSysNo = Using<ICrCustomer>().Register(model);
                    if (customerExtSysNo <= 0)
                    {
                        throw new Exception("创建创建扩展注册失败、请联系管理员!");
                    }
                    response.Status = true;
                    response.Message = "注册成功、请登录！";

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    response.Message = ex.Message;
                }
                finally
                {
                    tran.Dispose();
                }
            }



            return response;
        }
        #endregion

        #region 绑定推荐人

        /// <summary>
        /// 绑定推荐人
        /// </summary>
        /// <param name="request">参数</param>
        public JResult SingleSync(SingleSyncRequest request)
        {
            var response = new JResult()
            {
                Status = false,
            };

            try
            {
                //会员
                var customer = Using<ICrCustomer>().Get(request.CustomerSysNo);
                if (customer != null)
                {
                    //会员扩展
                    var customerExt = Using<ICrCustomer>().Get(request.CustomerSysNo);
                    if (customerExt == null)
                    {
                        #region 会员扩展表
                        //var model = new CrCustomerExt()
                        //{
                        //    CustomerSysNo = request.CustomerSysNo,
                        //    ReferrerSysNo = 0,
                        //    RealName = "",
                        //    Email = "",
                        //    HeadImgUrl = "",
                        //    IDNumber = "",
                        //    TeamCount = 0,
                        //    Grade = CustomerEnum.Grade.普通会员.GetHashCode(),
                        //    Level = 0,
                        //    LevelCustomerStr = "",
                        //    Bank = 0,
                        //    BankNumber = "",
                        //    WalletAmount = 0,
                        //    HistoryWalletAmount = 0,
                        //    UpgradeFundAmount = 0,
                        //    GeneralBonus = 0,
                        //    AreaBonus = 0,
                        //    GlobalBonus = 0,
                        //    Expires = CustomerEnum.Expires.已过期.GetHashCode(),
                        //    CreatedDate = DateTime.Now
                        //};

                        //if (customer.suid > 0)
                        //{
                        //    #region
                        //    model.ReferrerSysNo = customer.suid;
                        //    //会员推荐人
                        //    var customerExtParent = Using<ICrCustomerExt>().GetByCustomerSysNo(customer.suid);
                        //    if (customerExtParent == null)
                        //    {
                        //        throw new Exception("推荐人不存在!");
                        //    }
                        //    //层级
                        //    model.Level = customerExtParent.Level + 1;
                        //    //会员扩展列表
                        //    var customerExtList = Using<ICrCustomerExt>().GetList(new CustomerExtRequest() { });
                        //    //获取当前会员上层所有推荐编号
                        //    var referrerIds = CustomerExtApp.Instance.GetReferrer(customerExtList, new List<int>(),
                        //        customer.suid);
                        //    if (referrerIds.Count <= 0)
                        //    {
                        //        throw new Exception("推荐人数计算错误!");
                        //    }
                        //    //更新团队人数
                        //    if (Using<ICrCustomerExt>().UpdateTeamCount(referrerIds) < 0)
                        //    {
                        //        throw new Exception("更新更新团队人数失败!");
                        //    }
                        //    //更新所有推荐的层级字符串
                        //    if (Using<ICrCustomerExt>().UpdateLevelStr(referrerIds, request.CustomerSysNo) < 0)
                        //    {
                        //        throw new Exception("更新推荐的层级字符串失败!");
                        //    }
                        //    //更新扩展表
                        //    var customerExtSysNo = Using<ICrCustomerExt>().Register(model);
                        //    if (customerExtSysNo <= 0)
                        //    {
                        //        throw new Exception("创建创建扩展注册失败、请联系管理员!");
                        //    }
                        //    #endregion
                        //}
                        //else
                        //{
                        //    #region
                        //    model.ReferrerSysNo = request.ReferrerSysNo;
                        //    //会员推荐人
                        //    var customerExtParent = Using<ICrCustomerExt>().GetByCustomerSysNo(request.ReferrerSysNo);
                        //    if (customerExtParent == null)
                        //    {
                        //        throw new Exception("推荐人不存在!");
                        //    }
                        //    //层级
                        //    model.Level = customerExtParent.Level + 1;
                        //    //更新扩展表
                        //    var customerExtSysNo = Using<ICrCustomerExt>().Register(model);
                        //    if (customerExtSysNo <= 0)
                        //    {
                        //        throw new Exception("创建创建扩展注册失败、请联系管理员!");
                        //    }
                        //    //会员扩展列表
                        //    var customerExtList = Using<ICrCustomerExt>().GetList(new CustomerExtRequest() { });
                        //    //获取当前会员上层所有推荐编号
                        //    var referrerIds = CustomerExtApp.Instance.GetReferrer(customerExtList, new List<int>(), request.ReferrerSysNo);
                        //    if (referrerIds.Count <= 0)
                        //    {
                        //        throw new Exception("推荐人数计算错误!");
                        //    }
                        //    //更新团队人数
                        //    if (Using<ICrCustomerExt>().UpdateTeamCount(referrerIds) < 0)
                        //    {
                        //        throw new Exception("更新更新团队人数失败!");
                        //    }
                        //    //更新所有推荐的层级字符串
                        //    if (Using<ICrCustomerExt>().UpdateLevelStr(referrerIds, request.CustomerSysNo) < 0)
                        //    {
                        //        throw new Exception("更新推荐的层级字符串失败!");
                        //    }
                        //}
                        #endregion
                    }
                    else
                    {
                        #region 更新会员扩展表
                        //if (customerExt.ReferrerSysNo <= 0)
                        //{
                        //    //会员推荐人
                        //    var customerExtParent = Using<ICrCustomerExt>().GetByCustomerSysNo(request.ReferrerSysNo);
                        //    if (customerExtParent == null)
                        //    {
                        //        throw new Exception("推荐人不存在!");
                        //    }
                        //    //层级
                        //    customerExt.Level = customerExtParent.Level + 1;
                        //    customerExt.ReferrerSysNo = request.ReferrerSysNo;
                        //    Using<ICrCustomerExt>().UpdateLevelAndReferrerSysNo(customerExt);
                        //    //会员扩展列表
                        //    var customerExtList = Using<ICrCustomerExt>().GetList(new CustomerExtRequest() { });
                        //    //获取当前会员上层所有推荐编号
                        //    var referrerIds = CustomerExtApp.Instance.GetReferrer(customerExtList, new List<int>(), request.ReferrerSysNo);
                        //    if (referrerIds.Count <= 0)
                        //    {
                        //        throw new Exception("推荐人数计算错误!");
                        //    }
                        //    //更新团队人数
                        //    if (Using<ICrCustomerExt>().UpdateTeamCount(referrerIds) < 0)
                        //    {
                        //        throw new Exception("更新更新团队人数失败!");
                        //    }
                        //    //更新所有推荐的层级字符串
                        //    if (Using<ICrCustomerExt>().UpdateLevelStr(referrerIds, request.CustomerSysNo) < 0)
                        //    {
                        //        throw new Exception("更新推荐的层级字符串失败!");
                        //    }
                        //}
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }
        #endregion

        #region 用户登录

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="request">登录请求参数</param>
        public JResult<CrCustomer> Login(LoginRequest request)
        {
            var response = new JResult<CrCustomer>()
            {
                Status = false,
                Message = "登录失败,请稍后重试！"
            };

            try
            {
                var customer = Using<ICrCustomer>().GetByAccount(request.Account);

                if (customer != null && EncryptionUtil.VerifyCiphetrextWithMd5AndSalt(request.Password, customer.Password))
                {
                    if (customer.Status == StatusEnum.启用.GetHashCode())
                    {
                        #region 更新用户登录信息

                        Using<ICrCustomer>().UpdateLogin(new CrCustomer
                        {
                            SysNo = customer.SysNo,
                            LastLoginIP = WebUtil.GetUserIp(),
                            LastLoginDate = DateTime.Now,
                            LoginCount = customer.LoginCount + 1
                        });

                        #endregion

                        #region 认证信息

                        AuthApp.Instance.Ticket(new TicketRequest()
                        {
                            SysNo = customer.SysNo,
                            Account = customer.Account,
                            ReferrerSysNo = customer.ReferrerSysNo,
                            SerialNumber = customer.SerialNumber,
                            PhoneNumber = customer.PhoneNumber,
                            RealName = customer.RealName,
                            Nickname = customer.Nickname,
                            HeadImgUrl = customer.HeadImgUrl,
                            Grade = customer.Grade,
                            FollowDate = customer.FollowDate,
                            ExpiresTime = customer.ExpiresTime
                        }, true);

                        #endregion

                        response.Status = true;
                        response.Message = "登录成功！";
                    }
                    else
                    {
                        response.Message = "您的账户已被系统锁定，请联系客服！";
                    }
                }
                else
                {
                    response.Status = false;
                    response.Message = "账户名或密码错误";
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }

        /// <summary>
        /// 微信OpenId登录
        /// </summary>
        /// <param name="request">请求参数</param>
        public JResult<CrCustomer> OpenIdLogin(LoginRequest request)
        {
            var response = new JResult<CrCustomer>()
            {
                Status = false,
                Message = "登录失败,请稍后重试！"
            };

            try
            {
                var customer = Using<ICrCustomer>().GetByOpenId(request.OpenId);
                if (customer != null)
                {
                    if (customer.Status == StatusEnum.启用.GetHashCode())
                    {
                        #region 更新用户登录信息
                        var updateCustomer = new CrCustomer();
                        updateCustomer.SysNo = customer.SysNo;
                        updateCustomer.LastLoginIP = WebUtil.GetUserIp();
                        updateCustomer.LastLoginDate = DateTime.Now;
                        updateCustomer.LoginCount++;
                        Using<ICrCustomer>().UpdateLogin(updateCustomer);
                        #endregion

                        #region 认证信息
                        AuthApp.Instance.Ticket(new TicketRequest()
                        {
                            SysNo = customer.SysNo,
                            Account = customer.Account,
                            ReferrerSysNo = customer.ReferrerSysNo,
                            SerialNumber = customer.SerialNumber,
                            PhoneNumber = customer.PhoneNumber,
                            RealName = customer.RealName,
                            Nickname = customer.Nickname,
                            HeadImgUrl = customer.HeadImgUrl,
                            Grade = customer.Grade,
                            FollowDate = customer.FollowDate,
                            ExpiresTime = customer.ExpiresTime
                        }, true);
                        #endregion

                        response.Status = true;
                        response.Message = "登录成功！";
                    }
                    else
                    {
                        response.Message = "您的账户已被系统锁定，请联系客服！";
                    }
                }
                else
                {
                    response.Message = "用户不存在";
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }


        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="request">请求参数</param>
        public JResult<CrCustomer> AccountLogin(LoginRequest request)
        {
            var response = new JResult<CrCustomer>()
            {
                Status = false,
                Message = "登录失败,请稍后重试！"
            };

            try
            {
                #region 自动登录
                var customer = Using<ICrCustomer>().GetByAccount(request.Account);
                if (customer != null)
                {
                    #region 更新用户登录信息
                    var updateCustomer = new CrCustomer();
                    updateCustomer.SysNo = customer.SysNo;
                    updateCustomer.LastLoginIP = WebUtil.GetUserIp();
                    updateCustomer.LastLoginDate = DateTime.Now;
                    updateCustomer.LoginCount++;
                    Using<ICrCustomer>().UpdateLogin(updateCustomer);
                    #endregion

                    #region 认证信息
                    AuthApp.Instance.Ticket(new TicketRequest()
                    {
                        SysNo = customer.SysNo,
                        Account = customer.Account,
                        ReferrerSysNo = customer.ReferrerSysNo,
                        SerialNumber = customer.SerialNumber,
                        PhoneNumber = customer.PhoneNumber,
                        RealName = customer.RealName,
                        Nickname = customer.Nickname,
                        HeadImgUrl = customer.HeadImgUrl,
                        Grade = customer.Grade,
                        FollowDate = customer.FollowDate,
                        ExpiresTime = customer.ExpiresTime
                    }, true);
                    #endregion

                    response.Status = true;
                }
                else
                {
                    response.Message = "用户不存在";
                }
                #endregion
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// 绑定用户
        /// </summary>
        /// <param name="request">参数</param>
        public JResult<CrCustomer> MemberBind(LoginRequest request)
        {
            var response = new JResult<CrCustomer>()
            {
                Status = false,
                Message = "登录失败,请稍后重试！"
            };

            try
            {
                var customer = Using<ICrCustomer>().GetByAccount(request.Account);
                if (customer != null && EncryptionUtil.VerifyCiphetrextWithMd5AndSalt(request.Password, customer.Password))
                {
                    if (customer.Status == StatusEnum.启用.GetHashCode())
                    {
                        #region 更新微信基本信息
                        if (Using<ICrCustomer>().MemberBindUpdate(new CustomerExtRequest()
                        {
                            SysNo = customer.SysNo,
                            OpenId = request.OpenId,
                            Nickname = request.Nickname,
                            HeadImgUrl = request.HeadImgUrl
                        }) <= 0)
                        {
                            response.Status = false;
                            response.Message = "绑定用户失败";
                        }
                        else
                        {
                            response.Status = true;
                        }
                        #endregion

                        #region 更新用户登录信息
                        var updateCustomer = new CrCustomer();
                        updateCustomer.SysNo = customer.SysNo;
                        updateCustomer.LastLoginIP = WebUtil.GetUserIp();
                        updateCustomer.LastLoginDate = DateTime.Now;
                        updateCustomer.LoginCount++;
                        Using<ICrCustomer>().UpdateLogin(updateCustomer);
                        #endregion

                        #region 认证信息
                        AuthApp.Instance.Ticket(new TicketRequest()
                        {
                            SysNo = customer.SysNo,
                            Account = customer.Account,
                            ReferrerSysNo = customer.ReferrerSysNo,
                            SerialNumber = customer.SerialNumber,
                            PhoneNumber = customer.PhoneNumber,
                            RealName = customer.RealName,
                            Nickname = customer.Nickname,
                            HeadImgUrl = customer.HeadImgUrl,
                            Grade = customer.Grade,
                            FollowDate = customer.FollowDate,
                            ExpiresTime = customer.ExpiresTime
                        }, true);
                        #endregion

                        response.Status = true;
                        response.Message = "登录成功！";
                    }
                    else
                    {
                        response.Message = "您的账户已被系统锁定，请联系客服！";
                    }
                }
                else
                {
                    response.Status = false;
                    response.Message = "账户名或密码错误";
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }

        #endregion

        /// <summary>
        /// 新升级代码
        /// </summary>
        /// <returns></returns>
        public JResult NewUpgrade(UpgradeRequest request) 
        {
            var response = new JResult()
            {
                Status = false
            };

            //当前会员
            var customer = Using<ICrCustomer>().Get(request.CustomerSysNo);
            if (customer == null)
            {
                response.Message = "无扩展表信息！";
                return response;
            }

            //会员列表
            var customerList = Using<ICrCustomer>().GetList(new CustomerExtRequest() { });

            var codeList = Using<IBsCode>().GetListByTypeList(new List<int>() { CodeEnum.CodeTypeEnum.会员等级信息.GetHashCode(), CodeEnum.CodeTypeEnum.代理销售奖金.GetHashCode() });
            if (codeList == null)
            {
                response.Message = "请设置代理销售奖金！";
                return response;
            }

            //会员等级
            var gradeStr = codeList.First(p => p.Type == CodeEnum.CodeTypeEnum.会员等级信息.GetHashCode()).Value;
            var gradeList = gradeStr.ToObject<List<GradeView>>();
            if (gradeList == null || !gradeList.Any())
            {
                response.Message = "会员等级序列化失败！";
                return response;
            }

            //代理销售奖金
            var salesBonusStr = codeList.First(p => p.Type == CodeEnum.CodeTypeEnum.代理销售奖金.GetHashCode()).Value;
            var salesAgencyBonus = salesBonusStr.ToObject<List<SalesAgencyBonus>>();
            if (salesAgencyBonus == null || !salesAgencyBonus.Any())
            {
                response.Message = "代理销售奖金序列化失败！";
                return response;
            }

            var upgradelist = salesAgencyBonus.Where(p => p.Grade.Equals(request.SelectGrade)).ToList();

            if (upgradelist != null && upgradelist.Any())
            {
                var parentIds = CustomerApp.Instance.GetUpgradeReferrer(customerList, new List<ParentIds>(), customer.ReferrerSysNo, upgradelist.Count, 1);
                if(parentIds!=null && parentIds.Any())
                {
                    foreach (var item in parentIds)
                    {
                        upgradelist.FirstOrDefault(p => p.Sort.Equals(item.Sort));
                    }
                }
                
            }
            return response;
        }

        #region 代理升级
        /// <summary>
        /// 代理升级
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public JResult Upgrade(UpgradeRequest request)
        {
            var response = new JResult()
            {
                Status = false
            };

            var customer = Using<ICrCustomer>().Get(request.CustomerSysNo);
            if (customer == null)
            {
                response.Message = "无扩展表信息！";
                return response;
            }

            var codeList = Using<IBsCode>().GetListByTypeList(new List<int>() { CodeEnum.CodeTypeEnum.会员等级信息.GetHashCode(), CodeEnum.CodeTypeEnum.代理销售奖金.GetHashCode() });
            if (codeList == null)
            {
                response.Message = "请设置代理销售奖金！";
                return response;
            }
            //代理销售奖金
            var agencyStr = codeList.First(p => p.Type == CodeEnum.CodeTypeEnum.代理销售奖金.GetHashCode()).Value;

            var salesAgencyBonus = agencyStr.ToObject<List<SalesAgencyBonus>>();
            if (salesAgencyBonus == null || !salesAgencyBonus.Any())
            {
                response.Message = "请设置代理销售奖金！";
                return response;
            }

            //会员等级信息
            var gradeStr = codeList.First(p => p.Type == CodeEnum.CodeTypeEnum.会员等级信息.GetHashCode()).Value;
            var gradeList = gradeStr.ToObject<List<GradeView>>();
            if (gradeList == null || !gradeList.Any())
            {
                response.Message = "请设置会员等级信息！";
                return response;
            }

            //会员扩展列表
            var customerExtList = Using<ICrCustomer>().GetList(new CustomerExtRequest() { });

            if (request.SelectGrade.Equals(CustomerEnum.Grade.普通代理.GetHashCode()))
            {
                #region 普通代理
                using (var tran = new TransactionScope())
                {
                    try
                    {
                        //升级金额验证 
                        var grade = gradeList.FirstOrDefault(p => p.Type.Equals(CustomerEnum.Grade.普通代理.GetHashCode()));
                        if (grade == null)
                        {
                            throw new Exception("请设置普通代理");
                        }
                        if (customer.UpgradeFundAmount < request.Amount)
                        {
                            response.StatusCode = ErrorEnum.余额不足.GetHashCode().ToString();
                            response.Message = "升级基金不足!";
                            return response;
                        }

                        var commonAgencyList = salesAgencyBonus.Where(p => p.Grade.Equals(CustomerEnum.Grade.普通代理.GetHashCode())).ToList();
                        if (!commonAgencyList.Count.Equals(13))
                        {
                            throw new Exception("请设置普通代理");
                        }
                        //
                        var weiXinTipList = new List<WeiXinTip>();

                        //获取当前会员上层所有推荐编号
                        if (customer.ReferrerSysNo > 0)
                        {
                            var parentIds = CustomerApp.Instance.GetUpgradeReferrer(customerExtList, new List<ParentIds>(), customer.ReferrerSysNo, 13, 1);
                            if (parentIds.Count <= 0)
                            {
                                throw new Exception("推荐人数计算错误!");
                            }

                            //批量更新上层
                            var batchUpgradeParentList = new List<BatchUpgradeParent>();
                            //批量添加奖金日志对象
                            var batchInertBonusLogList = new List<FnBonusLog>();


                            foreach (var item in parentIds)
                            {
                                if (new List<int>() { 1, 2, 3 }.Contains(item.Sort))
                                {
                                    #region 1到3层
                                    var bonus = commonAgencyList.FirstOrDefault(p => p.Sort.Equals(item.Sort)).Bonus;

                                    #region
                                    var walletAmount = item.Grade >= CustomerEnum.Grade.普通代理.GetHashCode() ? bonus : 0;

                                    var generalBonus = new decimal(0);

                                    if (item.Grade < CustomerEnum.Grade.普通代理.GetHashCode())
                                    {
                                        generalBonus = bonus / 2;
                                        //当前用户的上层所有推荐人
                                        var upAllReferrer = CustomerApp.Instance.GetUpgradeReferrer(customerExtList, new List<ParentIds>(), item.ReferrerSysNo);
                                        if (upAllReferrer.Count > 0)
                                        {
                                            //查找parentIds中查看级别为大于或等于普通代理
                                            LevelUpgrade(batchUpgradeParentList, batchInertBonusLogList, weiXinTipList, upAllReferrer, CustomerEnum.Grade.普通代理.GetHashCode(), generalBonus, customer.SysNo, customer.RealName);
                                        }

                                    }
                                    else
                                    {
                                        generalBonus = 0;
                                    }
                                    #endregion

                                    var batchUpgradeParent = new BatchUpgradeParent()
                                    {
                                        CustomerSysNo = item.CustomerSysNo,
                                        WalletAmount = walletAmount,
                                        GeneralBonus = generalBonus,
                                        AreaBonus = 0,
                                        GlobalBonus = 0
                                    };

                                    batchUpgradeParentList.Add(batchUpgradeParent);

                                    var amount = walletAmount > 0 ? walletAmount : generalBonus;

                                    var bonusLog = new FnBonusLog()
                                    {
                                        CustomerSysNo = item.CustomerSysNo,
                                        SourceSysNo = customer.SysNo,
                                        SourceSerialNumber = "",
                                        Amount = amount,
                                        Type = FnEnum.BonusLogType.代理奖励.GetHashCode(),
                                        Remarks = string.Format("{0}贡献,升级类型：普通代理,钱包：{1}，升级基金：{2}",customer.RealName,customer.WalletAmount,customer.UpgradeFundAmount),
                                        Status = FnEnum.BonusLogStatus.撤销.GetHashCode(),
                                        CreatedDate = DateTime.Now
                                    };
                                    batchInertBonusLogList.Add(bonusLog);

                                    var model = Using<ICrCustomer>().Get(item.CustomerSysNo);
                                    if (model != null)
                                    {
                                        if (!string.IsNullOrWhiteSpace(model.OpenId))
                                        {
                                            //计算总收益额
                                            int bonusSum = FnBonusLogApp.Instance.GetBonusSum(model.SysNo);

                                            weiXinTipList.Add(new WeiXinTip()
                                            {
                                                OpenId = model.OpenId,
                                                Keyword1 = amount.ToString() + "元",
                                                Keyword2 = (bonusSum + amount).ToString() + "元",
                                                Remark = string.Format("业绩来源：{0}", customer.RealName)
                                            });
                                        }

                                    }
                                    #endregion
                                }

                                if (true)
                                {
                                    if (new List<int>() { 4, 5, 6, 7, 8 }.Contains(item.Sort))
                                    {
                                        #region 4到8层
                                        var bonus = commonAgencyList.FirstOrDefault(p => p.Sort.Equals(item.Sort)).Bonus;

                                        #region
                                        var walletAmount = item.Grade >= CustomerEnum.Grade.区域代理.GetHashCode() ? bonus : 0;

                                        var generalBonus = new decimal(0);

                                        if (item.Grade < CustomerEnum.Grade.区域代理.GetHashCode())
                                        {
                                            continue;
                                        }
                                        #endregion

                                        var batchUpgradeParent = new BatchUpgradeParent()
                                        {
                                            CustomerSysNo = item.CustomerSysNo,
                                            WalletAmount = walletAmount,
                                            GeneralBonus = generalBonus,
                                            AreaBonus = 0,
                                            GlobalBonus = 0
                                        };

                                        batchUpgradeParentList.Add(batchUpgradeParent);

                                        var amount = walletAmount > 0 ? walletAmount : generalBonus;

                                        var bonusLog = new FnBonusLog()
                                        {
                                            CustomerSysNo = item.CustomerSysNo,
                                            SourceSysNo = customer.SysNo,
                                            SourceSerialNumber = "",
                                            Amount = amount,
                                            Type = FnEnum.BonusLogType.代理奖励.GetHashCode(),
                                           Remarks = string.Format("{0}贡献,升级类型：普通代理,钱包：{1}，升级基金：{2}",customer.RealName,customer.WalletAmount,customer.UpgradeFundAmount),
                                            Status = FnEnum.BonusLogStatus.撤销.GetHashCode(),
                                            CreatedDate = DateTime.Now
                                        };
                                        batchInertBonusLogList.Add(bonusLog);

                                        var model = Using<ICrCustomer>().Get(item.CustomerSysNo);
                                        if (model != null)
                                        {
                                            if (!string.IsNullOrWhiteSpace(model.OpenId))
                                            {
                                                //计算总收益额
                                                int bonusSum = FnBonusLogApp.Instance.GetBonusSum(model.SysNo);

                                                weiXinTipList.Add(new WeiXinTip()
                                                {
                                                    OpenId = model.OpenId,
                                                    Keyword1 = amount.ToString() + "元",
                                                    Keyword2 = (bonusSum + amount).ToString() + "元",
                                                    Remark = string.Format("业绩来源：{0}", customer.RealName),
                                                });
                                            }

                                        }
                                        #endregion
                                    }

                                    if (new List<int>() { 9, 10, 11, 12, 13 }.Contains(item.Sort))
                                    {
                                        #region 9到13层
                                        var bonus = commonAgencyList.FirstOrDefault(p => p.Sort.Equals(item.Sort)).Bonus;

                                        #region
                                        var walletAmount = item.Grade >= CustomerEnum.Grade.全国代理.GetHashCode() ? bonus : 0;

                                        var generalBonus = new decimal(0);

                                        if (item.Grade < CustomerEnum.Grade.全国代理.GetHashCode())
                                        {
                                            continue;
                                        }
                                        #endregion

                                        var batchUpgradeParent = new BatchUpgradeParent()
                                        {
                                            CustomerSysNo = item.CustomerSysNo,
                                            WalletAmount = walletAmount,
                                            GeneralBonus = generalBonus,
                                            AreaBonus = 0,
                                            GlobalBonus = 0
                                        };

                                        batchUpgradeParentList.Add(batchUpgradeParent);

                                        var amount = walletAmount > 0 ? walletAmount : generalBonus;

                                        var bonusLog = new FnBonusLog()
                                        {
                                            CustomerSysNo = item.CustomerSysNo,
                                            SourceSysNo = customer.SysNo,
                                            SourceSerialNumber = "",
                                            Amount = amount,
                                            Type = FnEnum.BonusLogType.代理奖励.GetHashCode(),
                                            //Remarks = string.Format("{0}贡献", customer.RealName),
                                            Remarks = string.Format("{0}贡献,升级类型：普通代理,钱包：{1}，升级基金：{2}",customer.RealName,customer.WalletAmount,customer.UpgradeFundAmount),
                                            Status = FnEnum.BonusLogStatus.撤销.GetHashCode(),
                                            CreatedDate = DateTime.Now
                                        };
                                        batchInertBonusLogList.Add(bonusLog);

                                        var model = Using<ICrCustomer>().Get(item.CustomerSysNo);
                                        if (model != null)
                                        {
                                            if (!string.IsNullOrWhiteSpace(model.OpenId))
                                            {
                                                //计算总收益额
                                                int bonusSum = FnBonusLogApp.Instance.GetBonusSum(model.SysNo);

                                                weiXinTipList.Add(new WeiXinTip()
                                                {
                                                    OpenId = model.OpenId,
                                                    Keyword1 = amount.ToString() + "元",
                                                    Keyword2 = (bonusSum + amount).ToString() + "元",
                                                    Remark = string.Format("业绩来源：{0}", customer.RealName),
                                                });
                                            }

                                        }
                                        #endregion
                                    }
                                }
                            }

                            if (batchUpgradeParentList != null && batchUpgradeParentList.Any())
                            {
                                if (Using<ICrCustomer>().BatchUpdate(batchUpgradeParentList) <= 0)
                                {
                                    throw new Exception("批量更新推荐人奖金失败!");
                                }
                            }

                            if (batchInertBonusLogList != null && batchInertBonusLogList.Any())
                            {
                                if (Using<IFnBonusLog>().BatchInsert(batchInertBonusLogList) <= 0)
                                {
                                    throw new Exception("批量添加推荐人奖金日志失败!");
                                }
                            }
                        }


                        //更新升级基金（升级基金(DB)-（升级基金-当前升级金额)）
                        if (Using<ICrCustomer>().SubtractUpgradeFundAmount(new CrCustomer()
                        {
                            SysNo = customer.SysNo,
                            UpgradeFundAmount = request.Amount,
                        }) <= 0)
                        {
                            throw new Exception("更新升级基金失败!");
                        }
                        //更新等级
                        if (Using<ICrCustomer>().UpdateGrade(new CrCustomer()
                        {
                            SysNo = customer.SysNo,
                            Grade = CustomerEnum.Grade.普通代理.GetHashCode()
                        }) <= 0)
                        {
                            throw new Exception("更新级别失败!");
                        }
                        //将结算奖金写入钱包
                        if (Using<ICrCustomer>().UpdateGeneralBonus(new CrCustomer() { SysNo = customer.SysNo }) <= 0)
                        {
                            throw new Exception("更新结算奖金失败!");
                        }
                        //如果过期时间为空则添加一年有效期
                        if (string.IsNullOrWhiteSpace(customer.ExpiresTime))
                        {
                            customer.ExpiresTime = DateTime.Now.AddYears(1).ToString("yyyy-MM-dd");
                            if (Using<ICrCustomer>().UpdateExpiresTime(customer) <= 0)
                            {
                                throw new Exception("更新过期时间失败!");
                            }
                        }

                        if (weiXinTipList != null && weiXinTipList.Any())
                        {
                            foreach (var p in weiXinTipList)
                            {
                                WeiXinApp.Instance.SendMessage(new SendMessageRequest()
                                {
                                    WeiXinAppId = "wx9535283296d186c3",
                                    WeiXinAppSecret = "543e75c11909f438c02870ecbab85f5d",
                                    OpenId = p.OpenId,
                                    TemplateId = "a8oNldLv7Nlvbkajt6vQbWUBOgmu7EWW2j08UzdQgBg",
                                    Data = new
                                    {
                                        first = new { color = "#000000", value = "您有一笔新的分红奖金到账啦！" },
                                        keyword1 = new { color = "#000000", value = p.Keyword1 },
                                        keyword2 = new { color = "#000000", value = p.Keyword2 },
                                        remark = new { color = "#000000", value = p.Remark }
                                    },
                                    Url = "#"
                                });
                            }
                        }


                        response.Message = "升级成功！";
                        response.Status = true;
                        tran.Complete();
                    }
                    catch (Exception ex)
                    {
                        tran.Dispose();
                        response.Message = ex.Message;
                    }
                }
                #endregion
            }
            else if (request.SelectGrade.Equals(CustomerEnum.Grade.区域代理.GetHashCode()))
            {
                #region 区域代理
                using (var tran = new TransactionScope())
                {
                    try
                    {
                        //升级金额验证 
                        var grade = gradeList.FirstOrDefault(p => p.Type.Equals(CustomerEnum.Grade.区域代理.GetHashCode()));
                        if (grade == null)
                        {
                            throw new Exception("请设置普通代理");
                        }
                        if (customer.UpgradeFundAmount < request.Amount)
                        {
                            response.StatusCode = ErrorEnum.余额不足.GetHashCode().ToString();
                            response.Message = "升级基金不足!";
                            return response;
                        }

                        var commonAgencyList = salesAgencyBonus.Where(p => p.Grade.Equals(CustomerEnum.Grade.区域代理.GetHashCode())).ToList();
                        if (!commonAgencyList.Count.Equals(7))
                        {
                            response.Message = "请设置区域代理！";
                            return response;
                        }
                        var weiXinTipList = new List<WeiXinTip>();
                        //获取当前会员上层所有推荐编号
                        if (customer.ReferrerSysNo > 0)
                        {
                            var parentIds = CustomerApp.Instance.GetUpgradeReferrer(customerExtList, new List<ParentIds>(), customer.ReferrerSysNo, 7,1);
                            if (parentIds.Count <= 0)
                            {
                                throw new Exception("推荐人数计算错误!");
                            }
                            //批量更新上层
                            var batchUpgradeParentList = new List<BatchUpgradeParent>();
                            //批量添加奖金日志对象
                            var batchInertBonusLogList = new List<FnBonusLog>();

                            foreach (var item in parentIds)
                            {
                                var bonus = commonAgencyList.FirstOrDefault(p => p.Sort.Equals(item.Sort)).Bonus;

                                #region
                                var walletAmount = item.Grade >= CustomerEnum.Grade.区域代理.GetHashCode() ? bonus : 0;

                                var areaBonus = new decimal(0);

                                if (item.Grade < CustomerEnum.Grade.区域代理.GetHashCode())
                                {
                                    areaBonus = bonus / 2;
                                    //当前用户的上层所有推荐人
                                    var upAllReferrer = CustomerApp.Instance.GetUpgradeReferrer(customerExtList, new List<ParentIds>(), item.ReferrerSysNo);
                                    if (upAllReferrer.Count > 0)
                                    {
                                        //查找parentIds中查看级别为大于或等于普通代理
                                        LevelUpgrade(batchUpgradeParentList, batchInertBonusLogList, weiXinTipList, upAllReferrer, CustomerEnum.Grade.区域代理.GetHashCode(), areaBonus, customer.SysNo, customer.RealName);
                                    }

                                }
                                else
                                {
                                    areaBonus = 0;
                                }
                                #endregion

                                var batchUpgradeParent = new BatchUpgradeParent()
                                {
                                    CustomerSysNo = item.CustomerSysNo,
                                    WalletAmount = walletAmount,
                                    GeneralBonus = 0,
                                    AreaBonus = areaBonus,
                                    GlobalBonus = 0
                                };

                                batchUpgradeParentList.Add(batchUpgradeParent);

                                var amount = walletAmount > 0 ? walletAmount : areaBonus;

                                var bonusLog = new FnBonusLog()
                                {
                                    CustomerSysNo = item.CustomerSysNo,
                                    SourceSysNo = customer.SysNo,
                                    SourceSerialNumber = "",
                                    Amount = amount,
                                    Type = FnEnum.BonusLogType.代理奖励.GetHashCode(),
                                    Remarks = string.Format("{0}贡献,升级类型：普通代理,钱包：{1}，升级基金：{2}",customer.RealName,customer.WalletAmount,customer.UpgradeFundAmount),
                                    Status = FnEnum.BonusLogStatus.撤销.GetHashCode(),
                                    CreatedDate = DateTime.Now
                                };
                                batchInertBonusLogList.Add(bonusLog);

                                var model = Using<ICrCustomer>().Get(item.CustomerSysNo);
                                if (model != null)
                                {
                                    if (!string.IsNullOrWhiteSpace(model.OpenId))
                                    {
                                        //计算总收益额
                                        int bonusSum = FnBonusLogApp.Instance.GetBonusSum(model.SysNo);

                                        weiXinTipList.Add(new WeiXinTip()
                                        {
                                            OpenId = model.OpenId,
                                            Keyword1 = amount.ToString() + "元",
                                            Keyword2 = (bonusSum + amount).ToString() + "元",
                                            Remark = string.Format("业绩来源：{0}", customer.RealName),
                                        });
                                    }
                                }
                            }

                            if (batchUpgradeParentList != null && batchUpgradeParentList.Any())
                            {
                                if (Using<ICrCustomer>().BatchUpdate(batchUpgradeParentList) <= 0)
                                {
                                    throw new Exception("批量更新推荐人奖金失败!");
                                }
                            }

                            if (batchInertBonusLogList != null && batchInertBonusLogList.Any())
                            {
                                if (Using<IFnBonusLog>().BatchInsert(batchInertBonusLogList) <= 0)
                                {
                                    throw new Exception("批量添加推荐人奖金日志失败!");
                                }
                            }
                        }


                        //更新升级基金（升级基金(DB)-（升级基金-当前升级金额)）
                        if (Using<ICrCustomer>().SubtractUpgradeFundAmount(new CrCustomer()
                        {
                            SysNo = customer.SysNo,
                            UpgradeFundAmount = request.Amount,
                        }) <= 0)
                        {
                            throw new Exception("更新升级基金失败!");
                        }
                        //更新等级
                        if (Using<ICrCustomer>().UpdateGrade(new CrCustomer()
                        {
                            SysNo = customer.SysNo,
                            Grade = CustomerEnum.Grade.区域代理.GetHashCode()
                        }) <= 0)
                        {
                            throw new Exception("更新级别失败!");
                        }
                        //将结算奖金写入钱包
                        if (Using<ICrCustomer>().UpdateAreaBonus(new CrCustomer() { SysNo = customer.SysNo }) <= 0)
                        {
                            throw new Exception("更新结算奖金失败!");
                        }
                        //如果过期时间为空则添加一年有效期
                        if (string.IsNullOrWhiteSpace(customer.ExpiresTime))
                        {
                            customer.ExpiresTime = DateTime.Now.AddYears(1).ToString("yyyy-MM-dd");
                            if (Using<ICrCustomer>().UpdateExpiresTime(customer) <= 0)
                            {
                                throw new Exception("更新过期时间失败!");
                            }
                        }

                        if (weiXinTipList != null && weiXinTipList.Any())
                        {
                            foreach (var p in weiXinTipList)
                            {
                                WeiXinApp.Instance.SendMessage(new SendMessageRequest()
                                {
                                    WeiXinAppId = "wx9535283296d186c3",
                                    WeiXinAppSecret = "543e75c11909f438c02870ecbab85f5d",
                                    OpenId = p.OpenId,
                                    TemplateId = "a8oNldLv7Nlvbkajt6vQbWUBOgmu7EWW2j08UzdQgBg",
                                    Data = new
                                    {
                                        first = new { color = "#000000", value = "您有一笔新的分红奖金到账啦！" },
                                        keyword1 = new { color = "#000000", value = p.Keyword1 },
                                        keyword2 = new { color = "#000000", value = p.Keyword2 },
                                        remark = new { color = "#000000", value = p.Remark }
                                    },
                                    Url = "#"
                                });
                            }
                        }
                        response.Message = "升级成功！";
                        response.Status = true;
                        tran.Complete();
                    }
                    catch (Exception ex)
                    {
                        tran.Dispose();
                        response.Message = ex.Message;
                    }
                }
                #endregion
            }
            else if (request.SelectGrade.Equals(CustomerEnum.Grade.全国代理.GetHashCode()))
            {
                #region 全国代理
                using (var tran = new TransactionScope())
                {
                    try
                    {
                        //升级金额验证 
                        var grade = gradeList.FirstOrDefault(p => p.Type.Equals(CustomerEnum.Grade.全国代理.GetHashCode()));
                        if (grade == null)
                        {
                            throw new Exception("请设置全国代理");
                        }
                        if (customer.UpgradeFundAmount < request.Amount)
                        {
                            response.StatusCode = ErrorEnum.余额不足.GetHashCode().ToString();
                            response.Message = "升级基金不足!";
                            return response;
                        }

                        var commonAgencyList = salesAgencyBonus.Where(p => p.Grade.Equals(CustomerEnum.Grade.全国代理.GetHashCode())).ToList();
                        if (!commonAgencyList.Count.Equals(22))
                        {
                            response.Message = "请设置全国代理！";
                            return response;
                        }
                        var weiXinTipList = new List<WeiXinTip>();
                        //获取当前会员上层所有推荐编号
                        if (customer.ReferrerSysNo > 0)
                        {
                            var parentIds = CustomerApp.Instance.GetUpgradeReferrer(customerExtList, new List<ParentIds>(), customer.ReferrerSysNo, 22,1);
                            if (parentIds.Count <= 0)
                            {
                                throw new Exception("推荐人数计算错误!");
                            }
                            //批量更新上层
                            var batchUpgradeParentList = new List<BatchUpgradeParent>();
                            //批量添加奖金日志对象
                            var batchInertBonusLogList = new List<FnBonusLog>();

                            foreach (var item in parentIds)
                            {
                                var bonus = commonAgencyList.FirstOrDefault(p => p.Sort.Equals(item.Sort)).Bonus;

                                #region
                                var walletAmount = item.Grade >= CustomerEnum.Grade.全国代理.GetHashCode() ? bonus : 0;

                                var globalBonus = new decimal(0);

                                if (item.Grade < CustomerEnum.Grade.全国代理.GetHashCode())
                                {
                                    globalBonus = bonus / 2;
                                    //当前用户的上层所有推荐人
                                    var upAllReferrer = CustomerApp.Instance.GetUpgradeReferrer(customerExtList, new List<ParentIds>(), item.ReferrerSysNo);
                                    if (upAllReferrer.Count > 0)
                                    {
                                        //查找parentIds中查看级别为大于或等于普通代理
                                        LevelUpgrade(batchUpgradeParentList, batchInertBonusLogList, weiXinTipList, upAllReferrer, CustomerEnum.Grade.全国代理.GetHashCode(), globalBonus, customer.SysNo, customer.RealName);
                                    }
                                }
                                else
                                {
                                    globalBonus = 0;
                                }
                                #endregion

                                var batchUpgradeParent = new BatchUpgradeParent()
                                {
                                    CustomerSysNo = item.CustomerSysNo,
                                    WalletAmount = walletAmount,
                                    GeneralBonus = 0,
                                    AreaBonus = 0,
                                    GlobalBonus = globalBonus
                                };

                                batchUpgradeParentList.Add(batchUpgradeParent);

                                var amount = walletAmount > 0 ? walletAmount : globalBonus;

                                var bonusLog = new FnBonusLog()
                                {
                                    CustomerSysNo = item.CustomerSysNo,
                                    SourceSysNo = customer.SysNo,
                                    SourceSerialNumber = "",
                                    Amount = amount,
                                    Type = FnEnum.BonusLogType.代理奖励.GetHashCode(),
                                    Remarks = string.Format("{0}贡献,升级类型：普通代理,钱包：{1}，升级基金：{2}",customer.RealName,customer.WalletAmount,customer.UpgradeFundAmount),
                                    Status = FnEnum.BonusLogStatus.撤销.GetHashCode(),
                                    CreatedDate = DateTime.Now
                                };
                                batchInertBonusLogList.Add(bonusLog);

                                var model = Using<ICrCustomer>().Get(item.CustomerSysNo);
                                if (model != null) {
                                    if (!string.IsNullOrWhiteSpace(model.OpenId))
                                    {
                                        //计算总收益额
                                        int bonusSum = FnBonusLogApp.Instance.GetBonusSum(model.SysNo);

                                        weiXinTipList.Add(new WeiXinTip()
                                        {
                                            OpenId = model.OpenId,
                                            Keyword1 = amount.ToString() + "元",
                                            Keyword2 = (bonusSum + amount).ToString() + "元",
                                            Remark = string.Format("业绩来源：{0}", customer.RealName),
                                        });
                                    }
                                }
                                
                            }

                            if (batchUpgradeParentList != null && batchUpgradeParentList.Any())
                            {
                                if (Using<ICrCustomer>().BatchUpdate(batchUpgradeParentList) <= 0)
                                {
                                    throw new Exception("批量更新推荐人奖金失败!");
                                }
                            }

                            if (batchInertBonusLogList != null && batchInertBonusLogList.Any())
                            {
                                if (Using<IFnBonusLog>().BatchInsert(batchInertBonusLogList) <= 0)
                                {
                                    throw new Exception("批量添加推荐人奖金日志失败!");
                                }
                            }
                        }

                        //更新升级基金（升级基金(DB)-（升级基金-当前升级金额)）
                        if (Using<ICrCustomer>().SubtractUpgradeFundAmount(new CrCustomer()
                        {
                            SysNo = customer.SysNo,
                            UpgradeFundAmount = request.Amount,
                        }) <= 0)
                        {
                            throw new Exception("更新升级基金失败!");
                        }
                        //更新等级
                        if (Using<ICrCustomer>().UpdateGrade(new CrCustomer()
                        {
                            SysNo = customer.SysNo,
                            Grade = CustomerEnum.Grade.全国代理.GetHashCode()
                        }) <= 0)
                        {
                            throw new Exception("更新级别失败!");
                        }
                        //将结算奖金写入钱包
                        if (Using<ICrCustomer>().UpdateGlobalBonus(new CrCustomer() { SysNo = customer.SysNo }) <= 0)
                        {
                            throw new Exception("更新结算奖金失败!");
                        }
                        //如果过期时间为空则添加一年有效期
                        if (string.IsNullOrWhiteSpace(customer.ExpiresTime))
                        {
                            customer.ExpiresTime = DateTime.Now.AddYears(1).ToString("yyyy-MM-dd");
                            if (Using<ICrCustomer>().UpdateExpiresTime(customer) <= 0)
                            {
                                throw new Exception("更新过期时间失败!");
                            }
                        }

                        if (weiXinTipList != null && weiXinTipList.Any())
                        {
                            foreach (var p in weiXinTipList)
                            {
                                WeiXinApp.Instance.SendMessage(new SendMessageRequest()
                                {
                                    WeiXinAppId = "wx9535283296d186c3",
                                    WeiXinAppSecret = "543e75c11909f438c02870ecbab85f5d",
                                    OpenId = p.OpenId,
                                    TemplateId = "a8oNldLv7Nlvbkajt6vQbWUBOgmu7EWW2j08UzdQgBg",
                                    Data = new
                                    {
                                        first = new { color = "#000000", value = "您有一笔新的分红奖金到账啦！" },
                                        keyword1 = new { color = "#000000", value = p.Keyword1 },
                                        keyword2 = new { color = "#000000", value = p.Keyword2 },
                                        remark = new { color = "#000000", value = p.Remark }
                                    },
                                    Url = "#"
                                });
                            }
                        }

                        response.Message = "升级成功！";
                        response.Status = true;
                        tran.Complete();
                    }
                    catch (Exception ex)
                    {
                        tran.Dispose();
                        response.Message = ex.Message;
                    }
                }
                #endregion
            }
            return response;
        }

        #region 级差待结果
        /// <summary>
        /// 级差待结果
        /// </summary>
        /// <param name="batchUpgradeParentList">当前入队奖金对象</param>
        /// <param name="bonusLog">奖金日</param>
        /// <param name="upAllReferrer">当前用户所有上升</param>
        /// <param name="grade">级别</param>
        /// <param name="bonus">奖金</param>
        public void LevelUpgrade(List<BatchUpgradeParent> batchUpgradeParentList, List<FnBonusLog> bonusLogList, List<WeiXinTip> weiXinTipList, List<ParentIds> upAllReferrer,int grade, decimal bonus,int customerSysNo,string realName) {
            foreach (var item in upAllReferrer) {
                if (item.Grade >= grade) {
                    var batchUpgradeParent = new BatchUpgradeParent()
                    {
                        CustomerSysNo = item.CustomerSysNo,
                        WalletAmount = bonus,
                        GeneralBonus = 0,
                        AreaBonus = 0,
                        GlobalBonus = 0
                    };

                    batchUpgradeParentList.Add(batchUpgradeParent);

                    var strGrade = "";

                    switch (grade) {
                        case 10:
                            strGrade = "普通代理";
                            break;
                        case 20:
                            strGrade = "普通代理";
                            break;
                        case 30:
                            strGrade = "区域代理";
                            break;
                        case 40:
                            strGrade = "全国代理";
                            break;
                        case 50:
                            strGrade = "股东";
                            break;
                    };

                    var bonusLog = new FnBonusLog()
                    {
                        CustomerSysNo = item.CustomerSysNo,
                        SourceSysNo = customerSysNo,
                        SourceSerialNumber = "",
                        Amount = bonus,
                        Type = FnEnum.BonusLogType.代理奖励.GetHashCode(),
                        Remarks = string.Format("{0}贡献,【升级类型：{1}】", realName, strGrade),
                        Status = FnEnum.BonusLogStatus.撤销.GetHashCode(),
                        CreatedDate = DateTime.Now
                    };

                    bonusLogList.Add(bonusLog);


                    if (!string.IsNullOrWhiteSpace(item.OpenId))
                    {
                        //计算总收益额
                        int bonusSum = FnBonusLogApp.Instance.GetBonusSum(item.CustomerSysNo);

                        weiXinTipList.Add(new WeiXinTip()
                        {
                            OpenId = item.OpenId,
                            Keyword1 = bonus.ToString() + "元",
                            Keyword2 = bonusSum.ToString() + "元",
                            Remark = string.Format("业绩来源：{0}", realName),
                        });
                    }

                    break;
                }
            }
        }
        #endregion

        #endregion

        #region 升级计算
        /// <summary>
        /// 升级计算
        /// </summary>
        /// <param name="request">参数</param>
        /// <returns></returns>
        public JResult<UpgradeComputeResponse> UpgradeCompute(UpgradeComputeRequest request)
        {
            var response = new JResult<UpgradeComputeResponse>()
            {
                Status = false
            };

            var customer = Using<ICrCustomer>().Get(request.CustomerSysNo);
            if (customer == null)
            {
                response.Message = "用户不存在";
                return response;
            }
            //获取等级字符串
            var code = CodeApp.Instance.GetByType(CodeEnum.CodeTypeEnum.会员等级信息.GetHashCode());
            if (code == null)
            {
                response.Message = "获取会员等级信息失败﹗";
                return response;
            }

            var gradeList = JsonUtil.ToObject<List<GradeView>>(code.Value);
            if (gradeList == null && !gradeList.Any())
            {
                response.Message = "请设置会员等级信息！";
                return response;
            }


            //升级总金额
            var upgradeTotalAmount = new decimal(0);

            var grade = gradeList.FirstOrDefault(p => p.Type.Equals(request.SelectGrade));
            if (grade == null) {
                response.Message = "请选择会员等级！";
                return response;
            }

            upgradeTotalAmount = grade.Amount;


            var onlinePaymentAmount = new decimal(0);
            if (customer.UpgradeFundAmount > upgradeTotalAmount)
            {
                onlinePaymentAmount = 0;
            }
            else
            {
                if (customer.UpgradeFundAmount < upgradeTotalAmount)
                {
                    onlinePaymentAmount = upgradeTotalAmount - customer.UpgradeFundAmount;
                }
            }

            response.Data = new UpgradeComputeResponse()
            {
                DeductedAmount = upgradeTotalAmount,
                OnlinePaymentAmount = onlinePaymentAmount,
            };
            response.Status = true;
            return response;
        }

        #endregion

        #region 随机数
        /// <summary>
        /// 八位随机数
        /// </summary>
        /// <returns>八位随机数</returns>
        public string GetRand()
        {
            var serialNumber = Rand();

            var customerExt = Using<ICrCustomer>().GetBySerialNumber(serialNumber);
            if (customerExt != null)
            {
                this.GetRand();
            }

            return serialNumber;
        }

        /// <summary>
        /// 生成8位长度随机数字、首字母不能为零
        /// </summary>
        /// <returns>指定长度的随机数</returns>
        private string Rand()
        {
            if (true)
                System.Threading.Thread.Sleep(2);

            string result = "";

            var random = new Random();

            for (int i = 0; i < 7; i++)
            {
                result += random.Next(10).ToString();
            }
            return new Random().Next(1, 9) + result;
        }
        #endregion

        #region 获取推荐人
        /// <summary>
        /// 获取推荐人
        /// </summary>
        /// <param name="customersList">所有会员</param>
        /// <param name="ids">推荐人编号</param>
        /// <param name="referrerSysNo">推荐编号</param>
        /// <returns>List<int></returns>
        public List<int> GetReferrer(IList<CrCustomer> customersList, List<int> ids, int referrerSysNo)
        {
            var customer = customersList.FirstOrDefault(p => p.SysNo.Equals(referrerSysNo));
            if (customer != null)
            {
                ids.Add(customer.SysNo);
                return GetReferrer(customersList, ids, customer.ReferrerSysNo);
            }
            return ids;
        }

        /// <summary>
        /// 获取升级推荐人列表
        /// </summary>
        /// <param name="customersList">所有会员</param>
        /// <param name="parentIds">推荐人列表</param>
        /// <param name="referrerSysNo">推荐编号</param>
        /// <param name="layer">获取上级层数</param>
        /// <param name="sort">排序</param>
        /// <returns>List<ParentIds></returns>
        public List<ParentIds> GetUpgradeReferrer(IList<CrCustomer> customersList, List<ParentIds> parentIds, int referrerSysNo, int layer, int sort = 1)
        {
            var customer = customersList.FirstOrDefault(p => p.SysNo.Equals(referrerSysNo));
            if (customer != null)
            {
                parentIds.Add(new ParentIds()
                {
                    CustomerSysNo = customer.SysNo,
                    RealName = customer.RealName,
                    Grade = customer.Grade,
                    Sort = sort,
                    ReferrerSysNo = customer.ReferrerSysNo,
                    OpenId = customer.OpenId,
                    WalletAmount = customer.WalletAmount,
                    UpgradeFundAmount = customer.UpgradeFundAmount,
                    GeneralBonus = customer.GeneralBonus,
                    AreaBonus = customer.AreaBonus,
                    GlobalBonus = customer.GlobalBonus
                });
                if (sort.Equals(layer))
                    return parentIds;
                sort++;
                return GetUpgradeReferrer(customersList, parentIds, customer.ReferrerSysNo, layer, sort);
            }
            return parentIds;
        }

        /// <summary>
        /// 获取升级推荐人列表
        /// </summary>
        /// <param name="customersList">所有会员</param>
        /// <param name="parentIds">推荐人列表</param>
        /// <param name="referrerSysNo">推荐编号</param>
        /// <param name="layer">获取上级层数</param>
        /// <param name="sort">排序</param>
        /// <returns>List<ParentIds></returns>
        public List<ParentIds> GetUpgradeReferrer(IList<CrCustomer> customersList, List<ParentIds> parentIds, int referrerSysNo, int sort=1)
        {
            var customer = customersList.FirstOrDefault(p => p.SysNo.Equals(referrerSysNo));
            if (customer != null)
            {
                parentIds.Add(new ParentIds()
                {
                    CustomerSysNo = customer.SysNo,
                    RealName = customer.RealName,
                    Grade = customer.Grade,
                    Sort = sort,
                    ReferrerSysNo = customer.ReferrerSysNo,
                    OpenId = customer.OpenId,
                    WalletAmount = customer.WalletAmount,
                    UpgradeFundAmount = customer.UpgradeFundAmount,
                    GeneralBonus = customer.GeneralBonus,
                    AreaBonus = customer.AreaBonus,
                    GlobalBonus = customer.GlobalBonus
                });
                sort++;
                return GetUpgradeReferrer(customersList, parentIds, customer.ReferrerSysNo,sort);
            }
            return parentIds;
        }
        #endregion

        #region 更新用户基本资料
        /// <summary>
        /// 更新用户基本资料
        /// </summary>
        /// <param name="model">数据模型</param>
        /// <returns>影响行数</returns>
        public int UpdateBankInfo(CrCustomer model)
        {
            return Using<ICrCustomer>().UpdateBankInfo(model);
        }
        #endregion

        #region 更新用户密码
        /// <summary>
        /// 更新用户密码
        /// </summary>
        /// <param name="sysNo">会员编号</param>
        /// <param name="password">密码</param>
        /// <returns>返回结果</returns>
        public JResult UpdatePassword(int sysNo, string password)
        {
            var result = new JResult();

            try
            {
                if (Using<ICrCustomer>().UpdatePassword(new CrCustomer()
                {
                    SysNo = sysNo,
                    Password = EncryptionUtil.EncryptWithMd5AndSalt(password)
                }) <= 0)
                {
                    throw new Exception("更新密码失败");
                }

                result.Status = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }
        #endregion

        #region 更新用户基础信息
        /// <summary>
        /// 更新用户基础信息
        /// </summary>
        /// <param name="model">用户信息</param>
        /// <returns>返回结果</returns>
        public JResult UpdateBaseInfo(CrCustomer model)
        {
            var result = new JResult();

            try
            {
                if (model.SysNo > 0)
                {
                    if (Using<ICrCustomer>().UpdateBaseInfo(model) <= 0)
                    {
                        throw new Exception("更新用户信息失败");
                    }
                }
                else
                {
                    var serialNumber = GetRand();

                    var para = new RegisterRequest()
                    {
                        Account = model.Account,
                        Password = EncryptionUtil.EncryptWithMd5AndSalt("tt123456"),
                        SerialNumber = serialNumber,
                        RealName = model.RealName,
                        ReferrerSysNo = model.ReferrerSysNo,
                        PhoneNumber = model.PhoneNumber,
                        Nickname = model.Nickname,
                        HeadImgUrl = model.HeadImgUrl,
                    };

                    if (!CustomerApp.Instance.Register(para).Status)
                    {
                        throw new Exception("更新用户信息失败");
                    }
                }

                result.Status = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }
        #endregion

        #region 更新用户等级
        /// <summary>
        /// 更新用户等级
        /// </summary>
        public int UpdateGrade(CrCustomer model)
        {
            return Using<ICrCustomer>().UpdateGrade(model);
        }
        #endregion

        #region 更新提现(钱包金额-提现金额)(升级金额+提现金额/(如果是普通、区域、全国才除)2)
        /// <summary>
        /// 更新提现(钱包金额-提现金额)(升级金额+提现金额/(如果是普通、区域、全国才除)2)
        /// </summary>
        public int UpdateWalletAmount(UpdateWalletAmountRequest request)
        {
            return Using<ICrCustomer>().UpdateWalletAmount(request);
        }

        #endregion

        #region 更新用户头像
        /// <summary>
        /// 更新用户头像
        /// </summary>
        public int UpdateHead(int sysNo, string headImgUrl)
        {
            return Using<ICrCustomer>().UpdateHead(sysNo, headImgUrl);
        }
        #endregion

        #region 获取会员扩展信息列表
        /// <summary>
        /// 获取会员扩展信息列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>会员扩展信息列表</returns>
        public IList<CrCustomer> GetList(CustomerExtRequest request)
        {
            return Using<ICrCustomer>().GetList(request);
        }
        #endregion

        #region 获取会员扩展信息分页列表
        /// <summary>
        /// 获取会员扩展信息分页列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>会员扩展信息分页列表</returns>
        public PagedList<CustomerExt> GetExtPagerList(CustomerExtRequest request)
        {
            return Using<ICrCustomer>().GetExtPagerList(request);
        }
        #endregion

        #region 更新奖金充值(基金充值[充值方式:奖金余额]、帐户续费[续费方式:奖金余额])
        /// <summary>
        /// 更新奖金充值(基金充值[充值方式:奖金余额]、帐户续费[续费方式:奖金余额])
        /// </summary>
        /// <returns></returns>
        public int UpdateBonusRecharge(CrCustomer model)
        {
            return Using<ICrCustomer>().UpdateBonusRecharge(model);
        }
        #endregion

        #region 续费更新钱包
        /// <summary>
        /// 续费更新钱包
        /// </summary>
        public int UpdateWalletAmountRenew(CrCustomer model)
        {
            return Using<ICrCustomer>().UpdateWalletAmountRenew(model);
        }
        #endregion

        #region 获取平台参数
        /// <summary>
        /// 获取平台参数
        /// </summary>
        /// <returns>平台参数</returns>
        public PlatformView GetPlatformValue()
        {
            var platformValue = Using<ICrCustomer>().GetPlatformValue();
            var codeList = Using<IBsCode>().GetListByTypeList(new List<int>()
            {
                 CodeEnum.CodeTypeEnum.股池资金.GetHashCode(),
                 CodeEnum.CodeTypeEnum.股权数总额.GetHashCode(),
                 CodeEnum.CodeTypeEnum.股权价格.GetHashCode()
            });

            var stockTotalAmountModel = codeList.FirstOrDefault(p => p.Type == CodeEnum.CodeTypeEnum.股池资金.GetHashCode());
            var stockNumModel = codeList.FirstOrDefault(p => p.Type == CodeEnum.CodeTypeEnum.股权数总额.GetHashCode());
            var stockPriceModel = codeList.FirstOrDefault(p => p.Type == CodeEnum.CodeTypeEnum.股权价格.GetHashCode());

            platformValue.StockTotalAmount = stockTotalAmountModel != null ? decimal.Round(Convert.ToDecimal(stockTotalAmountModel.Value), 3) : 0;
            platformValue.StockNum = stockNumModel != null ? Convert.ToInt32(stockNumModel.Value) : 0;
            platformValue.StockPrice = stockPriceModel != null ? decimal.Round(Convert.ToDecimal(stockPriceModel.Value), 3) : 0;
            platformValue.RechargeTotalAmount = decimal.Round(platformValue.RechargeTotalAmount, 3);
            platformValue.WithdrawalsTotalAmount = decimal.Round(platformValue.WithdrawalsTotalAmount, 3);

            return platformValue;
        }
        #endregion

        #region 更新过期时间
        /// <summary>
        /// 更新过期时间
        /// </summary>
        public int UpdateExpiresTime(CrCustomer model)
        {
            return Using<ICrCustomer>().UpdateExpiresTime(model);
        }
        #endregion

        #region 会员是否VIP
        /// <summary>
        /// 会员是否VIP
        /// </summary>
        /// <param name="customer">会员信息</param>
        /// <returns>结果</returns>
        public JResult IsCustomerVip(CustomerView customer)
        {
            var result = new JResult();

            try
            {
                if (customer.Grade == CustomerEnum.Grade.普通会员.GetHashCode())
                {
                    throw new Exception("会员为普通会员");
                }
                if (string.IsNullOrEmpty(customer.ExpiresTime) || DateTime.Compare(Convert.ToDateTime(customer.ExpiresTime), DateTime.Now) < 0)
                {
                    throw new Exception("会员为已过期，请及时充值");
                }

                result.Status = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }
        #endregion

        #region 获取会员层级列表
        /// <summary>
        /// 获取会员层级列表
        /// </summary>
        /// <param name="levelCustomerSysNo">层级会员编号（用于查询层级推荐会员）</param>
        /// <returns>会员层级列表</returns>
        public List<CustomerLevelView> GetCustomerLevelCount(int levelCustomerSysNo)
        {
            string[] letterArrStr = { "A", "B", "C"};

            var list = Using<ICrCustomer>().GetCustomerLevelCount(levelCustomerSysNo).ToList();

            if (!list.Any()) return list;
            var minLevel = list.Min(p => p.Level);
            foreach (var item in list.Where(item => item.Level - minLevel <= 3))
            {
                item.LevelLetter = letterArrStr[item.Level - minLevel];
            }

            return list.OrderBy(p => p.Level).ToList();
        }

        /// <summary>
        /// 获取会员层级列表
        /// </summary>
        /// <param name="levelCustomerSysNo">层级会员编号（用于查询层级推荐会员）</param>
        /// <returns>会员层级列表</returns>
        public int GetCustomerTeaCount(int levelCustomerSysNo)
        {
            //string[] letterArrStr = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

            var list = Using<ICrCustomer>().GetCustomerTeaCount(levelCustomerSysNo).ToList();

            //if (!list.Any()) return list;
            //var minLevel = list.Min(p => p.Level);

            int teaCount = 0;


            foreach (var item in list)
            {
                teaCount += item.LevelTeamCount;
            }

            return teaCount;
        }
        #endregion
    }
}
