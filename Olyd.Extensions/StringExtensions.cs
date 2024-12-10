namespace Olyd.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// 扩展方法：检查字符串是否为 null 或者只包含空白字符。
        /// </summary>
        /// <param name="str">要检查的字符串。</param>
        /// <returns>如果字符串为 null 或者只包含空白字符，则返回 true；否则返回 false。</returns>
        public static bool IsNullOrWhiteSpace(this string str)
            => string.IsNullOrWhiteSpace(str);

        /// <summary>
        /// 扩展方法：检查字符串是否为 null 或者为空字符串。
        /// </summary>
        /// <param name="str">要检查的字符串。</param>
        /// <returns>如果字符串为 null 或者为空字符串，则返回 true；否则返回 false。</returns>
        public static bool IsNullOrEmpty(this string str)
            => string.IsNullOrEmpty(str);
    }
}
