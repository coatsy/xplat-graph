using MvvmCross.Core.ViewModels;
using PropertyManager.Services;

namespace PropertyManager.ViewModels
{
    public class FirstViewModel 
        : MvxViewModel
    {
        private string _hello = "Hello MvvmCross";
        public string Hello
        { 
            get { return _hello; }
            set { SetProperty (ref _hello, value); }
        }

        public FirstViewModel(IAuthenticationService authenticationService)
        {
            authenticationService.AcquireTokenAsync();
        }
    }
}
