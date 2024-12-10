namespace Olyd.Util.WinForm
{
    /// <summary>
    /// 表示矩形的结构体，包括左上角坐标、宽度和高度。
    /// </summary>
    public struct Rectangle
    {
        /// <summary>
        /// 获取或设置矩形的左边界（X坐标）。
        /// </summary>
        public int Left { get; set; }

        /// <summary>
        /// 获取或设置矩形的上边界（Y坐标）。
        /// </summary>
        public int Top { get; set; }

        /// <summary>
        /// 获取矩形的右边界（X坐标），即 Left + Width。
        /// </summary>
        public readonly int Right => Left + Width;

        /// <summary>
        /// 获取矩形的下边界（Y坐标），即 Top + Height。
        /// </summary>
        public readonly int Bottom => Top + Height;

        /// <summary>
        /// 获取或设置矩形的宽度。
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 获取或设置矩形的高度。
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// 初始化矩形结构体的所有字段。
        /// </summary>
        /// <param name="left">矩形的左边界（X坐标）。</param>
        /// <param name="top">矩形的上边界（Y坐标）。</param>
        /// <param name="width">矩形的宽度。</param>
        /// <param name="height">矩形的高度。</param>
        public Rectangle(int left, int top, int width, int height)
        {
            Left = left;
            Top = top;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// 获取矩形的面积（宽度 * 高度）。
        /// </summary>
        public int Area => Width * Height;

        /// <summary>
        /// 返回矩形的字符串表示形式，包含左上角和右下角的坐标。
        /// </summary>
        /// <returns>矩形的字符串表示形式。</returns>
        public override string ToString() => $"Left: {Left}, Top: {Top}, Right: {Right}, Bottom: {Bottom}, Width: {Width}, Height: {Height}";
    }
}
