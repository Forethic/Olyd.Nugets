namespace Olyd.History
{
    /// <summary>
    /// 表示字典添加操作的变更，用于支持撤销和重做功能。
    /// </summary>
    /// <typeparam name="TKey">字典键的类型。</typeparam>
    /// <typeparam name="TValue">字典值的类型。</typeparam>
    public class DictionaryAddChange<TKey, TValue> : BaseChange
    {
        /// <summary>
        /// 操作的目标字典。
        /// </summary>
        public Dictionary<TKey, TValue> Dictionary { get; }

        /// <summary>
        /// 要添加的键。
        /// </summary>
        public TKey Key { get; }

        /// <summary>
        /// 要添加的值。
        /// </summary>
        public TValue Value { get; }

        /// <summary>
        /// 初始化字典添加操作的变更实例。
        /// </summary>
        /// <param name="dictionary">目标字典。</param>
        /// <param name="key">要添加的键。</param>
        /// <param name="value">要添加的值。</param>
        /// <exception cref="ArgumentNullException">
        /// 如果 <paramref name="dictionary"/>、<paramref name="key"/> 或 <paramref name="value"/> 为 null。
        /// </exception>
        public DictionaryAddChange(Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            Dictionary = dictionary ?? throw new ArgumentNullException(nameof(dictionary));
            Key = key ?? throw new ArgumentNullException(nameof(key));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// 重做添加操作，将键值对重新添加到字典中。
        /// </summary>
        public override void Redo()
        {
            if (!Dictionary.ContainsKey(Key))
            {
                Dictionary.Add(Key, Value);
            }
        }

        /// <summary>
        /// 撤销添加操作，从字典中移除指定的键值对。
        /// </summary>
        public override void Undo()
        {
            if (Dictionary.ContainsKey(Key))
            {
                Dictionary.Remove(Key);
            }
        }
    }
}
