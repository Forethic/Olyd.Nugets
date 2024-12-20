namespace Olyd.Wpf.SingleStartup
{
    /// <summary>
    /// 单例Application必要的功能函数
    /// </summary>
    public interface ISingleApp
    {
        /// <summary>
        /// 另一个进程启动
        /// </summary>
        /// <param name="args">启动参数</param>
        void NextRun(string[] args);

        /// <summary>
        /// 唯一标识ID
        /// </summary>
        string InstanceID { get; set; }
    }
}
