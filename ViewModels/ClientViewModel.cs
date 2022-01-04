using PropertyAgencyDesktopApp.Commands;
using PropertyAgencyDesktopApp.Models.Entities;
using PropertyAgencyDesktopApp.Services;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PropertyAgencyDesktopApp.ViewModels
{
    public class ClientViewModel : ViewModelBase
    {
        private readonly PropertyAgencyBaseEntities _context;
        private IEnumerable<Client> _clients;
        private string _searchText = string.Empty;
        public ClientViewModel()
        {
            Title = "Clients";
            _context = new PropertyAgencyBaseEntities();
            LoadClients();
        }

        private async void LoadClients()
        {
            Clients = await _context.Client.ToListAsync();
            if (!string.IsNullOrEmpty(SearchText))
            {
                IWordIndefiniteSearcher distanceCalculator =
                    DependencyService
                    .Get<IWordIndefiniteSearcher>();
                _ = await Task.Run(() =>
                  {
                      return Clients = from Client c in Clients
                                       where (c.FirstName != null
                                       && distanceCalculator
                                          .Calculate(SearchText,
                                                     c.FirstName) < 4)
                                       || (c.LastName != null
                                       && distanceCalculator
                                          .Calculate(SearchText,
                                                     c.LastName) < 4)
                                       || (c.MiddleName != null
                                       && distanceCalculator
                                          .Calculate(SearchText,
                                                     c.MiddleName) < 4)
                                       select c;
                  });
            }
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

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value)
                    && !string.IsNullOrEmpty(value))
                {
                    LoadClients();
                }
            }
        }

        private async void DeleteClient(object commandParameter)
        {
            Client client = commandParameter as Client;
            if (client.Offer.Count > 0 || client.Demand.Count > 0)
            {
                IsMessageClosed = false;
                MessageType = "Alert";
                ValidationMessage = ShowDeleteResultService
                                    .OnRelatedObjectsError(nameof(Client),
                                                           nameof(Offer),
                                                           nameof(Demand));
                return;
            }
            IQuestionService questionService =
                DependencyService.Get<IQuestionService>();
            string deleteTemplate = ShowDeleteResultService
                                    .OnDelete(nameof(Client));
            if (questionService.Ask(deleteTemplate))
            {
                IsMessageClosed = false;
                Client clientFromDatabase = _context.Client.Find(client.Id);
                _ = _context.Client.Remove(clientFromDatabase);
                try
                {
                    _ = await _context.SaveChangesAsync();
                    MessageType = "Alert";
                    ValidationMessage = ShowDeleteResultService
                                        .OnSuccessfulDelete(nameof(Client));
                    LoadClients();
                }
                catch (DbException ex)
                {
                    System.Diagnostics.Debug.Write(ex.StackTrace);
                    MessageType = "Danger";
                    ValidationMessage = ShowDeleteResultService
                                        .OnCommonDeleteError(nameof(Client));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Write(ex.StackTrace);
                    MessageType = "Danger";
                    ValidationMessage = ShowDeleteResultService
                                        .OnFatalDeleteError(nameof(Client));
                }
            }
        }
    }
}
