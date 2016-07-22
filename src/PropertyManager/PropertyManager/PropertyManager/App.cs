using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using PropertyManager.Services;

namespace PropertyManager
{
    public class App : MvvmCross.Core.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
			
			Mvx.LazyConstructAndRegisterSingleton<IHttpService, HttpService>();
			Mvx.LazyConstructAndRegisterSingleton<IConfigService, ConfigService>();
			Mvx.LazyConstructAndRegisterSingleton<IGraphService, GraphService>();
            RegisterAppStart<ViewModels.LoginViewModel>();
        }
    }
}
