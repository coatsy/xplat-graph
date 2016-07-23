﻿using System;
using MvvmCross.iOS.Views;
using PropertyManager.ViewModels;
using UIKit;

namespace PropertyManager.iOS
{
	public partial class DetailsView : MvxViewController<DetailsViewModel>
	{
		public DetailsView() : base("DetailsView", null)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			// Set navigation bar style.
			this.SetNavigationBarStyle();
		}

		public override void ViewWillAppear(bool animated)
		{
			ViewModel.OnResume();
			base.ViewWillAppear(animated);
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}


