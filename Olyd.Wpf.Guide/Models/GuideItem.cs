namespace Olyd.Wpf.Guide.Models
{
    /// <summary>
    /// Represents an individual item in a user guide, including its direction, content, and associated element.
    /// </summary>
    public class GuideItem
    {
        /// <summary>
        /// Gets or sets the direction in which the guide item points or applies.
        /// </summary>
        public Direction Direction { get; set; }

        /// <summary>
        /// Gets or sets the content of the guide item, such as instructional text or a description.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the name of the associated UI element that the guide item refers to.
        /// </summary>
        public string ElementName { get; set; }
    }
}
