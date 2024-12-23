using System.Globalization;
using System.Windows.Data;

namespace Olyd.Wpf.Converters
{
    /// <summary>
    /// A converter that returns different values based on a given condition.
    /// This converter supports both converting and converting back operations. 
    /// It checks if a provided value matches a condition (either a parameter or a predefined 'When' value),
    /// and returns different results depending on whether the condition is met.
    /// </summary>
    public class ValueWhenConverter : IValueConverter
    {
        /// <summary>
        /// Value to return when condition is met
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Value to return when condition is not met
        /// </summary>
        public object Otherwise { get; set; }

        /// <summary>
        /// Condition to check against
        /// </summary>
        public object When { get; set; }

        /// <summary>
        /// Value to return when ConvertBack is called and the condition is not met
        /// </summary>
        public object OtherwiseValueBack { get; set; }

        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Simplified check for parameter or When
            var condition = parameter ?? When;

            // Return Value if condition matches, otherwise return Otherwise
            return object.Equals(value, condition) ? Value : Otherwise;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Early check for OtherwiseValueBack
            if (OtherwiseValueBack == null)
                throw new InvalidOperationException("Cannot ConvertBack if no OtherwiseValueBack is set!");

            // Return When if value matches Value, otherwise return OtherwiseValueBack
            return object.Equals(value, Value) ? When : OtherwiseValueBack;
        }
    }
}