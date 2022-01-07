using PropertyAgencyDesktopApp.Commands;
using PropertyAgencyDesktopApp.Models.Entities;
using PropertyAgencyDesktopApp.Services;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PropertyAgencyDesktopApp.ViewModels
{
    public class OfferViewModel : ViewModelBase
    {
        private readonly PropertyAgencyBaseEntities _context =
            new PropertyAgencyBaseEntities();
        private IEnumerable<Offer> _offers;
        public OfferViewModel()
        {
            Title = "Offers";
            _ = LoadDemands();
        }

        private async Task LoadDemands()
        {
            Demands = await _context.Demand.ToListAsync();
            Demands.Insert(0, new Demand());
            CurrentDemand = Demands.First();
        }

        private async void LoadOffers()
        {
            List<Offer> currentOffers = await _context.Offer.ToListAsync();
            currentOffers = currentOffers.Where(o =>
            {
                if (CurrentDemand.DemandId != 0)
                {
                    if (CurrentDemand.RealEstateType.TypeName == "квартира")
                    {
                        if (o.Property.Apartment.Count == 0)
                        {
                            return false;
                        }
                        Apartment apartment = o.Property.Apartment.First();
                        ApartmentDemand apartmentDemand =
                        CurrentDemand as ApartmentDemand;
                        if ((apartmentDemand.MaxArea == null
                            || apartment.TotalArea <= CurrentDemand.MaxArea)
                        && (apartmentDemand.MinArea == null
                            || apartment.TotalArea >= CurrentDemand.MinArea)
                        && (apartmentDemand.MaxRooms == null
                            || apartment.RoomsCount <= apartmentDemand.MaxRooms)
                        && (apartmentDemand.MinRooms == null
                            || apartment.RoomsCount <= apartmentDemand.MinRooms)
                        && (apartmentDemand.MaxFloor == null
                            || apartment.FloorNumber <= apartmentDemand.MaxFloor)
                        && (apartmentDemand.MinFloor == null
                            || apartment.FloorNumber >= apartmentDemand.MinFloor))
                        {
                            return true;
                        }
                        return false;
                    }
                    else if (CurrentDemand.RealEstateType.TypeName == "дом")
                    {
                        if (o.Property.House.Count == 0)
                        {
                            return false;
                        }
                        House house = o.Property.House.First();
                        HouseDemand houseDemand = CurrentDemand as HouseDemand;
                        if ((houseDemand.MaxArea == null
                            || house.TotalArea <= CurrentDemand.MaxArea)
                        && (houseDemand.MinArea == null
                            || house.TotalArea >= CurrentDemand.MinArea)
                        && (houseDemand.MaxRooms == null
                            || house.RoomsCount <= houseDemand.MaxRooms)
                        && (houseDemand.MinRooms == null
                            || house.RoomsCount <= houseDemand.MinRooms)
                        && (houseDemand.MinFloorsCount == null
                            || house.TotalFloors <= houseDemand.MinFloorsCount)
                        && (houseDemand.MaxFloorsCount == null
                            || house.TotalFloors >= houseDemand.MaxFloorsCount))
                        {
                            return true;
                        }
                        return false;
                    }
                    else if (CurrentDemand.RealEstateType.TypeName == "земля")
                    {
                        if (o.Property.Land.Count == 0)
                        {
                            return false;
                        }
                        Land land = o.Property.Land.First();
                        return (CurrentDemand.MaxArea == null
                        || land.TotalArea <= CurrentDemand.MaxArea)
                        && (CurrentDemand.MinArea == null
                        || land.TotalArea >= CurrentDemand.MinArea);
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }).ToList();
            Offers = currentOffers;
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
                ValidationMessage = ShowDeleteResultService
                                    .OnRelatedObjectsError(nameof(Offer),
                                                           nameof(Deal));
                return;
            }
            IQuestionService questionService =
                DependencyService.Get<IQuestionService>();
            string deleteTemplate = ShowDeleteResultService
                                    .OnDelete(nameof(Offer));
            if (questionService.Ask(deleteTemplate))
            {
                IsMessageClosed = false;
                Offer offerFromDatabase = _context.Offer
                                                        .Find(offer.Id);
                _ = _context.Offer.Remove(offerFromDatabase);
                try
                {
                    _ = await _context.SaveChangesAsync();
                    MessageType = "Alert";
                    ValidationMessage = ShowDeleteResultService
                                        .OnSuccessfulDelete(nameof(Offer));
                    LoadOffers();
                }
                catch (DbException ex)
                {
                    System.Diagnostics.Debug.Write(ex.StackTrace);
                    MessageType = "Danger";
                    ValidationMessage = ShowDeleteResultService
                                        .OnCommonDeleteError(nameof(Offer));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Write(ex.StackTrace);
                    MessageType = "Danger";
                    ValidationMessage = ShowDeleteResultService
                                        .OnFatalDeleteError(nameof(Offer));
                }
            }
        }

        private IList<Demand> _demands;

        public IList<Demand> Demands
        {
            get => _demands;
            set => SetProperty(ref _demands, value);
        }

        private Demand _currentDemand;

        public Demand CurrentDemand
        {
            get => _currentDemand;
            set
            {
                if (SetProperty(ref _currentDemand, value))
                {
                    LoadOffers();
                }
            }
        }
    }
}
