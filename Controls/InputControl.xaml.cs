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

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text",
                typeof(string),
                typeof(InputControl),
                new FrameworkPropertyMetadata("Notext",
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
