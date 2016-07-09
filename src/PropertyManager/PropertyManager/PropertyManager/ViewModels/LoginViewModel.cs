using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PropertyManager.Models;
using PropertyManager.Services;

namespace PropertyManager.ViewModels
{
    public class LoginViewModel
        : MvxViewModel
    {
        private readonly IGraphService _graphService;
        private readonly IConfigService _configService;

        private bool _isLoading;

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                RaisePropertyChanged(() => IsLoading);
            }
        }

        public ICommand LoginCommand => new MvxCommand(LoginAsync);

        public LoginViewModel(IGraphService graphService, IConfigService configService)
        {
            _graphService = graphService;
            _configService = configService;
            LoginCommand.Execute(null);
        }

        private async void LoginAsync()
        {
            IsLoading = true;

            // Get the current user and all of the groups.
            var user = await _graphService.GetUserAsync();
            var allGroups = await _graphService.GetGroupsAsync();

            // Get the group belonging to this app.
            var appGroup = allGroups.FirstOrDefault(g => g.Mail.StartsWith(
                Constants.AppGroupMail + "@"));

            // If the app group doesn't exist, create it.
            if (appGroup == null)
            {
                appGroup = await _graphService.AddGroupAsync(GroupModel.CreateUnified(
                    Constants.AppGroupDisplayName, 
                    Constants.AppGroupDescription, 
                    Constants.AppGroupMail));

                // Add the current user as a member of the app group.
                await _graphService.AddGroupUserAsync(appGroup, _configService.User);

                // We need the file storage to be ready in order to place the data file. 
                // Wait for it to be configured.
                await _graphService.WaitForGroupDriveAsync(appGroup);
            }

            // Add the current user as a member of the app group.
            var appGroupUsers = await _graphService.GetGroupUsersAsync(appGroup);
            if (appGroupUsers.All(u => u.UserPrincipalName != user.UserPrincipalName))
            {
                await _graphService.AddGroupUserAsync(appGroup, user);
            }

            // Get the app group files and the property data file.
            var appGroupDriveItems = await _graphService.GetGroupDriveItemsAsync(appGroup);
            var dataDriveItem = appGroupDriveItems.FirstOrDefault(i => i.Name.Equals(
                Constants.DataFileName));

            // If the data file doesn't exist, create it.
            if (dataDriveItem == null)
            {
                // Get the data file template from the resources.
                var assembly = typeof (App).GetTypeInfo().Assembly;
                using (var stream = assembly.GetManifestResourceStream(Constants.DataFileResourceName))
                {
                    dataDriveItem = await _graphService.AddGroupDriveItemAsync(appGroup,
                        Constants.DataFileName, stream, Constants.ExcelContentType);
                }

                if (dataDriveItem == null)
                {
                    throw new Exception("Could not create the property data file in the group.");
                }
            }

            // Get the property table.
            var propertyTable = await _graphService.GetTableAsync<PropertyTableRowModel>(
                dataDriveItem, Constants.DataFilePropertyTable, appGroup);

            // Create the data file represenation.
            var dataFile = new DataFileModel
            {
                DriveItem = dataDriveItem,
                PropertyTable = propertyTable
            };

            // Get groups that the user is a member of and represents 
            // a property.
            var userGroups = await _graphService.GetUserGroupsAsync();
            var propertyGroups = userGroups
                .Where(g => propertyTable["Id"]
                    .Values.Any(v => v.Any() &&
                                     v[0].Type == JTokenType.String &&
                                     v[0].Value<string>().Equals(g.Mail,
                                         StringComparison.OrdinalIgnoreCase)))
                .ToArray();

            // Set (singleton) config.
            _configService.User = user;
            _configService.AppGroup = appGroup;
            _configService.Groups = new List<GroupModel>(propertyGroups);
            _configService.DataFile = dataFile;

            // Navigate to the groups view.
            ShowViewModel<GroupsViewModel>();
            IsLoading = false;
        }
    }
}
