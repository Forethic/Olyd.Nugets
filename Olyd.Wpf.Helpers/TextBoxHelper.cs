using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Olyd.Wpf.Helpers
{
    /// <summary>
    /// Provides attached properties and behaviors for WPF TextBox controls
    /// </summary>
    public static class TextBoxHelper
    {
        #region [[ Placeholder ]]

        /// <summary>
        /// The attached property to set/get the placeholder text for a TextBox.
        /// </summary>
        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.RegisterAttached(
                "Placeholder",
                typeof(string),
                typeof(TextBoxHelper),
                new PropertyMetadata(string.Empty));

        /// <summary>
        /// Gets the placeholder text for the TextBox.
        /// </summary>
        public static string GetPlaceholder(DependencyObject d)
            => (string)d.GetValue(PlaceholderProperty);

        /// <summary>
        /// Sets the placeholder text for the TextBox.
        /// </summary>
        public static void SetPlaceholder(DependencyObject d, string value)
            => d.SetValue(PlaceholderProperty, value);

        #endregion

        #region [[ OnlyNumber ]]

        /// <summary>
        /// The attached property to enable/disable numeric-only input for a TextBox.
        /// </summary>
        public static readonly DependencyProperty OnlyNumberProperty =
            DependencyProperty.RegisterAttached(
                "OnlyNumber",
                typeof(bool),
                typeof(TextBoxHelper),
                new PropertyMetadata(BoxValue.False, OnOnlyNumberPropertyChanged));

        /// <summary>
        /// Gets whether the TextBox allows only numeric input.
        /// </summary>
        public static bool GetOnlyNumber(DependencyObject d)
            => (bool)d.GetValue(OnlyNumberProperty);

        /// <summary>
        /// Sets whether the TextBox should allow only numeric input.
        /// </summary>
        public static void SetOnlyNumber(DependencyObject d, bool value)
            => d.SetValue(OnlyNumberProperty, value);

        /// <summary>
        /// Called when the `OnlyNumber` property changes to add or remove event handlers for numeric validation.
        /// </summary>
        private static void OnOnlyNumberPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox textbox)
            {
                if ((bool)e.NewValue)
                {
                    WeakEventManager<TextBox, TextCompositionEventArgs>.AddHandler(textbox, nameof(TextBox.PreviewTextInput), Textbox_PreviewTextInput);
                    WeakEventManager<TextBox, KeyEventArgs>.AddHandler(textbox, nameof(TextBox.PreviewKeyDown), Textbox_PreviewKeydown);
                }
                else
                {
                    WeakEventManager<TextBox, TextCompositionEventArgs>.RemoveHandler(textbox, nameof(TextBox.PreviewTextInput), Textbox_PreviewTextInput);
                    WeakEventManager<TextBox, KeyEventArgs>.RemoveHandler(textbox, nameof(TextBox.PreviewKeyDown), Textbox_PreviewKeydown);
                }
            }
        }

        /// <summary>
        /// Handles the PreviewTextInput event to restrict input to numeric values only.
        /// </summary>
        private static void Textbox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out _);
        }

        /// <summary>
        /// Handles the PreviewKeyDown event to validate pasted content when Ctrl+V is pressed.
        /// </summary>
        private static void Textbox_PreviewKeydown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.V && Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
            {
                string clipboardText = Clipboard.GetText();

                if (string.IsNullOrEmpty(clipboardText))
                    e.Handled = true;

                e.Handled = !int.TryParse(clipboardText, out _);
            }
        }

        #endregion

        #region [[ EnterTrigger ]]

        /// <summary>
        /// The attached property to enable or disable triggering the TextBox's binding update when the Enter key is pressed.
        /// </summary>
        public static readonly DependencyProperty EnterTriggerProperty = DependencyProperty.RegisterAttached(
            "EnterTrigger",
            typeof(bool),
            typeof(TextBoxHelper),
            new PropertyMetadata(BoxValue.False, OnEnterTriggerChanged));

        /// <summary>
        /// Gets whether the Enter key triggers a binding update for the TextBox.
        /// </summary>
        /// <param name="d">The dependency object (should be a TextBox).</param>
        /// <returns>True if the Enter key triggers a binding update, otherwise false.</returns>
        public static bool GetEnterTrigger(DependencyObject d) =>
            (bool)d.GetValue(EnterTriggerProperty);

        /// <summary>
        /// Sets whether the Enter key should trigger a binding update for the TextBox.
        /// </summary>
        /// <param name="d">The dependency object (should be a TextBox).</param>
        /// <param name="value">True to enable the Enter key trigger, otherwise false.</param>
        public static void SetEnterTrigger(DependencyObject d, bool value) =>
            d.SetValue(EnterTriggerProperty, value);

        /// <summary>
        /// Handles changes to the `EnterTrigger` property. Adds or removes event handlers based on the property value.
        /// </summary>
        private static void OnEnterTriggerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox textBox)
            {
                if ((bool)e.NewValue)
                {
                    WeakEventManager<TextBox, KeyEventArgs>.AddHandler(textBox, nameof(TextBox.KeyDown), TextBox_KeyDown);
                }
                else
                {
                    WeakEventManager<TextBox, KeyEventArgs>.RemoveHandler(textBox, nameof(TextBox.KeyDown), TextBox_KeyDown);
                }
            }
        }

        /// <summary>
        /// Handles the KeyDown event to update the TextBox's binding source when the Enter key is pressed.
        /// </summary>
        private static void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (sender is TextBox textBox && e.Key == Key.Enter)
            {
                var expression = textBox.GetBindingExpression(TextBox.TextProperty);
                expression?.UpdateSource();
            }
        }

        #endregion
    }
}
