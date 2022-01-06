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
    public class DealViewModel : ViewModelBase
    {
        private IEnumerable<Deal> _deals;
        private readonly PropertyAgencyBaseEntities _context =
            new PropertyAgencyBaseEntities();
        public DealViewModel()
        {
            Title = "Deals";
            LoadDeals();
        }

        private async void LoadDeals()
        {
            Deals = await _context.Deal.ToListAsync();
        }

        public IEnumerable<Deal> Deals
        {
            get => _deals;
            set => SetProperty(ref _deals, value);
        }

        private RelayCommand addNewDealCommand;

        public ICommand AddNewDealCommand
        {
            get
            {
                if (addNewDealCommand == null)
                {
                    addNewDealCommand = new RelayCommand(AddNewDeal);
                }

                return addNewDealCommand;
            }
        }

        private void AddNewDeal(object commandParameter)
        {
            DependencyService.Get<INavigationService<ViewModelBase>>()
                             .Navigate<AddEditDealViewModel>();
        }

        private RelayCommand editDealCommand;

        public ICommand EditDealCommand
        {
            get
            {
                if (editDealCommand == null)
                {
                    editDealCommand = new RelayCommand(EditDeal);
                }

                return editDealCommand;
            }
        }

        private void EditDeal(object commandParameter)
        {
            Deal dealToEdit = commandParameter as Deal;
            DependencyService.Get<INavigationService<ViewModelBase>>()
                             .NavigateWithParameter<AddEditDealViewModel>
                             (dealToEdit);
        }

        private RelayCommand deleteDealCommand;

        public ICommand DeleteDealCommand
        {
            get
            {
                if (deleteDealCommand == null)
                {
                    deleteDealCommand = new RelayCommand(DeleteDeal);
                }

                return deleteDealCommand;
            }
        }

        private async void DeleteDeal(object commandParameter)
        {
            Deal deal = commandParameter as Deal;
            IQuestionService questionService =
                DependencyService.Get<IQuestionService>();
            string deleteTemplate = ShowDeleteResultService
                                    .OnDelete(nameof(Deal));
            if (questionService.Ask(deleteTemplate))
            {
                IsMessageClosed = false;
                Deal dealFromDatabase = _context.Deal
                                                        .Find(deal.DealId);
                _ = _context.Deal.Remove(dealFromDatabase);
                try
                {
                    _ = await _context.SaveChangesAsync();
                    MessageType = "Alert";
                    ValidationMessage = ShowDeleteResultService
                                        .OnSuccessfulDelete(nameof(Deal));
                    LoadDeals();
                }
                catch (DbException ex)
                {
                    System.Diagnostics.Debug.Write(ex.StackTrace);
                    MessageType = "Danger";
                    ValidationMessage = ShowDeleteResultService
                                        .OnCommonDeleteError(nameof(Deal));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Write(ex.StackTrace);
                    MessageType = "Danger";
                    ValidationMessage = ShowDeleteResultService
                                        .OnFatalDeleteError(nameof(Deal));
                }
            }
        }
    }
}
