using System.ComponentModel;

namespace Olyd.History
{
    /// <summary>
    /// 历史管理器：用于记录和管理可撤销与重做的操作。
    /// </summary>
    /// <remarks>
    /// 支持多线程操作的安全性。
    /// </remarks>
    public class HistoryManager : INotifyPropertyChanged
    {
        private static readonly List<HistoryItem> _items = new(); // 存储历史记录项的列表
        private static int _curIndex = -1; // 当前历史记录索引，初始化为-1，表示无可撤销操作
        private static readonly object _lock = new();   // 线程同步锁

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 标识当前是否处于撤销或重做操作中
        /// </summary>
        public static bool InUndoRedo { get; private set; }

        /// <summary>
        /// 单例模式：获取当前的历史管理实例，用于属性变更通知。
        /// </summary>
        public static HistoryManager Instance { get; } = new();

        /// <summary>
        /// 添加一个历史记录项，并移除当前位置之后的所有记录
        /// </summary>
        /// <param name="historyItem">要添加的历史记录项</param>
        public static void AddChange(HistoryItem historyItem)
        {
            if (historyItem == null)
                throw new ArgumentNullException(nameof(historyItem));

            lock (_lock)
            {
                // 如果当前索引在最后，直接添加新记录；否则清除当前位置之后的记录。
                if (_curIndex < _items.Count - 1)
                {
                    var deletedIndex = _curIndex + 1;
                    _items.RemoveRange(deletedIndex, _items.Count - deletedIndex);
                }

                // 添加新的历史记录并更新索引。
                _items.Add(historyItem);
                _curIndex = _items.Count - 1;

                NotifyPropertyChanges();
            }
        }

        /// <summary>
        /// 撤销最近的一次操作。
        /// </summary>
        /// <param name="list">回退操作前的选择项列表</param>
        /// <returns>如果撤销成功返回 true，否则返回 false</returns>
        public static bool Undo(out IList<object> list)
        {
            InUndoRedo = true;
            var result = false;
            list = null;

            lock (_lock)
            {
                if (CanUndo)
                {
                    list = _items[_curIndex--].Undo();
                    result = true;

                    NotifyPropertyChanges();
                }
            }

            InUndoRedo = false;
            return result;
        }

        /// <summary>
        /// 重做最近撤销的一次操作。
        /// </summary>
        /// <param name="list">重做操作后的选择项列表</param>
        /// <returns>如果重做成功返回 true，否则返回 false</returns>
        public static bool Redo(out IList<object> list)
        {
            list = null;
            InUndoRedo = true;
            var result = false;

            lock (_lock)
            {
                if (CanRedo)
                {
                    list = _items[++_curIndex].Redo();
                    result = true;

                    NotifyPropertyChanges();
                }
            }

            InUndoRedo = false;
            return result;
        }

        /// <summary>
        /// 清空所有历史记录
        /// </summary>
        public static void Clear()
        {
            lock (_lock)
            {
                _items.Clear();
                _curIndex = -1;
                NotifyPropertyChanges();
            }
        }

        /// <summary>
        /// 检查是否可以执行撤销操作
        /// </summary>
        public static bool CanUndo
        {
            get
            {
                lock (_lock)
                {
                    return _curIndex >= 0;
                }
            }
        }

        /// <summary>
        /// 检查是否可以执行重做操作
        /// </summary>
        public static bool CanRedo
        {
            get
            {
                lock (_lock)
                {
                    return _curIndex < _items.Count - 1;
                }
            }
        }

        /// <summary>
        /// 通知属性变更。
        /// </summary>
        private static void NotifyPropertyChanges()
        {
            var manager = Instance;
            manager.RaisePropertyChanged(nameof(CanUndo));
            manager.RaisePropertyChanged(nameof(CanRedo));
        }

        /// <summary>
        /// 触发属性变更事件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        protected virtual void RaisePropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
