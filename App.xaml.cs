using PropertyAgencyDesktopApp.Services;
using System.Windows;

namespace PropertyAgencyDesktopApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            DependencyService.Register<ViewModelNavigationService>();
            NavigationView view = new NavigationView();
            view.Show();
        }
    }
}
