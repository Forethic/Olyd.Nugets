using Olyd.Wpf.Helpers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Olyd.Wpf.Guide.UserControls
{
    /// <summary>
    /// Represents a rectangular highlight used in a guide overlay, typically for drawing attention to specific UI elements.
    /// </summary>
    public class GuideRectangle : Control
    {
        /// <summary>
        /// Identifies the <see cref="Fill"/> dependency property, which specifies the background brush of the rectangle.
        /// </summary>
        public static readonly DependencyProperty FillProperty =
            DependencyProperty.Register(
                nameof(Fill),
                typeof(Brush),
                typeof(GuideRectangle),
                new PropertyMetadata(Brushes.Transparent));

        /// <summary>
        /// Identifies the <see cref="Stroke"/> dependency property, which specifies the border brush of the rectangle.
        /// </summary>
        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register(
                nameof(Stroke),
                typeof(Brush),
                typeof(GuideRectangle),
                new PropertyMetadata(Brushes.Black));

        /// <summary>
        /// Identifies the <see cref="StrokeDashArray"/> dependency property, which specifies the dash pattern for the rectangle border.
        /// </summary>
        public static readonly DependencyProperty StrokeDashArrayProperty =
            DependencyProperty.Register(
                nameof(StrokeDashArray),
                typeof(DoubleCollection),
                typeof(GuideRectangle),
                new PropertyMetadata(default));

        /// <summary>
        /// Identifies the <see cref="Radius"/> dependency property, which specifies the corner radius of the rectangle.
        /// </summary>
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register(
                nameof(Radius),
                typeof(double),
                typeof(GuideRectangle),
                new PropertyMetadata(BoxValue.Double8));

        /// <summary>
        /// Gets or sets the background brush of the rectangle.
        /// </summary>
        public Brush Fill
        {
            get => (Brush)GetValue(FillProperty);
            set => SetValue(FillProperty, value);
        }

        /// <summary>
        /// Gets or sets the border brush of the rectangle.
        /// </summary>
        public Brush Stroke
        {
            get => (Brush)GetValue(StrokeProperty);
            set => SetValue(StrokeProperty, value);
        }

        /// <summary>
        /// Gets or sets the dash pattern for the rectangle border.
        /// </summary>
        public DoubleCollection StrokeDashArray
        {
            get => (DoubleCollection)GetValue(StrokeDashArrayProperty);
            set => SetValue(StrokeDashArrayProperty, value);
        }

        /// <summary>
        /// Gets or sets the corner radius of the rectangle.
        /// </summary>
        public double Radius
        {
            get => (double)GetValue(RadiusProperty);
            set => SetValue(RadiusProperty, value);
        }

        /// <summary>
        /// Static constructor to override the default style key for the control.
        /// </summary>
        static GuideRectangle()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GuideRectangle), new FrameworkPropertyMetadata(typeof(GuideRectangle)));
        }
    }
}
