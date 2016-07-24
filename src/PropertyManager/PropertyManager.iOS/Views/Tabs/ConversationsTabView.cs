﻿using MvvmCross.iOS.Views;
using PropertyManager.ViewModels;
using UIKit;

namespace PropertyManager.iOS
{
	public partial class ConversationsTabView : MvxViewController
	{
		public ConversationsTabView() : base("ConversationsTabView", null)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var viewModel = ViewModel as GroupViewModel;

			// Set view title and prompt.
			NavigationItem.Title = "Conversations";
			NavigationItem.Prompt = viewModel.Group.DisplayName;

			// Add left navigation bar item.
			var leftNavigationButton = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, (sender, e) =>
															viewModel.GoBackCommand.Execute(null));
			NavigationItem.LeftBarButtonItem = leftNavigationButton;
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}

