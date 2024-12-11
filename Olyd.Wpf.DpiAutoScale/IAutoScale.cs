using System.Windows;

namespace Olyd.Wpf.DpiAutoScale
{
    /// <summary>
    /// 自适应缩放接口
    /// </summary>
    public interface IAutoScale
    {
        /// <summary>
        /// WindowState为Normal时，<see cref="Child"/>的宽度
        /// </summary>
        double ChildWidth { get; }

        /// <summary>
        /// WindowState为Normal时，<see cref="Child"/>的高度
        /// </summary>
        double ChildHeight { get; }

        /// <summary>
        /// WindowState为Normal时，<see cref="Child"/>的Margin
        /// </summary>
        Thickness ChildMargin { get; }

        /// <summary>
        /// Viewbox 的 Content对象
        /// </summary>
        FrameworkElement Child { get; }

        /// <summary>
        /// 标识窗体是否显示，无需设置初始值
        /// </summary>
        bool IsShown { get; set; }

        /// <summary>
        /// 上一次窗体显示的DPI值
        /// </summary>
        double? LastDpiScaleX { get; set; }

        /// <summary>
        /// 自适应窗体
        /// </summary>
        Window Window { get; }

        /// <summary>
        /// Dpi变化后窗体尺寸响应变化后方法
        /// </summary>
        void RaiseDpiResized();

        event EventHandler DpiResized;
    }
}
