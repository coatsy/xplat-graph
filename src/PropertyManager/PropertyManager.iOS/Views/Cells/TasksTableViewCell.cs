using System;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using PropertyManager.Models;
using UIKit;

namespace PropertyManager.iOS
{
	public partial class TasksTableViewCell : MvxTableViewCell
	{
		public static readonly NSString Key = new NSString("TasksTableViewCell");
		public static readonly UINib Nib;

		static TasksTableViewCell()
		{
			Nib = UINib.FromName("TasksTableViewCell", NSBundle.MainBundle);
		}

		protected TasksTableViewCell(IntPtr handle) : base(handle)
		{
			return;
			this.DelayBind(() =>
			{
				// Create and apply the binding set.
				var set = this.CreateBindingSet<TasksTableViewCell, TaskModel>();
				set.Bind(TitleLabel).To(vm => vm.Title);
				set.Apply();
			});
		}
	}
}
