using Android.App;
using Android.Content;
using MvvmCross.Platform;
using PropertyManager.Services;

namespace PropertyManager.Droid.Services
{
    public static class FilePickerAgentContinuationHelper
    {
        public static void SetAuthenticationAgentContinuationEventArgs(ContentResolver contentResolver, int requestCode,
            Result resultCode, Intent data)
        {
            (Mvx.Resolve<IFilePickerService>() as FilePickerService)?.ResolveTask(
                contentResolver, requestCode, resultCode, data);
        }
    }
}