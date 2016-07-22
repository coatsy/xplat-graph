using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using PropertyManager.Services;

namespace PropertyManager
{
    public class App : MvvmCross.Core.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            Mvx.RegisterSingleton(typeof(IHttpService), typeof(HttpService));
            Mvx.RegisterSingleton(typeof(IGraphService), typeof(GraphService));
			Mvx.RegisterSingleton(typeof(IConfigService), new ConfigService());
            RegisterAppStart<ViewModels.LoginViewModel>();
        }
    }
}
