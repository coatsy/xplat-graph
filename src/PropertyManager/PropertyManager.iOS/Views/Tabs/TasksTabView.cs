﻿using System;
using CoreGraphics;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.iOS.Views;
using PropertyManager.ViewModels;
using UIKit;

namespace PropertyManager.iOS
{
	public partial class TasksTabView : MvxViewController
	{
		void HandleAction()
		{

		}

		public TasksTabView() : base("TasksTabView", null)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var viewModel = ViewModel as GroupViewModel;

			// Set view title and prompt.
			NavigationItem.Title = "Tasks";
			NavigationItem.Prompt = viewModel.Group.DisplayName;

			// Add left navigation bar item.
			var leftNavigationButton = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, (sender, e) =>
															viewModel.GoBackCommand.Execute(null));
			NavigationItem.LeftBarButtonItem = leftNavigationButton;

			// Add right navigation bar item.
			var rightNavigationButton = new UIBarButtonItem(UIBarButtonSystemItem.Add, async (sender, e) =>
			{
				var result = await this.GetTextFromAlertAsync("New Task", null, "Type a task...");
				if (result != null) {
					viewModel.TaskText = result;
					viewModel.AddTaskCommand.Execute(null);
				}
			});
			NavigationItem.RightBarButtonItem = rightNavigationButton;

			// Register collection changed handler.
			viewModel.TasksChanged += (sender) =>
				TableView.SetContentOffset(new CGPoint(0, nfloat.MaxValue), true);

			// Create the table view source.
			var source = new MvxSimpleTableViewSource(TableView, TasksTableViewCell.Key, TasksTableViewCell.Key);

			// Create and apply the binding set.
			var set = this.CreateBindingSet<TasksTabView, GroupViewModel>();
			set.Bind(source).To(vm => vm.Tasks);
			set.Bind(source).For(s => s.SelectionChangedCommand).To(vm => vm.TaskClickCommand);
			set.Apply();

			// Set the table view source and refresh.
			TableView.Source = source;
			TableView.RowHeight = 60;
			TableView.ReloadData();

		
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}


