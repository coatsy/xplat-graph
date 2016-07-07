using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

        private string _message;

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                RaisePropertyChanged(() => Message);
            }
        }

        public PropertyTableRowModel PropertyData { get; set; }

        public GroupModel Group { get; set; }

        public ObservableCollection<DriveItemModel> MediaFiles { get; set; }

        public ObservableCollection<DriveItemModel> DocumentFiles { get; set; }

        public ObservableCollection<ConversationModel> Conversations { get; set; }

        public ICommand GoBackCommand => new MvxCommand(() => Close(this));

        public ICommand SaveDetailsCommand => new MvxCommand(SaveDetailsAsync);

        public ICommand SendMessageCommand => new MvxCommand(SendMessageAsync);

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
            // Deserialize the group.
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

        public void LaunchDriveItemAsync(DriveItemModel driveItem)
        {
            _launcherService.LaunchWebUri(new Uri(driveItem.WebUrl));
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

        private async void SendMessageAsync()
        {
            IsLoading = true;

            // Reset the text box.
            var message = Message;
            Message = "";

            // Create a local message entry and add it.
            var newConversation = new ConversationModel
            {
                Preview = message,
                UniqueSenders = new List<string> { _configService.User.DisplayName }
            };

            if (Conversations.Any())
            {
                Conversations.Insert(0, newConversation);
            }
            else
            {
                Conversations.Add(newConversation);
            }

            // Create the request object.
            var newThread = new NewConversationModel
            {
                Topic = "Property Manager",
                Posts = new List<NewPostModel>
                {
                    new NewPostModel
                    {
                        Body = new BodyModel
                        {
                            Content = message,
                            ContentType = "html"
                        },
                        NewParticipants = new List<ParticipantModel>
                        {
                            new ParticipantModel
                            {
                                EmailAddress = new EmailAddressModel
                                {
                                    Name = _configService.User.DisplayName,
                                    Address = _configService.User.Mail
                                }
                            }
                        }
                    }
                }
            };

            // Send the message.
            await _graphService.AddGroupConversation(Group, newThread);
            IsLoading = false;
        }
    }
}