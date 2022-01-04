using PropertyAgencyDesktopApp.Commands;
using PropertyAgencyDesktopApp.Models.Entities;
using System;
using System.Data.Common;
using System.Data.Entity;
using System.Windows.Input;

namespace PropertyAgencyDesktopApp.ViewModels
{
    public class AddEditAgentViewModel : ViewModelBase
    {
        private Agent _currentAgent;
        private readonly PropertyAgencyBaseEntities _context =
            new PropertyAgencyBaseEntities();
        private bool _isInEditMode;


        public Agent CurrentAgent
        {
            get => _currentAgent;
            set => SetProperty(ref _currentAgent, value);
        }
        public AddEditAgentViewModel()
        {
            Title = "Add a new agent";
            CurrentAgent = new Agent();
        }

        public AddEditAgentViewModel(Agent agent)
        {
            Title = "Edit an agent";
            IsInEditMode = true;
            LoadAgent(agent);
        }

        private async void LoadAgent(Agent agent)
        {
            CurrentAgent = await _context.Agent.FirstAsync(a => a.Id == agent.Id);
        }

        private RelayCommand saveAgentCommand;

        public ICommand SaveAgentCommand
        {
            get
            {
                if (saveAgentCommand == null)
                {
                    saveAgentCommand = new RelayCommand(SaveAgent);
                }

                return saveAgentCommand;
            }
        }

        public bool IsInEditMode
        {
            get => _isInEditMode;
            set => SetProperty(ref _isInEditMode, value);
        }

        private async void SaveAgent(object commandParameter)
        {
            IsMessageClosed = false;
            if (CurrentAgent.Id == 0)
            {
                _ = _context.Agent.Add(CurrentAgent);
            }
            try
            {
                _ = await _context.SaveChangesAsync();
                MessageType = "Alert";
                ValidationMessage = ShowSaveResultService
                                    .GetOnSavedTemplate(nameof(Agent));
            }
            catch (DbException ex)
            {
                System.Diagnostics.Debug.Write(ex.StackTrace);
                MessageType = "Danger";
                ValidationMessage = ShowSaveResultService
                                    .GetOnCommonErrorTemplate(nameof(Agent));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex.StackTrace);
                MessageType = "Danger";
                ValidationMessage = ShowSaveResultService
                                    .GetOnFatalErrorTemplate(nameof(Agent));
            }
        }
    }
}
