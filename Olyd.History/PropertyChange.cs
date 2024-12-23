using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

namespace Olyd.History
{
    /// <summary>
    /// 表示对属性值的更改操作，用于支持撤销和重做功能。
    /// </summary>
    public class PropertyChange : BaseChange
    {
        /// <summary>
        /// 要修改的目标对象。
        /// </summary>
        public object Target { get; }

        /// <summary>
        /// 被更改的属性名称。
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// 属性的旧值。
        /// </summary>
        public object OldValue { get; }

        /// <summary>
        /// 属性的新值。
        /// </summary>
        public object NewValue { get; }

        private readonly PropertyInfo _propertyInfo;
        private readonly INotifyPropertyChanged _notifyPropertyChanged;

        /// <summary>
        /// 初始化 <see cref="PropertyChange"/> 类的新实例。
        /// </summary>
        /// <param name="target">目标对象，必须实现要变更的属性。</param>
        /// <param name="propertyName">需要变更的属性名称。</param>
        /// <param name="oldValue">属性的旧值。</param>
        /// <param name="newValue">属性的新值。</param>
        /// <exception cref="ArgumentNullException">如果目标对象或属性名称为 null，则抛出。</exception>
        /// <exception cref="ArgumentException">如果属性名称无效或值类型不兼容，则抛出。</exception>
        public PropertyChange(object target, string propertyName, object oldValue, object newValue)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentException("Property name cannot be null or whitespace.", nameof(propertyName));

            _propertyInfo = target.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance)
                ?? throw new ArgumentException($"Property '{propertyName}' not found on target of type {target.GetType().FullName}.");

            // 类型兼容性检查
            if (oldValue != null && !_propertyInfo.PropertyType.IsAssignableFrom(oldValue.GetType()))
                throw new ArgumentException($"OldValue is not compatible with the property type {_propertyInfo.PropertyType}.");
            if (newValue != null && !_propertyInfo.PropertyType.IsAssignableFrom(newValue.GetType()))
                throw new ArgumentException($"NewValue is not compatible with the property type {_propertyInfo.PropertyType}.");

            Target = target;
            PropertyName = propertyName;
            OldValue = oldValue;
            NewValue = newValue;

            if (target is INotifyPropertyChanged notifyPropertyChanged)
                _notifyPropertyChanged = notifyPropertyChanged;
        }

        /// <summary>
        /// 执行重做操作，将属性值设置为新值。
        /// </summary>
        public override void Redo()
        {
            try
            {
                _propertyInfo.SetValue(Target, NewValue);
                _notifyPropertyChanged?.RaisePropertyChanged(PropertyName);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to set property '{PropertyName}' to NewValue: {ex.Message}");
            }
        }

        /// <summary>
        /// 执行撤销操作，将属性值恢复为旧值。
        /// </summary>
        public override void Undo()
        {
            try
            {
                _propertyInfo.SetValue(Target, OldValue);
                _notifyPropertyChanged?.RaisePropertyChanged(PropertyName);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to set property '{PropertyName}' to OldValue: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// 提供扩展方法以触发 <see cref="INotifyPropertyChanged"/> 的 PropertyChanged 事件。
    /// </summary>
    public static class PropertyChangedExtensions
    {
        /// <summary>
        /// 手动触发 PropertyChanged 事件。
        /// </summary>
        /// <param name="notify">实现 <see cref="INotifyPropertyChanged"/> 的对象。</param>
        /// <param name="propertyName">变更的属性名称。</param>
        public static void RaisePropertyChanged(this INotifyPropertyChanged notify, string propertyName)
        {
            if (notify == null) throw new ArgumentNullException(nameof(notify));

            // 使用反射获取事件字段
            var propertyChangedField = notify.GetType().GetField("PropertyChanged", BindingFlags.Instance | BindingFlags.NonPublic);
            if (propertyChangedField == null) return;

            if (propertyChangedField.GetValue(notify) is PropertyChangedEventHandler handler)
            {
                handler.Invoke(notify, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

}
