using PropertyAgencyDesktopApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

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

        private void LoadRealEstates()
        {
            RealEstateTypes = new List<string>
            {
                "Apartment",
                "House",
                "Land"
            };
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

        private IEnumerable<string> _realEstateTypes;
        private string _currentRealEstateType;

        public IEnumerable<string> RealEstateTypes
        {
            get => _realEstateTypes;
            set => SetProperty(ref _realEstateTypes, value);
        }
        public string CurrentRealEstateType
        {
            get => _currentRealEstateType;
            set => SetProperty(ref _currentRealEstateType, value);
        }
        public bool IsInCreateMode
        {
            get => _isInCreateMode;
            set => SetProperty(ref _isInCreateMode, value);
        }
    }
}
