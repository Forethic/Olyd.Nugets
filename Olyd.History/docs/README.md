# Olyd.History

操作栈管理解决方案，适用于记录和管理撤消/重做操作的应用场景。

## 功能特点

- **操作记录与管理：** 支持撤销与重做功能。
- **灵活的 `ChangeTracker`**： 支持嵌套操作和分组管理。
- **自动推送变更通知**：针对实现 INotifyPropertyChanged 的对象，撤销/重做时自动触发属性变更通知。
- **多种预定义变更类型**：内置支持字典和列表的增删操作。
- **支持自定义变更**：继承 BaseChange 实现自定义变更逻辑。
- **提供 `HistoryManager.InUndoRedo`**： 属性，方便开发者判断当前操作是否处于撤消/重做流程中。

## 使用指南

### 1. 初始化编辑器或切换文件
  
```cs {.line-numbers}
HistoryManager.Clear(); // 清除之前的操作队列
```

### 2. 使用 ChangeTracker 管理变更

```cs {.line-numbers}
using (ChangeTracker changeTracker = new(SelectedItems))
{
    // 添加 Dictionary 的键值对新增操作
    changeTracker.AddChange(new DictionaryAddChange(
        dictionary,
        key,
        value
    ));

    // 添加 Dictionary 的键值对删除操作
    changeTracker.AddChange(new DictionaryRemoveChange(
        dictionary,
        key,
        value
    ));

    // 添加 List 的新增操作
    changeTracker.AddChange(new ListAddChange(
        list,
        item
    ));

    // 添加 List 的删除操作
    changeTracker.AddChange(new ListRemoveChange(
        list,
        item
    ));
}
```

### 3. 自定义Change

**自定义 Change 的基本步骤**

1. 继承 `BaseChange`：实现 `Undo` 和 `Redo` 方法，定义撤销与重做逻辑。
2. 注册到 `ChangeTracker`：通过 `ChangeTracker.AddChange()` 添加自定义操作。

## 内置Change

| Class Name             | Remark         |
| ---------------------- | -------------- |
| PropertyChange         | 属性值变更     |
| DictionaryAddChange    | 字典项添加变更 |
| DictionaryRemoveChange | 字典项移除变更 |
| ListAddChange          | 列表项添加变更 |
| ListRemoveChange       | 列表项移除变更 |
