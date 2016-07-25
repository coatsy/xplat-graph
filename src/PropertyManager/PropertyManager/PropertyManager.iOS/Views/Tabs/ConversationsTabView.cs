﻿using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
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

			// Add right navigation bar item.
			var rightNavigationButton = new UIBarButtonItem(UIBarButtonSystemItem.Add, async (sender, e) =>
			{
				var result = await this.GetTextFromAlertAsync("New Message", null, "Type a message...");
				if (result != null)
				{
					viewModel.ConversationText = result;
					viewModel.AddConversationCommand.Execute(null);
				}
			});
			NavigationItem.RightBarButtonItem = rightNavigationButton;

			// Create the table view source.
			var source = new ConversationsTabViewSource(TableView, viewModel);

			// Create and apply the binding set.
			var set = this.CreateBindingSet<ConversationsTabView, GroupViewModel>();
			set.Bind(source).To(vm => vm.Conversations);
			set.Apply();

			// Set the table view source and refresh.
			TableView.Source = source;
			TableView.ReloadData();
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}


