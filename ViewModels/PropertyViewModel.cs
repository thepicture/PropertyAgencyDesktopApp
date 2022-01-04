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
    public class PropertyViewModel : ViewModelBase
    {
        private readonly PropertyAgencyBaseEntities _context = new PropertyAgencyBaseEntities();
        private IEnumerable<Property> _properties;
        public PropertyViewModel()
        {
            Title = "Properties";
            _ = LoadCities()
            .ContinueWith(t => LoadProperties());
            CurrentPropertyType = PropertyTypes.First();
        }

        private async Task LoadCities()
        {
            Cities = await _context.City.ToListAsync();
            Cities.Insert(0, new City
            {
                CityName = "All cities"
            });
            CurrentCity = Cities.First();
        }

        private async void LoadProperties()
        {
            IEnumerable<Property> currentProperties
                = await _context.Property.ToListAsync();
            switch (CurrentPropertyType)
            {
                case "Apartment":
                    currentProperties =
                        currentProperties.Where(p =>
                        {
                            return p.Apartment.Count() > 0;
                        });
                    break;
                case "House":
                    currentProperties =
                       currentProperties.Where(p =>
                       {
                           return p.House.Count() > 0;
                       });
                    break;
                case "Land":
                    currentProperties =
                       currentProperties.Where(p =>
                       {
                           return p.Land.Count() > 0;
                       });
                    break;
                default:
                    break;
            }
            if (!string.IsNullOrEmpty(SearchText))
            {
                IWordIndefiniteSearcher distanceCalculator = DependencyService
                                         .Get<IWordIndefiniteSearcher>();
                currentProperties = currentProperties
                    .Where(p => distanceCalculator.Calculate(
                        SearchText.ToLower(),
                        p.PropertyAddress.City.CityName.ToLower()) < 3
                    || distanceCalculator.Calculate(
                        SearchText.ToLower(),
                        p.PropertyAddress.District.DistrictName.ToLower()) < 3
                    || distanceCalculator.Calculate(
                        SearchText.ToLower(),
                        p.PropertyAddress.AddressNumber) < 1
                    || distanceCalculator.Calculate(
                        SearchText.ToLower(),
                        p.PropertyAddress.AddressHouse) < 1
                    || p.PropertyAddress.District.DistrictName
                                                 .Contains(SearchText));
            }
            if (CurrentCity != null && CurrentCity.CityName != "All cities")
            {
                currentProperties = currentProperties.Where(p =>
                {
                    return p.PropertyAddress.City.CityName
                    == CurrentCity.CityName;
                });
            }
            Properties = currentProperties;
        }

        private RelayCommand addNewPropertyCommand;

        public ICommand AddNewPropertyCommand
        {
            get
            {
                if (addNewPropertyCommand == null)
                {
                    addNewPropertyCommand = new RelayCommand(AddNewProperty);
                }

                return addNewPropertyCommand;
            }
        }

        public IEnumerable<Property> Properties
        {
            get => _properties;
            set => SetProperty(ref _properties, value);
        }

        private void AddNewProperty(object commandParameter)
        {
            DependencyService.Get<INavigationService<ViewModelBase>>()
                             .Navigate<AddEditPropertyViewModel>();
        }

        private RelayCommand editPropertyCommand;

        public ICommand EditPropertyCommand
        {
            get
            {
                if (editPropertyCommand == null)
                {
                    editPropertyCommand = new RelayCommand(EditProperty);
                }

                return editPropertyCommand;
            }
        }

        private void EditProperty(object commandParameter)
        {
            DependencyService.Get<INavigationService<ViewModelBase>>()
                             .NavigateWithParameter<AddEditPropertyViewModel>
                             (commandParameter as Property);
        }

        private RelayCommand deletePropertyCommand;

        public ICommand DeletePropertyCommand
        {
            get
            {
                if (deletePropertyCommand == null)
                {
                    deletePropertyCommand = new RelayCommand(DeleteProperty);
                }

                return deletePropertyCommand;
            }
        }

        private async void DeleteProperty(object commandParameter)
        {
            Property property = commandParameter as Property;
            if (property.Offer.Count > 0)
            {
                IsMessageClosed = false;
                MessageType = "Warning";
                ValidationMessage = ShowDeleteResultService
                                    .OnRelatedObjectsError(nameof(Property),
                                                           nameof(Offer));
                return;
            }
            IQuestionService questionService =
                DependencyService.Get<IQuestionService>();
            string deleteTemplate = ShowDeleteResultService
                                    .OnDelete(nameof(Property));
            if (questionService.Ask(deleteTemplate))
            {
                IsMessageClosed = false;
                Property propertyFromDatabase = _context.Property
                                                        .Find(property.Id);
                _ = _context.Property.Remove(propertyFromDatabase);
                try
                {
                    _ = await _context.SaveChangesAsync();
                    MessageType = "Alert";
                    ValidationMessage = ShowDeleteResultService
                                        .OnSuccessfulDelete(nameof(Property));
                    LoadProperties();
                }
                catch (DbException ex)
                {
                    System.Diagnostics.Debug.Write(ex.StackTrace);
                    MessageType = "Danger";
                    ValidationMessage = ShowDeleteResultService
                                        .OnCommonDeleteError(nameof(Property));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Write(ex.StackTrace);
                    MessageType = "Danger";
                    ValidationMessage = ShowDeleteResultService
                                        .OnFatalDeleteError(nameof(Property));
                }
            }
        }

        private IEnumerable<string> _propertyTypes = new List<string>
        {
            "All property types",
            "Apartment",
            "House",
            "Land"
        };
        private string _currentPropertyType;
        private string _searchText = string.Empty;

        public IEnumerable<string> PropertyTypes
        {
            get => _propertyTypes;
            set => SetProperty(ref _propertyTypes, value);
        }
        public string CurrentPropertyType
        {
            get => _currentPropertyType;
            set
            {
                if (SetProperty(ref _currentPropertyType, value)
                    && value != "All property types")
                {
                    LoadProperties();
                }
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value)
                    && !string.IsNullOrEmpty(value))
                {
                    LoadProperties();
                }
            }
        }

        private List<City> _cities;

        public List<City> Cities
        {
            get => _cities;
            set => SetProperty(ref _cities, value);
        }
        public City CurrentCity
        {
            get => _currentCity;
            set => SetProperty(ref _currentCity, value);
        }

        private City _currentCity;
    }
}
