using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Media;

namespace PropertyAgencyDesktopApp.Controls
{
    public class EmailInputControl : InputControl
    {
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (Regex.IsMatch(Text, @"^\w+@\w+\.\w+$")
                || (CanBeNull && string.IsNullOrEmpty(Text)))
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
