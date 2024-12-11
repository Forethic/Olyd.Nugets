using System.Windows.Media;
using System.Windows;
using Olyd.Util.WinForm;
using System.Windows.Input;
using System.Windows.Interop;

namespace Olyd.Wpf.DpiAutoScale
{
    /// <summary>
    /// 系统缩放工具
    /// </summary>
    public static class DpiScaleUtil
    {
        public static void InitAutoScale(this IAutoScale autoScale)
        {
            if (autoScale == null)
                throw new ArgumentNullException(nameof(autoScale));

            if (autoScale.Window == null)
                throw new ArgumentNullException(nameof(autoScale.Window));

            autoScale.Window.Loaded += (s, e) => Resize(autoScale);
            autoScale.Window.MouseLeftButtonUp += (s, e) => Resize(autoScale);
            autoScale.Window.StateChanged += (s, e) => Resize(autoScale);
            autoScale.Window.DpiChanged += (s, e) =>
            {
                var bound = ScreenHelper.GetScreenBound(new WindowInteropHelper(autoScale.Window).Handle);
                var dpiScale = VisualTreeHelper.GetDpi(autoScale.Window);
                autoScale.Window.MaxHeight = bound.Height / dpiScale.DpiScaleY; // 避免遮住任务栏

                // 移动时不要执行操作
                if (Mouse.LeftButton == MouseButtonState.Pressed)
                {
                    autoScale.LastDpiScaleX = null;
                    return;
                }

                if (e.NewDpi.DpiScaleX == autoScale.LastDpiScaleX)
                    return;

                // NOTE: 在DpiChanged 事件中修改窗体大小不会生效，猜测是会被内核覆盖掉修改
                Task.Delay(100).ContinueWith(t => Resize(autoScale, e.NewDpi));
            };
        }

        /// <summary>
        /// 重置窗体尺寸
        /// </summary>
        /// <param name="autoScale">需要自适应缩放的对象</param>
        /// <param name="dpiScale">当前Dpi值</param>
        private static void Resize(IAutoScale autoScale, DpiScale? dpiScale = null)
        {
            var window = autoScale.Window;
            var content = autoScale.Child;

            if (dpiScale == null)
            {
                dpiScale = VisualTreeHelper.GetDpi(window);
            }

            window.Dispatcher.Invoke(() =>
            {
                if (window.WindowState == WindowState.Maximized)
                {
                    autoScale.LastDpiScaleX = null;

                    var bound = ScreenHelper.GetScreenBound(new WindowInteropHelper(window).Handle);
                    window.Width = bound.Width / dpiScale.Value.DpiScaleX;
                    window.Height = bound.Height / dpiScale.Value.DpiScaleY;

                    content.Width = bound.Width;
                    content.Height = bound.Height;
                    content.Margin = default;
                }
                else
                {
                    if (autoScale.LastDpiScaleX.HasValue && autoScale.LastDpiScaleX == dpiScale.Value.DpiScaleX)
                        return;

                    autoScale.LastDpiScaleX = dpiScale.Value.DpiScaleX;

                    window.Width = (autoScale.ChildWidth + autoScale.ChildMargin.Left + autoScale.ChildMargin.Right) / dpiScale.Value.DpiScaleX;
                    window.Height = (autoScale.ChildHeight + autoScale.ChildMargin.Top + autoScale.ChildMargin.Bottom) / dpiScale.Value.DpiScaleY;
                    content.Width = autoScale.ChildWidth;
                    content.Height = autoScale.ChildHeight;
                    content.Margin = autoScale.ChildMargin;

                    // 还未显示过的话，相对屏幕居中定位
                    if (!autoScale.IsShown && window.WindowStartupLocation == WindowStartupLocation.CenterScreen)
                    {
                        var screen = ScreenHelper.GetScreenBound(new WindowInteropHelper(window).Handle);
                        window.Left = (screen.Left + (screen.Width - (window.Width * dpiScale.Value.DpiScaleX)) / 2) / dpiScale.Value.DpiScaleX;
                        window.Top = (screen.Top + (screen.Height - (window.Height * dpiScale.Value.DpiScaleY)) / 2) / dpiScale.Value.DpiScaleY;
                    }
                    autoScale.IsShown = true;

                    // NOTE：需要延后执行，同时执行时可能会导致UI响应缓慢
                    Task.Delay(50).ContinueWith(t =>
                    {
                        window.Dispatcher.BeginInvoke(() =>
                        {
                            window.Opacity = 1;
                            autoScale.RaiseDpiResized();
                        });
                    });
                }
            });
        }
    }
}
