using Android.App;
using Android.OS;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;
using PropertyManager.Droid.Services;
using PropertyManager.Services;

namespace PropertyManager.Droid.Views
{
    [Activity(Label = "View for FirstViewModel")]
    public class FirstView : MvxActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            ((AuthenticationService) Mvx.Resolve<IAuthenticationService>()).CallerActivity = this;
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.FirstView);
        }
    }
}
