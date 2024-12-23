namespace Olyd.History
{
    /// <summary>
    /// 表示一个历史记录项，包含一组更改以及操作前后选择项的快照。
    /// </summary>
    public class HistoryItem
    {
        /// <summary>
        /// 操作前的选择项快照。
        /// </summary>
        private readonly IList<object> _beforeSelectedItems;

        /// <summary>
        /// 操作后的选择项快照。
        /// </summary>
        private readonly IList<object> _afterSelectedItems;

        /// <summary>
        /// 本历史记录包含的所有变更操作。
        /// </summary>
        private readonly List<BaseChange> _changes;

        /// <summary>
        /// 初始化历史记录项的实例。
        /// </summary>
        /// <param name="beforeSelectedItems">操作前的选择项快照。</param>
        /// <param name="afterSelectedItems">操作后的选择项快照。</param>
        /// <param name="changes">操作包含的变更列表。</param>
        public HistoryItem(IList<object> beforeSelectedItems, IList<object> afterSelectedItems, List<BaseChange> changes)
        {
            _beforeSelectedItems = beforeSelectedItems ?? new List<object>();
            _afterSelectedItems = afterSelectedItems ?? new List<object>();
            _changes = changes ?? new List<BaseChange>();
        }

        /// <summary>
        /// 撤销此历史记录项的操作。
        /// </summary>
        /// <returns>撤销后操作前的选择项快照。</returns>
        public IList<object> Undo()
        {
            // 按变更记录的顺序逆序撤销操作。
            for (int i = _changes.Count - 1; i >= 0; i--)
            {
                _changes[i].Undo();
            }

            return _beforeSelectedItems;
        }

        /// <summary>
        /// 重做此历史记录项的操作。
        /// </summary>
        /// <returns>重做后操作后的选择项快照。</returns>
        public IList<object> Redo()
        {
            // 按变更记录的顺序重新执行操作。
            foreach (var change in _changes)
            {
                change.Redo();
            }

            return _afterSelectedItems;
        }
    }

}
