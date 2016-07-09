using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using Newtonsoft.Json.Linq;
using PropertyManager.Models;
using PropertyManager.Services;

namespace PropertyManager.ViewModels
{
    public class DetailsViewModel
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

        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        private string _streetName;

        public string StreetName
        {
            get { return _streetName; }
            set
            {
                _streetName = value;
                RaisePropertyChanged(() => StreetName);
            }
        }

        public PropertyTableRowModel Details { get; set; }

        public bool IsExisting { get; set; }

        public ICommand GoBackCommand => new MvxCommand(() => Close(this));

        public ICommand SaveDetailsCommand => new MvxCommand(SaveDetailsAsync);

        public DetailsViewModel(IGraphService graphService, IConfigService configService)
        {
            _graphService = graphService;
            _configService = configService;
        }

        public void Init(string id)
        {
            // Get details.
            Details = _configService.DataFile.PropertyTable
                .Rows.FirstOrDefault(r => r.Id == id);
            IsExisting = Details != null;

            // Set title.
            Title = (IsExisting ? "Add" : "Edit") + " a property";
            if (Details == null)
            {
                Details = new PropertyTableRowModel();
            }
        }

        public async void SaveDetailsAsync()
        {
            IsLoading = true;

            if (IsExisting)
            {
                // Calculate address (range).
                const int startRow = 2;
                var endRow = 2 + (_configService.DataFile.PropertyTable.Rows.Length - 1);
                var address = $"{Constants.DataFilePropertyTableColumnStart}{startRow}:" +
                              $"{Constants.DataFilePropertyTableColumnEnd}{endRow}";

                // Update the table row.
                await _graphService.UpdateTableRowsAsync(_configService.DataFile.DriveItem,
                    Constants.DataFileDataSheet, address, _configService.DataFile.PropertyTable.Rows
                        .Cast<TableRowModel>().ToArray(), _configService.AppGroup);
            }
            else
            {
                // Create property group.
                var mailNickname = new string(_streetName.ToCharArray()
                    .Where(char.IsLetterOrDigit)
                    .ToArray())
                    .ToLower();
                var propertyGroup = await _graphService.AddGroupAsync(GroupModel.CreateUnified(
                    StreetName, 
                    Details.Description, 
                    mailNickname));

                // Add the current user as a member of the app group.
                await _graphService.AddGroupUserAsync(propertyGroup, _configService.User);

                // We need the file storage to be ready in order to place any files.
                // Wait for it to be configured.
                await _graphService.WaitForGroupDriveAsync(propertyGroup);

                // Add details to data file.
                Details.Id = propertyGroup.Mail;
                await _graphService.AddTableRowAsync(_configService.DataFile.DriveItem,
                    Constants.DataFilePropertyTable, Details, _configService.AppGroup);

                // Add group and details to local config.
                _configService.Groups.Add(propertyGroup);
                _configService.DataFile.PropertyTable.AddRow(Details);
            }

            IsLoading = false;
            GoBackCommand.Execute(null);
        }
    }
}