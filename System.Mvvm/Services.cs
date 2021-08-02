using System;
using System.Collections.Generic;
using System.Linq;
using System.Mvvm.Attributes;
using System.Reflection;
using System.Text;

namespace System.Mvvm
{
    /// <summary>
    /// Simple Dependency service container
    /// </summary>
    public static class Services
    {
        #region Public members

        /// <summary>
        /// Registers this instance.
        /// </summary>
        public static void Register()
        {
            var assm = Assembly.GetCallingAssembly();

            LoadServices(new Assembly[] { assm });
        }

        /// <summary>
        /// Register a Mvvm Service
        /// </summary>
        /// <typeparam name="T">Service implementation type</typeparam>
        public static void Register<T>() where T : new()
        {
            var newType = typeof(T);

            AddService(newType);

        }

        /// <summary>
        /// Registers the interface and implementation types
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <exception cref="Exception">The first type must be an interface when calling Resgister<T,T2></exception>
        public static void Register<T, T2>() where T2 : class, new()
        {
            var interfaceType = typeof(T);

            if (!interfaceType.IsInterface)
                throw new Exception("The first type must be an interface when calling Resgister<T,T2>");


            var implementationType = typeof(T2);

            AddService(implementationType, interfaceType);

        }

        /// <summary>
        /// Register all Mvvm Service implementation in the external assemblies
        /// </summary>
        /// <param name="assemblies">Array of external assemblies</param>
        public static void Register(params Assembly[] assemblies)
        {
            var assms = new List<Assembly>() { Assembly.GetCallingAssembly() };

            if (assemblies != null && assemblies.Length > 0)
            {
                foreach (var aAssm in assemblies)
                {
                    if (!assms.Contains(aAssm))
                        assms.Add(aAssm);
                }
            }

            LoadServices(assms);
        }

        /// <summary>
        ///  Register all Mvvm Services in the assemblies conatining the specified types
        /// </summary>
        /// <param name="types">Types to process in external assemblies</param>
        public static void Register(params Type[] types)
        {
            foreach (var type in types)
                AddService(type);
        }

        /// <summary>
        /// Get a UI Service implementation
        /// </summary>
        /// <typeparam name="T">The inherited type</typeparam>
        /// <returns></returns>
        public static T Get<T>()
        {
            var type = FindImplementation<T>();

            //if the implentation type is null then it must not have been rergistered so through an error
            if (type == null)
                throw new Exception(string.Format("There is no registered implementation for type: {0}", typeof(T).Name));

            var cachedAttribute = type.GetTypeInfo().GetCustomAttribute<SingletonServiceAttribute>();

            if (cachedAttribute == null)
                return (T)Activator.CreateInstance(type);

            if (_cachedServices.ContainsKey(type))
            {
                return (T)_cachedServices[type];
            }   
            else
            {
                var newType = (T)Activator.CreateInstance(type);

                _cachedServices.Add(type, newType);

                return newType;
            }

        }

        #endregion

        #region Private members
        private static Type FindImplementation<T>()
        {
            Type type = null;

            //get the request type
            var reqType = typeof(T);

            //check to see if there is an explicitly type service entry
            if (_explicitlyTypedServices.ContainsKey(reqType))
                type = _explicitlyTypedServices[reqType];

            //if there is not explicityly type service see if there an matched type or assginable type in service types
            if (type == null)
                type = _serviceTypes.FirstOrDefault(x => x.Equals(reqType) || reqType.IsAssignableFrom(x));

            return type;
        }

        /// <summary>
        /// Stored instances of the 
        /// </summary>
        private static Dictionary<Type, object> _cachedServices { get; set; } = new Dictionary<Type, object>();
        
        private static Dictionary<Type, Type> _explicitlyTypedServices { get; set; } = new Dictionary<Type, Type>();

        /// <summary>
        /// Register types
        /// </summary>
        private static List<Type> _serviceTypes { get; set; } = new List<Type>();

        private static void LoadServices(IEnumerable<Assembly> assemblies)
        {
            var custAttr = typeof(MvvmServiceAttribute);

            foreach (var assembly in assemblies)
            {
                var serAttrs = assembly.GetCustomAttributes(custAttr, true);

                foreach (MvvmServiceAttribute attrib in serAttrs)
                {
                    AddService(attrib.Implementation, attrib.Interface);

                }
                    

            }
        }

        private static void AddService(Type implementationType, Type interfaceType = null)
        {
            if(interfaceType == null)
            {
                if (!_serviceTypes.Contains(implementationType))
                    _serviceTypes.Add(implementationType);
            }
            else
            {
                if (!_explicitlyTypedServices.ContainsKey(interfaceType))
                    _explicitlyTypedServices.Add(interfaceType, implementationType);
            }
            
        }



        #endregion

    }
}
