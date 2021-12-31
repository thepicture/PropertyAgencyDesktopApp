using PropertyAgencyDesktopApp.Commands;
using PropertyAgencyDesktopApp.Models.Entities;
using PropertyAgencyDesktopApp.Services;
using System.Collections.Generic;
using System.Data.Entity;
using System.Windows.Input;

namespace PropertyAgencyDesktopApp.ViewModels
{
    public class ClientViewModel : ViewModelBase
    {
        private readonly PropertyAgencyBaseEntities _context;
        private IEnumerable<Client> _clients;
        public ClientViewModel()
        {
            Title = "Clients";
            _context = new PropertyAgencyBaseEntities();
            LoadClients();
        }

        private async void LoadClients()
        {
            Clients = await _context.Client.ToListAsync();
        }

        public IEnumerable<Client> Clients
        {
            get => _clients;
            set => SetProperty(ref _clients, value);
        }

        private RelayCommand addNewClientCommand;

        public ICommand AddNewClientCommand
        {
            get
            {
                if (addNewClientCommand == null)
                {
                    addNewClientCommand = new RelayCommand(AddNewClient);
                }

                return addNewClientCommand;
            }
        }

        private void AddNewClient(object commandParameter)
        {
            DependencyService.Get<INavigationService<ViewModelBase>>()
                             .Navigate
                             <AddEditClientViewModel>();
        }

        private RelayCommand editClientCommand;

        public ICommand EditClientCommand
        {
            get
            {
                if (editClientCommand == null)
                {
                    editClientCommand = new RelayCommand(EditClient);
                }

                return editClientCommand;
            }
        }

        private void EditClient(object commandParameter)
        {
            Client client = commandParameter as Client;
            DependencyService.Get<INavigationService<ViewModelBase>>()
                          .NavigateWithParameter
                          <AddEditClientViewModel>(client);
        }
    }
}
