namespace Olyd.History
{
    /// <summary>
    /// 表示对列表的添加操作变更。通过该类可以实现对列表项的添加和撤销添加操作。
    /// </summary>
    /// <typeparam name="T">列表中元素的类型。</typeparam>
    public class ListAddChange<T> : BaseChange
    {
        /// <summary>
        /// 要变更的列表集合。
        /// </summary>
        public IList<T> Collection { get; }

        /// <summary>
        /// 要添加到集合中的目标元素。
        /// </summary>
        public T Target { get; }

        /// <summary>
        /// 初始化一个新的 <see cref="ListAddChange{T}"/> 实例。
        /// </summary>
        /// <param name="collection">需要变更的列表。</param>
        /// <param name="target">要添加的目标元素。</param>
        /// <exception cref="ArgumentNullException">如果传入的参数为 null，则抛出异常。</exception>
        public ListAddChange(IList<T> collection, T target)
        {
            Collection = collection ?? throw new ArgumentNullException(nameof(collection));
            Target = target ?? throw new ArgumentNullException(nameof(target));
        }

        /// <summary>
        /// 执行重做操作，添加目标元素到列表中。
        /// </summary>
        public override void Redo()
        {
            if (Collection.Contains(Target))
                return;

            Collection.Add(Target);
        }

        /// <summary>
        /// 执行撤销操作，从列表中移除目标元素。
        /// </summary>
        public override void Undo()
        {
            if (Collection.Contains(Target))
                Collection.Remove(Target);
        }
    }

}
