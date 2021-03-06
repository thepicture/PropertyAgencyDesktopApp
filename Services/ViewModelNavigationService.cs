using PropertyAgencyDesktopApp.ViewModels;
using System;
using System.Collections.Generic;

namespace PropertyAgencyDesktopApp.Services
{
    public class ViewModelNavigationService : INavigationService<ViewModelBase>
    {
        public ViewModelNavigationService()
        {
            Journal = new Stack<ViewModelBase>();
        }

        public Stack<ViewModelBase> Journal { get; private set; }

        public ViewModelBase CurrentNavigationTarget
        {
            get => Journal.Peek();
            private set => Journal.Push(value);
        }

        public event Action Navigated;

        public void GoBack()
        {
            _ = Journal.Pop();
            CurrentNavigationTarget = (ViewModelBase)
                Activator.CreateInstance(Journal.Peek().GetType());
            Navigated?.Invoke();
        }

        public bool IsCanGoBack()
        {
            return Journal.Count > 1;
        }

        public void Navigate<T>()
        {
            ViewModelBase instance = (ViewModelBase)Activator
                                                    .CreateInstance(typeof(T));
            Journal.Push(instance);
            Navigated?.Invoke();
        }
        public void NavigateWithParameter<T>(object parameter)
        {
            ViewModelBase instance = (ViewModelBase)Activator
                                                    .CreateInstance(typeof(T),
                new object[] { parameter });
            Journal.Push(instance);
            Navigated?.Invoke();
        }

    }
}
