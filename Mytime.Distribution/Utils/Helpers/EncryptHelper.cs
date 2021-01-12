using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Mytime.Distribution.Utils.Helpers
{
    /// <summary>
    /// 加密帮助
    /// </summary>
    public class EncryptHelper
    {
        /// <summary>
        /// MD5 Encrypt
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string MD5ToBase64(string str, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] hashBytes = md5.ComputeHash(encoding.GetBytes(str));
            return Convert.ToBase64String(hashBytes);
        }

        /// <summary>
        /// MD5 Encrypt
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string MD5ToBase64(byte[] bytes)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] hashBytes = md5.ComputeHash(bytes);
            return Convert.ToBase64String(hashBytes);
        }

        /// <summary>
        /// MD5 Encrypt
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string MD5ToBase64(Stream stream)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] hashBytes = md5.ComputeHash(stream);
            return Convert.ToBase64String(hashBytes);
        }

        /// <summary>
        /// Md5转十六进制
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string MD5ToHex(string str, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] hashBytes = md5.ComputeHash(encoding.GetBytes(str));
            StringBuilder hex = new StringBuilder();
            foreach (var item in hashBytes)
            {
                hex.Append($"{item:X2}");
            }
            return hex.ToString();
        }
    }
}