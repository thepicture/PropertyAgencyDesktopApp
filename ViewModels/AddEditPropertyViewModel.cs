using PropertyAgencyDesktopApp.Commands;
using PropertyAgencyDesktopApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Windows.Input;

namespace PropertyAgencyDesktopApp.ViewModels
{
    public class AddEditPropertyViewModel : ViewModelBase
    {
        private readonly PropertyAgencyBaseEntities _context = new PropertyAgencyBaseEntities();
        private Property _currentProperty;
        private PropertyAddress _currentAddress = new PropertyAddress();
        private District _currentDistrict = new District();
        private City _currentCity = new City();

        private Apartment _apartment = new Apartment();
        private House _house = new House();
        private Land _land = new Land();
        public AddEditPropertyViewModel()
        {
            Title = "Add a new property";
            CurrentProperty = new Property();
            CurrentPropertyType = PropertyTypes.First();
        }

        public Property CurrentProperty
        {
            get => _currentProperty;
            set => SetProperty(ref _currentProperty, value);
        }
        public string CurrentPropertyType
        {
            get => _currentPropertyType;
            set => SetProperty(ref _currentPropertyType, value);
        }

        private RelayCommand savePropertyCommand;

        public ICommand SavePropertyCommand
        {
            get
            {
                if (savePropertyCommand == null)
                {
                    savePropertyCommand = new RelayCommand(SaveProperty);
                }

                return savePropertyCommand;
            }
        }

        public PropertyAddress CurrentAddress
        {
            get => _currentAddress;
            set => SetProperty(ref _currentAddress, value);
        }
        public District CurrentDistrict
        {
            get => _currentDistrict;
            set => SetProperty(ref _currentDistrict, value);
        }
        public City CurrentCity
        {
            get => _currentCity;
            set => SetProperty(ref _currentCity, value);
        }

        private async void SaveProperty(object commandParameter)
        {
            IsMessageClosed = false;
            if (CurrentProperty.Id == 0)
            {
                _ = _context.Property.Add(CurrentProperty);
            }
            switch (CurrentPropertyType)
            {
                case "Apartment":
                    CurrentProperty.Apartment.Add(Apartment);
                    break;
                case "House":
                    CurrentProperty.House.Add(House);
                    break;
                case "Land":
                    CurrentProperty.Land.Add(Land);
                    break;
                default:
                    break;
            }

            City existingCity = _context.City
                                .FirstOrDefault(c => c.CityName.ToLower()
                                .Contains(CurrentCity.CityName));
            if (existingCity != null)
            {
                CurrentAddress.City = existingCity;
            }
            else
            {
                CurrentAddress.City = CurrentCity;
            }
            District existingDistrict = _context.District
                               .FirstOrDefault(d => d.DistrictName.ToLower()
                               .Contains(CurrentDistrict.DistrictName));
            if (existingCity != null)
            {
                CurrentAddress.District = existingDistrict;
            }
            else
            {
                CurrentAddress.District = CurrentDistrict;
            }
            try
            {
                _ = await _context.SaveChangesAsync();
                MessageType = "Alert";
                ValidationMessage = "Property was successfully saved!";
            }
            catch (DbException ex)
            {
                System.Diagnostics.Debug.Write(ex.StackTrace);
                MessageType = "Danger";
                ValidationMessage = "Can't save the property " +
                    "into the database. " +
                    "Try to go back and to the agent again," +
                    "or reload the app if it doesn't help";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex.StackTrace);
                MessageType = "Danger";
                ValidationMessage = "Can't save the property " +
                    "into the database. " +
                    "Fatal error encountered. " +
                    "Reload the app and try again";
            }
        }

        private IEnumerable<string> _propertyTypes = new List<string>
        {
            "Apartment",
            "House",
            "Land"
        };
        private string _currentPropertyType;

        public IEnumerable<string> PropertyTypes
        {
            get => _propertyTypes;
            set => SetProperty(ref _propertyTypes, value);
        }
        public Apartment Apartment
        {
            get => _apartment;
            set => SetProperty(ref _apartment, value);
        }
        public House House
        {
            get => _house;
            set => SetProperty(ref _house, value);
        }
        public Land Land
        {
            get => _land;
            set => SetProperty(ref _land, value);
        }
    }
}
