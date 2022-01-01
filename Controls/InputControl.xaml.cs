using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PropertyAgencyDesktopApp.Controls
{
    /// <summary>
    /// Interaction logic for InputControl.xaml
    /// </summary>
    public partial class InputControl : UserControl
    {
        public InputControl()
        {
            InitializeComponent();

            DataContext = this;
        }

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
                                            false));

        public bool IsValidated
        {
            get { return (bool)GetValue(IsValidatedProperty); }
            set { SetValue(IsValidatedProperty, value); }
        }

        public static readonly DependencyProperty IsValidatedProperty =
            DependencyProperty.Register("IsValidated",
                                        typeof(bool),
                                        typeof(InputControl),
                                        new PropertyMetadata(false));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }



        public int MaxLength
        {
            get { return (int)GetValue(MaxLengthProperty); }
            set { SetValue(MaxLengthProperty, value); }
        }

        public static readonly DependencyProperty MaxLengthProperty =
            DependencyProperty.Register("MaxLength",
                                        typeof(int),
                                        typeof(InputControl),
                                        new PropertyMetadata(int.MaxValue));

        public int MinLength
        {
            get { return (int)GetValue(MinLengthProperty); }
            set { SetValue(MinLengthProperty, value); }
        }

        public static readonly DependencyProperty MinLengthProperty =
            DependencyProperty.Register("MinLength",
                                        typeof(int),
                                        typeof(InputControl),
                                        new PropertyMetadata(0));

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text",
                typeof(string),
                typeof(InputControl),
                new FrameworkPropertyMetadata(string.Empty,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public Brush Color
        {
            get { return (Brush)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color",
                                        typeof(Brush),
                                        typeof(InputControl),
                                        new PropertyMetadata(Brushes.Gray));



        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header",
                                        typeof(string),
                                        typeof(InputControl),
                                        new PropertyMetadata("Header"));



        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register("Placeholder",
                                        typeof(string),
                                        typeof(InputControl),
                                        new PropertyMetadata("Enter a text"));



        public string ValidationText
        {
            get { return (string)GetValue(ValidationTextProperty); }
            set { SetValue(ValidationTextProperty, value); }
        }

        public static readonly DependencyProperty ValidationTextProperty =
            DependencyProperty.Register("ValidationText",
                                        typeof(string),
                                        typeof(InputControl),
                                        new PropertyMetadata(string.Empty));
    }
}
