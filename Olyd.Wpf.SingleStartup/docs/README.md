# Olyd.Wpf.SingleStartup

WPF 单例进程启动解决方案

## 使用手册

1. App 类继承 `ISingleApp`, 实现 `NextRun` 方法（另一个进程启动时调用的方法）
2. OnStartup 方法优点调用以下代码:

```cs {.line-numbers}
  protected override void OnStartup(StartupEventArgs e)
  {
      var singlestartup = new SingletonStartUp(this);
      singlestartup.Run(e.Args);
      // .....
  }
```

> 如果使用 `Costura.Fody` 整合了所有的依赖项，不能使用上述的是方法，需要替换成内部类实现 `ISingleApp` 接口。
> 使用示例如下:

```cs {.line-numbers}
public partial class App : Application
{
  // 实现 ISingleApp
  private class SingleApp : ISingleApp
  {
      private Action<string[]> _nextRunAction;

      public SingleApp(Action<string[]> nextRunAction)
      {
          _nextRunAction = nextRunAction;
      }

      // 可以指定唯一标识名称，不提供的话，默认是exe名称
      public string? InstanceID { get; set; }

      public void NextRun(string[] args) => _nextRunAction?.Invoke(args);
  }

  protected override void OnStartup(StartupEventArgs e)
  {
      var singlestartup = new SingletonStartUp(new SingleApp(NextRun));
      singlestartup.Run(e.Args);

      // ....
  }

  // NextRun 显示主界面或者其他逻辑
  private void NextRun(string[] args)
  {
  }
}
```
