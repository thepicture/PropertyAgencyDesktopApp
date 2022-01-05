using PropertyAgencyDesktopApp.Commands;
using PropertyAgencyDesktopApp.Models.Entities;
using PropertyAgencyDesktopApp.Services;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
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
    }
}
