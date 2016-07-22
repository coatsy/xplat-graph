using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Platform;
using MvvmCross.iOS.Views.Presenters;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using UIKit;
using PropertyManager.ViewModels;
using PropertyManager.Services;
using System.Diagnostics;

namespace PropertyManager.iOS
{
    public class Setup : MvxIosSetup
    {
        public Setup(MvxApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
        {
        }
        
        public Setup(MvxApplicationDelegate applicationDelegate, IMvxIosViewPresenter presenter)
            : base(applicationDelegate, presenter)
        {
        }

        protected override IMvxApplication CreateApp()
        {
			// Register platform services.
			Mvx.RegisterSingleton(typeof(IAuthenticationService), new AuthenticationService());
			Mvx.RegisterSingleton(typeof(ILauncherService), new LauncherService());
			Mvx.RegisterSingleton(typeof(IFilePickerService), new FilePickerService());
			Mvx.RegisterSingleton(typeof(IDialogService), new DialogService());
            return new App();
        }
        
        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }
    }
}
