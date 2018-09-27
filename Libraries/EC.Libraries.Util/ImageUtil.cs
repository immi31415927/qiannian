using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;

namespace EC.Libraries.Util
{
    /// <summary>
    /// 图片工具类
    /// </summary>
    public class ImageUtil
    {
        /// <summary>
        /// 略缩图类型
        /// </summary>
        public enum ThumbnailMode
        {
            /// <summary>
            /// 固定宽度，高度做相应计算
            /// </summary>
            Width,

            /// <summary>
            /// 固定高度，宽度做相应计算
            /// </summary>
            Height,

            /// <summary>
            /// 固定宽度和高度
            /// </summary>
            WidthHeighLimitted,
            /// <summary>
            /// 指定高宽裁减（不变形） yhy
            /// </summary>
            Cut
        }

        /// <summary>
        /// 创建高清缩略图
        /// </summary>
        /// <param name="imgStream">原图路径</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <param name="mode">尺寸模式</param>
        /// <returns>图片流</returns>
        public static MemoryStream CreateThumbnail(Stream imgStream, int width, int height, ThumbnailMode mode)
        {
            MemoryStream ms = new MemoryStream();

            using (Image source = Image.FromStream(imgStream))
            {
                #region 计算坐标和宽高

                int x = 0;
                int y = 0;
                int ow = source.Width;
                int oh = source.Height;

                switch (mode)
                {
                    case ThumbnailMode.Width:
                        height = (int)Math.Round(source.Height * ((double)width / source.Width));
                        break;
                    case ThumbnailMode.Height:
                        width = (int)Math.Round(source.Width * ((double)height / source.Height));
                        break;
                    case ThumbnailMode.Cut://指定高宽裁减（不变形）  yhy              
                        if ((double)source.Width / (double)source.Height > (double)width / (double)height)
                        {
                            oh = source.Height;
                            ow = source.Height * width / height;
                            y = 0;
                            x = (source.Width - ow) / 2;
                        }
                        else
                        {
                            ow = source.Width;
                            oh = source.Width * height / width;
                            x = 0;
                            y = (source.Height - oh) / 2;
                        }
                        break;
                }

                #endregion

                #region 尺寸不变则直接返回

                if (width == ow && height == oh)
                {
                    imgStream.Position = 0;
                    imgStream.CopyTo(ms);
                    return ms;
                }

                #endregion

                #region 生成缩略图

                using (Bitmap bmp = new Bitmap(width, height))
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.DrawImage(source, new Rectangle(0, 0, width, height), new Rectangle(x, y, ow, oh), GraphicsUnit.Pixel);

                    EncoderParameters parameters = new EncoderParameters();
                    parameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, new long[] { 100 });

                    string lookupkey = "image/jpeg";
                    var codecjpg = ImageCodecInfo.GetImageEncoders().Where(i => i.MimeType.Equals(lookupkey)).FirstOrDefault();

                    bmp.Save(ms, codecjpg, parameters);
                }

                #endregion
            }
            return ms;
        }

        /// <summary>
        /// 将图片转换为jpg格式
        /// </summary>
        /// <param name="imgStream">文件流</param>
        /// <returns>转换后的文件</returns>
        public static MemoryStream ConvertToJpg(Stream imgStream)
        {
            using (Image source = Bitmap.FromStream(imgStream))
            {
                if (source.RawFormat.Guid == ImageFormat.Jpeg.Guid)
                {
                    imgStream.Seek(0, SeekOrigin.Begin);
                    byte[] buffer = new byte[imgStream.Length];
                    imgStream.Read(buffer, 0, buffer.Length);
                    imgStream.Seek(0, SeekOrigin.Begin);

                    return new MemoryStream(buffer);
                }
                else
                {
                    var ms = new MemoryStream();
                    source.Save(ms, ImageFormat.Jpeg);
                    return ms;

                }
            }
        }

        /// <summary>
        /// 流转换为Bytes
        /// </summary>
        /// <param name="stream">数据流</param>
        /// <returns>图片字节数组</returns>
        public static byte[] StreamConvertToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Position = 0;
            stream.Read(bytes, 0, bytes.Length);
            return bytes;
        }

        /// <summary>
        /// 通过链接获取Image对象
        /// </summary>
        /// <param name="strUri">链接</param>
        /// <returns>Image对象</returns>
        public static Image GetImageByUri(string strUri)
        {
            Image myImage = null;
            var request = HttpWebRequest.Create(strUri);
            using (var response = request.GetResponse())
            {
                var stream = response.GetResponseStream();

                if (stream != null)
                {
                    myImage = new Bitmap(stream);
                }
            }

            return myImage;
        }
    }
}
