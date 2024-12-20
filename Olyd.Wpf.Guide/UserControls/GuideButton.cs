using System.Windows;
using System.Windows.Controls;

namespace Olyd.Wpf.Guide.UserControls
{
    /// <summary>
    /// A custom button control that supports executing an action when clicked.
    /// This is typically used in a guide or tutorial system where specific actions need to be triggered by user interaction.
    /// </summary>
    public class GuideButton : Button
    {
        /// <summary>
        /// Dependency property for storing the action that should be invoked when the button is clicked.
        /// </summary>
        public static readonly DependencyProperty ActionProperty
            = DependencyProperty.Register(
                nameof(Action),
                typeof(Action),
                typeof(GuideButton),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the action to be invoked when the button is clicked.
        /// </summary>
        public Action Action
        {
            get => (Action)GetValue(ActionProperty);
            set => SetValue(ActionProperty, value);
        }

        /// <summary>
        /// Static constructor to override the default style of the GuideButton control.
        /// </summary>
        static GuideButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GuideButton), new FrameworkPropertyMetadata(typeof(GuideButton)));
        }

        /// <summary>
        /// Called when the button is clicked. Invokes the <see cref="Action"/> if it is set.
        /// </summary>
        protected override void OnClick()
        {
            base.OnClick();
            Action?.Invoke();
        }
    }
}
