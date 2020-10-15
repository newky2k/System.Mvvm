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
        /// Init will register any instances of the MvvmServiceAttribute in the calling assembly
        /// </summary>
        public static void Init() => Register();

        /// <summary>
        /// Init will register any instances of the MvvmServiceAttribute in the calling assembly and the asssemblies provided
        /// </summary>
        /// <param name="assemblies">Array of external assemblies</param>
        public static void Init(params Assembly[] assemblies) => Register(assemblies);

        /// <summary>
        /// Init will registrer an instance MvvmServiceAttribute in the calling assembly and the assemblies containing the specified types
        /// </summary>
        /// <param name="types"></param>
        public static void Init(params Type[] types) => Register(types);

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

            if (!_serviceTypes.Contains(newType))
                _serviceTypes.Add(newType);
        }

        public static void Register<T>(params Assembly[] assemblies)
        {
            var newAssemblies = new List<Assembly>()
            {
                typeof(T).Assembly,
            };

            newAssemblies.AddRange(assemblies);

            Register(newAssemblies.ToArray());
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

        public static void Register<T>(params Type[] types) where T : new()
        {
            var newTypes = new List<Type>()
            {
                typeof(T),
            };

            newTypes.AddRange(types);

            Register(newTypes.ToArray());
        }

        /// <summary>
        ///  Register all Mvvm Services in the assemblies conatining the specified types
        /// </summary>
        /// <param name="types">Types to process in external assemblies</param>
        public static void Register(params Type[] types)
        {
            var assms = new List<Assembly>() { Assembly.GetCallingAssembly() };

            if (types != null && types.Any())
            {
                foreach (var aAssm in types.Select(x => x.Assembly))
                {
                    if (!assms.Contains(aAssm))
                        assms.Add(aAssm);
                }
            }

            LoadServices(assms);
        }

        /// <summary>
        /// Get a UI Service implementation
        /// </summary>
        /// <typeparam name="T">The inherited type</typeparam>
        /// <param name="cachedInstance">Cache the instance for use later, default is true</param>
        /// <returns></returns>
        public static T Get<T>(bool cachedInstance = true)
        {
            var type = _serviceTypes.FirstOrDefault(x => x.Equals(typeof(T)) || typeof(T).IsAssignableFrom(x));

            if (type != null)
            {
                if (!cachedInstance == true)
                {
                    return (T)Activator.CreateInstance(type);
                }

                if (_services.ContainsKey(type))
                    return (T)_services[type];
                else
                {
                    var newType = (T)Activator.CreateInstance(type);

                    _services.Add(type, newType);

                    return newType;
                }
            }

            throw new Exception($"{typeof(T).Name} not registered");
        }
        #endregion

        #region Private members


        /// <summary>
        /// Stored instances of the 
        /// </summary>
        private static Dictionary<Type, object> _services { get; set; } = new Dictionary<Type, object>();

        /// <summary>
        /// Register types
        /// </summary>
        private static List<Type> _serviceTypes { get; set; } = new List<Type>();

        private static void LoadServices(IEnumerable<Assembly> assemblies)
        {
            var custAttr = typeof(MvvmServiceAttribute);
            var serviceAttr = typeof(UIServiceAttribute);

            foreach (var assembly in assemblies)
            {
                var serAttrs = assembly.GetCustomAttributes(custAttr, true);
                var uiAttrs = assembly.GetCustomAttributes(serviceAttr, true);

                foreach (MvvmServiceAttribute attrib in serAttrs)
                {
                    if (!_serviceTypes.Contains(attrib.Implementation))
                        _serviceTypes.Add(attrib.Implementation);
                }

                foreach (UIServiceAttribute attrib in uiAttrs)
                {
                    if (!_serviceTypes.Contains(attrib.Implementation))
                        _serviceTypes.Add(attrib.Implementation);
                }

            }
        }





        #endregion
    }
}
