﻿using System.Windows;
using System.Windows.Media;

namespace Olyd.Wpf.Helpers
{
    /// <summary>
    /// WPF UI 帮助类
    /// </summary>
    public static class UIHelpers
    {
        /// <summary>
        /// 获取当前对象的父级对象
        /// </summary>
        /// <param name="child">需要查找父对象的子对象</param>
        /// <param name="searchCompleteVisualTree">是否搜索整个树</param>
        /// <returns>父级对象</returns>
        public static DependencyObject GetParentObject(this DependencyObject child, bool searchCompleteVisualTree)
        {
            if (child == null) return null;

            if (!searchCompleteVisualTree)
            {
                if (child is ContentElement contentElement)
                {
                    DependencyObject parent = ContentOperations.GetParent(contentElement);
                    if (parent != null) return parent;

                    return contentElement is FrameworkContentElement fce ? fce.Parent : null;
                }

                if (child is FrameworkElement frameworkElement)
                {
                    DependencyObject parent = frameworkElement.Parent;
                    if (parent != null) return parent;
                }
            }

            return VisualTreeHelper.GetParent(child);
        }

        /// <summary>
        /// 获取指定类型的父级对象
        /// </summary>
        /// <param name="child">需要查找父对象的子对象</param>
        /// <param name="searchCompleteVisualTree">是否搜索整个树</param>
        /// <returns>返回第一个类型为T的父级对象</returns>
        public static T TryFindParent<T>(this DependencyObject child, bool searchCompleteVisualTree = false) where T : DependencyObject
        {
            DependencyObject parentObject = GetParentObject(child, searchCompleteVisualTree);

            if (parentObject == null) return null;

            if (parentObject is T parent)
            {
                return parent;
            }

            return TryFindParent<T>(parentObject);
        }

        /// <summary>
        /// 返回第一个符合指定类型的子对象
        /// </summary>
        /// <param name="parent">当前开始的对象.</param>
        /// <returns>null：没有找到，否则为第一个符合指定类型的子对象.</returns>
        public static T TryFindChild<T>(this DependencyObject parent) where T : DependencyObject
        {
            var childCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);

                if (child is T t)
                {
                    return t;
                }

                child = TryFindChild<T>(child);
                if (child != null)
                {
                    return (T)child;
                }
            }
            return null;
        }
    }
}
