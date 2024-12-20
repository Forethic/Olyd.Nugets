using Olyd.Wpf.DpiAutoScale;
using Olyd.Wpf.Guide.Helpers;
using Olyd.Wpf.Guide.Models;
using Olyd.Wpf.Guide.UserControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Olyd.Wpf.Guide
{
    /// <summary>
    /// Represents a guide window used for interactive tutorials or guidance.
    /// </summary>
    public partial class GuideWindow : Window, IAutoScale
    {
        private int _currentIndex;
        private PathGeometry _pathGeometry = new();
        private readonly IList<GuideGroup> _guideList;
        private readonly IGuideTarget _guideTarget;
        private readonly List<GuideComponent> _guideComponents = new();

        /// <summary>
        /// 指引阶段名称
        /// </summary>
        public string StepName => _currentIndex < _guideList.Count ? _guideList[_currentIndex].StepName : "";

        /// <summary>
        /// Event triggered for the next guide action.
        /// </summary>
        public event EventHandler OnNextActionTrigger;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuideWindow"/> class.
        /// </summary>
        /// <param name="guideList">A list of guide groups defining the guide steps.</param>
        /// <param name="guideTarget">The target window or element to guide.</param>
        public GuideWindow(IList<GuideGroup> guideList, IGuideTarget guideTarget)
        {
            InitializeComponent();
            this.InitAutoScale();

            _guideList = guideList ?? throw new ArgumentNullException(nameof(guideList));
            _guideTarget = guideTarget ?? throw new ArgumentNullException(nameof(guideTarget));

            InitializeGuideWindow();
            MouseLeftButtonDown += Window_MouseLeftButtonDown;
            Loaded += GuideWindow_Loaded;
        }

        #region [[ IAutoScale Implementation ]]

        /// <inheritdoc/>
        public double ChildWidth => _guideTarget.ContentWidth;

        /// <inheritdoc/>
        public double ChildHeight => _guideTarget.ContentHeight;

        /// <inheritdoc/>
        public Thickness ChildMargin => default;

        /// <inheritdoc/>
        public FrameworkElement Child => grdRoot;

        /// <inheritdoc/>
        public double? LastDpiScaleX { get; set; }

        /// <inheritdoc/>
        public bool IsShown { get; set; }

        /// <inheritdoc/>
        public Window Window => this;

        /// <inheritdoc/>
        public event EventHandler DpiResized;

        /// <inheritdoc/>
        public void RaiseDpiResized()
        {
            ShowGuide(); // 重新计算并展示
            DpiResized?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        /// <summary>
        /// Initializes the guide window properties and layout based on the target.
        /// </summary>
        private void InitializeGuideWindow()
        {
            Top = _guideTarget.Top;
            Left = _guideTarget.Left;
            Width = _guideTarget.Width;
            Height = _guideTarget.Height;
            grdRoot.Width = _guideTarget.ContentWidth;
            grdRoot.Height = _guideTarget.ContentHeight;

            WindowState = _guideTarget.WindowState == WindowState.Maximized
                ? WindowState.Maximized
                : WindowState.Normal;

            WindowStartupLocation = _guideTarget is Window
                ? WindowStartupLocation.Manual
                : _guideTarget.WindowStartupLocation;
        }

        /// <summary>
        /// Handles the Loaded event to trigger the next guide action.
        /// </summary>
        private void GuideWindow_Loaded(object sender, RoutedEventArgs e)
        {
            OnNextActionTrigger?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Handles mouse drag movement for the guide window.
        /// </summary>
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        /// <summary>
        /// Displays the current guide step.
        /// </summary>
        public void ShowGuide()
        {
            canvas.Children.Clear();
            _guideComponents.Clear();
            ShowGuide(_guideList[_currentIndex], _currentIndex == _guideList.Count - 1);
        }

        /// <summary>
        /// Displays the specified guide group.
        /// </summary>
        public void ShowGuide(GuideGroup guideGroup, bool isLastStep = false)
        {
            var rg = new RectangleGeometry
            {
                Rect = new Rect(0, 0, _guideTarget.ContentWidth, _guideTarget.ContentHeight)
            };

            _pathGeometry = Geometry.Combine(_pathGeometry, rg, GeometryCombineMode.Union, null);

            var guideButton = CreateGuideButton(isLastStep);
            PositionGuideComponents(guideGroup, guideButton);

            brdGuide.Clip = _pathGeometry;
            canvas.Children.Add(guideButton);
        }

        /// <summary>
        /// Creates a guide button for navigation.
        /// </summary>
        private GuideButton CreateGuideButton(bool isLastStep)
        {
            return new GuideButton
            {
                Content = isLastStep ? _guideTarget.CloseString : _guideTarget.NextString,
                Action = NextAction
            };
        }

        /// <summary>
        /// Positions the guide components and adds them to the canvas.
        /// </summary>
        private void PositionGuideComponents(GuideGroup guideGroup, GuideButton guideButton)
        {
            double x = 0, y = 0;

            foreach (var item in guideGroup.Items)
            {
                var targetElement = GuideHelper.FindControl<FrameworkElement>(grdContent, item.ElementName);

                if (targetElement == null || !targetElement.IsVisible)
                    continue;

                var guideComponent = GuideComponent.ShowGuideControl(targetElement, item.Direction, item.Content, canvas);
                _pathGeometry = Geometry.Combine(_pathGeometry, guideComponent.ClipGeometry, GeometryCombineMode.Xor, null);

                var position = CalculateGuideButtonPosition(targetElement, item.Direction);
                x = position.X;
                y = position.Y;

                _guideComponents.Add(guideComponent);
            }

            guideButton.SetValue(Canvas.LeftProperty, x);
            guideButton.SetValue(Canvas.TopProperty, y);
        }

        /// <summary>
        /// Calculates the position of the guide button based on the target element and direction.
        /// </summary>
        private Point CalculateGuideButtonPosition(FrameworkElement target, Direction direction)
        {
            var targetPosition = target.TranslatePoint(new Point(0, 0), canvas);

            return direction switch
            {
                Direction.Bottom => new Point(targetPosition.X + target.ActualWidth / 2, targetPosition.Y + target.ActualHeight + 128),
                Direction.Top => new Point(targetPosition.X + target.ActualWidth / 2, targetPosition.Y - 164),
                Direction.Right => new Point(targetPosition.X + target.ActualWidth + 109, targetPosition.Y + target.ActualHeight / 2 + 55),
                Direction.Left => new Point(targetPosition.X - 109, targetPosition.Y + target.ActualHeight / 2 + 55),
                _ => targetPosition
            };
        }

        /// <inheritdoc/>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _guideTarget.Show();
        }

        /// <summary>
        /// Proceeds to the next guide step.
        /// </summary>
        private void NextAction()
        {
            if (_guideList != null)
            {
                _guideList[_currentIndex].OnNextAction?.Invoke(this);
                _currentIndex++;

                if (_currentIndex >= _guideList.Count)
                {
                    Close();
                    return;
                }
            }

            OnNextActionTrigger?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Displays the guide window and binds the guide container to it.
        /// </summary>
        public static GuideWindow ShowGuideWindow(GuideContainer container, IGuideTarget target, IList<GuideGroup> guideList)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            var guideWindow = new GuideWindow(guideList, target);
            guideWindow.grdContent.Children.Add(container);

            // 设置新手指引窗口显示位置
            target.Hide(); // 隐藏窗体

            container.BindGuideWindow(guideWindow);
            return guideWindow;
        }
    }
}
