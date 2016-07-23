using System;
using System.Linq;
using MvvmCross.iOS.Views;
using PropertyManager.ViewModels;

namespace PropertyManager.iOS
{
	public partial class GroupView : MvxTabBarViewController<GroupViewModel>
	{
		private bool _viewConstructed;

		public GroupView()
		{
			// The ViewDidLoad method is called from the UIKit base constructor before 
			// the class constructor. This makes the ViewModel property inaccessible during
			// ViewDidLoad for MvxTabBarViewController. Workaround is to push logic to class
			// constructor. Shown at: https://github.com/MvvmCross/NPlus1DaysOfMvvmCross/blob/976ede3aafd3a7c6e06717ee48a9a45f08eedcd0/N-25-Tabbed/Tabbed.Touch/Views/FirstView.cs#L17
			_viewConstructed = true;
			ViewDidLoad();
		}

		public override void ViewDidLoad()
		{
			if (!_viewConstructed) 
			{
				return;
			}

			base.ViewDidLoad();

			// Create and set tabs.
			var viewControllers = new MvxViewController[]
			{
				CreateTab<DetailsTabView>("Details"),
				CreateTab<ConversationsTabView>("Conversations"),
				CreateTab<FilesTabView>("Files"),
				CreateTab<TasksTabView>("Tasks")
			};
			ViewControllers = viewControllers;
			SelectedViewController = ViewControllers.First();
		}

		private MvxViewController CreateTab<T>(string title) where T : MvxViewController
		{
			// Create view controller.
			var viewController = Activator.CreateInstance(typeof(T)) as T;
			viewController.Title = title;
			viewController.ViewModel = ViewModel;
			return viewController;
		}

		public override void ViewWillAppear(bool animated)
		{
			//ViewModel.OnResume();
			base.ViewWillAppear(animated);
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}


