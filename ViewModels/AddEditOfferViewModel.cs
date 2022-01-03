using PropertyAgencyDesktopApp.Commands;
using PropertyAgencyDesktopApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
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
            LoadClients();
            LoadAgents();
            LoadProperties();
        }

        private async void LoadProperties()
        {
            RealEstates = await _context.Property.ToListAsync();
            CurrentRealEstate = RealEstates.First();
        }

        private async void LoadAgents()
        {
            Agents = await _context.Agent.ToListAsync();
            CurrentAgent = Agents.First();
        }

        private async void LoadClients()
        {
            Clients = await _context.Client.ToListAsync();
            CurrentClient = Clients.First();
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
                ValidationMessage = "Offer was successfully saved!";
            }
            catch (DbException ex)
            {
                System.Diagnostics.Debug.Write(ex.StackTrace);
                MessageType = "Danger";
                ValidationMessage = "Can't save the offer " +
                    "into the database. " +
                    "Try to go back and to the offer again," +
                    "or reload the app if it doesn't help";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex.StackTrace);
                MessageType = "Danger";
                ValidationMessage = "Can't save the offer " +
                    "into the database. " +
                    "Fatal error encountered. " +
                    "Reload the app and try again";
            }
        }

        private Client _currentClient;

        public Client CurrentClient
        {
            get => _currentClient;
            set => SetProperty(ref _currentClient, value);
        }
    }
}
