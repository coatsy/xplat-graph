using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using PropertyManager.Models;

namespace PropertyManager.Services
{
    public class GraphService : IGraphService
    {
        private readonly IHttpService _httpService;

        private readonly IAuthenticationService _authenticationService;

        public string Endpoint => "https://graph.microsoft.com/beta/";

        public GraphService(IHttpService httpService,
            IAuthenticationService authenticationService)
        {
            _httpService = httpService;
            _authenticationService = authenticationService;
        }

        private async Task EnsureTokenIsPresentAsync()
        {
            // TODO: This is probably not a good pattern... fix this.
            // Not sure if ADAL is smart enough to just grap the valid tokens from
            // memory or not... 
            var authenticationResult = await GetAuthenticationResultAsync();
            _httpService.Endpoint = new Uri(Endpoint);
            _httpService.AccessToken = authenticationResult.AccessToken;
        }

        private async Task<AuthenticationResult> GetAuthenticationResultAsync()
        {
            AuthenticationResult authenticationResult;
            try
            {
                // Try to get the tokens silently.
                authenticationResult = await _authenticationService.AcquireTokenSilentAsync();
                return authenticationResult;
            }
            catch
            {
            }

            // Prompt the user.
            authenticationResult = await _authenticationService.AcquireTokenAsync();
            return authenticationResult;
        }

        public async Task<GroupModel[]> GetGroupsAsync()
        {
            await EnsureTokenIsPresentAsync();
            return (await _httpService.GetAsync<ResponseModel<GroupModel>>(
                "groups")).Value;
        }

        public async Task<GroupModel[]> GetUserGroupsAsync()
        {
            await EnsureTokenIsPresentAsync();
            return (await _httpService.GetAsync<ResponseModel<GroupModel>>(
                "me/memberOf")).Value;
        }

        public async Task<DriveItemModel[]> GetGroupDriveItemsAsync(GroupModel group)
        {
            await EnsureTokenIsPresentAsync();
            try
            {
                return (await _httpService.GetAsync<ResponseModel<DriveItemModel>>(
                    $"groups/{group.Id}/drive/root/children?select=id,name")).Value;
            }
            catch (HttpRequestException exception)
            {
                return new DriveItemModel[] {};
            }
        }
    }
}
