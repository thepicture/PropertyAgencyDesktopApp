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
    public class DemandViewModel : ViewModelBase
    {
        private readonly PropertyAgencyBaseEntities _context =
            new PropertyAgencyBaseEntities();
        public DemandViewModel()
        {
            Title = "Demands";
            _ = LoadOffers();
        }

        private async Task LoadOffers()
        {
            Offers = await _context.Offer.ToListAsync();
            Offers.Insert(0, new Offer());
            CurrentOffer = Offers.First();
        }

        private async void LoadDemands()
        {
            List<Demand> currentDemands =
                await _context.Demand.ToListAsync();
            currentDemands = currentDemands.Where(d =>
            {
                if (CurrentOffer.Id != 0)
                {
                    if (CurrentOffer.Property.Apartment.Count > 0)
                    {
                        if (d.RealEstateType.TypeName != "квартира")
                        {
                            return false;
                        }
                        Apartment apartment =
                        CurrentOffer.Property.Apartment.First();
                        ApartmentDemand apartmentDemand = d as ApartmentDemand;
                        if (apartmentDemand == null)
                        {
                            return false;
                        }
                        if ((apartmentDemand.MaxArea == null
                            || apartment.TotalArea <= apartmentDemand.MaxArea)
                        && (apartmentDemand.MinArea == null
                            || apartment.TotalArea >= apartmentDemand.MinArea)
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
                    else if (CurrentOffer.Property.House.Count > 0)
                    {
                        if (d.RealEstateType.TypeName != "дом")
                        {
                            return false;
                        }
                        House house = CurrentOffer.Property.House.First();
                        HouseDemand houseDemand = d as HouseDemand;
                        if ((houseDemand.MaxArea == null
                            || house.TotalArea <= houseDemand.MaxArea)
                        && (houseDemand.MinArea == null
                            || house.TotalArea >= houseDemand.MinArea)
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
                    else if (CurrentOffer.Property.Land.Count > 0)
                    {
                        if (d.RealEstateType.TypeName != "земля")
                        {
                            return false;
                        }
                        Land land = CurrentOffer.Property.Land.First();
                        return (d.MaxArea == null
                        || land.TotalArea <= d.MaxArea)
                        && (d.MinArea == null
                        || land.TotalArea >= d.MinArea);
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
            Demands = currentDemands;
        }

        private RelayCommand addNewDemandCommand;
        private List<Demand> _demands;

        public ICommand AddNewDemandCommand
        {
            get
            {
                if (addNewDemandCommand == null)
                {
                    addNewDemandCommand = new RelayCommand(AddNewDemand);
                }

                return addNewDemandCommand;
            }
        }

        public List<Demand> Demands
        {
            get => _demands;
            set => SetProperty(ref _demands, value);
        }

        private void AddNewDemand(object commandParameter)
        {
            DependencyService.Get<INavigationService<ViewModelBase>>()
                             .Navigate<AddEditDemandViewModel>();
        }

        private RelayCommand editDemandCommand;

        public ICommand EditDemandCommand
        {
            get
            {
                if (editDemandCommand == null)
                {
                    editDemandCommand = new RelayCommand(EditDemand);
                }

                return editDemandCommand;
            }
        }

        private void EditDemand(object commandParameter)
        {
            Demand demandToEdit = commandParameter as Demand;
            DependencyService.Get<INavigationService<ViewModelBase>>()
                             .NavigateWithParameter<AddEditDemandViewModel>
                             (demandToEdit);
        }

        private RelayCommand deleteDemandCommand;

        public ICommand DeleteDemandCommand
        {
            get
            {
                if (deleteDemandCommand == null)
                {
                    deleteDemandCommand = new RelayCommand(DeleteDemandAsync);
                }

                return deleteDemandCommand;
            }
        }

        private async void DeleteDemandAsync(object commandParameter)
        {
            Demand demand = commandParameter as Demand;
            if (demand.Deal.Count > 0)
            {
                IsMessageClosed = false;
                MessageType = "Warning";
                ValidationMessage = ShowDeleteResultService
                                    .OnRelatedObjectsError(nameof(Demand),
                                                           nameof(Deal));
                return;
            }
            IQuestionService questionService =
                DependencyService.Get<IQuestionService>();
            string deleteTemplate = ShowDeleteResultService
                                    .OnDelete(nameof(Demand));
            if (questionService.Ask(deleteTemplate))
            {
                IsMessageClosed = false;
                Demand demandFromDatabase = _context.Demand
                                                        .Find(demand.DemandId);
                _ = _context.Demand.Remove(demandFromDatabase);
                try
                {
                    _ = await _context.SaveChangesAsync();
                    MessageType = "Alert";
                    ValidationMessage = ShowDeleteResultService
                                        .OnSuccessfulDelete(nameof(Demand));
                    LoadDemands();
                }
                catch (DbException ex)
                {
                    System.Diagnostics.Debug.Write(ex.StackTrace);
                    MessageType = "Danger";
                    ValidationMessage = ShowDeleteResultService
                                        .OnCommonDeleteError(nameof(Demand));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Write(ex.StackTrace);
                    MessageType = "Danger";
                    ValidationMessage = ShowDeleteResultService
                                        .OnFatalDeleteError(nameof(Demand));
                }
            }
        }

        private IList<Offer> _offers;

        public IList<Offer> Offers
        {
            get => _offers;
            set => SetProperty(ref _offers, value);
        }
        public Offer CurrentOffer
        {
            get => _currentOffer;
            set
            {
                if (SetProperty(ref _currentOffer, value))
                {
                    LoadDemands();
                }
            }
        }

        private Offer _currentOffer;

        private RelayCommand dealDemandWithSelectedOfferCommand;

        public ICommand DealDemandWithSelectedOfferCommand
        {
            get
            {
                if (dealDemandWithSelectedOfferCommand == null)
                {
                    dealDemandWithSelectedOfferCommand =
                        new RelayCommand(DealDemandWithSelectedOffer);
                }

                return dealDemandWithSelectedOfferCommand;
            }
        }

        private void DealDemandWithSelectedOffer(object commandParameter)
        {
            Demand selectedDemand = commandParameter as Demand;
            DependencyService.Get<INavigationService<ViewModelBase>>()
                             .NavigateWithParameter<AddEditDealViewModel>
                             (Tuple.Create(
                                 CurrentOffer,
                                 selectedDemand
                             ));
        }
    }
}
