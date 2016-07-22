using System;
using MvvmCross.iOS.Views;
using UIKit;
using PropertyManager.ViewModels;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platform;
using PropertyManager.Services;

namespace PropertyManager.iOS
{
	public partial class LoginView : MvxViewController<LoginViewModel>
	{
		public LoginView() : base("LoginView", null)
		{
			
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			// Hide the navigation bar.
			NavigationController.SetNavigationBarHidden(true, false);

			// Create and apply the binding set.
			var set = this.CreateBindingSet<LoginView, LoginViewModel>();
			set.Bind(SignInButton).To(vm => vm.LoginCommand);
			set.Bind(SignInButton).For("Visibility").To(vm => vm.IsLoading).WithConversion("InvertedVisibility");
			set.Bind(ActivityIndicator).For("Visibility").To(vm => vm.IsLoading).WithConversion("Visibility");
			set.Apply();
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}


