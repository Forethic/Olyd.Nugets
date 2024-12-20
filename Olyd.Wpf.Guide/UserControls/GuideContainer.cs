namespace Olyd.Wpf.Guide.UserControls
{
    /// <summary>
    /// A container control that acts as a bridge to manage interactions with a GuideWindow.
    /// It provides functionality for triggering guide steps and managing step progression.
    /// </summary>
    public class GuideContainer : System.Windows.Controls.UserControl
    {
        private GuideWindow _currentGuideWindow;

        internal void BindGuideWindow(GuideWindow guideWindow)
        {
            // Unbind from the previous GuideWindow if any
            if (_currentGuideWindow != null)
            {
                _currentGuideWindow.OnNextActionTrigger -= GuideWindow_OnNextActionTrigger;
            }

            // Bind to the new GuideWindow
            _currentGuideWindow = guideWindow ?? throw new ArgumentNullException(nameof(guideWindow), "GuideWindow cannot be null.");
            _currentGuideWindow.OnNextActionTrigger += GuideWindow_OnNextActionTrigger;
        }

        /// <summary>
        /// Event handler triggered when the GuideWindow's next action event occurs.
        /// </summary>
        private void GuideWindow_OnNextActionTrigger(object sender, EventArgs e) => OnNextShowGuide();

        /// <summary>
        /// Invokes the logic for showing the next guide step. Can be overridden in derived classes.
        /// </summary>
        protected virtual void OnNextShowGuide() => ShowGuide();

        /// <summary>
        /// Displays the next guide step by delegating to the associated GuideWindow.
        /// </summary>
        protected void ShowGuide() => _currentGuideWindow?.ShowGuide();

        /// <summary>
        /// Gets the name of the current guide step from the associated GuideWindow.
        /// </summary>
        protected string StepName => _currentGuideWindow?.StepName;
    }
}
