using System;
using System.Collections.Generic;

namespace PropertyAgencyDesktopApp.Services
{
    public interface INavigationService<TNavigationTarget>
    {
        TNavigationTarget CurrentNavigationTarget { get; }
        Stack<TNavigationTarget> Journal { get; }
        event Action Navigated;
        void Navigate<T>();
        void NavigateWithParameter<T>(object parameter);
        void GoBack();
        bool IsCanGoBack();
    }
}
