using Windows.UI.Core;
using MvvmCross.WindowsUWP.Views;
using PropertyManager.ViewModels;

namespace PropertyManager.UWP.Views
{
    public sealed partial class GroupsView : MvxWindowsPage
    {
        public GroupsView()
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
            var groupsViewModel = ViewModel as GroupsViewModel;
            groupsViewModel?.GoBackCommand.Execute(null);
        }
    }
}
