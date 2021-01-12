using Mytime.Distribution.Utils.Helpers;

namespace Mytime.Distribution.Extensions
{
    /// <summary>
    /// 字符串扩展
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 字符串转md5
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToMD5Base64(this string s)
        {
            return EncryptHelper.MD5ToBase64(s);
        }
    }
}