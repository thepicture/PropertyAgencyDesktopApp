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

        private RelayCommand navigateToOffersCommand;

        public ICommand NavigateToOffersCommand
        {
            get
            {
                if (navigateToOffersCommand == null)
                {
                    navigateToOffersCommand = new RelayCommand(NavigateToOffers);
                }

                return navigateToOffersCommand;
            }
        }

        private void NavigateToOffers(object commandParameter)
        {
            DependencyService.Get<INavigationService<ViewModelBase>>()
                        .Navigate<OfferViewModel>();
        }

        private RelayCommand navigateToDemandsCommand;

        public ICommand NavigateToDemandsCommand
        {
            get
            {
                if (navigateToDemandsCommand == null)
                {
                    navigateToDemandsCommand = new RelayCommand(NavigateToDemands);
                }

                return navigateToDemandsCommand;
            }
        }

        private void NavigateToDemands(object commandParameter)
        {
            DependencyService.Get<INavigationService<ViewModelBase>>()
                       .Navigate<DemandViewModel>();
        }

        private RelayCommand navigateToDealsCommand;

        public ICommand NavigateToDealsCommand
        {
            get
            {
                if (navigateToDealsCommand == null)
                {
                    navigateToDealsCommand = new RelayCommand(NavigateToDeals);
                }

                return navigateToDealsCommand;
            }
        }

        private void NavigateToDeals(object commandParameter)
        {
            DependencyService.Get<INavigationService<ViewModelBase>>()
                     .Navigate<DealViewModel>();
        }
    }
}
