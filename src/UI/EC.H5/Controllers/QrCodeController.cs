using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Web.Mvc;
using EC.Application.Tables.CRM;
using EC.Application.Tables.WeiXin;
using EC.Entity;
using EC.Entity.Tables.CRM;
using EC.Libraries.Util;

namespace EC.H5.Controllers
{
    public class QrCodeController : BaseController
    {
        //
        // GET: /QrCode/

        /// <summary>
        /// 二维码页面
        /// </summary>
        /// <returns>视图</returns>
        public ActionResult Index()
        {
            var customer = CustomerContext.Context;

            var fileName = string.Format("{0}.png", customer.SysNo);
            var dertory = string.Format("\\UploadFiles{0}\\{1}", "\\WeiXinQRCode", fileName);
            var imagePatgh = ConfigurationManager.AppSettings["ImagePatgh"];
            var fullFileName = string.Format("{0}{1}", imagePatgh, dertory);
            var imageDomain = ConfigurationManager.AppSettings["ImageDomain"];
            try
            {
                if (!System.IO.File.Exists(fullFileName))
                {
                    //创建文件夹，保存文件
                    var path = Path.GetDirectoryName(fullFileName);

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    var appKey = ConfigurationManager.AppSettings["appid"];
                    var appSecret = ConfigurationManager.AppSettings["appSecret"];

                    var originalImageUri = Server.MapPath(@"/Theme/images/1000n.png");
                    var headImageUri = customer.HeadImgUrl.Contains("http:") ? customer.HeadImgUrl : string.Format("{0}{1}", imageDomain, customer.HeadImgUrl);
                    var qrcodeImageUri = WeiXinApp.Instance.GetTicketWidthSceneStr(appKey, appSecret, customer.SysNo.ToString());

                    ViewBag.originalImageUri = originalImageUri;
                    ViewBag.headImageUri = headImageUri;
                    ViewBag.qrcodeImageUri = qrcodeImageUri;

                    Image originalImage = Image.FromFile(originalImageUri);
                    //重新定义画布
                    Graphics g = Graphics.FromImage(originalImage);

                    if (!string.IsNullOrWhiteSpace(qrcodeImageUri))
                    {
                        try
                        {
                            Image qrcodeImage = ImageUtil.GetImageByUri(qrcodeImageUri);
                            g.DrawImage(qrcodeImage, 183, 855, 275, 240);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("二维码" + ex.Message);
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(customer.HeadImgUrl))
                    {
                        try
                        {
                            Image headImage = ImageUtil.GetImageByUri(headImageUri);
                            if (headImage != null)
                            {
                                g.DrawImage(headImage, 170, 620, 65, 65);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("头像" + ex.Message);
                        }

                    }
                    if (!string.IsNullOrWhiteSpace(customer.Nickname))
                    {
                        g.DrawString(customer.Nickname, new Font("微软雅黑", 26), new SolidBrush(Color.FloralWhite), 320, 605);
                    }
                    GC.Collect();
                    originalImage.Save(fullFileName, System.Drawing.Imaging.ImageFormat.Png);
                    originalImage.Dispose();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Err = ex.Message;
            }
            
            ViewBag.ImgUrl = string.Format("{0}/UploadFiles{1}/{2}", imageDomain, "/WeiXinQRCode", fileName);

            return View();
        }

        /// <summary>
        /// 绑定粉丝
        /// </summary>
        /// <param name="referrerSysNo">推荐编号</param>
        /// <returns>视图</returns>
        public ActionResult BindFans(int referrerSysNo)
        {
            var customer = CustomerApp.Instance.Get(referrerSysNo);

            if (customer == null)
            {
                return View("Error", new JResult() { Message = "信息参数不合法" });
            }

            var openId = CookieUtil.Get("OpenId");
            var headImgUrl = CookieUtil.Get("HeadImgUrl");
            var nickname = CookieUtil.Get("Nickname");

            var customerExt = CustomerApp.Instance.GetByOpenId(openId);

            if (customerExt != null)
            {
                return View("Error", new JResult() { Message = "用户微信账号已注册" });
            }

            var recommend = RecommendApp.Instance.GetByopenId(openId);

            if (recommend != null)
            {
                return Redirect("/Account/Register");
            }

            var result = RecommendApp.Instance.Insert(new CrRecommend()
            {
                Openid = openId,
                ReferrerSysNo = referrerSysNo,
                //Nickname = nickname,
                //HeadImgUrl = headImgUrl
            });

            if (!result.Status)
            {
                return View("Error", new JResult() { Message = "绑定粉丝失败" });
            }

            return Redirect("/Account/Register");
        }
    }
}
