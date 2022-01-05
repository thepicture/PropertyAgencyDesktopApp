using PropertyAgencyDesktopApp.Commands;
using PropertyAgencyDesktopApp.Models.Entities;
using PropertyAgencyDesktopApp.Services;
using System.Collections.Generic;
using System.Data.Entity;
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
            LoadDemands();
        }

        private async void LoadDemands()
        {
            Demands = await _context.Demand.ToListAsync();
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
    }
}
