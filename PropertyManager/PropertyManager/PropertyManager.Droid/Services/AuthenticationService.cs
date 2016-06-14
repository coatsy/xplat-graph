using System;
using System.Threading.Tasks;
using Android.App;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using PropertyManager.Services;

namespace PropertyManager.Droid.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        public Activity CallerActivity { get; set; }

        public string Authority => "https://login.microsoftonline.com/simonj.onmicrosoft.com";

        public string Resource => "https://graph.microsoft.com/";

        public string ClientId => "fdeaed4c-1bf1-4431-9033-3ef270889eb5";

        public Uri RedirectUri => new Uri("https://propertymanager");

        public async Task<AuthenticationResult> AcquireTokenAsync()
        {
            if (CallerActivity == null)
            {
                throw new Exception("Caller Activity must be set.");
            }

            // Create the authentication context.
            var authenticationContext = new AuthenticationContext(Authority);

            // Create the platform parameters.
            var platformParameters = new PlatformParameters(CallerActivity);

            // Authenticate the user.
            var authenticationResult = await authenticationContext.AcquireTokenAsync(Resource,
                ClientId, RedirectUri, platformParameters);
            return authenticationResult;
        }
    }
}
