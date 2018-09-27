using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Wth.Amps.Core.ImageHandle
{
    /// <summary>
    /// 图形管理
    /// </summary>
    public class ImageManage
    {
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
        /// <summary>
        /// 读取url数据
        /// </summary>
        /// <param name="url">url</param>
        /// <returns>字节数组</returns>
        public static byte[] GetUriBytes(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            using (var webResponse = (HttpWebResponse)request.GetResponse())
            {
                return GetWebResponseData(webResponse);
            }
        }

        private static byte[] GetWebResponseData(HttpWebResponse response)
        {
            using (var stream = response.GetResponseStream())
            {
                if (response.ContentEncoding.ToLower().Contains("gzip"))
                {
                    using (var gZipStream = new System.IO.Compression.GZipStream(stream, System.IO.Compression.CompressionMode.Decompress))
                    {
                        var bytes = ReadFully(gZipStream);
                        return bytes;
                    }
                }
                else
                {
                    var bytes = ReadFully(stream);
                    return bytes;
                }
            }
        }
       
        private static byte[] ReadFully(Stream stream)
        {
            byte[] buffer = new byte[128];
            byte[] result;
            using (var ms = new MemoryStream())
            {
                while (true)
                {
                    int read = stream.Read(buffer, 0, buffer.Length);
                    if (read <= 0)
                    {
                        break;
                    }
                    ms.Write(buffer, 0, read);
                }
                result = ms.ToArray();
            }
            return result;
        }

        /// <summary>
        /// 设置图片属性值（暂时只支持修改时间属性）
        /// </summary>
        /// <param name="newImage">图片对象</param>
        /// <param name="propertyId">属性项Id</param>
        /// <param name="value">修改值</param>
        public static void SetImagePropertyValue(Image newImage, int propertyId, object value)
        {
            var item = newImage.GetPropertyItem(propertyId);
            byte[] data = null;

            if (item.Type == 2)
            {
                data = ConvertDateTimeValue((DateTime)value);
            }

            if (data == null)
            {
                return;
            }

            var propItem = newImage.PropertyItems.FirstOrDefault(sub => sub.Id == propertyId);

            if (propItem != null)
            {
                newImage.RemovePropertyItem(propertyId);
            }

            propItem = newImage.PropertyItems[0];
            propItem.Type = item.Type;
            propItem.Id = propertyId;
            propItem.Len = data.Length;
            propItem.Value = data;
            newImage.SetPropertyItem(propItem);
        }

        /// <summary>
        /// 时间类型转换为byte
        /// </summary>
        /// <param name="value">时间</param>
        /// <returns>byte</returns>
        private static byte[] ConvertDateTimeValue(DateTime value)
        {
            var result = new byte[20];
            var tempvalue = value.ToString("yyyy:MM:dd HH:mm:ss");
            var midresult = Encoding.ASCII.GetBytes(tempvalue);
            for (var i = 0; i < midresult.Length; i++)
            {
                result[i] = midresult[i];
            }
            return result;
        }

        /// <summary>
        /// 按指定的压缩质量及格式保存图片（微软的Image.Save方法保存到图片压缩质量为75)
        /// </summary>
        /// <param name="sourceImage">要保存的图片的Image对象</param>
        /// <param name="savePath">图片要保存的绝对路径</param>
        /// <param name="imageQualityValue">图片要保存的压缩质量，该参数的值为1至100的整数，数值越大，保存质量越好</param>
        /// <returns>保存成功，返回true；反之，返回false</returns>
        public static bool SaveImageForSpecifiedQuality(Image sourceImage, string savePath, int imageQualityValue)
        {
            //以下代码为保存图片时，设置压缩质量
            var encoderParameters = new EncoderParameters();
            var encoderParameter = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, imageQualityValue);
            encoderParameters.Param[0] = encoderParameter;
            try
            {
                var imageCodecInfoArray = ImageCodecInfo.GetImageEncoders();
                var jpegImageCodecInfo = imageCodecInfoArray.FirstOrDefault(t => t.FormatDescription.Equals("JPEG"));
                sourceImage.Save(savePath, jpegImageCodecInfo, encoderParameters);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 无损压缩图片
        /// </summary>
        /// <param name="imgbytes">原图片</param>
        /// <param name="dHeight">高度</param>
        /// <param name="dWidth"></param>
        /// <param name="flag">压缩质量 1-100</param>
        /// <returns></returns>
        public static byte[] GetPicThumbnail(byte[] imgbytes, int dHeight, int dWidth, int flag)
        {
            using (MemoryStream ms = new MemoryStream(imgbytes))
            {
                Image iSource = Image.FromStream(ms);
                ImageFormat tFormat = iSource.RawFormat;
                int sW = 0, sH = 0;
                //按比例缩放
                Size tem_size = new Size(iSource.Width, iSource.Height);
                if (tem_size.Width > dHeight || tem_size.Width > dWidth) //将**改成c#中的或者操作符号
                {
                    if ((tem_size.Width * dHeight) > (tem_size.Height * dWidth))
                    {
                        sW = dWidth;
                        sH = (dWidth * tem_size.Height) / tem_size.Width;
                    }
                    else
                    {
                        sH = dHeight;
                        sW = (tem_size.Width * dHeight) / tem_size.Height;
                    }
                }
                else
                {
                    sW = tem_size.Width;
                    sH = tem_size.Height;
                }
                Bitmap ob = new Bitmap(dWidth, dHeight);
                Graphics g = Graphics.FromImage(ob);
                g.Clear(Color.WhiteSmoke);
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(iSource, new Rectangle((dWidth - sW) / 2, (dHeight - sH) / 2, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);
                g.Dispose();

                //以下代码为保存图片时，设置压缩质量
                EncoderParameters ep = new EncoderParameters();
                long[] qy = new long[1];
                qy[0] = flag;//设置压缩的比例1-100
                EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
                ep.Param[0] = eParam;
                try
                {
                    ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                    ImageCodecInfo jpegICIinfo = null;
                    for (int x = 0; x < arrayICI.Length; x++)
                    {
                        if (arrayICI[x].FormatDescription.Equals("JPEG"))
                        {
                            jpegICIinfo = arrayICI[x];
                            break;
                        }
                    }
                    if (jpegICIinfo != null)
                    {
                        return ImgToByt(ob);
                    }
                    else
                    {
                        return null;
                    }
                }
                catch
                {
                    return null;
                }
                finally
                {
                    iSource.Dispose();
                    ob.Dispose();
                }
            }
        }

        /// <summary>
        /// 图片转换成字节流
        /// </summary>
        /// <param name="img">要转换的Image对象</param>
        /// <returns>转换后返回的字节流</returns>
        private static byte[] ImgToByt(Image img)
        {
            MemoryStream ms = new MemoryStream();
            byte[] imagedata = null;
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            imagedata = ms.GetBuffer();
            return imagedata;
        }

        /// <summary>
        /// 图片转换成字节流
        /// </summary>
        /// <param name="image">要转换的Image对象</param>
        /// <returns>转换后返回的字节流</returns>
        public static byte[] ImgToByte(Image image)
        {
            var format = image.RawFormat;
            using (var ms = new MemoryStream())
            {
                if (format.Equals(ImageFormat.Jpeg))
                {
                    image.Save(ms, ImageFormat.Jpeg);
                }
                else if (format.Equals(ImageFormat.Png))
                {
                    image.Save(ms, ImageFormat.Png);
                }
                else if (format.Equals(ImageFormat.Bmp))
                {
                    image.Save(ms, ImageFormat.Bmp);
                }
                else if (format.Equals(ImageFormat.Gif))
                {
                    image.Save(ms, ImageFormat.Gif);
                }
                else if (format.Equals(ImageFormat.Icon))
                {
                    image.Save(ms, ImageFormat.Icon);
                }

                var buffer = new byte[ms.Length];
                ms.Seek(0, SeekOrigin.Begin);
                ms.Read(buffer, 0, buffer.Length);
                return buffer;
            }
        }
    }
}
