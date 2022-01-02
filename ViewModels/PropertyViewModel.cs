using PropertyAgencyDesktopApp.Commands;
using PropertyAgencyDesktopApp.Models.Entities;
using PropertyAgencyDesktopApp.Services;
using System.Collections.Generic;
using System.Data.Entity;
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
            LoadProperties();
        }

        private async void LoadProperties()
        {
            Properties = await _context.Property.ToListAsync();
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

        private void DeleteProperty(object commandParameter)
        {
        }
    }
}
