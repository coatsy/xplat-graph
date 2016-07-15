using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using MvvmCross.Droid.Support.V7.AppCompat;
using PropertyManager.ViewModels;

namespace PropertyManager.Droid.Views
{
    [Activity(Label = "GroupView", Theme = "@style/Theme.Light",
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class GroupView : MvxAppCompatActivity
    {
        public new GroupViewModel ViewModel => base.ViewModel as GroupViewModel;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;
            base.OnCreate(savedInstanceState);
        }

        protected override void OnViewModelSet()
        {
            Title = ViewModel.Group.DisplayName;
            SetContentView(Resource.Layout.GroupView);
            base.OnViewModelSet();
        }

        protected override void OnResume()
        {
            ViewModel.OnResume();
            base.OnResume();
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            AuthenticationAgentContinuationHelper.SetAuthenticationAgentContinuationEventArgs(
                requestCode, resultCode, data);
        }
    }
}