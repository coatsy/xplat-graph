using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using PropertyManager.ViewModels;

namespace PropertyManager.iOS.Views
{
    public partial class FirstView : MvxViewController
    {
        public FirstView() : base("FirstView", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var set = this.CreateBindingSet<FirstView, LoginViewModel>();
            //set.Bind(Label).To(vm => vm.Hello);
            set.Bind(TextField).To(vm => vm.IsLoading);
            set.Apply();
        }
    }
}
