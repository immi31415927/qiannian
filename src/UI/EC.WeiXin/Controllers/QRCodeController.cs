using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EC.Application.Tables.WeiXin;
using EC.Libraries.Util;

namespace EC.WeiXin.Controllers
{
    public class QRCodeController : Controller
    {
        //
        // GET: /QRCode/

        /// <summary>
        /// 微信公众号WeixinAppId
        /// </summary>
        private readonly string _weixinAppId = ConfigurationManager.AppSettings["WeixinAppId"];

        /// <summary>
        /// 微信公众号AppSecret
        /// </summary>
        private readonly string _weixinAppSecret = ConfigurationManager.AppSettings["WeixinAppSecret"];

        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string str)
        {
            var urlStr = WeiXinApp.Instance.GetTicketWidthSceneStr(_weixinAppId, _weixinAppSecret, str);
            var img = ImageUtil.GetImageByUri(urlStr);


            var uploadDate = DateTime.Now.ToString("yyyyMMdd");
            var fileName = string.Format("{0}.png", DateTime.Now.ToString("yyyyMMddHHmmssffff"));
            var dertory = string.Format("/UploadFiles{0}/{1}/{2}", "/WeiXinQRCode", uploadDate, fileName);
            var fullFileName = Server.MapPath(dertory);
            //创建文件夹，保存文件
            var path = Path.GetDirectoryName(fullFileName);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            img.Save(fullFileName, System.Drawing.Imaging.ImageFormat.Png);
            img.Dispose();

            ViewBag.ImgUrl = dertory;

            return View();
        }
    }
}