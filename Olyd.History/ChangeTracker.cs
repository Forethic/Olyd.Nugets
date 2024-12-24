namespace Olyd.History
{
    /// <summary>
    /// 变更跟踪器，用于捕获一组与历史记录相关的变更操作，
    /// 并在操作完成后自动提交到操作队列或父级跟踪器。
    /// </summary>
    public class ChangeTracker : IDisposable
    {
        /// <summary>
        /// 当前激活的 ChangeTracker 实例。
        /// 用于支持嵌套的操作跟踪。
        /// </summary>
        private static ChangeTracker _current = null;

        /// <summary>
        /// 变更前选中的对象列表。
        /// </summary>
        public IList<object> BeforeSelectedItems { get; private set; }

        /// <summary>
        /// 变更后选中的对象列表。
        /// </summary>
        public IList<object> AfterSelectedItems { get; private set; }

        /// <summary>
        /// 父级跟踪器，用于嵌套的操作跟踪。
        /// </summary>
        private ChangeTracker _parent;

        /// <summary>
        /// 当前跟踪的变更列表。
        /// </summary>
        private List<BaseChange> _changes;

        /// <summary>
        /// 变更个数
        /// </summary>
        public int ChangesCount => _changes.Count;

        private static readonly object _lock = new();

        /// <summary>
        /// 初始化 ChangeTracker 实例。
        /// </summary>
        /// <param name="selectedItems">当前选中的对象列表。</param>
        public ChangeTracker(IList<object> selectedItems)
        {
            // 初始化选中对象列表
            ChangeSelectedItems(true, selectedItems);  // 变更前选中的对象
            ChangeSelectedItems(false, selectedItems); // 变更后选中的对象

            _changes = new List<BaseChange>(); // 初始化变更列表

            lock (_lock)
            {
                _parent = _current;  // 保存当前激活的父级跟踪器
                _current ??= this;   // 设置当前 ChangeTracker 为激活状态
            }
        }

        /// <summary>
        /// 析构函数，确保在对象被垃圾回收之前调用 Dispose。
        /// </summary>
        ~ChangeTracker()
        {
            Dispose(false);  // 调用 Dispose 来处理资源释放
        }

        /// <summary>
        /// 更改选中的对象列表。
        /// </summary>
        /// <param name="isBefore">是否为变更前的对象列表。</param>
        /// <param name="selectedItems">选中的对象列表。</param>
        private void ChangeSelectedItems(bool isBefore, IEnumerable<object> selectedItems)
        {
            // 将 selectedItems 转换为 List 对象，确保是一个新的列表
            List<object> list = selectedItems?.ToList() ?? new List<object>();

            if (isBefore)
                BeforeSelectedItems = list;
            else
                AfterSelectedItems = list;
        }

        /// <summary>
        /// 更改变更后的选中对象列表。
        /// </summary>
        /// <param name="selectedItems">新的选中对象。</param>
        public void ChangeAfterSelectedItems(params object[] selectedItems)
        {
            // 更新变更后的选中对象列表
            ChangeSelectedItems(false, selectedItems);
        }

        /// <summary>
        /// 添加一个变更操作到当前跟踪器。
        /// </summary>
        /// <param name="change">要添加的变更操作。</param>
        public void AddChange(BaseChange change)
        {
            _changes.Add(change);  // 将变更添加到变更列表
        }

        /// <summary>
        /// 将多个变更操作合并到当前跟踪器。
        /// </summary>
        /// <param name="changes">要合并的变更操作列表。</param>
        private void AddRange(IList<BaseChange> changes)
        {
            _changes.AddRange(changes);  // 合并多个变更
        }

        /// <summary>
        /// 完成跟踪并提交变更。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);  // 释放资源并提交变更
            GC.SuppressFinalize(this);  // 防止运行 finalizer
        }

        private bool _disposed = false;

        /// <summary>
        /// 释放资源并提交变更。
        /// </summary>
        /// <param name="disposing">指示是否释放托管资源。</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;  // 防止重复释放资源

            if (disposing)
            {
                // 如果没有变更，直接退出
                if (_changes.Count == 0)
                {
                    // 这里必须清除当前激活的 ChangeTracker，不然后续无法继续添加变更
                    if (_current == this)
                        _current = null;

                    _disposed = true;
                    return;
                }

                if (_parent == null)
                {
                    // 没有父级跟踪器，将变更提交到历史记录管理器
                    HistoryManager.AddChange(new HistoryItem(BeforeSelectedItems, AfterSelectedItems, _changes));
                    if (_current == this)
                        _current = null; // 清除当前激活的 ChangeTracker
                }
                else
                {
                    // 将变更合并到父级跟踪器
                    _parent.AddRange(_changes);
                }

                _changes.Clear();  // 清空当前的变更列表
            }

            _disposed = true;  // 标记为已释放
        }
    }
}
