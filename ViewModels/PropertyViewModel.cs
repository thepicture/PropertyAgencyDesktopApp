using PropertyAgencyDesktopApp.Commands;
using PropertyAgencyDesktopApp.Models.Entities;
using PropertyAgencyDesktopApp.Services;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Windows.Input;

namespace PropertyAgencyDesktopApp.ViewModels
{
    public class PropertyViewModel : ViewModelBase
    {
        private PropertyAgencyBaseEntities _context = new PropertyAgencyBaseEntities();
        private IEnumerable<Property> _properties;
        public PropertyViewModel()
        {
            Title = "Properties";
            CurrentPropertyType = PropertyTypes.First();
        }

        private async void LoadProperties()
        {
            IEnumerable<Property> currentProperties
                = await _context.Property.ToListAsync();
            switch (CurrentPropertyType)
            {
                case "Apartment":
                    currentProperties =
                        currentProperties.Where(p => p.Apartment.Count() > 0);
                    break;
                case "House":
                    currentProperties =
                       currentProperties.Where(p => p.House.Count() > 0);
                    break;
                case "Land":
                    currentProperties =
                       currentProperties.Where(p => p.Land.Count() > 0);
                    break;
                default:
                    break;
            }
            if (!string.IsNullOrEmpty(SearchText))
            {
                currentProperties = currentProperties
                    .Where(p => p.PropertyAddress.City.CityName.Contains(SearchText)
                    || p.PropertyAddress.District.DistrictName.Contains(SearchText));
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
                ValidationMessage = "Can't delete a property with " +
                    "offers related to the property";
                return;
            }
            IQuestionService questionService =
                DependencyService.Get<IQuestionService>();
            if (questionService.Ask("Do you really want " +
                "to delete property?"))
            {
                IsMessageClosed = false;
                Property propertyFromDatabase = _context.Property.Find(property.Id);
                _ = _context.Property.Remove(propertyFromDatabase);
                try
                {
                    _ = await _context.SaveChangesAsync();
                    MessageType = "Alert";
                    ValidationMessage = "Property was successfully deleted!";
                    LoadProperties();
                }
                catch (DbException ex)
                {
                    System.Diagnostics.Debug.Write(ex.StackTrace);
                    MessageType = "Danger";
                    ValidationMessage = "Can't delete the property " +
                        "from the database. " +
                        "Try to go back and to the property again," +
                        "or reload the app if it doesn't help";
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Write(ex.StackTrace);
                    MessageType = "Danger";
                    ValidationMessage = "Can't delete the property " +
                        "from the database. " +
                        "Fatal error encountered. " +
                        "Reload the app and try again";
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
                SetProperty(ref _currentPropertyType, value);
                LoadProperties();
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                LoadProperties();
            }
        }
    }
}
