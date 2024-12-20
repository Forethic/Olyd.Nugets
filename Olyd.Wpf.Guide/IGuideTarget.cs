using System.Windows;

namespace Olyd.Wpf.Guide
{
    /// <summary>
    /// Defines an interface for a guide target object, providing information about the visibility, position, and layout of the target element for a guide.
    /// </summary>
    public interface IGuideTarget
    {
        /// <summary>
        /// Hides the visual element of the guide target.
        /// </summary>
        void Hide();

        /// <summary>
        /// Gets the left position of the guide target element (relative to the parent container).
        /// </summary>
        double Left { get; }

        /// <summary>
        /// Gets the top position of the guide target element (relative to the parent container).
        /// </summary>
        double Top { get; }

        /// <summary>
        /// Gets the width of the guide target element.
        /// </summary>
        double Width { get; }

        /// <summary>
        /// Gets the height of the guide target element.
        /// </summary>
        double Height { get; }

        /// <summary>
        /// Gets the maximum height of the guide target element, used to constrain the visible area size.
        /// </summary>
        double MaxHeight { get; }

        /// <summary>
        /// Gets the content area width of the guide target, typically used for displaying text or controls.
        /// </summary>
        double ContentWidth { get; }

        /// <summary>
        /// Gets the content area height of the guide target, typically used for displaying text or controls.
        /// </summary>
        double ContentHeight { get; }

        /// <summary>
        /// Gets or sets the text for the "Next" button in the guide target.
        /// </summary>
        string NextString { get; }

        /// <summary>
        /// Gets or sets the text for the "Close" button in the guide target.
        /// </summary>
        string CloseString { get; }

        /// <summary>
        /// Gets the window state of the guide target element (e.g., whether it is maximized, minimized, or normal).
        /// </summary>
        WindowState WindowState { get; }

        /// <summary>
        /// Gets the window startup location of the guide target element (e.g., whether it should be centered or positioned at a specific place).
        /// </summary>
        WindowStartupLocation WindowStartupLocation { get; }

        /// <summary>
        /// Shows the visual element of the guide target, typically used to display the guide interface.
        /// </summary>
        void Show();
    }
}
