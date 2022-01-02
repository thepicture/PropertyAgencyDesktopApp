using PropertyAgencyDesktopApp.Commands;
using PropertyAgencyDesktopApp.Models.Entities;
using System.Collections.Generic;
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

        private void SaveProperty(object commandParameter)
        {
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
    }
}
