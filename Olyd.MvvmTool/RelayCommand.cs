using System.Windows.Input;

namespace Olyd.MvvmTool
{
    public class RelayCommand : ICommand
    {
        // 事件：当 CanExecute 状态发生变化时通知绑定的控件更新。
        public event EventHandler? CanExecuteChanged;

        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="execute">执行命令的逻辑</param>
        /// <param name="canExecute">判断是否可以执行命令的逻辑（可选，默认可以执行）</param>
        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute ?? (() => true);  // 默认可以执行
        }

        /// <summary>
        /// 判断命令是否可以执行
        /// </summary>
        /// <param name="parameter">命令参数（不使用时为 null）</param>
        /// <returns>返回命令是否可以执行</returns>
        public bool CanExecute(object? parameter) => _canExecute();

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="parameter">命令参数（不使用时为 null）</param>
        public void Execute(object? parameter) => _execute();

        /// <summary>
        /// 引发 CanExecuteChanged 事件
        /// </summary>
        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    public class RelayCommand<T> : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="execute">执行命令的逻辑</param>
        /// <param name="canExecute">判断是否可以执行命令的逻辑</param>
        public RelayCommand(Action<T> execute, Func<T, bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute ?? (_ => true);  // 默认可以执行
        }

        /// <summary>
        /// 判断命令是否可以执行
        /// </summary>
        /// <param name="parameter">命令参数</param>
        /// <returns>返回命令是否可以执行</returns>
        public bool CanExecute(object? parameter)
        {
            if (parameter is T t)
            {
                return _canExecute(t);
            }
            return false;
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="parameter">命令参数</param>
        public void Execute(object? parameter)
        {
            if (parameter is T t)
            {
                _execute(t);
            }
        }

        /// <summary>
        /// 引发 CanExecuteChanged 事件
        /// </summary>
        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

}
