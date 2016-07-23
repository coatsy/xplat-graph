using UIKit;
using CoreGraphics;
using PropertyManager.Services;

namespace PropertyManager.iOS
{
	public class ProgressDialogHandle : IDialogHandle
	{
		public UIAlertController AlertController { get; }

		public ProgressDialogHandle(string title, string message)
		{
			// Create alert view controller.
			AlertController = UIAlertController.Create(title, message + "\n\n\n\n", UIAlertControllerStyle.Alert);
			var activityIndicator = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.WhiteLarge);
			activityIndicator.Center = new CGPoint(130.5, 120);
			activityIndicator.Color = UIColor.Black;
			activityIndicator.StartAnimating();

			// Add activity indicator.
			AlertController.View.AddSubview(activityIndicator);

			var viewController = UIApplication.SharedApplication.KeyWindow.RootViewController;
			viewController.PresentViewController(AlertController, true, null);
		}

		public void Close()
		{
			AlertController.DismissViewController(true, null);
		}
	}
}

