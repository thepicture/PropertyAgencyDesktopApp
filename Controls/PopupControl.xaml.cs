using System.Windows;
using System.Windows.Controls;

namespace PropertyAgencyDesktopApp.Controls
{
    /// <summary>
    /// Interaction logic for PopupControl.xaml
    /// </summary>
    public partial class PopupControl : UserControl
    {
        public PopupControl()
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
                typeof(PopupControl),
                new PropertyMetadata(string.Empty));



        public string Type
        {
            get { return (string)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type",
                typeof(string),
                typeof(PopupControl),
                new PropertyMetadata("Alert"));

        private void OnClosing(object sender,
                                    System.Windows.Input.MouseButtonEventArgs e)
        {
            Visibility = Visibility.Collapsed;
        }
    }
}
