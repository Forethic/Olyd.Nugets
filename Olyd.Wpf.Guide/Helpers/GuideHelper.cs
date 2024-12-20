using Olyd.Wpf.Helpers;
using System.Windows;
using System.Windows.Media;

namespace Olyd.Wpf.Guide.Helpers
{
    /// <summary>
    /// A utility class that provides attached properties for guiding features in a WPF application.
    /// These properties allow assigning metadata to UI elements to facilitate the guide system.
    /// </summary>
    public class GuideHelper
    {
        /// <summary>
        /// Attached property for setting or getting a name identifier for a UI element.
        /// This identifier can be used to reference the element in a guide system.
        /// </summary>
        public static readonly DependencyProperty NameProperty =
            DependencyProperty.RegisterAttached(
                "Name",
                typeof(string),
                typeof(GuideHelper),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets the value of the <see cref="NameProperty"/> attached to the specified object.
        /// </summary>
        /// <param name="obj">The object from which to retrieve the name.</param>
        /// <returns>The name associated with the object, or null if none is set.</returns>
        public static string GetName(DependencyObject obj)
            => (string)obj.GetValue(NameProperty);

        /// <summary>
        /// Sets the value of the <see cref="NameProperty"/> attached to the specified object.
        /// </summary>
        /// <param name="obj">The object to which the name will be attached.</param>
        /// <param name="value">The name to attach to the object.</param>
        public static void SetName(DependencyObject obj, string value)
            => obj.SetValue(NameProperty, value);

        /// <summary>
        /// Attached property for marking a UI element as ignored by the guide system.
        /// This is useful for elements that should not be included in guide overlays.
        /// </summary>
        public static readonly DependencyProperty IsIgnoreProperty =
            DependencyProperty.RegisterAttached(
                "IsIgnore",
                typeof(bool),
                typeof(GuideHelper),
                new PropertyMetadata(BoxValue.False));

        /// <summary>
        /// Gets the value of the <see cref="IsIgnoreProperty"/> attached to the specified object.
        /// </summary>
        /// <param name="obj">The object from which to retrieve the ignore flag.</param>
        /// <returns>True if the object should be ignored, otherwise false.</returns>
        public static bool GetIsIgnore(DependencyObject obj)
            => (bool)obj.GetValue(IsIgnoreProperty);

        /// <summary>
        /// Sets the value of the <see cref="IsIgnoreProperty"/> attached to the specified object.
        /// </summary>
        /// <param name="obj">The object to which the ignore flag will be attached.</param>
        /// <param name="value">True to mark the object as ignored, otherwise false.</param>
        public static void SetIsIgnore(DependencyObject obj, bool value)
            => obj.SetValue(IsIgnoreProperty, value);

        /// <summary>
        /// Recursively searches for a control with a specified name within a parent element.
        /// </summary>
        /// <typeparam name="T">The type of the control to search for, must derive from FrameworkElement.</typeparam>
        /// <param name="parent">The parent control where the search begins.</param>
        /// <param name="name">The name of the target control.</param>
        /// <returns>The found control instance of type T, or null if no matching control is found.</returns>
        internal static T FindControl<T>(FrameworkElement parent, string name)
           where T : FrameworkElement
        {
            if (parent == null)
                return null;

            if (GetIsIgnore(parent))
                return null;

            if (GetName(parent) == name)
                return (T)parent;

            int childCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childCount; i++)
            {
                if (VisualTreeHelper.GetChild(parent, i) is FrameworkElement element)
                {
                    var foundControl = FindControl<T>(element, name);
                    if (foundControl != null)
                        return foundControl;
                }
            }

            return null;
        }
    }
}
