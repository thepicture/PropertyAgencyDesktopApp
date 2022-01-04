using PropertyAgencyDesktopApp.Services;
using PropertyAgencyDesktopApp.ViewModels;
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
            DependencyService.Register<MessageBoxQuestionService>();
            DependencyService.Register<LevenshteinWordIndefiniteSearcher>();
            DependencyService.Register<PropertyAgencyShowSaveResultService>();
            DependencyService.Get<INavigationService<ViewModelBase>>()
                 .Navigate<ClientViewModel>();
        }
    }
}
