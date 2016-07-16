using System;
using PropertyManager.Services;
using Android.Content;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;

namespace PropertyManager.Droid.Services
{
    public class LauncherService : ILauncherService
    {
        public void LaunchWebUri(Uri uri)
        {
            // Get the top activity.
            var topActivity = Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;

            if (topActivity == null)
            {
                throw new Exception("Could not find the top Activity.");
            }

            // Launch the URI.
            var browserIntent = new Intent(Intent.ActionView, 
                Android.Net.Uri.Parse(uri.AbsoluteUri)); 
            topActivity.StartActivity(browserIntent);
        }
    }
}