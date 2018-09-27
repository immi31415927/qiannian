using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace EC.Libraries.Util
{
    /// <summary>
    /// DES算法工具类
    /// </summary>
    public class DesUtil
    {
        //默认密钥向量  
        private static readonly byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };


        /// <summary>
        /// DES加密字符串  待加密的字符串   加密密钥,要求为8位  加密成功返回加密后的字符串，失败返回源串
        /// </summary>
        /// <param name="encryptString">需加密的字符串</param>
        /// <param name="encryptKey">加密密钥</param>
        /// <returns>加密字符串</returns>
        public static string EncryptDes(string encryptString, string encryptKey = "Des58098")
        {
            try
            {
                if (string.IsNullOrEmpty(encryptString))
                {
                    return encryptString;
                }
                var rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
                var rgbIv = Keys;
                var inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                var dCsp = new DESCryptoServiceProvider();
                var mStream = new MemoryStream();
                var cStream = new CryptoStream(mStream, dCsp.CreateEncryptor(rgbKey, rgbIv), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch
            {
                return encryptString;
            }
        }

        /// <summary>
        /// DES解密字符串          
        /// 待解密的字符串  
        /// 解密密钥,要求为8位,和加密密钥相同  
        /// 解密成功返回解密后的字符串，失败返源串 
        /// </summary>
        /// <param name="decryptString">需解密的字符串</param>
        /// <param name="decryptKey">解密密钥</param>
        /// <returns>解密字符串</returns>
        public static string DecryptDes(string decryptString, string decryptKey = "Des58098")
        {
            try
            {
                if (string.IsNullOrEmpty(decryptString))
                {
                    return decryptString;
                }
                var rgbKey = Encoding.UTF8.GetBytes(decryptKey);
                var rgbIv = Keys;
                var inputByteArray = Convert.FromBase64String(decryptString);
                var dcsp = new DESCryptoServiceProvider();
                var mStream = new MemoryStream();
                var cStream = new CryptoStream(mStream, dcsp.CreateDecryptor(rgbKey, rgbIv), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return decryptString;
            }
        }
    }
}
