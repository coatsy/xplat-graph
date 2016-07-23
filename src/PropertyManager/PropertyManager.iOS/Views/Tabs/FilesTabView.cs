using MvvmCross.iOS.Views;
using PropertyManager.ViewModels;

namespace PropertyManager.iOS
{
	public partial class FilesTabView : MvxViewController
	{
		public FilesTabView() : base("FilesTabView", null)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}


