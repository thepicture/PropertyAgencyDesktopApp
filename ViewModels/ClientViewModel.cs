using PropertyAgencyDesktopApp.Models.Entities;
using System.Collections.Generic;
using System.Data.Entity;

namespace PropertyAgencyDesktopApp.ViewModels
{
    public class ClientViewModel : ViewModelBase
    {
        private readonly PropertyAgencyBaseEntities _context;
        private IEnumerable<Client> _clients;
        public ClientViewModel()
        {
            Title = "Clients";
            _context = new PropertyAgencyBaseEntities();
            LoadClients();
        }

        private async void LoadClients()
        {
            Clients = await _context.Client.ToListAsync();
        }

        public IEnumerable<Client> Clients
        {
            get => _clients;
            set => SetProperty(ref _clients, value);
        }
    }
}
