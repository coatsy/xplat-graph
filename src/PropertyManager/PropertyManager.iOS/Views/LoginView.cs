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

			// Hide navigation bar.
			NavigationController.SetNavigationBarHidden(true, false);

			// Create binding set.
			var set = this.CreateBindingSet<LoginView, LoginViewModel>();

			// Create SignInButton bindings.
			set.Bind(SignInButton)
			   .To(vm => vm.LoginCommand);
			set.Bind(SignInButton)
			   .For("Visibility")
			   .To(vm => vm.IsLoading)
			   .WithConversion("InvertedVisibility");

			// Create ActivityIndicator bindings.
			set.Bind(ActivityIndicator)
			   .For("Visibility")
			   .To(vm => vm.IsLoading)
			   .WithConversion("Visibility");

			// Apply bindings.
			set.Apply();
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}


