using Olyd.Wpf.Guide.Models;
using Olyd.Wpf.Helpers;
using System.Windows;
using System.Windows.Controls;

namespace Olyd.Wpf.Guide.UserControls
{
    /// <summary>
    /// A custom control that visually represents a guide indicator, which consists of a line with small and large balls.
    /// It can be used in a tutorial or guide system to indicate directions or actions to be taken by the user.
    /// </summary>
    public class GuideIndicator : Control
    {
        /// <summary>
        /// Dependency property for the direction of the guide indicator (e.g., Left, Right, etc.).
        /// Determines the direction in which the indicator points.
        /// </summary>
        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register(
                nameof(Direction),
                typeof(Direction),
                typeof(GuideIndicator),
                new PropertyMetadata(Direction.Left));

        /// <summary>
        /// Dependency property for the size of the small ball in the guide indicator.
        /// </summary>
        public static readonly DependencyProperty SmallBallSizeProperty =
            DependencyProperty.Register(
                nameof(SmallBallSize),
                typeof(double),
                typeof(GuideIndicator),
                new PropertyMetadata(BoxValue.Double6));

        /// <summary>
        /// Dependency property for the size of the large ball in the guide indicator.
        /// </summary>
        public static readonly DependencyProperty BigBallSizeProperty =
            DependencyProperty.Register(
                nameof(BigBallSize),
                typeof(double),
                typeof(GuideIndicator),
                new PropertyMetadata(BoxValue.Double12));

        /// <summary>
        /// Dependency property for the width of the line in the guide indicator.
        /// </summary>
        public static readonly DependencyProperty LineWidthProperty =
            DependencyProperty.Register(
                nameof(LineWidth),
                typeof(double),
                typeof(GuideIndicator),
                new PropertyMetadata(BoxValue.Double2));

        /// <summary>
        /// Dependency property for the length of the line in the guide indicator.
        /// </summary>
        public static readonly DependencyProperty LineLengthProperty =
            DependencyProperty.Register(
                nameof(LineLength),
                typeof(double),
                typeof(GuideIndicator),
                new PropertyMetadata(BoxValue.Double50));

        /// <summary>
        /// Gets or sets the direction of the guide indicator (e.g., Left, Right, etc.).
        /// </summary>
        public Direction Direction
        {
            get => (Direction)GetValue(DirectionProperty);
            set => SetValue(DirectionProperty, value);
        }

        /// <summary>
        /// Gets or sets the size of the small ball in the guide indicator.
        /// </summary>
        public double SmallBallSize
        {
            get => (double)GetValue(SmallBallSizeProperty);
            set => SetValue(SmallBallSizeProperty, value);
        }

        /// <summary>
        /// Gets or sets the size of the large ball in the guide indicator.
        /// </summary>
        public double BigBallSize
        {
            get => (double)GetValue(BigBallSizeProperty);
            set => SetValue(BigBallSizeProperty, value);
        }

        /// <summary>
        /// Gets or sets the width of the line in the guide indicator.
        /// </summary>
        public double LineWidth
        {
            get => (double)GetValue(LineWidthProperty);
            set => SetValue(LineWidthProperty, value);
        }

        /// <summary>
        /// Gets or sets the length of the line in the guide indicator.
        /// </summary>
        public double LineLength
        {
            get => (double)GetValue(LineLengthProperty);
            set => SetValue(LineLengthProperty, value);
        }

        /// <summary>
        /// Static constructor to override the default style of the GuideIndicator control.
        /// </summary>
        static GuideIndicator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GuideIndicator), new FrameworkPropertyMetadata(typeof(GuideIndicator)));
        }
    }
}
