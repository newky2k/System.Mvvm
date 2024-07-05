using System;
using System.Windows;
using System.Windows.Markup;

namespace System.Mvvm
{
    /// <summary>
    /// Manager for Data Templates
    /// </summary>
    public static class DataTemplateManager
    {
        #region Methods

        /// <summary>
        /// Registers the data template.
        /// </summary>
        /// <typeparam name="TViewModel">Type of the view model.</typeparam>
        /// <typeparam name="TView">     Type of the view.</typeparam>
        public static void RegisterDataTemplate<TViewModel, TView>() where TView : FrameworkElement
        {
            RegisterDataTemplate(typeof(TViewModel), typeof(TView));
        }

        /// <summary>
        /// Registers the data template.
        /// </summary>
        /// <param name="viewModelType">Type of the view model.</param>
        /// <param name="viewType">     Type of the view.</param>
        public static void RegisterDataTemplate(Type viewModelType, Type viewType)
        {
            var template = CreateTemplate(viewModelType, viewType);
            var key = template.DataTemplateKey;

            if (!Application.Current.Resources.Contains(key))
                Application.Current.Resources.Add(key, template);
        }

        private static DataTemplate CreateTemplate(Type viewModelType, Type viewType)
        {
            const string xamlTemplate = "<DataTemplate DataType=\"{{x:Type vm:{0}}}\"><v:{1} /></DataTemplate>";
            var xaml = string.Format(xamlTemplate, viewModelType.Name, viewType.Name, viewModelType.Namespace, viewType.Namespace);

            var context = new ParserContext();

            context.XamlTypeMapper = new XamlTypeMapper(new string[0]);
            context.XamlTypeMapper.AddMappingProcessingInstruction("vm", viewModelType.Namespace, viewModelType.Assembly.FullName);
            context.XamlTypeMapper.AddMappingProcessingInstruction("v", viewType.Namespace, viewType.Assembly.FullName);

            context.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            context.XmlnsDictionary.Add("x", "http://schemas.microsoft.com/winfx/2006/xaml");
            context.XmlnsDictionary.Add("vm", "vm");
            context.XmlnsDictionary.Add("v", "v");

            context.ToString();
            var template = (DataTemplate)XamlReader.Parse(xaml, context);
            return template;
        }

        #endregion
    }
}
