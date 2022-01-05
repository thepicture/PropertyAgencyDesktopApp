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
    public class AddEditOfferViewModel : ViewModelBase
    {
        private Offer _currentOffer = new Offer();
        private readonly PropertyAgencyBaseEntities _context =
            new PropertyAgencyBaseEntities();
        public AddEditOfferViewModel()
        {
            Title = "Add a new offer";
            _ = LoadClients()
            .ContinueWith((t) => LoadAgents())
            .ContinueWith((t) => LoadProperties());
        }

        public AddEditOfferViewModel(Offer offer)
        {
            IsInCreateMode = false;
            Title = "Edit the existing offer";
            CurrentOffer = _context.Offer.First(o => o.Id == offer.Id);
            _ = LoadClients()
            .ContinueWith((t) => LoadAgents())
            .ContinueWith((t) => LoadProperties());
        }

        private async Task LoadProperties()
        {
            RealEstates = await _context.Property.ToListAsync();
            if (IsInCreateMode)
            {
                CurrentRealEstate = RealEstates.First();
            }
            else
            {
                CurrentRealEstate = RealEstates
                               .First(re => CurrentOffer.PropertyId == re.Id);
            }
        }

        private async Task LoadAgents()
        {
            Agents = await _context.Agent.ToListAsync();
            CurrentAgent = IsInCreateMode
                ? Agents.First()
                : Agents
                               .First(a => CurrentOffer.AgentId == a.Id);
        }

        private async Task LoadClients()
        {
            Clients = await _context.Client.ToListAsync();
            CurrentClient = IsInCreateMode
                ? Clients.First()
                : Clients
                                .First(c => CurrentOffer.ClientId == c.Id);
        }

        public Offer CurrentOffer
        {
            get => _currentOffer;
            set => SetProperty(ref _currentOffer, value);
        }

        private bool _isInCreateMode = true;

        public bool IsInCreateMode
        {
            get => _isInCreateMode;
            set => SetProperty(ref _isInCreateMode, value);
        }

        private IEnumerable<Client> _clients;

        public IEnumerable<Client> Clients
        {
            get => _clients;
            set => SetProperty(ref _clients, value);
        }

        private IEnumerable<Property> _realEstates;

        public IEnumerable<Property> RealEstates
        {
            get => _realEstates;
            set => SetProperty(ref _realEstates, value);
        }

        private Property _currentRealEstate;

        public Property CurrentRealEstate
        {
            get => _currentRealEstate;
            set => SetProperty(ref _currentRealEstate, value);
        }

        private IEnumerable<Agent> _agents;

        public IEnumerable<Agent> Agents
        {
            get => _agents;
            set => SetProperty(ref _agents, value);
        }

        private Agent _currentAgent;

        public Agent CurrentAgent
        {
            get => _currentAgent;
            set => SetProperty(ref _currentAgent, value);
        }

        private RelayCommand _saveOfferCommand;

        public ICommand SaveOfferCommand
        {
            get
            {
                if (_saveOfferCommand == null)
                {
                    _saveOfferCommand = new RelayCommand(SaveOffer);
                }

                return _saveOfferCommand;
            }
        }

        private async void SaveOffer(object commandParameter)
        {
            IsMessageClosed = false;
            if (CurrentOffer.Id == 0)
            {
                _ = _context.Offer.Add(CurrentOffer);
            }
            try
            {
                CurrentOffer.Client = CurrentClient;
                CurrentOffer.Agent = CurrentAgent;
                CurrentOffer.Property = CurrentRealEstate;
                _ = await _context.SaveChangesAsync();
                MessageType = "Alert";
                ValidationMessage = ValidationMessage = ShowSaveResultService
                                    .GetOnSavedTemplate(nameof(Offer));
            }
            catch (DbException ex)
            {
                System.Diagnostics.Debug.Write(ex.StackTrace);
                MessageType = "Danger";
                ValidationMessage = ShowSaveResultService
                                    .GetOnCommonErrorTemplate(nameof(Offer));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex.StackTrace);
                MessageType = "Danger";
                ValidationMessage = ShowSaveResultService
                                    .GetOnFatalErrorTemplate(nameof(Offer));
            }
        }

        private Client _currentClient;

        public Client CurrentClient
        {
            get => _currentClient;
            set => SetProperty(ref _currentClient, value);
        }

        private System.Collections.IEnumerable realEstateTypes;

        public System.Collections.IEnumerable RealEstateTypes { get => realEstateTypes; set => SetProperty(ref realEstateTypes, value); }

        private object currentRealEstateType;

        public object CurrentRealEstateType { get => currentRealEstateType; set => SetProperty(ref currentRealEstateType, value); }

        private System.Collections.IEnumerable adresses;

        public System.Collections.IEnumerable Adresses { get => adresses; set => SetProperty(ref adresses, value); }

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

        private void SaveDemand(object commandParameter)
        {
        }
    }
}
