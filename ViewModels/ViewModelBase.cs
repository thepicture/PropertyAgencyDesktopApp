using PropertyAgencyDesktopApp.Commands;
using PropertyAgencyDesktopApp.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace PropertyAgencyDesktopApp.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _title = string.Empty;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        protected void OnPropertyChanged(
            [CallerMemberName] string propertyName = null)
        {
            PropertyChanged?
                .Invoke(this,
                        new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field,
                                      T newValue,
                                      [CallerMemberName]
                                      string propertyName = null)
        {
            if (!Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?
                    .Invoke(this,
                            new PropertyChangedEventArgs(propertyName));
                return true;
            }

            return false;
        }

        private RelayCommand _goBackCommand;

        public ICommand GoBackCommand
        {
            get
            {
                if (_goBackCommand == null)
                {
                    _goBackCommand = new RelayCommand(GoBack);
                }

                return _goBackCommand;
            }
        }

        private void GoBack(object commandParameter)
        {
            DependencyService.Get<INavigationService<ViewModelBase>>()
                             .GoBack();
        }

        private string _validationMessage;

        public string ValidationMessage
        {
            get => _validationMessage;
            set => SetProperty(ref _validationMessage, value);
        }

        private string _messageType;

        public string MessageType
        {
            get => _messageType;
            set => SetProperty(ref _messageType, value);
        }

        private bool _isMessageClosed = true;

        public bool IsMessageClosed
        {
            get => _isMessageClosed;
            set => SetProperty(ref _isMessageClosed, value);
        }

        public IShowSaveResultService ShowSaveResultService =>
            DependencyService
                 .Get<IShowSaveResultService>();
        public IShowDeleteResultService ShowDeleteResultService =>
            DependencyService
                .Get<IShowDeleteResultService>();
    }
}
