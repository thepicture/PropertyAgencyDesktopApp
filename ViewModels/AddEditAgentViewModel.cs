using PropertyAgencyDesktopApp.Commands;
using PropertyAgencyDesktopApp.Models.Entities;
using System;
using System.Data.Common;
using System.Windows.Input;

namespace PropertyAgencyDesktopApp.ViewModels
{
    public class AddEditAgentViewModel : ViewModelBase
    {
        private Agent _currentAgent;
        private readonly PropertyAgencyBaseEntities _context =
            new PropertyAgencyBaseEntities();


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
                ValidationMessage = "Agent was successfully saved!";
            }
            catch (DbException ex)
            {
                System.Diagnostics.Debug.Write(ex.StackTrace);
                MessageType = "Danger";
                ValidationMessage = "Can't save the agent " +
                    "into the database. " +
                    "Try to go back and to the agent again," +
                    "or reload the app if it doesn't help";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex.StackTrace);
                MessageType = "Danger";
                ValidationMessage = "Can't save the agent " +
                    "into the database. " +
                    "Fatal error encountered. " +
                    "Reload the app and try again";
            }
        }
    }
}
