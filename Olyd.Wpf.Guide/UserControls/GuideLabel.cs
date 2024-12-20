using System.Windows;
using System.Windows.Controls;

namespace Olyd.Wpf.Guide.UserControls
{
    /// <summary>
    /// Represents a custom label control specifically designed for use in guide overlays.
    /// This class can be styled and extended to meet the requirements of guide features.
    /// </summary>
    public class GuideLabel : Label
    {
        /// <summary>
        /// Static constructor to override the default style key for the control.
        /// </summary>
        static GuideLabel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GuideLabel), new FrameworkPropertyMetadata(typeof(GuideLabel)));
        }
    }
}
