﻿using Windows.UI.Core;
using MvvmCross.WindowsUWP.Views;
using PropertyManager.ViewModels;
using Windows.System;

namespace PropertyManager.UWP.Views
{
    public sealed partial class DetailsView : MvxWindowsPage
    {
        public new DetailsViewModel ViewModel => base.ViewModel as DetailsViewModel;

        public DetailsView()
        { 
            InitializeComponent();

            // Register for back requests.
            var systemNavigationManager = SystemNavigationManager.GetForCurrentView();
            systemNavigationManager.AppViewBackButtonVisibility = 
                AppViewBackButtonVisibility.Visible;
            systemNavigationManager.BackRequested += OnBackRequested;
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs backRequestedEventArgs)
        {
            backRequestedEventArgs.Handled = true;
            ViewModel?.GoBackCommand.Execute(null);
        }

        private void OnTextChanged(object sender, Windows.UI.Xaml.Controls.TextChangedEventArgs e)
        {
            ViewModel?.Validate();
        }

        private void OnTextBoxKeyUp(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key != VirtualKey.Enter || !e.KeyStatus.WasKeyDown)
            {
                return;
            }
            if (sender == StreetNameTextBox)
            {
                DescriptionTextBox.Focus(Windows.UI.Xaml.FocusState.Keyboard);
            }
            else if (sender == RoomsTextBox)
            {
                LivingAreaTextBox.Focus(Windows.UI.Xaml.FocusState.Keyboard);
            }
            else if (sender == LivingAreaTextBox)
            {
                LotSizeTextBox.Focus(Windows.UI.Xaml.FocusState.Keyboard);
            }
            else if (sender == LotSizeTextBox)
            {
                OperatingCostsTextBox.Focus(Windows.UI.Xaml.FocusState.Keyboard);
            }
        }
    }
}
