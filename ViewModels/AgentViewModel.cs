using PropertyAgencyDesktopApp.Commands;
using PropertyAgencyDesktopApp.Models.Entities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Windows.Input;

namespace PropertyAgencyDesktopApp.ViewModels
{
    public class AgentViewModel : ViewModelBase
    {
        private PropertyAgencyBaseEntities _context = new PropertyAgencyBaseEntities();
        public AgentViewModel()
        {
            Title = "Agents";
            LoadAgents();
        }

        private async void LoadAgents()
        {
            Agents = await _context.Agent.ToListAsync();
        }

        private RelayCommand addNewAgentCommand;

        public ICommand AddNewAgentCommand
        {
            get
            {
                if (addNewAgentCommand == null)
                {
                    addNewAgentCommand = new RelayCommand(AddNewAgent);
                }

                return addNewAgentCommand;
            }
        }

        private void AddNewAgent(object commandParameter)
        {
        }

        private IEnumerable<Agent> _agents;

        public IEnumerable<Agent> Agents
        {
            get => _agents;
            set => SetProperty(ref _agents, value);
        }
    }
}
