﻿using MVVMSample.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for ListWindow.xaml
    /// </summary>
    public partial class ListWindow : Window
    {
        private ListWindowViewModel _viewModel;

        public ListWindowViewModel ViewModel
        {
            get { return _viewModel; }
            set { _viewModel = value; DataContext = _viewModel; }
        }

        public ListWindow()
        {
            InitializeComponent();

            ViewModel = new ListWindowViewModel();
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (!ViewModel.IsLoaded)
            {
                await ViewModel.RefreshAsync();

                ViewModel.IsLoaded = true;
            }
        }
    }
}
