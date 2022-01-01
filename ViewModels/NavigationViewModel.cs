using PropertyAgencyDesktopApp.Commands;
using PropertyAgencyDesktopApp.Services;
using System.Windows.Input;

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

        private RelayCommand navigateToClientsCommand;

        public ICommand NavigateToClientsCommand
        {
            get
            {
                if (navigateToClientsCommand == null)
                {
                    navigateToClientsCommand = new RelayCommand(NavigateToClients);
                }

                return navigateToClientsCommand;
            }
        }

        private void NavigateToClients(object commandParameter)
        {
            DependencyService.Get<INavigationService<ViewModelBase>>()
                             .Navigate<ClientViewModel>();
        }

        private RelayCommand navigateToAgentsCommand;

        public ICommand NavigateToAgentsCommand
        {
            get
            {
                if (navigateToAgentsCommand == null)
                {
                    navigateToAgentsCommand = new RelayCommand(NavigateToAgents);
                }

                return navigateToAgentsCommand;
            }
        }

        private void NavigateToAgents(object commandParameter)
        {
            DependencyService.Get<INavigationService<ViewModelBase>>()
                            .Navigate<AgentViewModel>();
        }

        private RelayCommand navigateToPropertiesCommand;

        public ICommand NavigateToPropertiesCommand
        {
            get
            {
                if (navigateToPropertiesCommand == null)
                {
                    navigateToPropertiesCommand = new RelayCommand(NavigateToProperties);
                }

                return navigateToPropertiesCommand;
            }
        }

        private void NavigateToProperties(object commandParameter)
        {
            DependencyService.Get<INavigationService<ViewModelBase>>()
                          .Navigate<PropertyViewModel>();
        }
    }
}
