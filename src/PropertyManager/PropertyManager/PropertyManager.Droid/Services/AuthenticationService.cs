﻿using System;
using System.Threading.Tasks;
using Android.App;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using PropertyManager.Services;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;
using static Android.Provider.Settings;
using Android.Telephony;
using Android.Content;
using System.Linq;

namespace PropertyManager.Droid.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private void SaveCurrentUUID(string uuid)
        {
            var preferences = Application.Context.GetSharedPreferences("PM",
                      FileCreationMode.Private);
            var editor = preferences.Edit();
            editor.PutString("UUID", uuid);
            editor.Commit();
        }

        private string GetCurrentUUID()
        {
            var preferences = Application.Context.GetSharedPreferences("PM",
                FileCreationMode.Private);
            return preferences.GetString("UUID", null);
        }

        public string Authority => "https://login.microsoftonline.com/simonj.onmicrosoft.com";

        public string Resource => "https://graph.microsoft.com/";

        public string ClientId => "fdeaed4c-1bf1-4431-9033-3ef270889eb5";

        public Uri RedirectUri => new Uri("https://propertymanager");

        public async Task<AuthenticationResult> AcquireTokenAsync()
        {
            // Get the top activity.
            var topActivity = Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;

            if (topActivity == null)
            {
                throw new Exception("Could not find the top Activity.");
            }

            // Clear the cache.
            TokenCache.DefaultShared.Clear();

            // Create the authentication context.
            var authenticationContext = new AuthenticationContext(Authority);

            // Create the platform parameters.
            var platformParameters = new PlatformParameters(topActivity);
            try
            {
                // Authenticate the user.
                var authenticationResult = await authenticationContext.AcquireTokenAsync(Resource,
                    ClientId, RedirectUri, platformParameters);

                // Naively store the unique user id.
                SaveCurrentUUID(authenticationResult.UserInfo.UniqueId);
                return authenticationResult;
            }
            catch (AdalException ex)
            {
                if (ex.ErrorCode == AdalError.AuthenticationCanceled)
                {
                    return null;
                }
                throw;
            }
        }

        public async Task<AuthenticationResult> AcquireTokenSilentAsync()
        {
            // Create the authentication context.
            var authenticationContext = new AuthenticationContext(Authority);

            // Try to get a unique user id.
            var uuid = GetCurrentUUID();

            // Authenticate the user.
            var authenticationResult = await authenticationContext.AcquireTokenSilentAsync(
                Resource, ClientId, new UserIdentifier(uuid, UserIdentifierType.UniqueId));
            return authenticationResult;
        }
    }
}
