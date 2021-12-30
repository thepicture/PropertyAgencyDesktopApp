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
            private set => CurrentNavigationTarget = value;
        }

        public event Action Navigated;

        public void GoBack()
        {
            _ = Journal.Pop();
            CurrentNavigationTarget = Journal.Peek();
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
    }
}
