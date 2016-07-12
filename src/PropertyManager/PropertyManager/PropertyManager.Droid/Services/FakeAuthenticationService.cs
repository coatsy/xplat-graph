using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using PropertyManager.Services;

namespace PropertyManager.Droid.Services
{
    class FakeAuthenticationService : IAuthenticationService
    {
        public Task<AuthenticationResult> AcquireTokenAsync()
        {
            return null;
        }

        public Task<AuthenticationResult> AcquireTokenSilentAsync()
        {
            return null;
        }
    }
}