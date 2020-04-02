using System;
using System.Collections.Generic;
using System.Text;

namespace Iris.Infrastructure.ExtensionMethods
{
    public static class StringExtension
    {
        /// <summary>
        /// 判断字符串是否可用（是否 不为空）， true = 不为空； false = 为空
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool EnabledStr(this string str)
        {
            return !string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// 判断字符串是否可用（是否 不为空）， true = 不为空； false = 为空
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool EnabledDateTime(this DateTime dateTime)
        {
            return dateTime != DateTime.MinValue;
        }
    }
}
