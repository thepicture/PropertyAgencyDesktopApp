using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace PropertyAgencyDesktopApp.Controls
{
    public class DistinctIntegerInputControl : InputControl
    {
       
        public decimal MinInteger
        {
            get { return (decimal)GetValue(MinIntegerProperty); }
            set { SetValue(MinIntegerProperty, value); }
        }

        public static readonly DependencyProperty MinIntegerProperty =
            DependencyProperty.Register("MinInteger",
                                        typeof(decimal),
                                        typeof(DistinctIntegerInputControl),
                                        new PropertyMetadata(default(decimal)));



        public decimal MaxInteger
        {
            get { return (decimal)GetValue(MaxIntegerProperty); }
            set { SetValue(MaxIntegerProperty, value); }
        }

        public static readonly DependencyProperty MaxIntegerProperty =
            DependencyProperty.Register("MaxInteger",
                                        typeof(decimal),
                                        typeof(DistinctIntegerInputControl),
                                        new PropertyMetadata(decimal.MaxValue));



        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (decimal.TryParse(Text, out decimal result)
                && result >= MinInteger
                && result <= MaxInteger || (string.IsNullOrEmpty(Text) && CanBeNull))
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
