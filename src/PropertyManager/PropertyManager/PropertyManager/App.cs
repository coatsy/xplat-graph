using MvvmCross.Platform;
using PropertyManager.Services;

namespace PropertyManager
{
    public class App : MvvmCross.Core.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            Mvx.RegisterType(typeof(IHttpService), typeof(HttpService));
            Mvx.RegisterType(typeof(IGraphService), typeof(GraphService));
            RegisterAppStart<ViewModels.LoginViewModel>();
        }
    }
}
