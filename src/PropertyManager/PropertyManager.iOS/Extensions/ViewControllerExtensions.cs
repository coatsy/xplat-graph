using System;
using MvvmCross.iOS.Views;
using UIKit;

namespace PropertyManager.iOS
{
	public static class ViewControllerExtensions
	{
		public static void SetNavigationBarStyle(this MvxViewController viewController)
		{
			viewController.NavigationController.NavigationBar.TintColor = UIColor.White;
			viewController.NavigationController.NavigationBar.BarTintColor = UIColor.FromRGB(0, 120, 215);
			viewController.NavigationController.NavigationBar.BackgroundColor = UIColor.FromRGB(0, 120, 215);
			viewController.NavigationController.NavigationBar.BarStyle = UIBarStyle.Black;
			viewController.NavigationController.NavigationBar.Translucent = false;
			viewController.NavigationController.NavigationBar.Layer.BorderWidth = 0;
			viewController.NavigationController.NavigationBar.Layer.BorderColor = UIColor.Clear.CGColor;
			viewController.NavigationController.NavigationBar.SetBackgroundImage(new UIImage(), UIBarPosition.Any, UIBarMetrics.Default);
			viewController.NavigationController.NavigationBar.ShadowImage = new UIImage();
			UINavigationBar.Appearance.SetTitleTextAttributes(new UITextAttributes
			{
				TextColor = UIColor.White
			});
		}

		public static void ShowNavigationBar(this MvxViewController viewController)
		{
			viewController.NavigationController.SetNavigationBarHidden(false, false);
		}

		public static void HideNavigationBar(this MvxViewController viewController)
		{
			viewController.NavigationController.SetNavigationBarHidden(true, false);
		}
	}
}