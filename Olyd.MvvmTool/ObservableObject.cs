using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Olyd.MvvmTool
{
    public class ObservableObject : INotifyPropertyChanged
    {
        /// <summary>
        /// 发生属性更改时引发的事件。
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// 引发属性更改通知事件。
        /// </summary>
        /// <param name="propertyNames">要通知的属性名称列表。</param>
        public void RaisePropertyChanged(params string[] propertyNames)
        {
            // 遍历所有需要更新的属性名称，并引发相应的通知
            foreach (var propertyName in propertyNames)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// 设置属性的值，并在值发生变化时引发属性更改通知。
        /// </summary>
        /// <typeparam name="T">属性的类型。</typeparam>
        /// <param name="field">属性的字段（由调用者传入）。</param>
        /// <param name="newValue">属性的新值。</param>
        /// <param name="after">值更改后执行的回调（可选）。</param>
        /// <param name="propertyName">属性的名称，默认由 CallerMemberName 特性提供。</param>
        /// <returns>如果属性值发生更改，则返回 true；否则返回 false。</returns>
        public bool SetProperty<T>(ref T field, T newValue, Action<T>? after = null, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue))
                return false;

            var oldValue = field;
            field = newValue;

            after?.Invoke(oldValue);
            RaisePropertyChanged(propertyName);

            return true;
        }
    }
}
