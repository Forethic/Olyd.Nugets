# Olyd.Wpf.Guide

WPF应用程序, 新手引导解决方案

## 使用步骤

1. 实现继承 GuideContainer 的控件类
   1. 通过 StepName 属性，明白当前指引的步骤阶段
   2. 重载 OnNextShowGuide() 方法，可以在下一个指引之前进行窗体内部页面跳转等操作，记得要调用 ShowGuide 方法
   3. 记得在控件上使用附加属性 GuideHelper.Name 设定引导名称
   4. 如果控件太多，可以使用附加属性 GuideHelper.Ignore 设定忽略
2. 实现 IGuideTarget 接口
   1. 一种直接添加到要显示新手指引的窗体类上
   2. 一种是没有窗体类，可以使用单独类显示
3. 编写 新手引导列表

```cs {.line-numbers}
var guideList = new List<GuideGroup>();
{
    GuideItem item = new GuideItem { ElementName = "ConnectBtn_Guid", Content = LocalizedStrings.Guide_device_view, Direction = Direction.Bottom };
    GuideGroup vo = new GuideGroup { Items = new List<GuideItem>() };
    vo.StepName = "Guid_Page1";
    vo.Items.Add(item);
    guideList.Add(vo);
}
{
    GuideItem item1 = new GuideItem { ElementName = "GuideGrid", Content = LocalizedStrings.Guide_device_find, Direction = Direction.Top };
    GuideGroup vo = new GuideGroup { Items = new List<GuideItem>() };
    vo.StepName = "Guid_Page2";
    vo.Items.Add(item1);
    guideList.Add(vo);
}
{
    GuideItem item1 = new GuideItem { ElementName = "GuideBtn", Content = LocalizedStrings.Guide_device_connect, Direction = Direction.Right };
    GuideGroup vo = new GuideGroup { Items = new List<GuideItem>() };
    vo.StepName = "Guid_Page3";
    vo.Items.Add(item1);
    vo.OnNextAction = (w) =>
    {
        WindowManager.Instance.GotoHomePage();
    };
    guideList.Add(vo);
}
GuideLists.Add(guideList); // GuideLists 存放所有的新手指引
```

1. 调用 GuideWindow.ShowGuideWindow() 方法，传入继承 GuideContainer 的控件、实现 IGuideTarget 的类对象、本次新手引导的列表。

```cs {.line-numbers}
// GuideMainView 继承 GuideContainer
// _mainWindow 实现 IGuideTarget
// GuideConfig.GuideList 是 List<GuideGroup>
var guideWindow = GuideWindow.ShowGuideWindow(new GuideMainView(), _mainWindow, GuideConfig.GuideLists[0]);
guideWindow.ShowDialog();
```

5. App.xaml 引入主题和样式

```xaml {.line-numbers}
<ResourceDictionary Source="pack://application:,,,/Olyd.Wpf.Guide;component/Themes/Dark.xaml" />
<ResourceDictionary Source="pack://application:,,,/Olyd.Wpf.Guide;component/Themes/Generic.xaml" />
```
