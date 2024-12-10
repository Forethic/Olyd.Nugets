namespace Olyd.Util.WinForm
{
    public class ScreenHelper
    {
        /// <summary>
        /// 获取指定窗体所在屏幕的工作区大小。
        /// 工作区指的是屏幕的区域，不包括任务栏、工具栏等。
        /// </summary>
        /// <param name="windowHandle">窗体的窗口句柄。</param>
        /// <returns>返回包含屏幕工作区的矩形，表示窗体所在屏幕的可用显示区域。</returns>
        /// <exception cref="ArgumentException">如果提供的窗口句柄无效。</exception>
        public static Rectangle GetScreenBound(IntPtr windowHandle)
        {
            if (windowHandle == IntPtr.Zero)
            {
                throw new ArgumentException("窗口句柄无效。", nameof(windowHandle));
            }

            try
            {
                var screen = Screen.FromHandle(windowHandle);
                // 返回屏幕的工作区矩形，即去除任务栏等的区域
                return new Rectangle(screen.WorkingArea.Left, screen.WorkingArea.Top, screen.WorkingArea.Width, screen.WorkingArea.Height);
            }
            catch (ArgumentException)
            {
                // 处理无效句柄的情况
                throw new ArgumentException("无法从提供的窗口句柄获取屏幕信息。", nameof(windowHandle));
            }
        }
    }
}
