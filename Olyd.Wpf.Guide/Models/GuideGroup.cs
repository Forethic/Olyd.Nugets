using System.Windows;

namespace Olyd.Wpf.Guide.Models
{
    /// <summary>
    /// Represents a group of guide items that collectively define a step in a user guide process.
    /// </summary>
    public class GuideGroup
    {
        /// <summary>
        /// Gets or sets an action to execute when the user proceeds to the next step.
        /// This action takes the current window as a parameter for context.
        /// </summary>
        public Action<Window> OnNextAction { get; set; }

        /// <summary>
        /// Gets or sets the collection of guide items included in this guide group.
        /// Each item provides details about a specific part of the guide.
        /// </summary>
        public List<GuideItem> Items { get; set; }

        /// <summary>
        /// Gets or sets the name of the current step in the guide.
        /// This name can be used for identification or display purposes.
        /// </summary>
        public string StepName { get; set; }
    }
}
