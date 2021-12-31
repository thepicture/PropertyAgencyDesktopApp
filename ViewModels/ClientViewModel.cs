using PropertyAgencyDesktopApp.Commands;
using PropertyAgencyDesktopApp.Models.Entities;
using PropertyAgencyDesktopApp.Services;
using System;
using System.Collections.Generic;
using System.Data.Common;
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

        private RelayCommand deleteClientCommand;

        public ICommand DeleteClientCommand
        {
            get
            {
                if (deleteClientCommand == null)
                {
                    deleteClientCommand = new RelayCommand(DeleteClient);
                }

                return deleteClientCommand;
            }
        }

        private async void DeleteClient(object commandParameter)
        {
            Client client = commandParameter as Client;
            var questionService = DependencyService.Get<IQuestionService>();
            if (questionService.Ask("Do you really want " +
                "to delete client " +
                $"with credentials {client.Phone ?? client.Email}?"))
            {
                IsMessageClosed = false;
                Client clientFromDatabase = _context.Client.Find(client.Id);
                _ = _context.Client.Remove(clientFromDatabase);
                try
                {
                    _ = await _context.SaveChangesAsync();
                    MessageType = "Alert";
                    ValidationMessage = "Client was successfully deleted!";
                    LoadClients();
                }
                catch (DbException ex)
                {
                    System.Diagnostics.Debug.Write(ex.StackTrace);
                    MessageType = "Danger";
                    ValidationMessage = "Can't delete the client " +
                        "from the database. " +
                        "Try to go back and to the client again," +
                        "or reload the app if it doesn't help";
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Write(ex.StackTrace);
                    MessageType = "Danger";
                    ValidationMessage = "Can't delete the client " +
                        "from the database. " +
                        "Fatal error encountered. " +
                        "Reload the app and try again";
                }
            }
        }
    }
}
