using PropertyAgencyDesktopApp.Services;

namespace PropertyAgencyDesktopApp.ViewModels
{
    public class NavigationViewModel : ViewModelBase
    {
        public ViewModelBase CurrentViewModel =>
            DependencyService
            .Get<INavigationService<ViewModelBase>>()
            .CurrentNavigationTarget;
        public NavigationViewModel()
        {
            DependencyService.Get<INavigationService<ViewModelBase>>()
                             .Navigated += OnNavigated;
            DependencyService.Get<INavigationService<ViewModelBase>>()
                             .Navigate<ClientViewModel>();
        }

        private void OnNavigated()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
