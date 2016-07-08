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
            ViewModel?.GoBackCommand.Execute(null);
        }
    }
}
