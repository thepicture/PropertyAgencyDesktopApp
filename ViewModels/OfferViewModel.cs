using PropertyAgencyDesktopApp.Commands;
using PropertyAgencyDesktopApp.Models.Entities;
using System.Collections.Generic;
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
        }
    }
}
