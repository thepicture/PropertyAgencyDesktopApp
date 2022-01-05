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
        private ApartmentDemand _currentApartmentDemand =
            new ApartmentDemand();
        private HouseDemand _currentHouseDemand =
            new HouseDemand();
        private LandDemand _currentLandDemand =
            new LandDemand();
        public AddEditDemandViewModel()
        {
            Title = "Add a new demand";
            _ = LoadClients()
            .ContinueWith(t => LoadAgents())
            .ContinueWith(t => LoadAdresses())
            .ContinueWith(t => LoadRealEstates());
        }

        public AddEditDemandViewModel(Demand demand)
        {
            Title = "Edit the existing demand";
            _ = LoadClients(demand.ClientId)
            .ContinueWith(t => LoadAgents(demand.AgentId))
            .ContinueWith(t => LoadAdresses(demand.AddressId))
            .ContinueWith(t => LoadRealEstates(demand.RealEstateTypeId));
            CurrentDemand = 
                _context.Demand
                .FirstOrDefault(d => d.DemandId == demand.DemandId);
            switch (demand.RealEstateType.Id)
            {
                case 1:
                    CurrentApartmentDemand =
                        _context.Demand
                        .OfType<ApartmentDemand>()
                        .FirstOrDefault(ad => ad.DemandId == demand.DemandId);
                    break;
                case 2:
                    CurrentHouseDemand =
                        _context.Demand
                        .OfType<HouseDemand>()
                        .FirstOrDefault(hd => hd.DemandId == demand.DemandId);
                    break;
                case 3:
                    CurrentLandDemand =
                        _context.Demand
                        .OfType<LandDemand>()
                        .FirstOrDefault(di => di.DemandId == demand.DemandId);
                    break;
                default:
                    break;
            }
        }

        private async void LoadRealEstates(int? id = null)
        {
            RealEstateTypes = await _context.RealEstateType.ToListAsync();
            CurrentRealEstateType = id == null
                ? RealEstateTypes.FirstOrDefault()
                : RealEstateTypes
                                        .FirstOrDefault(re => re.Id == id);
        }

        private async Task LoadAdresses(int? id = null)
        {
            Addresses = await _context.PropertyAddress.ToListAsync();
            CurrentAddress = id == null
                ? Addresses.FirstOrDefault()
                : Addresses
                                        .FirstOrDefault(a => a.Id == id);
        }

        private async Task LoadAgents(int? id = null)
        {
            Agents = await _context.Agent.ToListAsync();
            CurrentAgent = id == null
                ? Agents.FirstOrDefault()
                : Agents
                                        .FirstOrDefault(a => a.Id == id);
        }

        private async Task LoadClients(int? id = null)
        {
            Clients = await _context.Client.ToListAsync();
            CurrentClient = id == null
               ? Clients.FirstOrDefault()
               : Clients
                                       .FirstOrDefault(c => c.Id == id);
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

        private RelayCommand _saveDemandCommand;

        public ICommand SaveDemandCommand
        {
            get
            {
                if (_saveDemandCommand == null)
                {
                    _saveDemandCommand = new RelayCommand(SaveDemand);
                }

                return _saveDemandCommand;
            }
        }

        public ApartmentDemand CurrentApartmentDemand
        {
            get => _currentApartmentDemand;
            set => SetProperty(ref _currentApartmentDemand, value);
        }
        public HouseDemand CurrentHouseDemand
        {
            get => _currentHouseDemand;
            set => SetProperty(ref _currentHouseDemand, value);
        }
        public LandDemand CurrentLandDemand
        {
            get => _currentLandDemand;
            set => SetProperty(ref _currentLandDemand, value);
        }

        private async void SaveDemand(object commandParameter)
        {
            IsMessageClosed = false;
            if (CurrentDemand.DemandId == 0)
            {
                switch (CurrentRealEstateType.Id)
                {
                    case 1:
                        ApartmentDemand apartmentDemand = new ApartmentDemand
                        {
                            Client = CurrentClient,
                            Agent = CurrentAgent,
                            PropertyAddress = CurrentAddress,
                            RealEstateType = CurrentRealEstateType,
                            MinPrice = CurrentDemand.MinPrice,
                            MaxPrice = CurrentDemand.MaxPrice,
                            MinArea = CurrentApartmentDemand.MinArea,
                            MaxArea = CurrentApartmentDemand.MaxArea,
                            MinRooms = CurrentApartmentDemand.MinRooms,
                            MaxRooms = CurrentApartmentDemand.MaxRooms,
                            MinFloor = CurrentApartmentDemand.MinFloor,
                            MaxFloor = CurrentApartmentDemand.MaxFloor
                        };
                        _ = _context.Demand.Add(apartmentDemand);
                        break;
                    case 2:
                        HouseDemand houseDemand = new HouseDemand
                        {
                            Client = CurrentClient,
                            Agent = CurrentAgent,
                            PropertyAddress = CurrentAddress,
                            RealEstateType = CurrentRealEstateType,
                            MinPrice = CurrentDemand.MinPrice,
                            MaxPrice = CurrentDemand.MaxPrice,
                            MinArea = CurrentHouseDemand.MinArea,
                            MaxArea = CurrentHouseDemand.MaxArea,
                            MinRooms = CurrentHouseDemand.MinRooms,
                            MaxRooms = CurrentHouseDemand.MaxRooms,
                            MinFloorsCount = CurrentHouseDemand.MinFloorsCount,
                            MaxFloorsCount = CurrentHouseDemand.MaxFloorsCount
                        };
                        _ = _context.Demand.Add(houseDemand);
                        break;
                    case 3:
                        LandDemand landDemand = new LandDemand
                        {
                            Client = CurrentClient,
                            Agent = CurrentAgent,
                            PropertyAddress = CurrentAddress,
                            RealEstateType = CurrentRealEstateType,
                            MinPrice = CurrentDemand.MinPrice,
                            MaxPrice = CurrentDemand.MaxPrice,
                            MinArea = CurrentLandDemand.MinArea,
                            MaxArea = CurrentLandDemand.MaxArea,
                        };
                        _ = _context.Demand.Add(landDemand);
                        break;
                    default:
                        break;
                }
            }
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
