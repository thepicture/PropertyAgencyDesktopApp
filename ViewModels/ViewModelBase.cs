using System.ComponentModel;
using System.Runtime.CompilerServices;

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
    }
}
