using System;
using System.Collections.Generic;
using System.Linq;
using System.Mvvm;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DSoft.System.Mvvm.Uwp
{
    public class MvvmManager
    {
        private static void CommonInit()
        {
            DelegateCommand.RequeryCommandsOnChange = true;
            //DelegateCommand.ExecuteChanged = (ev, shoudlAdd) =>
            //{
            //    if (shoudlAdd)
            //    {
            //        ev.Method.Invoke();

            //        //CommandManager.RequerySuggested += ev;
            //    }
            //    else
            //    {
            //        //CommandManager.RequerySuggested -= ev;
            //    }
            //};

            //Centralised management of errors notifications through the ViewModel
            ViewModel.OnErrorOccured += async (object sen, Exception e2)  =>
            {
                if (e2 is TitledException)
                {
                    var ex = e2 as TitledException;

                    await UI.ShowAlertAsync(ex.Title, e2.Message);
                }
                else
                {

                    await UI.ShowAlertAsync("System Error", e2.Message);
                }

            };

        }
        /// <summary>
        /// Initializes the MvvmManager
        /// </summary>
        public static void Init()
        {
            CommonInit();

            UI.Init<PlatformUIProvider>(new List<Assembly>() { Assembly.GetCallingAssembly(), typeof(MvvmManager).Assembly });
        }

        public static void Init(params Assembly[] assemblies)
        {
            CommonInit();

            var newAssms = new List<Assembly>()
            {
                typeof(MvvmManager).Assembly,
            };

            if (!assemblies.Contains(Assembly.GetCallingAssembly()))
                newAssms.Add(Assembly.GetCallingAssembly());

            newAssms.AddRange(assemblies);

            UI.Init<PlatformUIProvider>(newAssms);
        }

    }
}
