using Windows.UI.Core;
using MvvmCross.WindowsUWP.Views;
using PropertyManager.ViewModels;

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
    }
}
