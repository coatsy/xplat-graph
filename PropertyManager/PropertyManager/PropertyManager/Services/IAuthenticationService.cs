﻿using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace PropertyManager.Services
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResult> AcquireTokenAsync();

        Task<AuthenticationResult> AcquireTokenSilentAsync();
    }
}
