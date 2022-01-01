using PropertyAgencyDesktopApp.Commands;
using PropertyAgencyDesktopApp.Models.Entities;
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

        private void SaveAgent(object commandParameter)
        {
        }
    }
}
