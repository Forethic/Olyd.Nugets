using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Olyd.Wpf.Helpers
{
    /// <summary>
    /// A helper class for custom controls that provides attached properties to enhance control behavior and appearance.
    /// </summary>
    public static class ControlsHelper
    {
        #region [[ Hover Border Brush ]]

        /// <summary>
        /// Gets the hover border brush of a control.
        /// </summary>
        /// <param name="obj">The dependency object from which to retrieve the hover border brush.</param>
        /// <returns>The current hover border brush of the control.</returns>
        public static Brush GetHoverBorderBrush(DependencyObject obj)
            => (Brush)obj.GetValue(HoverBorderBrushProperty);

        /// <summary>
        /// Sets the hover border brush of a control.
        /// </summary>
        /// <param name="obj">The dependency object on which to set the hover border brush.</param>
        /// <param name="value">The brush value to set for the hover border.</param>
        public static void SetHoverBorderBrush(DependencyObject obj, Brush value)
            => obj.SetValue(HoverBorderBrushProperty, value);

        /// <summary>
        /// An attached property that allows the customization of the hover border brush for controls.
        /// </summary>
        public static readonly DependencyProperty HoverBorderBrushProperty
            = DependencyProperty.RegisterAttached(
                "HoverBorderBrush",
                typeof(Brush),
                typeof(ControlsHelper),
                new FrameworkPropertyMetadata(Brushes.Transparent,
                    FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits,
                    new PropertyChangedCallback(OnBorderBrushChanged)));

        #endregion

        #region [[ Hover Border Thickness ]]

        /// <summary>
        /// Gets the hover border thickness of a control.
        /// </summary>
        /// <param name="obj">The dependency object from which to retrieve the hover border thickness.</param>
        /// <returns>The current hover border thickness of the control.</returns>
        public static Thickness GetHoverBorderThickness(DependencyObject obj)
            => (Thickness)obj.GetValue(HoverBorderThicknessProperty);

        /// <summary>
        /// Sets the hover border thickness of a control.
        /// </summary>
        /// <param name="obj">The dependency object on which to set the hover border thickness.</param>
        /// <param name="value">The thickness value to set for the hover border.</param>
        public static void SetHoverBorderThickness(DependencyObject obj, Thickness value)
            => obj.SetValue(HoverBorderThicknessProperty, value);

        /// <summary>
        /// An attached property that allows the customization of the hover border thickness for controls.
        /// </summary>
        public static readonly DependencyProperty HoverBorderThicknessProperty =
            DependencyProperty.RegisterAttached(
                "HoverBorderThickness",
                typeof(Thickness),
                typeof(ControlsHelper),
                new FrameworkPropertyMetadata(
                    new Thickness(0),
                    FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

        #endregion

        #region [[ Hover Foreground Brush ]]

        /// <summary>
        /// Gets the hover foreground brush of a control.
        /// </summary>
        /// <param name="obj">The dependency object from which to retrieve the hover foreground brush.</param>
        /// <returns>The current hover foreground brush of the control.</returns>
        public static Brush GetHoverForegroundBrush(DependencyObject obj)
            => (Brush)obj.GetValue(HoverForegroundBrushProperty);

        /// <summary>
        /// Sets the hover foreground brush of a control.
        /// </summary>
        /// <param name="obj">The dependency object on which to set the hover foreground brush.</param>
        /// <param name="value">The brush value to set for the hover foreground.</param>
        public static void SetHoverForegroundBrush(DependencyObject obj, Brush value)
            => obj.SetValue(HoverForegroundBrushProperty, value);

        /// <summary>
        /// An attached property that allows the customization of the hover foreground brush for controls.
        /// </summary>
        public static readonly DependencyProperty HoverForegroundBrushProperty =
            DependencyProperty.RegisterAttached(
                "HoverForegroundBrush",
                typeof(Brush),
                typeof(ControlsHelper),
                new FrameworkPropertyMetadata(
                    Brushes.Transparent,
                    FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits,
                    new PropertyChangedCallback(OnForegroundChanged)));

        #endregion

        #region [[ Hover Background Brush ]]

        /// <summary>
        /// Gets the hover background brush of a control.
        /// </summary>
        /// <param name="obj">The dependency object from which to retrieve the hover background brush.</param>
        /// <returns>The current hover background brush of the control.</returns>
        public static Brush GetHoverBackgroundBrush(DependencyObject obj)
            => (Brush)obj.GetValue(HoverBackgroundBrushProperty);

        /// <summary>
        /// Sets the hover background brush of a control.
        /// </summary>
        /// <param name="obj">The dependency object on which to set the hover background brush.</param>
        /// <param name="value">The brush value to set for the hover background.</param>
        public static void SetHoverBackgroundBrush(DependencyObject obj, Brush value)
            => obj.SetValue(HoverBackgroundBrushProperty, value);

        /// <summary>
        /// An attached property that allows the customization of the hover background brush for controls.
        /// </summary>
        public static readonly DependencyProperty HoverBackgroundBrushProperty =
            DependencyProperty.RegisterAttached(
                "HoverBackgroundBrush",
                typeof(Brush),
                typeof(ControlsHelper),
                new FrameworkPropertyMetadata(
                    Brushes.Transparent,
                    FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits,
                    new PropertyChangedCallback(OnBackgroundChanged)));

        #endregion

        #region [[ Press Border Brush ]]

        /// <summary>
        /// Gets the pressed border brush of a control.
        /// </summary>
        /// <param name="obj">The dependency object from which to retrieve the pressed border brush.</param>
        /// <returns>The current pressed border brush of the control.</returns>
        public static Brush GetPressBorderBrush(DependencyObject obj)
            => (Brush)obj.GetValue(PressBorderBrushProperty);

        /// <summary>
        /// Sets the pressed border brush of a control.
        /// </summary>
        /// <param name="obj">The dependency object on which to set the pressed border brush.</param>
        /// <param name="value">The brush value to set for the pressed border.</param>
        public static void SetPressBorderBrush(DependencyObject obj, Brush value)
            => obj.SetValue(PressBorderBrushProperty, value);

        /// <summary>
        /// An attached property that allows the customization of the pressed border brush for controls.
        /// </summary>
        public static readonly DependencyProperty PressBorderBrushProperty =
            DependencyProperty.RegisterAttached(
                "PressBorderBrush",
                typeof(Brush),
                typeof(ControlsHelper),
                new FrameworkPropertyMetadata(
                    Brushes.Transparent,
                    FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits,
                    new PropertyChangedCallback(OnBorderBrushChanged)));

        #endregion

        #region [[ Press Border Thickness ]]

        /// <summary>
        /// Gets the pressed border thickness of a control.
        /// </summary>
        /// <param name="obj">The dependency object from which to retrieve the pressed border thickness.</param>
        /// <returns>The current pressed border thickness of the control.</returns>
        public static Thickness GetPressBorderThickness(DependencyObject obj)
            => (Thickness)obj.GetValue(PressBorderThicknessProperty);

        /// <summary>
        /// Sets the pressed border thickness of a control.
        /// </summary>
        /// <param name="obj">The dependency object on which to set the pressed border thickness.</param>
        /// <param name="value">The thickness value to set for the pressed border.</param>
        public static void SetPressBorderThickness(DependencyObject obj, Thickness value)
            => obj.SetValue(PressBorderThicknessProperty, value);

        /// <summary>
        /// An attached property that allows the customization of the pressed border thickness for controls.
        /// </summary>
        public static readonly DependencyProperty PressBorderThicknessProperty =
            DependencyProperty.RegisterAttached(
                "PressBorderThickness",
                typeof(Thickness),
                typeof(ControlsHelper),
                new FrameworkPropertyMetadata(
                    new Thickness(0),
                    FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

        #endregion

        #region [[ Press Foreground Brush ]]

        /// <summary>
        /// Gets the pressed foreground brush of a control.
        /// </summary>
        /// <param name="obj">The dependency object from which to retrieve the pressed foreground brush.</param>
        /// <returns>The current pressed foreground brush of the control.</returns>
        public static Brush GetPressForegroundBrush(DependencyObject obj)
            => (Brush)obj.GetValue(PressForegroundBrushProperty);

        /// <summary>
        /// Sets the pressed foreground brush of a control.
        /// </summary>
        /// <param name="obj">The dependency object on which to set the pressed foreground brush.</param>
        /// <param name="value">The brush value to set for the pressed foreground.</param>
        public static void SetPressForegroundBrush(DependencyObject obj, Brush value)
            => obj.SetValue(PressForegroundBrushProperty, value);

        /// <summary>
        /// An attached property that allows the customization of the pressed foreground brush for controls.
        /// </summary>
        public static readonly DependencyProperty PressForegroundBrushProperty =
            DependencyProperty.RegisterAttached(
                "PressForegroundBrush",
                typeof(Brush),
                typeof(ControlsHelper),
                new FrameworkPropertyMetadata(
                    Brushes.Transparent,
                    FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits,
                    new PropertyChangedCallback(OnForegroundChanged)));

        #endregion

        #region [[ Press Background Brush ]]

        /// <summary>
        /// Gets the pressed background brush of a control.
        /// </summary>
        /// <param name="obj">The dependency object from which to retrieve the pressed background brush.</param>
        /// <returns>The current pressed background brush of the control.</returns>
        public static Brush GetPressBackgroundBrush(DependencyObject obj)
            => (Brush)obj.GetValue(PressBackgroundBrushProperty);

        /// <summary>
        /// Sets the pressed background brush of a control.
        /// </summary>
        /// <param name="obj">The dependency object on which to set the pressed background brush.</param>
        /// <param name="value">The brush value to set for the pressed background.</param>
        public static void SetPressBackgroundBrush(DependencyObject obj, Brush value)
            => obj.SetValue(PressBackgroundBrushProperty, value);

        /// <summary>
        /// An attached property that allows the customization of the pressed background brush for controls.
        /// </summary>
        public static readonly DependencyProperty PressBackgroundBrushProperty =
            DependencyProperty.RegisterAttached(
                "PressBackgroundBrush",
                typeof(Brush),
                typeof(ControlsHelper),
                new FrameworkPropertyMetadata(
                    Brushes.Transparent,
                    FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits,
                    new PropertyChangedCallback(OnBackgroundChanged)));

        #endregion

        #region [[ Disable Border Brush ]]

        /// <summary>
        /// Gets the disabled border brush of a control.
        /// </summary>
        /// <param name="obj">The dependency object from which to retrieve the disabled border brush.</param>
        /// <returns>The current disabled border brush of the control.</returns>
        public static Brush GetDisableBorderBrush(DependencyObject obj)
            => (Brush)obj.GetValue(DisableBorderBrushProperty);

        /// <summary>
        /// Sets the disabled border brush of a control.
        /// </summary>
        /// <param name="obj">The dependency object on which to set the disabled border brush.</param>
        /// <param name="value">The brush value to set for the disabled border.</param>
        public static void SetDisableBorderBrush(DependencyObject obj, Brush value)
            => obj.SetValue(DisableBorderBrushProperty, value);

        /// <summary>
        /// An attached property that allows the customization of the disabled border brush for controls.
        /// </summary>
        public static readonly DependencyProperty DisableBorderBrushProperty =
            DependencyProperty.RegisterAttached(
                "DisableBorderBrush",
                typeof(Brush),
                typeof(ControlsHelper),
                new FrameworkPropertyMetadata(
                    Brushes.Transparent,
                    FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits,
                    new PropertyChangedCallback(OnBorderBrushChanged)));

        #endregion

        #region [[ Disable Border Thickness ]]

        /// <summary>
        /// Gets the disabled border thickness of a control.
        /// </summary>
        /// <param name="obj">The dependency object from which to retrieve the disabled border thickness.</param>
        /// <returns>The current disabled border thickness of the control.</returns>
        public static Thickness GetDisableBorderThickness(DependencyObject obj)
            => (Thickness)obj.GetValue(DisableBorderThicknessProperty);

        /// <summary>
        /// Sets the disabled border thickness of a control.
        /// </summary>
        /// <param name="obj">The dependency object on which to set the disabled border thickness.</param>
        /// <param name="value">The thickness value to set for the disabled border.</param>
        public static void SetDisableBorderThickness(DependencyObject obj, Thickness value)
            => obj.SetValue(DisableBorderThicknessProperty, value);

        /// <summary>
        /// An attached property that allows the customization of the disabled border thickness for controls.
        /// </summary>
        public static readonly DependencyProperty DisableBorderThicknessProperty =
            DependencyProperty.RegisterAttached(
                "DisableBorderThickness",
                typeof(Thickness),
                typeof(ControlsHelper),
                new FrameworkPropertyMetadata(
                    new Thickness(0),
                    FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

        #endregion

        #region [[ Disable Foreground Brush ]]

        /// <summary>
        /// Gets the disabled foreground brush of a control.
        /// </summary>
        /// <param name="obj">The dependency object from which to retrieve the disabled foreground brush.</param>
        /// <returns>The current disabled foreground brush of the control.</returns>
        public static Brush GetDisableForegroundBrush(DependencyObject obj)
            => (Brush)obj.GetValue(DisableForegroundBrushProperty);

        /// <summary>
        /// Sets the disabled foreground brush of a control.
        /// </summary>
        /// <param name="obj">The dependency object on which to set the disabled foreground brush.</param>
        /// <param name="value">The brush value to set for the disabled foreground.</param>
        public static void SetDisableForegroundBrush(DependencyObject obj, Brush value)
            => obj.SetValue(DisableForegroundBrushProperty, value);

        /// <summary>
        /// An attached property that allows the customization of the disabled foreground brush for controls.
        /// </summary>
        public static readonly DependencyProperty DisableForegroundBrushProperty =
            DependencyProperty.RegisterAttached(
                "DisableForegroundBrush",
                typeof(Brush),
                typeof(ControlsHelper),
                new FrameworkPropertyMetadata(
                    Brushes.Transparent,
                    FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits,
                    new PropertyChangedCallback(OnForegroundChanged)));

        #endregion

        #region [[ Disable Background Brush ]]

        /// <summary>
        /// Gets the disabled background brush of a control.
        /// </summary>
        /// <param name="obj">The dependency object from which to retrieve the disabled background brush.</param>
        /// <returns>The current disabled background brush of the control.</returns>
        public static Brush GetDisableBackgroundBrush(DependencyObject obj)
            => (Brush)obj.GetValue(DisableBackgroundBrushProperty);

        /// <summary>
        /// Sets the disabled background brush of a control.
        /// </summary>
        /// <param name="obj">The dependency object on which to set the disabled background brush.</param>
        /// <param name="value">The brush value to set for the disabled background.</param>
        public static void SetDisableBackgroundBrush(DependencyObject obj, Brush value)
            => obj.SetValue(DisableBackgroundBrushProperty, value);

        /// <summary>
        /// An attached property that allows the customization of the disabled background brush for controls.
        /// </summary>
        public static readonly DependencyProperty DisableBackgroundBrushProperty =
            DependencyProperty.RegisterAttached(
                "DisableBackgroundBrush",
                typeof(Brush),
                typeof(ControlsHelper),
                new FrameworkPropertyMetadata(
                    Brushes.Transparent,
                    FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits,
                    new PropertyChangedCallback(OnBackgroundChanged)));

        #endregion

        #region [[ Corner Radius ]]

        /// <summary>
        /// Gets the corner radius of a control.
        /// </summary>
        /// <param name="obj">The dependency object from which to retrieve the corner radius.</param>
        /// <returns>The current corner radius of the control.</returns>
        public static CornerRadius GetCornerRadius(DependencyObject obj)
            => (CornerRadius)obj.GetValue(CornerRadiusProperty);

        /// <summary>
        /// Sets the corner radius of a control.
        /// </summary>
        /// <param name="obj">The dependency object on which to set the corner radius.</param>
        /// <param name="value">The corner radius value to set.</param>
        public static void SetCornerRadius(DependencyObject obj, CornerRadius value)
            => obj.SetValue(CornerRadiusProperty, value);

        /// <summary>
        /// An attached property that allows the customization of corner radius for controls.
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.RegisterAttached(
                nameof(CornerRadius), // The name of the attached property.
                typeof(CornerRadius), // The type of the property value.
                typeof(ControlsHelper), // The owner type of the property.
                new FrameworkPropertyMetadata(
                    new CornerRadius(), // Default value for the corner radius.
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender // Property change options that trigger re-measure and re-render.
                ));

        #endregion

        #region [[ Methods ]]

        private static void OnForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
            {
                d.SetValue(e.Property, d.GetValue(Control.ForegroundProperty));
            }
        }

        private static void OnBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
            {
                d.SetValue(e.Property, d.GetValue(Control.BackgroundProperty));
            }
        }

        private static void OnBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
            {
                d.SetValue(e.Property, d.GetValue(Control.BorderBrushProperty));
            }
        }

        #endregion
    }
}
