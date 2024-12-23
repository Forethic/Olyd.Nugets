namespace Olyd.History
{
    /// <summary>
    /// 表示对列表的移除操作变更。通过该类可以实现对列表项的移除和撤销移除操作。
    /// </summary>
    /// <typeparam name="T">列表中元素的类型。</typeparam>
    public class ListRemoveChange<T> : BaseChange
    {
        /// <summary>
        /// 要变更的列表集合。
        /// </summary>
        public IList<T> Collection { get; }

        /// <summary>
        /// 被移除的目标元素。
        /// </summary>
        public T Target { get; }

        /// <summary>
        /// 被移除元素的原始索引位置。
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// 初始化一个新的 <see cref="ListRemoveChange{T}"/> 实例。
        /// </summary>
        /// <param name="collection">需要变更的列表。</param>
        /// <param name="target">要移除的目标元素。</param>
        /// <param name="index">目标元素移除前在列表中的索引位置。</param>
        /// <exception cref="ArgumentNullException">如果传入的参数为 null，则抛出异常。</exception>
        public ListRemoveChange(IList<T> collection, T target, int index)
        {
            Collection = collection ?? throw new ArgumentNullException(nameof(collection));
            Target = target ?? throw new ArgumentNullException(nameof(target));
            Index = index;
        }

        /// <summary>
        /// 执行重做操作，从列表中移除目标元素。
        /// </summary>
        public override void Redo() => Collection.Remove(Target);

        /// <summary>
        /// 执行撤销操作，将目标元素恢复到移除前的位置。
        /// </summary>
        public override void Undo()
        {
            Collection.Remove(Target);

            // 如果原始索引无效（例如列表已发生变动），则将元素添加到列表末尾。
            if (Index < 0 || Index >= Collection.Count)
                Collection.Add(Target);
            else
                Collection.Insert(Index, Target); // 将元素插入到移除时的位置
        }
    }
}
