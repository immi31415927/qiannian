using System;

namespace EC.Libraries.Util
{
    /// <summary>
    /// 加密解密工具
    /// </summary>
    public class EncryptionUtil
    {
        /// <summary>
        /// 对字符串附加随机字符进行MD5加密
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <param name="offset">加密所需的偏移量</param>
        /// <returns>加密后的小写字符串</returns>
        public static string EncryptWithMd5AndSalt(string str, string offset)
        {
            return EncryptWithMd5(offset + str) + ":" + offset;
        }

        /// <summary>
        /// 对字符串进行MD5加密
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <returns>加密后的小写字符串</returns>
        public static string EncryptWithMd5(string str)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "md5").ToLower();
        }

        /// <summary>
        /// 兼容PHP中的MD5
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <returns>加密后的小写字符串</returns>
        public static string EncryptMD5(string str)
        {
            byte[] textBytes = System.Text.Encoding.Default.GetBytes(str);
            try
            {
                System.Security.Cryptography.MD5CryptoServiceProvider cryptHandler;
                cryptHandler = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] hash = cryptHandler.ComputeHash(textBytes);
                string ret = "";
                foreach (byte a in hash){
                    if (a < 16)
                        ret += "0" + a.ToString("x");
                    else
                        ret += a.ToString("x");
                }
                return ret;
            }
            catch
            {
                throw new Exception(" 兼容PHP中的MD5失败");
            }
        } 

        /// <summary>
        /// 对字符串附加随机字符进行MD5加密
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <returns>加密后的小写字符串</returns>
        public static string EncryptWithMd5AndSalt(string str)
        {
            string password = "";
            password = new Random().Next(10000000, 99999999).ToString();
            string salt = EncryptWithMd5(password).Substring(0, 2);
            password = EncryptWithMd5(salt + str) + ":" + salt;
            return password;
        }

        /// <summary>
        /// 对加密字符串（字符串附加随机字符进行MD5加密后的字符串）进行验证
        /// </summary>
        /// <param name="unencrypted">明文</param>
        /// <param name="encrypted">密文</param>
        /// <returns>验证结果</returns>
        public static bool VerifyCiphetrextWithMd5AndSalt(string unencrypted, string encrypted)
        {
            if (!string.IsNullOrEmpty(unencrypted) && !string.IsNullOrEmpty(encrypted))
            {
                string[] stack = encrypted.Split(':');
                if (stack.Length != 2) return false;
                if (EncryptWithMd5(stack[1] + unencrypted) == stack[0])
                {
                    return true;
                }
            }
            return false;
        }
    }
}
