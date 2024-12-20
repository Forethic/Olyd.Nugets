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
