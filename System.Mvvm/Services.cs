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
        /// <summary>
        /// Init will register any instances of the MvvmServiceAttribute in the calling assembly
        /// </summary>
        public static void Init()
        {
            var assm = Assembly.GetCallingAssembly();

            LoadServices(new Assembly[] { assm });
        }

        /// <summary>
        /// Init will register any instances of the MvvmServiceAttribute in the calling assembly and the asssemblies provided
        /// </summary>
        /// <param name="assemblies">Array of external assemblies</param>
        public static void Init(params Assembly[] assemblies)
        {
            var assms = new List<Assembly>(){ Assembly.GetCallingAssembly() };

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
        /// Init will registrer an instance MvvmServiceAttribute in the calling assembly and the assemblies containing the specified types
        /// </summary>
        /// <param name="types"></param>
        public static void Init(params Type[] types)
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

        #region UI Providers

        /// <summary>
        /// Stored instances of the 
        /// </summary>
        private static Dictionary<Type, object> _services { get; set; } = new Dictionary<Type, object>();

        /// <summary>
        /// Register types
        /// </summary>
        private static List<Type> ServiceTypes { get; set; } = new List<Type>();


        /// <summary>
        /// Register a Mvvm Service
        /// </summary>
        /// <typeparam name="T">Service implementation type</typeparam>
        public static void Register<T>() where T : new()
        {
            var newType = typeof(T);

            if (!ServiceTypes.Contains(newType))
                ServiceTypes.Add(newType);
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
            var type = ServiceTypes.FirstOrDefault(x => x.Equals(typeof(T)) || typeof(T).IsAssignableFrom(x));

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

            throw new Exception("Type not registered");
        }

        private static void LoadServices(IEnumerable<Assembly> assemblies)
        {
            var custAttr = typeof(MvvmServiceAttribute);

            foreach (var assembly in assemblies)
            {
                var serAttrs = assembly.GetCustomAttributes(custAttr, true);

                foreach (MvvmServiceAttribute attrib in serAttrs)
                {
                    if (!ServiceTypes.Contains(attrib.Implementation))
                        ServiceTypes.Add(attrib.Implementation);
                }
            }
        }
        #endregion
    }
}
