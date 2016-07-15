using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using MvvmCross.Droid.Support.V7.AppCompat;
using PropertyManager.ViewModels;
using Android.Support.V4.View;
using PropertyManager.Droid.Adapters;

namespace PropertyManager.Droid.Views
{
    [Activity(Label = "GroupView", Theme = "@style/Theme.Light.NoActionBar",
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class GroupView : MvxAppCompatActivity<GroupViewModel>
    {
        private Android.Support.Design.Widget.FloatingActionButton _addActionButton;
        private Android.Support.Design.Widget.FloatingActionButton _editActionButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        protected override void OnViewModelSet()
        {
            Title = ViewModel.Group.DisplayName;
            SetContentView(Resource.Layout.GroupView);
            base.OnViewModelSet();

            // Get toolbar and set title.
            var toolbar = (Android.Support.V7.Widget.Toolbar)FindViewById(Resource.Id.toolbar);
            toolbar.Title = Title;

            // Configure tab layout.
            ViewPager viewPager = (ViewPager)FindViewById(Resource.Id.view_pager);
            viewPager.Adapter = new GroupViewFragmentsAdapter(this);

            var tabLayout = FindViewById<Android.Support.Design.Widget.TabLayout>(
                Resource.Id.tab_layout);
            tabLayout.SetupWithViewPager(viewPager);

            // Get the FABs and hook up the event listener.
            _addActionButton = (Android.Support.Design.Widget.FloatingActionButton)
                FindViewById(Resource.Id.add_fab);
            _editActionButton = (Android.Support.Design.Widget.FloatingActionButton)
             FindViewById(Resource.Id.edit_fab);
            viewPager.PageSelected += OnPageSelected;
            OnPageSelected(null, new ViewPager.PageSelectedEventArgs(
                tabLayout.SelectedTabPosition));
        }

        private void OnPageSelected(object sender, ViewPager.PageSelectedEventArgs e)
        {
            // Check add FAB.
            if (e.Position == 2)
            {
                _addActionButton.Show();
            }
            else
            {
                _addActionButton.Hide();
            }

            // Check edit FAB.
            if (e.Position == 0)
            {
                _editActionButton.Show();
            }
            else
            {
                _editActionButton.Hide();
            }

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