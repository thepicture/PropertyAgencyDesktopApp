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
        private IList<Offer> _supplies;
        private IList<Demand> _demands;
        private readonly PropertyAgencyBaseEntities _context =
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

        public AddEditDealViewModel(Deal deal)
        {
            Title = "Edit the existing deal";
            _ = LoadDeal(deal.DealId)
                .ContinueWith(t => LoadDemands(deal.DemandId))
                .ContinueWith(t => LoadSupplies(deal.OfferId));
        }

        private async Task LoadDeal(int dealId)
        {
            CurrentDeal = await _context.Deal
                                        .FirstAsync(d => d.DealId == dealId);
        }

        private async Task LoadSupplies(int? supplyId = null)
        {
            if (supplyId == null)
            {
                Supplies = await _context.Offer.Where(o => o.Deal.Count == 0)
                                               .ToListAsync();
                CurrentSupply = Supplies.First();
            }
            else
            {
                Supplies = await _context.Offer.ToListAsync();
                Supplies = Supplies.Where(d => d.Deal.Count == 0 || d.Deal.Contains(CurrentDeal))
                                   .ToList();
                CurrentSupply = Supplies.First(s => s.Id == supplyId);
            }
        }

        private async Task LoadDemands(int? demandId = null)
        {
            if (demandId == null)
            {
                Demands = await _context.Demand.Where(d => d.Deal.Count == 0)
                                               .ToListAsync();
                CurrentDemand = Demands.First();
            }
            else
            {
                Demands = await _context.Demand.ToListAsync();
                Demands = Demands.Where(d => d.Deal.Count == 0 || d.Deal.Contains(CurrentDeal))
                                 .ToList();
                CurrentDemand = Demands.First(d => d.DemandId == demandId);
            }
        }

        public IList<Offer> Supplies
        {
            get => _supplies;
            set => SetProperty(ref _supplies, value);
        }
        public IList<Demand> Demands
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
            CurrentDeal.Offer = CurrentSupply;
            CurrentDeal.Demand = CurrentDemand;
            if (CurrentDeal.DealId == 0)
            {
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
