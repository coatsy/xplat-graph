using Windows.UI;
using Windows.UI.Xaml;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Platform;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
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
            var resources = Application.Current.Resources;
            var appView = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView();
            appView.TitleBar.BackgroundColor = (resources["BackgroundColorBrush"] as SolidColorBrush).Color;
            appView.TitleBar.InactiveBackgroundColor = (resources["BackgroundColorBrush"] as SolidColorBrush).Color;
            appView.TitleBar.ButtonBackgroundColor = (resources["BackgroundColorBrush"] as SolidColorBrush).Color;
            appView.TitleBar.ButtonHoverBackgroundColor = (resources["AccentColorBrush"] as SolidColorBrush).Color;
            appView.TitleBar.ButtonPressedBackgroundColor = (resources["DarkAccentColorBrush"] as SolidColorBrush).Color;
            appView.TitleBar.ButtonInactiveBackgroundColor = (resources["BackgroundColorBrush"] as SolidColorBrush).Color;
            appView.TitleBar.ButtonHoverForegroundColor = Colors.White;
            appView.TitleBar.ButtonPressedForegroundColor = Colors.White;
            (Window.Current.Content as Frame).Background = resources["BackgroundColorBrush"] as SolidColorBrush;

            Mvx.RegisterSingleton(typeof(ILauncherService), new LauncherService());
            Mvx.RegisterSingleton(typeof(IAuthenticationService), new AuthenticationService());
            return new PropertyManager.App();
        }

        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }
    }
}
