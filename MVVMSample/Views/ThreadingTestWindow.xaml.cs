using MVVMSample.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MVVMSample.Views
{
	/// <summary>
	/// Interaction logic for ThreadingTestWindow.xaml
	/// </summary>
	public partial class ThreadingTestWindow : Window
	{
		private ThreadingTestViewModel _viewModel;

		public ThreadingTestViewModel ViewModel
		{
			get { return _viewModel; }
			set { _viewModel = value; DataContext = _viewModel; }
		}

		public ThreadingTestWindow()
		{
			InitializeComponent();

			ViewModel = new ThreadingTestViewModel();

		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			if (!ViewModel.IsLoaded)
			{
				new Thread(async () =>
				{
					await ViewModel.LoadAsync();

					ViewModel.IsLoaded = true;

				}).Start();


			}
		}
	}
}
