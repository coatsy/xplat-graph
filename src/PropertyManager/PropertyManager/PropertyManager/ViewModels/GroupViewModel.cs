using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using Newtonsoft.Json;
using PropertyManager.Models;
using PropertyManager.Services;

namespace PropertyManager.ViewModels
{
    public class GroupViewModel
        : MvxViewModel
    {
        private readonly IGraphService _graphService;
        private readonly IConfigService _configService;
        private readonly ILauncherService _launcherService;

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

        public PropertyTableRowModel PropertyData { get; set; }

        public GroupModel Group { get; set; }

        public ObservableCollection<DriveItemModel> MediaFiles { get; set; }

        public ObservableCollection<DriveItemModel> DocumentFiles { get; set; }

        public ObservableCollection<ConversationModel> Conversations { get; set; }

        public ICommand GoBackCommand => new MvxCommand(() => Close(this));

        public ICommand SaveDetailsCommand => new MvxCommand(SaveDetailsAsync);

        public GroupViewModel(IGraphService graphService, IConfigService configService,
            ILauncherService launcherService)
        {
            _graphService = graphService;
            _configService = configService;
            _launcherService = launcherService;
            MediaFiles = new ObservableCollection<DriveItemModel>();
            DocumentFiles = new ObservableCollection<DriveItemModel>();
            Conversations = new ObservableCollection<ConversationModel>();
        }

        public void Init(string groupData)
        {
            // Deserialize Group.
            var group = JsonConvert.DeserializeObject<GroupModel>(groupData);
            Group = group;
        }

        public async override void Start()
        {
            IsLoading = true;

            // Get property data.
            PropertyData = _configService.DataFile.PropertyTable
                .Rows.FirstOrDefault(r => r.Id == Group.Mail);

            // Update the rest of the data.
            await Task.WhenAll(UpdateDriveItemsAsync(), UpdateConversationsAsync());
            IsLoading = false;
            base.Start();
        }

        private async Task UpdateDriveItemsAsync()
        {
            var driveItems = await _graphService.GetGroupDriveItemsAsync(Group);
            foreach (var driveItem in driveItems)
            {
                if (Constants.MediaFileExtensions.Any(e => driveItem.Name.Contains(e)))
                {
                    MediaFiles.Add(driveItem);
                }
                else if (Constants.DocumentFileExtensions.Any(e => driveItem.Name.Contains(e)))
                {
                    DocumentFiles.Add(driveItem);
                }
            }
        }

        private async Task UpdateConversationsAsync()
        {
            var conversations = await _graphService.GetGroupConversationsAsync(Group);
            foreach (var conversation in conversations)
            {
                Conversations.Add(conversation);
            }
        }

        private async void SaveDetailsAsync()
        {
            IsLoading = true;

            // Calculate address (range).
            const int startRow = 2;
            var endRow = 2 + (_configService.DataFile.PropertyTable.Rows.Length - 1);
            var address = $"{Constants.DataFilePropertyTableColumnStart}{startRow}:" +
                          $"{Constants.DataFilePropertyTableColumnEnd}{endRow}";

            // Update the table row.
            await _graphService.UpdateTableRowsAsync(_configService.DataFile.DriveItem,
                Constants.DataFileDataSheet,
                address, _configService.DataFile.PropertyTable.Rows
                    .Cast<TableRowModel>().ToArray(), _configService.AppGroup);

            IsLoading = false;
        }

        public void LaunchDriveItemAsync(DriveItemModel driveItem)
        {
            _launcherService.LaunchWebUri(new Uri(driveItem.WebUrl));
        }
    }
}