using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace PropertyAgencyDesktopApp.Controls
{
    public class TextInputControl : InputControl
    {
        public bool CanBeNull
        {
            get => (bool)GetValue(CanBeNullProperty);
            set => SetValue(CanBeNullProperty, value);
        }

        public static readonly DependencyProperty CanBeNullProperty =
            DependencyProperty.Register("CanBeNull",
                                        typeof(bool),
                                        typeof(TextInputControl),
                                        new PropertyMetadata(
                                            false,
                                            OnCanBeNullChanged));

        private static void OnCanBeNullChanged
            (DependencyObject d,
             DependencyPropertyChangedEventArgs e)
        {
            d.SetValue(IsValidatedProperty, true);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (CanBeNull || (Text != null
                && Regex.IsMatch(Text, @"^[a-zA-Zа-яА-Я]+$")
                && Text.Length >= MinLength
                && Text.Length <= MaxLength))
            {
                IsValidated = true;
                Color = Brushes.Green;
            }
            else
            {
                IsValidated = false;
                Color = Brushes.Red;
            }
            base.OnKeyUp(e);
        }
    }
}
