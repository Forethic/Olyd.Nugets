using System.Reflection;

namespace Olyd.Utils
{
    /// <summary>
    /// 程序集助手
    /// </summary>
    public static class AssemblyHelper
    {
        /// <summary>
        /// 获取当前 AppDomain 中所有引用的程序集，并过滤掉不需要的系统程序集。
        /// 默认排除系统程序集（如：System.* 和 Microsoft.*）。
        /// </summary>
        /// <param name="excludePrefixes">需要排除的程序集名称前缀（默认为 System 和 Microsoft）。</param>
        /// <returns>过滤后的程序集集合。</returns>
        public static IEnumerable<Assembly> GetAllReferenceAssembliesWithoutSystem(IEnumerable<string> excludePrefixes = null)
        {
            // 如果没有指定排除的前缀，则默认排除 'System' 和 'Microsoft'
            excludePrefixes ??= new[] { "System", "Microsoft" };

            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => !excludePrefixes.Any(prefix => assembly.FullName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)));
        }
    }
}
