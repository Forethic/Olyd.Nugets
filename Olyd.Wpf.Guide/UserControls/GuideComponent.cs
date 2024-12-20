using Olyd.Wpf.Guide.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Olyd.Wpf.Guide.UserControls
{
    /// <summary>
    /// Represents a component for guiding users with indicators, labels, and highlights.
    /// </summary>
    public class GuideComponent
    {
        /// <summary>
        /// Display direction of the guide.
        /// </summary>
        public Direction Direction { get; set; }

        /// <summary>
        /// Text content of the guide.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The clipping area for the guide's highlight.
        /// </summary>
        public Geometry ClipGeometry { get; private set; }

        private Rect _rect;               // The rectangular area for the guide
        private Canvas _owner;            // The canvas to render guide components
        private FrameworkElement _target; // The target element for the guide

        private GuideRectangle _guideRectangle;
        private GuideIndicator _guideIndicator;
        private GuideLabel _guideLabel;

        /// <summary>
        /// Creates and shows a guide component.
        /// </summary>
        /// <param name="element">The target element to guide.</param>
        /// <param name="direction">The direction of the guide indicator.</param>
        /// <param name="text">The text content for the guide label.</param>
        /// <param name="canvas">The canvas on which to render the guide.</param>
        /// <param name="radius">The corner radius of the highlight rectangle.</param>
        /// <param name="padding">The padding around the target element.</param>
        /// <param name="smallBallSize">The size of the small ball in the guide indicator.</param>
        /// <returns>A new instance of <see cref="GuideComponent"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if `element` or `canvas` is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if `padding` is negative.</exception>
        public static GuideComponent ShowGuideControl(
            FrameworkElement element,
            Direction direction,
            string text,
            Canvas canvas,
            double radius = 8.0,
            double padding = 10.0,
            double smallBallSize = 6.0)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            if (canvas == null)
                throw new ArgumentNullException(nameof(canvas));

            if (padding < 0)
                throw new InvalidOperationException("padding ");

            var control = new GuideComponent
            {
                Text = text,
                Direction = direction,
                _owner = canvas,
                _target = element
            };

            control.LocateGuideRectangle(radius, padding);
            control.LocateGuideIndicator(smallBallSize);
            control.LocateGuideLabel();

            return control;
        }

        /// <summary>
        /// Calculates and locates the highlight rectangle for the guide.
        /// </summary>
        private void LocateGuideRectangle(double radius, double padding)
        {
            _rect = CalculateRect(_target, _owner, padding);
            ClipGeometry = new RectangleGeometry(_rect, radius, radius);

            _guideRectangle = new GuideRectangle
            {
                Radius = radius,
                Width = _rect.Width,
                Height = _rect.Height
            };

            _guideRectangle.SetValue(Canvas.LeftProperty, _rect.Left);
            _guideRectangle.SetValue(Canvas.TopProperty, _rect.Top);
            _owner.Children.Add(_guideRectangle);
        }

        /// <summary>
        /// Creates and positions the guide indicator based on the direction.
        /// </summary>
        private void LocateGuideIndicator(double smallBallSize)
        {
            _guideIndicator = new GuideIndicator
            {
                Direction = Direction,
                SmallBallSize = smallBallSize,
                BigBallSize = smallBallSize * 2
            };

            var position = CalculateIndicatorPosition();
            Canvas.SetLeft(_guideIndicator, position.X);
            Canvas.SetTop(_guideIndicator, position.Y);

            _owner.Children.Add(_guideIndicator);
        }

        /// <summary>
        /// Creates and positions the guide label.
        /// </summary>
        private void LocateGuideLabel()
        {
            _guideLabel = new GuideLabel
            {
                Content = Text,
                Visibility = Visibility.Hidden
            };

            _guideLabel.Loaded += OnGuideLabelLoaded;
            _owner.Children.Add(_guideLabel);
        }

        /// <summary>
        /// Adjusts the position of the guide label after it is loaded.
        /// </summary>
        private void OnGuideLabelLoaded(object sender, RoutedEventArgs e)
        {
            var position = CalculateLabelPosition();
            _guideLabel.SetValue(Canvas.LeftProperty, position.X);
            _guideLabel.SetValue(Canvas.TopProperty, position.Y);

            _guideLabel.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Calculates the rectangle for the target element with padding.
        /// </summary>
        private static Rect CalculateRect(FrameworkElement element, Canvas canvas, double padding)
        {
            var point = element.TranslatePoint(new Point(0, 0), canvas);
            return new Rect(
                point.X - padding / 2,
                point.Y - padding / 2,
                element.ActualWidth + padding,
                element.ActualHeight + padding);
        }

        /// <summary>
        /// Calculates the position of the guide indicator based on the direction.
        /// </summary>
        private Point CalculateIndicatorPosition()
        {
            var midX = _rect.Left + _rect.Width / 2;
            var midY = _rect.Top + _rect.Height / 2;

            return Direction switch
            {
                Direction.Left =>
                    new Point(_rect.X - _guideIndicator.LineLength - _guideIndicator.BigBallSize / 2, midY - _guideIndicator.BigBallSize / 2),

                Direction.Right =>
                    new Point(_rect.Right - _guideIndicator.BigBallSize / 2, midY - _guideIndicator.BigBallSize / 2),

                Direction.Top =>
                    new Point(midX - _guideIndicator.BigBallSize / 2, _rect.Top - _guideIndicator.LineLength - _guideIndicator.BigBallSize / 2),

                Direction.Bottom =>
                    new Point(midX - _guideIndicator.BigBallSize / 2, _rect.Bottom - _guideIndicator.BigBallSize / 2),

                _ => throw new InvalidOperationException("Invalid guide direction.")
            };
        }

        /// <summary>
        /// Calculates the position of the guide label based on the direction.
        /// </summary>
        private Point CalculateLabelPosition()
        {
            var midX = _rect.Left + _rect.Width / 2;
            var midY = _rect.Top + _rect.Height / 2;

            return Direction switch
            {
                Direction.Top => new Point(
                    Math.Max(midX - _guideLabel.ActualWidth / 2, 0),
                    Canvas.GetTop(_guideIndicator) - _guideLabel.ActualHeight),

                Direction.Bottom => new Point(
                    Math.Max(midX - _guideLabel.ActualWidth / 2, 0),
                    Canvas.GetTop(_guideIndicator) + _guideIndicator.ActualHeight),

                Direction.Right => new Point(
                    Canvas.GetLeft(_guideIndicator) + _guideIndicator.ActualWidth,
                    midY - _guideLabel.ActualHeight / 2),

                Direction.Left => new Point(
                    Canvas.GetLeft(_guideIndicator) - _guideLabel.ActualWidth,
                    midY - _guideLabel.ActualHeight / 2),

                _ => throw new InvalidOperationException("Invalid guide direction.")
            };
        }
    }
}
