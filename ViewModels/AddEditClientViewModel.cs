using PropertyAgencyDesktopApp.Models.Entities;

namespace PropertyAgencyDesktopApp.ViewModels
{
    public class AddEditClientViewModel : ViewModelBase
    {
        private Client _currentClient;

        public Client CurrentClient
        {
            get => _currentClient;
            set => SetProperty(ref _currentClient, value);
        }

        public AddEditClientViewModel(Client client)
        {
            Title = "Client edit";
            CurrentClient = client;
        }

        public AddEditClientViewModel()
        {
            Title = "Client add";
            CurrentClient = new Client();
        }
    }
}
