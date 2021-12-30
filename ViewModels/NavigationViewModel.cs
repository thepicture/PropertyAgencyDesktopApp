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
            if (System.ComponentModel.DesignerProperties
                .GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                return;
            }
            DependencyService.Get<INavigationService<ViewModelBase>>()
                             .Navigated += OnNavigated;

        }

        private void OnNavigated()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
