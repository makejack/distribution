using System.Text;
using System;
using System.Security.Cryptography;
using IdGen;

namespace Mytime.Distribution.Utils.Helpers
{
    /// <summary>
    /// 生成帮助
    /// </summary>
    public class GenerateHelper
    {
        const int GEN_ORDER_NO_ID = 0;
        static IdGenerator _genOrderNo = new IdGenerator(GEN_ORDER_NO_ID);

        /// <summary>
        /// 生成刷新Token
        /// </summary>
        /// <returns></returns>
        public static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        /// <summary>
        /// 生成订单号
        /// </summary>
        /// <returns></returns>
        public static long GenOrderNo()
        {
            return _genOrderNo.CreateId();
        }


        /// <summary>
        /// 验证码
        /// </summary>
        /// <returns></returns>
        public static string VerifyCode(int length)
        {
            StringBuilder sb = new StringBuilder();
            Random r = new Random();
            for (int i = 0; i < length; i++)
            {
                sb.Append(r.Next(0, 9));
            }
            return sb.ToString();
        }
    }
}