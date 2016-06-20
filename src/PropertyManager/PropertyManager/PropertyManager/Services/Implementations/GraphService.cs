using System;
using System.IO;
using System.Linq;
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
                // Ignored.
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
            catch (HttpRequestException)
            {
                return new DriveItemModel[] {};
            }
        }

        public async Task<ConversationModel[]> GetGroupConversationsAsync(GroupModel group)
        {
            await EnsureTokenIsPresentAsync();
            try
            {
                return (await _httpService.GetAsync<ResponseModel<ConversationModel>>(
                    $"groups/{group.Id}/conversations")).Value;
            }
            catch (HttpRequestException)
            {
                return new ConversationModel[] {};
            }
        }

        public async Task<DriveItemModel> AddGroupDriveItemAsync(GroupModel group, string name,
            Stream stream, string contentType)
        {
            await EnsureTokenIsPresentAsync();
            try
            {
                return (await _httpService.PutAsync<DriveItemModel>(
                    $"groups/{group.Id}/drive/root:/{name}:/content", stream, contentType));
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        public async Task<DriveItemModel[]> GetDriveItemsAsync()
        {
            await EnsureTokenIsPresentAsync();
            try
            {
                return (await _httpService.GetAsync<ResponseModel<DriveItemModel>>(
                    $"me/drive/special/approot/children?select=id,name")).Value;
            }
            catch (HttpRequestException)
            {
                return new DriveItemModel[] {};
            }
        }

        public async Task<DriveItemModel> CreateDriveItemAsync(string name, Stream stream, string contentType)
        {
            await EnsureTokenIsPresentAsync();
            try
            {
                return (await _httpService.PutAsync<DriveItemModel>(
                    $"me/drive/special/approot:/{name}:/content", stream, contentType));
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        public async Task<TableColumnModel[]> GetTableColumnsAsync(DriveItemModel driveItem, string tableName)
        {
            await EnsureTokenIsPresentAsync();
            try
            {
                return (await _httpService.GetAsync<ResponseModel<TableColumnModel>>(
                    $"me/drive/items/{driveItem.Id}/workbook/tables/{tableName}/columns")).Value;
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        public async Task<TableRowModel> AddTableRowAsync(DriveItemModel driveItem, string tableName,
            TableRowModel tableRow)
        {
            await EnsureTokenIsPresentAsync();
            try
            {
                return (await _httpService.PostAsync<TableRowsModel>(
                    $"me/drive/items/{driveItem.Id}/workbook/tables/{tableName}/rows",
                    new TableRowsModel
                    {
                        Values = new[] {tableRow}
                    })).Values.FirstOrDefault();
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        public async Task<TableRowsModel> UpdateTableRowAsync(DriveItemModel driveItem, string sheetName,
            string address, TableRowModel tableRow)
        {
            await EnsureTokenIsPresentAsync();
            try
            {
                return (await _httpService.PatchAsync<TableRowsModel>(
                    $"me/drive/items/{driveItem.Id}/workbook/worksheets/{sheetName}/range(address='{sheetName}!{address}')",
                    new TableRowsModel
                    {
                        Values = new[] {tableRow}
                    }));
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }
    }
}