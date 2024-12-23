# Olyd.Wpf.Converters

WPF 常用 转换器

## Converters

| Class Name         | Remark                |
| ------------------ | --------------------- |
| ValueWhenConverter | 值匹配转换器          |

## Keys

| Key Name                                  | When               | Other                  |
| ----------------------------------------- | ------------------ | ---------------------- |
| AntiBooleanConverter                      | True --> False     | False --> True         |
| BoolToVisibilityVisibleHiddenConverter    | True --> Visible   | False --> Hidden       |
| BoolToVisibilityHiddenVisibleConverter    | True --> Hidden    | False --> Visible      |
| BoolToVisibilityVisibleCollapsedConverter | True --> Visible   | False --> Collapsed    |
| BoolToVisibilityCollapsedVisibleConverter | True --> Collapsed | False --> Visible      |
| NullToVisibilityVisibleHiddenConverter    | Null --> Visible   | Not Null --> Hidden    |
| NullToVisibilityHiddenVisibleConverter    | Null --> Hidden    | Not Null --> Visible   |
| NullToVisibilityVisibleCollapsedConverter | Null --> Visible   | Not Null --> Collapsed |
| NullToVisibilityCollapsedVisibleConverter | Null --> Collapsed | Not Null --> Visible   |
