namespace Olyd.History
{
    /// <summary>
    /// 表示历史记录中的基本变更操作类。
    /// 所有具体的变更操作（例如属性更改、集合更改等）应继承此类并实现撤销和重做的逻辑。
    /// </summary>
    public abstract class BaseChange
    {
        /// <summary>
        /// 执行撤销操作。
        /// 子类需要实现具体的撤销逻辑，用于还原到变更前的状态。
        /// </summary>
        public abstract void Undo();

        /// <summary>
        /// 执行重做操作。
        /// 子类需要实现具体的重做逻辑，用于恢复变更后的状态。
        /// </summary>
        public abstract void Redo();
    }
}
