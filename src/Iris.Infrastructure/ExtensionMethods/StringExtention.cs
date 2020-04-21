using System;
using System.Security.Cryptography;
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


        /// <summary>
        /// 获取MD5,去掉-，结果小写
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encoding">默认UTF8</param>
        /// <returns></returns>
        public static string MD5(this string str, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            var clearBytes = encoding.GetBytes(str);
            var hashedBytes = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(clearBytes);
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }
    }
}
