using System;
using System.Collections.Generic;
using System.Linq;

namespace PropertyAgencyDesktopApp.Services
{
    public static class DependencyService
    {
        private static readonly IDictionary<Type, object> _dependencies
            = new Dictionary<Type, object>();

        public static void Register<T>()
        {
            _dependencies.Add(typeof(T),
                              Activator.CreateInstance(typeof(T)));
        }

        public static T Get<T>()
        {
            object instance =
                _dependencies
                .FirstOrDefault(d =>
                {
                    return typeof(T).IsAssignableFrom(d.Value.GetType());
                }).Value;
            if (instance == null)
            {
                throw new NullReferenceException("Implementation " +
                    $"for {typeof(T).FullName}" +
                    "was not found");
            }
            return (T)instance;
        }
    }
}
