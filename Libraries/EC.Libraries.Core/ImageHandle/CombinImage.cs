using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Wth.Amps.Core.ImageHandle
{
    /// <summary>
    /// 合成图片
    /// </summary>
    public class CombinImage
    {
        /// <summary>
        /// 会产生graphics异常的PixelFormat
        /// </summary>
        private static readonly PixelFormat[] IndexedPixelFormats = { 
        PixelFormat.Undefined, PixelFormat.DontCare,PixelFormat.Format16bppArgb1555, PixelFormat.Format1bppIndexed,PixelFormat.Format4bppIndexed,PixelFormat.Format8bppIndexed
                                                                    };

        /// <summary>
        /// 判断图片的PixelFormat 是否在 引发异常的 PixelFormat 之中
        /// </summary>
        /// <param name="imgPixelFormat">原图片的PixelFormat</param>
        /// <returns>是否存在</returns>
        private static bool IsPixelFormatIndexed(PixelFormat imgPixelFormat)
        {
            return IndexedPixelFormats.Contains(imgPixelFormat);
        }

        /// <summary>
        /// 合并两种图片通过本地路径
        /// </summary>
        /// <param name="bgImgPath">背景图本地路径</param>
        /// <param name="destImgPath">目标图片本地路径</param>
        /// <param name="destImgWidth">目标图宽度</param>
        /// <param name="destImgHeight">目标图高度</param>
        /// <param name="x">所绘图左上角的x坐标</param>
        /// <param name="y">所绘图左上角的y坐标</param>
        /// <returns>合成图Image对象</returns>
        public static Image CombinImageByPath(string bgImgPath, string destImgPath, int destImgWidth, int destImgHeight, int x, int y)
        {
            var bgImg = Image.FromFile(bgImgPath);        //背景图片
            var destImg = Image.FromFile(destImgPath);    //目标图片

            return CombinImageToTwo(bgImg, destImg, destImgWidth, destImgHeight, x, y);
        }

        /// <summary>
        /// 合并两种图片通过链接
        /// </summary>
        /// <param name="bgImgUri">背景图链接</param>
        /// <param name="destImgUri">目标图片链接</param>
        /// <param name="destImgWidth">目标图宽度</param>
        /// <param name="destImgHeight">目标图高度</param>
        /// <param name="x">所绘图左上角的x坐标</param>
        /// <param name="y">所绘图左上角的y坐标</param>
        /// <returns>合成图Image对象</returns>
        public static Image CombinImageByUir(string bgImgUri, string destImgUri, int destImgWidth, int destImgHeight, int x, int y)
        {
            var bgImg = ImageManage.GetImageByUri(bgImgUri);
            var destImg = ImageManage.GetImageByUri(destImgUri);

            return CombinImageToTwo(bgImg, destImg, destImgWidth, destImgHeight, x, y);
        }

        /// <summary>
        /// 合并两种图片
        /// </summary>
        /// <param name="bgImg">背景图的Image对象</param>
        /// <param name="destImg">目标图的Image对象</param>
        /// <param name="destImgWidth">目标图宽度</param>
        /// <param name="destImgHeight">目标图高度</param>
        /// <param name="x">所绘图左上角的x坐标</param>
        /// <param name="y">所绘图左上角的y坐标</param>
        /// <returns>合成图Image对象</returns>
        public static Image CombinImageToTwo(Image bgImg, Image destImg, int destImgWidth, int destImgHeight, int x, int y)
        {
            if (destImg.Width != destImgWidth || destImg.Height != destImgHeight)
            {
                destImg = KiResizeImage(destImg, destImgWidth, destImgHeight);
            }

            var g = CreatGraphics(bgImg);
            g.DrawImage(bgImg, 0, 0, bgImg.Width, bgImg.Height);
            g.DrawImage(destImg, x, y, destImg.Width, destImg.Height);

            GC.Collect();
            return bgImg;
        }

        /// <summary>
        /// 创建Graphics
        /// </summary>
        /// <param name="img">图片的Image对象</param>
        /// <returns>Graphics</returns>
        private static Graphics CreatGraphics(Image img)
        {
            Graphics g = null;

            //如果原图片是索引像素格式之列的，则需要转换
            if (IsPixelFormatIndexed(img.PixelFormat))
            {
                var bmp = new Bitmap(img.Width, img.Height, PixelFormat.Format32bppArgb);
                g = Graphics.FromImage(bmp);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;
            }
            else //否则直接操作
            {
                g = Graphics.FromImage(img);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;
            }

            return g;
        }

        /// <summary>    
        /// 重新设置图片尺寸    
        /// </summary>    
        /// <param name="bmp">原始Bitmap</param>    
        /// <param name="newW">新的宽度</param>    
        /// <param name="newH">新的高度</param>    
        /// <returns>处理以后的图片</returns>
        public static Image KiResizeImage(Image bmp, int newW, int newH)
        {
            try
            {
                Image b = new Bitmap(newW, newH);
                var g = Graphics.FromImage(b);
                // 插值算法的质量    
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                g.Dispose();
                return b;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 将byte[]转换为Image
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns>Image</returns>
        public static Image ReadImage(byte[] bytes)
        {
            MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length);
            BinaryFormatter bf = new BinaryFormatter();
            object obj = bf.Deserialize(ms);
            ms.Close();
            return (Image)obj;
        }

        /// <summary>
        /// 将Image转换为byte[]
        /// </summary>
        /// <param name="image">Image</param>
        /// <returns>byte[]</returns>
        public static byte[] ConvertImage(Image image)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, (object)image);
            ms.Close();
            return ms.ToArray();
        }

        public Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }
    }
}
