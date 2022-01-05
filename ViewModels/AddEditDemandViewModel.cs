using PropertyAgencyDesktopApp.Commands;
using PropertyAgencyDesktopApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PropertyAgencyDesktopApp.ViewModels
{
    public class AddEditDemandViewModel : ViewModelBase
    {
        private Demand _currentDemand = new Demand();
        private readonly PropertyAgencyBaseEntities _context =
            new PropertyAgencyBaseEntities();
        private bool _isInCreateMode = true;
        public AddEditDemandViewModel()
        {
            Title = "Add a new demand";
            _ = LoadClients()
            .ContinueWith(t => LoadAgents())
            .ContinueWith(t => LoadAdresses())
            .ContinueWith(t => LoadRealEstates());
        }

        private async void LoadRealEstates()
        {
            RealEstateTypes = await _context.RealEstateType.ToListAsync();
            CurrentRealEstateType = RealEstateTypes.First();
        }

        private async Task LoadAdresses()
        {
            Addresses = await _context.PropertyAddress.ToListAsync();
            CurrentAddress = Addresses.FirstOrDefault();
        }

        private async Task LoadAgents()
        {
            Agents = await _context.Agent.ToListAsync();
            CurrentAgent = Agents.FirstOrDefault();
        }

        private async Task LoadClients()
        {
            Clients = await _context.Client.ToListAsync();
            CurrentClient = Clients.FirstOrDefault();
        }

        public Demand CurrentDemand
        {
            get => _currentDemand;
            set => SetProperty(ref _currentDemand, value);
        }

        private IEnumerable<Client> _clients;

        public IEnumerable<Client> Clients
        {
            get => _clients;
            set => SetProperty(ref _clients, value);
        }

        private Client _currentClient;

        private IEnumerable<Agent> _agents;

        public IEnumerable<Agent> Agents
        {
            get => _agents;
            set => SetProperty(ref _agents, value);
        }

        private Agent _currentAgent;

        private IEnumerable<PropertyAddress> _addresses;

        public IEnumerable<PropertyAddress> Addresses
        {
            get => _addresses;
            set => SetProperty(ref _addresses, value);
        }

        private PropertyAddress _currentAddress;

        public Client CurrentClient
        {
            get => _currentClient;
            set => SetProperty(ref _currentClient, value);
        }
        public Agent CurrentAgent
        {
            get => _currentAgent;
            set => SetProperty(ref _currentAgent, value);
        }
        public PropertyAddress CurrentAddress
        {
            get => _currentAddress;
            set => SetProperty(ref _currentAddress, value);
        }

        private IEnumerable<RealEstateType> _realEstateTypes;
        private RealEstateType _currentRealEstateType;

        public IEnumerable<RealEstateType> RealEstateTypes
        {
            get => _realEstateTypes;
            set => SetProperty(ref _realEstateTypes, value);
        }
        public RealEstateType CurrentRealEstateType
        {
            get => _currentRealEstateType;
            set => SetProperty(ref _currentRealEstateType, value);
        }
        public bool IsInCreateMode
        {
            get => _isInCreateMode;
            set => SetProperty(ref _isInCreateMode, value);
        }

        private RelayCommand saveDemandCommand;

        public ICommand SaveDemandCommand
        {
            get
            {
                if (saveDemandCommand == null)
                {
                    saveDemandCommand = new RelayCommand(SaveDemand);
                }

                return saveDemandCommand;
            }
        }

        private async void SaveDemand(object commandParameter)
        {
            IsMessageClosed = false;
            if (CurrentDemand.DemandId == 0)
            {
                switch (CurrentRealEstateType.TypeName)
                {
                    case "Apartment":
                        break;
                    case "House":
                        break;
                    case "Land":
                        break;
                    default:
                        break;
                }
                _ = _context.Demand.Add(CurrentDemand);

            }

            CurrentDemand.PropertyAddress = CurrentAddress;
            CurrentDemand.RealEstateType = CurrentRealEstateType;
            CurrentDemand.Agent = CurrentAgent;
            CurrentDemand.Client = CurrentClient;

            try
            {
                _ = await _context.SaveChangesAsync();
                MessageType = "Alert";
                ValidationMessage = ValidationMessage = ShowSaveResultService
                                    .GetOnSavedTemplate(nameof(Demand));
            }
            catch (DbException ex)
            {
                System.Diagnostics.Debug.Write(ex.StackTrace);
                MessageType = "Danger";
                ValidationMessage = ShowSaveResultService
                                    .GetOnCommonErrorTemplate(nameof(Demand));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex.StackTrace);
                MessageType = "Danger";
                ValidationMessage = ShowSaveResultService
                                    .GetOnFatalErrorTemplate(nameof(Demand));
            }
        }
    }
}
