using System.Windows.Media;
using System.Windows;
using Olyd.Util.WinForm;
using System.Windows.Input;
using System.Windows.Interop;

namespace Olyd.Wpf.DpiAutoScale
{
    /// <summary>
    /// ϵͳ���Ź���
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
                autoScale.Window.MaxHeight = bound.Height / dpiScale.DpiScaleY; // ������ס������

                // �ƶ�ʱ��Ҫִ�в���
                if (Mouse.LeftButton == MouseButtonState.Pressed)
                {
                    autoScale.LastDpiScaleX = null;
                    return;
                }

                if (e.NewDpi.DpiScaleX == autoScale.LastDpiScaleX)
                    return;

                // NOTE: ��DpiChanged �¼����޸Ĵ����С������Ч���²��ǻᱻ�ں˸��ǵ��޸�
                Task.Delay(100).ContinueWith(t => Resize(autoScale, e.NewDpi));
            };
        }

        /// <summary>
        /// ���ô���ߴ�
        /// </summary>
        /// <param name="autoScale">��Ҫ����Ӧ���ŵĶ���</param>
        /// <param name="dpiScale">��ǰDpiֵ</param>
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

                    // ��δ��ʾ���Ļ��������Ļ���ж�λ
                    if (!autoScale.IsShown && window.WindowStartupLocation == WindowStartupLocation.CenterScreen)
                    {
                        var screen = ScreenHelper.GetScreenBound(new WindowInteropHelper(window).Handle);
                        window.Left = (screen.Left + (screen.Width - (window.Width * dpiScale.Value.DpiScaleX)) / 2) / dpiScale.Value.DpiScaleX;
                        window.Top = (screen.Top + (screen.Height - (window.Height * dpiScale.Value.DpiScaleY)) / 2) / dpiScale.Value.DpiScaleY;
                    }
                    autoScale.IsShown = true;

                    // NOTE����Ҫ�Ӻ�ִ�У�ͬʱִ��ʱ���ܻᵼ��UI��Ӧ����
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
