using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Platform;
using Windows.UI.Xaml.Controls;
using MvvmCross.Platform;
using MvvmCross.WindowsUWP.Platform;
using PropertyManager.Services;
using PropertyManager.UWP.Services;

namespace PropertyManager.UWP
{
    public class Setup : MvxWindowsSetup
    {
        public Setup(Frame rootFrame) : base(rootFrame)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            Mvx.RegisterSingleton(typeof(IAuthenticationService), new AuthenticationService());
            return new PropertyManager.App();
        }

        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }
    }
}
