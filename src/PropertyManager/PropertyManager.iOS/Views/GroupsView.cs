using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.iOS.Views;
using PropertyManager.ViewModels;
using UIKit;

namespace PropertyManager.iOS
{
	public partial class GroupsView : MvxViewController<GroupsViewModel>
	{
		public GroupsView() : base("GroupsView", null)
		{
			Title = "Properties";
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			// Show the navigation bar.
			NavigationController.SetNavigationBarHidden(false, false);

			// Add right navigation bar item.
			var addPropertyButton = new UIBarButtonItem(UIBarButtonSystemItem.Add,
														(sender, e) => ViewModel.AddPropertyCommand.Execute(null));
			NavigationItem.RightBarButtonItem = addPropertyButton;

			// Configure the search bar button event handler.
			SearchBar.SearchButtonClicked += (sender, e) => ViewModel.FilterGroupsCommand.Execute(null);

			// Create the table view source.
			var source = new MvxSimpleTableViewSource(TableView, GroupsTableViewCell.Key, GroupsTableViewCell.Key);

			// Create and apply the binding set.
			var set = this.CreateBindingSet<GroupsView, GroupsViewModel>();
			set.Bind(source).To(vm => vm.FilteredGroups);
			set.Bind(SearchBar).For(sb => sb.Text).To(vm => vm.Query);
			set.Apply();

			// Set the table view source and refresh.
			TableView.Source = source;
			TableView.RowHeight = 65;
			TableView.ReloadData();
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}


