﻿using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Media;

namespace PropertyAgencyDesktopApp.Controls
{
    public class TextInputControl : InputControl
    {
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (Regex.IsMatch(Text, @"^[a-zA-Zа-яА-Я]+$")
                && Text.Length >= MinLength
                && Text.Length <= MaxLength)
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
