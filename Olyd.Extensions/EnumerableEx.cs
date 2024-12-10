namespace Olyd.Extensions
{
    public static class EnumerableEx
    {
        /// <summary>
        /// 扩展方法：对集合中的每个元素执行指定的操作。
        /// </summary>
        /// <typeparam name="T">集合中的元素类型。</typeparam>
        /// <param name="enumerable">要操作的集合。</param>
        /// <param name="action">要执行的操作。</param>
        public static void Foreach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            // 检查集合或操作是否为 null。如果为 null，则不执行任何操作。
            if (enumerable == null || action == null)
                return;

            // 遍历集合中的每个元素，并执行指定的操作。
            foreach (var item in enumerable)
            {
                action(item);
            }
        }
    }
}
