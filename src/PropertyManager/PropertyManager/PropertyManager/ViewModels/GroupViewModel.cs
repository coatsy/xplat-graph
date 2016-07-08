using System;
using System.Collections.Generic;
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
    public delegate void ConversationsChangedEventHandler(GroupViewModel sender);

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

        public PropertyTableRowModel Details { get; set; }

        public GroupModel Group { get; set; }

        public ObservableCollection<DriveItemModel> MediaFiles { get; set; }

        public ObservableCollection<DriveItemModel> DocumentFiles { get; set; }

        public ObservableCollection<ConversationModel> Conversations { get; set; }

        public ICommand GoBackCommand => new MvxCommand(() => Close(this));

        public ICommand SendMessageCommand => new MvxCommand(SendMessageAsync);

        public ICommand EditDetailsCommand => new MvxCommand(EditDetails);

        public event ConversationsChangedEventHandler ConversationsChanged;

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

        public override async void Start()
        {
            IsLoading = true;

            // Get datails.
            Details = _configService.DataFile.PropertyTable
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
            foreach (var conversation in conversations.Reverse())
            {
                Conversations.Add(conversation);
            }
            OnConversationsChanged();
        }

        public void LaunchDriveItemAsync(DriveItemModel driveItem)
        {
            _launcherService.LaunchWebUri(new Uri(driveItem.WebUrl));
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
                UniqueSenders = new List<string> {_configService.User.DisplayName}
            };
            Conversations.Add(newConversation);
            OnConversationsChanged();

            // Create the request object.
            var newThread = new NewConversationModel
            {
                Topic = "Property Manager",
                Posts = new List<NewPostModel>
                {
                    new NewPostModel
                    {
                        Body = new BodyModel(message, "html"),
                        NewParticipants = new List<ParticipantModel>
                        {
                            new ParticipantModel(_configService.User.DisplayName,
                                _configService.User.Mail)
                        }
                    }
                }
            };

            // Send the message.
            await _graphService.AddGroupConversation(Group, newThread);
            IsLoading = false;
        }

        private void EditDetails()
        {
            // Navigate to the details view.
            ShowViewModel<DetailsViewModel>(new { id = Group.Mail });
        }

        protected virtual void OnConversationsChanged()
        {
            ConversationsChanged?.Invoke(this);
        }
    }
}