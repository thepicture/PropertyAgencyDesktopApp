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
    public class OfferViewModel : ViewModelBase
    {
        private PropertyAgencyBaseEntities _context = new PropertyAgencyBaseEntities();
        private IEnumerable<Offer> _offers;
        public OfferViewModel()
        {
            Title = "Offers";
            LoadOffers();
        }

        private async void LoadOffers()
        {
            Offers = await _context.Offer.ToListAsync();
        }

        public IEnumerable<Offer> Offers
        {
            get => _offers;
            set => SetProperty(ref _offers, value);
        }

        private RelayCommand addNewOfferCommand;

        public ICommand AddNewOfferCommand
        {
            get
            {
                if (addNewOfferCommand == null)
                {
                    addNewOfferCommand = new RelayCommand(AddNewOffer);
                }

                return addNewOfferCommand;
            }
        }

        private void AddNewOffer(object commandParameter)
        {
            DependencyService.Get<INavigationService<ViewModelBase>>()
                             .Navigate<AddEditOfferViewModel>();
        }

        private RelayCommand editOfferCommand;

        public ICommand EditOfferCommand
        {
            get
            {
                if (editOfferCommand == null)
                {
                    editOfferCommand = new RelayCommand(EditOffer);
                }

                return editOfferCommand;
            }
        }

        private void EditOffer(object commandParameter)
        {
            Offer offer = commandParameter as Offer;
            DependencyService.Get<INavigationService<ViewModelBase>>()
                             .NavigateWithParameter<AddEditOfferViewModel>
                             (offer);
        }

        private RelayCommand deleteOfferCommand;

        public ICommand DeleteOfferCommand
        {
            get
            {
                if (deleteOfferCommand == null)
                {
                    deleteOfferCommand = new RelayCommand(DeleteOffer);
                }

                return deleteOfferCommand;
            }
        }

        private async void DeleteOffer(object commandParameter)
        {
            Offer offer = commandParameter as Offer;
            if (offer.Deal.Count > 0)
            {
                IsMessageClosed = false;
                MessageType = "Warning";
                ValidationMessage = "Can't delete an offer with the" +
                    "deals related to it";
                return;
            }
            IQuestionService questionService =
                DependencyService.Get<IQuestionService>();
            if (questionService.Ask("Do you really want " +
                "to delete the offer?"))
            {
                IsMessageClosed = false;
                Offer offerFromDatabase = _context.Offer
                                                        .Find(offer.Id);
                _ = _context.Offer.Remove(offerFromDatabase);
                try
                {
                    _ = await _context.SaveChangesAsync();
                    MessageType = "Alert";
                    ValidationMessage = "The offer was successfully deleted!";
                    LoadOffers();
                }
                catch (DbException ex)
                {
                    System.Diagnostics.Debug.Write(ex.StackTrace);
                    MessageType = "Danger";
                    ValidationMessage = "Can't delete the offer " +
                        "from the database. " +
                        "Try to go back and to the offer again," +
                        "or reload the app if it doesn't help";
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Write(ex.StackTrace);
                    MessageType = "Danger";
                    ValidationMessage = "Can't delete the offer " +
                        "from the database. " +
                        "Fatal error encountered. " +
                        "Reload the app and try again";
                }
            }
        }
    }
}
