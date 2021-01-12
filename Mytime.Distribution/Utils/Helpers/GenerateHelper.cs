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
    }
}