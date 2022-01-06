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
    public class AddEditDealViewModel : ViewModelBase
    {
        private IEnumerable<Offer> _supplies;
        private IEnumerable<Demand> _demands;
        private PropertyAgencyBaseEntities _context =
            new PropertyAgencyBaseEntities();
        private Offer _currentSupply;
        private Demand _currentDemand;
        private Deal _currentDeal = new Deal();
        public AddEditDealViewModel()
        {
            Title = "Add a new deal";
            _ = LoadDemands()
            .ContinueWith(t => LoadSupplies());
        }

        private async void LoadSupplies(int? supplyId = null)
        {
            Supplies = await _context.Offer.Where(s => s.Deal.Count == 0)
                                           .ToListAsync();
            CurrentSupply = supplyId != null
                ? Supplies.First(s => s.Id == supplyId)
                : Supplies.First();
        }

        private async Task LoadDemands(int? demandId = null)
        {
            Demands = await _context.Demand.Where(d => d.Deal.Count == 0)
                                           .ToListAsync();
            CurrentDemand = demandId != null
                ? Demands.First(d => d.DemandId == demandId)
                : Demands.First();
        }

        public IEnumerable<Offer> Supplies
        {
            get => _supplies;
            set => SetProperty(ref _supplies, value);
        }
        public IEnumerable<Demand> Demands
        {
            get => _demands;
            set => SetProperty(ref _demands, value);
        }
        public Offer CurrentSupply
        {
            get => _currentSupply;
            set => SetProperty(ref _currentSupply, value);
        }
        public Demand CurrentDemand
        {
            get => _currentDemand;
            set => SetProperty(ref _currentDemand, value);
        }
        public Deal CurrentDeal
        {
            get => _currentDeal;
            set => SetProperty(ref _currentDeal, value);
        }

        private RelayCommand saveDealCommand;

        public ICommand SaveDealCommand
        {
            get
            {
                if (saveDealCommand == null)
                {
                    saveDealCommand = new RelayCommand(SaveDealAsync);
                }

                return saveDealCommand;
            }
        }

        private async void SaveDealAsync(object commandParameter)
        {
            IsMessageClosed = false;
            if (CurrentDeal.DealId == 0)
            {
                CurrentDeal.Offer = CurrentSupply;
                CurrentDeal.Demand = CurrentDemand;
                _context.Deal.Add(CurrentDeal);
            }
            try
            {
                _ = await _context.SaveChangesAsync();
                MessageType = "Alert";
                ValidationMessage = ValidationMessage = ShowSaveResultService
                                    .GetOnSavedTemplate(nameof(Deal));
            }
            catch (DbException ex)
            {
                System.Diagnostics.Debug.Write(ex.StackTrace);
                MessageType = "Danger";
                ValidationMessage = ShowSaveResultService
                                    .GetOnCommonErrorTemplate(nameof(Deal));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex.StackTrace);
                MessageType = "Danger";
                ValidationMessage = ShowSaveResultService
                                    .GetOnFatalErrorTemplate(nameof(Deal));
            }
        }
    }
}
