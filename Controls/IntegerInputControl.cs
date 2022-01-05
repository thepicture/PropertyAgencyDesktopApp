using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Media;

namespace PropertyAgencyDesktopApp.Controls
{
    public class IntegerInputControl : InputControl
    {
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (Text != null && (Regex.IsMatch(Text, @"^[0-9\-]+$")
          && Text?.Length + 1 >= MinLength
          && Text?.Length + 1 <= MaxLength) || (CanBeNull
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
