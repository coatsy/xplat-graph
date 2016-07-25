using System;
using Foundation;
using PropertyManager.Services;
using UIKit;

namespace PropertyManager.iOS
{
	public class LauncherService : ILauncherService
	{
		public void LaunchWebUri(Uri uri)
		{
			UIApplication.SharedApplication.OpenUrl(NSUrl.FromString(uri.AbsoluteUri));
		}
	}
}

