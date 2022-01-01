using PropertyAgencyDesktopApp.Commands;
using PropertyAgencyDesktopApp.Models.Entities;
using System;
using System.Data.Common;
using System.Data.Entity;
using System.Windows.Input;

namespace PropertyAgencyDesktopApp.ViewModels
{
    public class AddEditClientViewModel : ViewModelBase
    {
        private Client _currentClient;
        private readonly PropertyAgencyBaseEntities _context =
            new PropertyAgencyBaseEntities();


        public Client CurrentClient
        {
            get => _currentClient;
            set => SetProperty(ref _currentClient, value);
        }

        public AddEditClientViewModel(Client client)
        {
            Title = "Client edit";
            LoadClient(client);
        }

        private async void LoadClient(Client client)
        {
            CurrentClient = await _context.Client.FirstAsync(c => c.Id == client.Id);
        }

        public AddEditClientViewModel()
        {
            Title = "Client add";
            CurrentClient = new Client();
        }

        private RelayCommand saveClientCommand;

        public ICommand SaveClientCommand
        {
            get
            {
                if (saveClientCommand == null)
                {
                    saveClientCommand = new RelayCommand(SaveClient);
                }

                return saveClientCommand;
            }
        }

        private async void SaveClient(object commandParameter)
        {
            IsMessageClosed = false;
            if (string.IsNullOrEmpty(CurrentClient.Email)
                && string.IsNullOrEmpty(CurrentClient.Phone))
            {
                MessageType = "Warning";
                ValidationMessage = "Client must have email or phone or both";
                return;
            }
            if (CurrentClient.Id == 0)
            {
                _ = _context.Client.Add(CurrentClient);
            }
            try
            {
                _ = await _context.SaveChangesAsync();
                MessageType = "Alert";
                ValidationMessage = "Client was successfully saved!";
            }
            catch (DbException ex)
            {
                System.Diagnostics.Debug.Write(ex.StackTrace);
                MessageType = "Danger";
                ValidationMessage = "Can't save the client " +
                    "into the database. " +
                    "Try to go back and to the client again," +
                    "or reload the app if it doesn't help";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex.StackTrace);
                MessageType = "Danger";
                ValidationMessage = "Can't save the client " +
                    "into the database. " +
                    "Fatal error encountered. " +
                    "Reload the app and try again";
            }
        }
    }
}
