using System;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using PropertyManager.Services;

namespace PropertyManager.iOS
{
	public class AuthenticationService : IAuthenticationService
	{
		public Task<AuthenticationResult> AcquireTokenAsync()
		{
			throw new NotImplementedException();
		}

		public Task<AuthenticationResult> AcquireTokenSilentAsync()
		{
			throw new NotImplementedException();
		}
	}
}

