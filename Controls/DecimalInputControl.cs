using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace PropertyAgencyDesktopApp.Controls
{
    public class DecimalInputControl : InputControl
    {


        public decimal MaxValue
        {
            get { return (decimal)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue",
                                        typeof(decimal),
                                        typeof(DecimalInputControl),
                                        new PropertyMetadata(decimal.MaxValue));



        public decimal MinValue
        {
            get { return (decimal)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue",
                                        typeof(decimal),
                                        typeof(DecimalInputControl),
                                        new PropertyMetadata(default(decimal)));



        protected override void OnKeyUp(KeyEventArgs e)
        {
            if ((decimal.TryParse(Text, out decimal number)
              && number >= MinValue
              && number <= MaxValue) || (CanBeNull
                && string.IsNullOrEmpty(Text)))
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
