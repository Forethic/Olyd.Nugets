namespace Olyd.History
{
    /// <summary>
    /// 表示字典移除操作的变更，用于支持撤销和重做功能。
    /// </summary>
    /// <typeparam name="TKey">字典键的类型。</typeparam>
    /// <typeparam name="TValue">字典值的类型。</typeparam>
    public class DictionaryRemoveChange<TKey, TValue> : BaseChange
    {
        /// <summary>
        /// 操作的目标字典。
        /// </summary>
        public Dictionary<TKey, TValue> Dictionary { get; }

        /// <summary>
        /// 被移除的键。
        /// </summary>
        public TKey Key { get; }

        /// <summary>
        /// 被移除的值。
        /// </summary>
        public TValue Value { get; }

        /// <summary>
        /// 初始化字典移除操作的变更实例。
        /// </summary>
        /// <param name="dictionary">目标字典。</param>
        /// <param name="key">被移除的键。</param>
        /// <param name="value">被移除的值。</param>
        /// <exception cref="ArgumentNullException">
        /// 如果 <paramref name="dictionary"/>、<paramref name="key"/> 或 <paramref name="value"/> 为 null。
        /// </exception>
        public DictionaryRemoveChange(Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            Dictionary = dictionary ?? throw new ArgumentNullException(nameof(dictionary));
            Key = key ?? throw new ArgumentNullException(nameof(key));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// 重做移除操作，将指定的键从字典中移除。
        /// </summary>
        public override void Redo()
        {
            if (Dictionary.ContainsKey(Key))
            {
                Dictionary.Remove(Key);
            }
        }

        /// <summary>
        /// 撤销移除操作，将键值对重新添加到字典中。
        /// </summary>
        public override void Undo()
        {
            if (!Dictionary.ContainsKey(Key))
            {
                Dictionary.Add(Key, Value);
            }
        }
    }

}
