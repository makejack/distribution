using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Mytime.Distribution.Extensions
{
    /// <summary>
    /// enum 扩展
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// 获取Enum的描述
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this System.Enum value)
        {
            return value.GetType()
                .GetMember(value.ToString())
                .FirstOrDefault()?
                .GetCustomAttribute<DescriptionAttribute>()?
                .Description;
        }
    }
}