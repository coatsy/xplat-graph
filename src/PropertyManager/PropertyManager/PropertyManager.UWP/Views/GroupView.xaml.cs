﻿using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using MvvmCross.WindowsUWP.Views;
using PropertyManager.ViewModels;

namespace PropertyManager.UWP.Views
{
    public sealed partial class GroupView : MvxWindowsPage
    {
        public GroupView()
        { 
            InitializeComponent();

            // Register for Back Requests.
            var systemNavigationManager = SystemNavigationManager.GetForCurrentView();
            systemNavigationManager.AppViewBackButtonVisibility = 
                AppViewBackButtonVisibility.Visible;
            systemNavigationManager.BackRequested += OnBackRequested;
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs backRequestedEventArgs)
        {
            var groupsViewModel = ViewModel as GroupViewModel;
            groupsViewModel?.GoBackCommand.Execute(null);
        }
    }
}