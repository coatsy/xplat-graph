using System;
using Windows.System;
using PropertyManager.Services;

namespace PropertyManager.UWP.Services
{
    public class LauncherService : ILauncherService
    {
        public void LaunchWebUri(Uri uri)
        {
            Launcher.LaunchUriAsync(uri);
        }
    }
}
