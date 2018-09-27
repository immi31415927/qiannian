using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Libraries.Util
{
    /// <summary>
    /// 生成验证码的工具类
    /// </summary>
    public class ValidateCodeUtil
    {
        /// <summary>
        /// 验证码的最大长度
        /// </summary>
        public int MaxLength
        {
            get { return 10; }
        }

        /// <summary>
        /// 验证码的最小长度
        /// </summary>
        public int MinLength
        {
            get { return 1; }
        }

        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <param name="length">指定验证码的长度</param>
        /// <returns>验证码</returns>
        public string CreateValidateCode(int length)
        {
            return SetRandomValue(length, null);
        }

        /// <summary>
        /// 设置随机参数
        /// </summary>
        /// <param name="length">长度</param>
        /// <param name="format">格式（l:字母 n:数字 比如格式 llnnnn）</param>
        /// <returns>随机参数</returns>
        public static string SetRandomValue(int length, string format)
        {
            const string numStr = "0123456789";
            const string letterStr = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var value = "";
            var rd = new Random();

            if (string.IsNullOrWhiteSpace(format))
            {
                const string setStr = numStr + letterStr;

                for (var i = 0; i < length; i++)
                {
                    value += setStr[rd.Next(setStr.Length)];
                }
            }
            else
            {
                for (var i = 0; i < format.Length; i++)
                {
                    var type = format.Substring(i, 1);
                    if (type.ToLower().Equals("l"))
                    {
                        value += letterStr[rd.Next(letterStr.Length)];
                    }
                    if (type.ToLower().Equals("n"))
                    {
                        value += numStr[rd.Next(numStr.Length)];
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// 创建验证码的图片
        /// </summary>
        /// <param name="validateCode">验证码</param>
        public byte[] CreateValidateGraphic(string validateCode)
        {
            var image = new Bitmap((int)Math.Ceiling(validateCode.Length * 12.0), 22);
            var g = Graphics.FromImage(image);
            try
            {
                //生成随机生成器
                var random = new Random();
                //清空图片背景色
                g.Clear(Color.White);
                //画图片的干扰线
                for (var i = 0; i < 25; i++)
                {
                    var x1 = random.Next(image.Width);
                    var x2 = random.Next(image.Width);
                    var y1 = random.Next(image.Height);
                    var y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }
                var font = new Font("Arial", 12, (FontStyle.Bold | FontStyle.Italic));
                var brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(validateCode, font, brush, 3, 2);
                //画图片的前景干扰点
                for (var i = 0; i < 100; i++)
                {
                    var x = random.Next(image.Width);
                    var y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }
                //画图片的边框线
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                //保存图片数据
                var stream = new MemoryStream();
                image.Save(stream, ImageFormat.Jpeg);
                //输出图片流
                return stream.ToArray();
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }

        /// <summary>
        /// 得到验证码图片的长度
        /// </summary>
        /// <param name="validateNumLength">验证码的长度</param>
        /// <returns></returns>
        public static int GetImageWidth(int validateNumLength)
        {
            return (int)(validateNumLength * 12.0);
        }

        /// <summary>
        /// 得到验证码的高度
        /// </summary>
        /// <returns></returns>
        public static double GetImageHeight()
        {
            return 22.5;
        }
    }
}
