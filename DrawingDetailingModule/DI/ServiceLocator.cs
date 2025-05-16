using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingDetailingModule.DI
{
    /// <summary>
    /// Simple service locator pattern implementation for dependency injection
    /// </summary>
    public class ServiceLocator
    {
        private static readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();
        private static readonly Dictionary<Type, Func<object>> _serviceFactories = new Dictionary<Type, Func<object>>();

        /// <summary>
        /// Registers a singleton instance of a service
        /// </summary>
        /// <typeparam name="TInterface">The interface type to register</typeparam>
        /// <param name="instance">The instance to register</param>
        public static void Register<TInterface>(TInterface instance) where TInterface : class
        {
            _services[typeof(TInterface)] = instance;
        }

        /// <summary>
        /// Registers a factory function that creates a service instance
        /// </summary>
        /// <typeparam name="TInterface">The interface type to register</typeparam>
        /// <param name="factory">Factory function to create the service</param>
        public static void RegisterFactory<TInterface>(Func<TInterface> factory) where TInterface : class
        {
            _serviceFactories[typeof(TInterface)] = () => factory();
        }

        /// <summary>
        /// Resolves a service by its interface
        /// </summary>
        /// <typeparam name="TInterface">The interface type to resolve</typeparam>
        /// <returns>The instance of the service</returns>
        public static TInterface Resolve<TInterface>() where TInterface : class
        {
            var interfaceType = typeof(TInterface);

            // First check if we have a singleton instance
            if (_services.TryGetValue(interfaceType, out var service))
            {
                return (TInterface)service;
            }

            // Then check if we have a factory for this type
            if (_serviceFactories.TryGetValue(interfaceType, out var factory))
            {
                var instance = (TInterface)factory();

                // Store it as a singleton
                _services[interfaceType] = instance;
                return instance;
            }

            throw new InvalidOperationException($"Service of type {interfaceType.Name} is not registered");
        }

        /// <summary>
        /// Clears all registered services
        /// </summary>
        public static void ClearAll()
        {
            _services.Clear();
            _serviceFactories.Clear();
        }

        /// <summary>
        /// Checks if a service is registered
        /// </summary>
        /// <typeparam name="TInterface">The interface type to check</typeparam>
        /// <returns>True if the service is registered</returns>
        public static bool IsRegistered<TInterface>() where TInterface : class
        {
            var interfaceType = typeof(TInterface);
            return _services.ContainsKey(interfaceType) || _serviceFactories.ContainsKey(interfaceType);
        }
    }
}
