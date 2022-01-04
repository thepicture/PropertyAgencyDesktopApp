using PropertyAgencyDesktopApp.Commands;
using PropertyAgencyDesktopApp.Models.Entities;
using PropertyAgencyDesktopApp.Services;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PropertyAgencyDesktopApp.ViewModels
{
    public class AgentViewModel : ViewModelBase
    {
        private readonly PropertyAgencyBaseEntities _context =
            new PropertyAgencyBaseEntities();
        private string _searchText;
        public AgentViewModel()
        {
            Title = "Agents";
            LoadAgents();
        }

        private async void LoadAgents()
        {
            Agents = await _context.Agent.ToListAsync();
            if (!string.IsNullOrEmpty(SearchText))
            {
                IWordIndefiniteSearcher distanceCalculator = DependencyService
                                         .Get<IWordIndefiniteSearcher>();
                _ = await Task.Run(() =>
                  {
                      return Agents = from Agent a in Agents
                                      where distanceCalculator
                                            .Calculate(SearchText,
                                                    a.FirstName) < 4
                                      || distanceCalculator
                                         .Calculate(SearchText,
                                                    a.LastName) < 4
                                      || distanceCalculator
                                         .Calculate(SearchText,
                                                    a.MiddleName) < 4
                                      select a;
                  });
            }
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
            DependencyService.Get<INavigationService<ViewModelBase>>()
                             .Navigate<AddEditAgentViewModel>();
        }

        private IEnumerable<Agent> _agents;

        public IEnumerable<Agent> Agents
        {
            get => _agents;
            set => SetProperty(ref _agents, value);
        }

        private RelayCommand editAgentCommand;

        public ICommand EditAgentCommand
        {
            get
            {
                if (editAgentCommand == null)
                {
                    editAgentCommand = new RelayCommand(EditAgent);
                }

                return editAgentCommand;
            }
        }

        private void EditAgent(object commandParameter)
        {
            Agent agent = commandParameter as Agent;
            DependencyService.Get<INavigationService<ViewModelBase>>()
                          .NavigateWithParameter
                          <AddEditAgentViewModel>(agent);
        }

        private RelayCommand deleteAgentCommand;

        public ICommand DeleteAgentCommand
        {
            get
            {
                if (deleteAgentCommand == null)
                {
                    deleteAgentCommand = new RelayCommand(DeleteAgent);
                }

                return deleteAgentCommand;
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value)
                    && !string.IsNullOrEmpty(value))
                {
                    LoadAgents();
                }
            }
        }

        private async void DeleteAgent(object commandParameter)
        {
            Agent agent = commandParameter as Agent;
            if (agent.Offer.Count > 0 || agent.Demand.Count > 0)
            {
                IsMessageClosed = false;
                MessageType = "Alert";
                ValidationMessage = ShowDeleteResultService
                                    .OnRelatedObjectsError(nameof(Agent),
                                                           nameof(Offer));
                return;
            }
            IQuestionService questionService =
                DependencyService.Get<IQuestionService>();
            string deleteTemplate = ShowDeleteResultService
                                    .OnDelete(nameof(Agent));
            if (questionService.Ask(deleteTemplate))
            {
                IsMessageClosed = false;
                Agent agentFromDatabase = _context.Agent.Find(agent.Id);
                _ = _context.Agent.Remove(agentFromDatabase);
                try
                {
                    _ = await _context.SaveChangesAsync();
                    MessageType = "Alert";
                    ValidationMessage = ShowDeleteResultService
                                        .OnSuccessfulDelete(nameof(Agent));
                    LoadAgents();
                }
                catch (DbException ex)
                {
                    System.Diagnostics.Debug.Write(ex.StackTrace);
                    MessageType = "Danger";
                    ValidationMessage = ShowDeleteResultService
                                        .OnCommonDeleteError(nameof(Agent));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Write(ex.StackTrace);
                    MessageType = "Danger";
                    ValidationMessage = ShowDeleteResultService
                                        .OnCommonDeleteError(nameof(Agent));
                }
            }
        }
    }
}
