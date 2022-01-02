using PropertyAgencyDesktopApp.Commands;
using PropertyAgencyDesktopApp.Models.Entities;
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
        }

        public Property CurrentProperty
        {
            get => _currentProperty;
            set => SetProperty(ref _currentProperty, value);
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
    }
}
