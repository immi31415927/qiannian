using System.Drawing;
using System.IO;
using ThoughtWorks.QRCode.Codec;
using ThoughtWorks.QRCode.Codec.Data;

namespace EC.Libraries.Core.QRCodeHelper
{
    /// <summary>
    /// 二维码生成
    /// </summary>
    public class QrCodeManage
    {
        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="value">生成参数</param>
        /// <param name="encoding">尺寸</param>
        /// <param name="level">大小</param>
        /// <param name="version">版本</param>
        /// <param name="scale">比例</param>
        /// <returns>图片对象</returns>
        public static Image CreateCode(string value, string encoding = "Byte", string level = "M", int version = 8, int scale = 4)
        {
            var qrCodeEncoder = new QRCodeEncoder
            {
                QRCodeScale = scale,
                QRCodeVersion = version
            };

            switch (encoding)
            {
                case "Byte":
                    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                    break;
                case "AlphaNumeric":
                    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.ALPHA_NUMERIC;
                    break;
                case "Numeric":
                    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.NUMERIC;
                    break;
                default:
                    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                    break;
            }

            switch (level)
            {
                case "L":
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
                    break;
                case "M":
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
                    break;
                case "Q":
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.Q;
                    break;
                default:
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;
                    break;
            }

            //文字生成图片对象
            Image image = qrCodeEncoder.Encode(value);

            return image;
        }

        /// <summary>
        /// 二维码解码
        /// </summary>
        /// <param name="filePath">图片路径</param>
        /// <returns>解码信息</returns>
        public static string CodeDecoderByPath(string filePath)
        {
            return !File.Exists(filePath) ? null : CodeDecoder(Image.FromFile(filePath));
        }

        /// <summary>
        /// 二维码解码
        /// </summary>
        /// <param name="img">二维码图片对象</param>
        /// <returns>解码信息</returns>
        public static string CodeDecoder(Image img)
        {
            var myBitmap = new Bitmap(img);
            var decoder = new QRCodeDecoder();
            var decodedString = decoder.decode(new QRCodeBitmapImage(myBitmap));
            return decodedString;
        }
    }
}
