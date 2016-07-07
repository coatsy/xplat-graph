using System.Diagnostics;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using MvvmCross.WindowsUWP.Views;
using PropertyManager.Models;
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

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            var groupsViewModel = ViewModel as GroupViewModel;
            groupsViewModel?.SaveDetailsCommand.Execute(null);
            base.OnNavigatingFrom(e);
        }

        private void OnDriveItemClick(object sender, ItemClickEventArgs e)
        {
            var group = e.ClickedItem as DriveItemModel;
            var groupsViewModel = ViewModel as GroupViewModel;
            groupsViewModel?.LaunchDriveItemAsync(group);
        }

        private void OnKeyUp(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key != VirtualKey.Enter || !e.KeyStatus.WasKeyDown)
            {
                return;
            }

            var groupsViewModel = ViewModel as GroupViewModel;
            groupsViewModel?.SendMessageCommand.Execute(null);
        }
    }
}
