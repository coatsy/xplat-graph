using Android.Content;
using MvvmCross.Droid.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform;
using PropertyManager.Services;
using PropertyManager.Droid.Services;
using System;
using System.Collections.Generic;
using MvvmCross.Plugins.Visibility;

namespace PropertyManager.Droid
{
    public class Setup : MvxAndroidSetup
    {
        protected override IEnumerable<Type> ValueConverterHolders
        {
            get
            {
                return new List<Type>
                {
                    typeof(MvxVisibilityValueConverter),
                    typeof(MvxInvertedVisibilityValueConverter),
                };
            }
        }

        public Setup(Context applicationContext) : base(applicationContext)
        {
        }

        protected override IMvxApplication CreateApp()
        {            
            // Register platform services.
            Mvx.RegisterSingleton(typeof(IAuthenticationService), new AuthenticationService());
            Mvx.RegisterSingleton(typeof(ILauncherService), new FakeLauncherService());
            Mvx.RegisterSingleton(typeof(IFilePickerService), new FakeFilePickerService());
            return new App();
        }

        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }
    }
}
