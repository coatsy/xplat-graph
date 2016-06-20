using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
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

        public GroupModel Group { get; set; }

        public ObservableCollection<DriveItemModel> MediaFiles { get; set; }

        public ObservableCollection<DriveItemModel> DocumentFiles { get; set; }

        public ObservableCollection<ConversationModel> Conversations { get; set; }

        public ICommand GoBackCommand => new MvxCommand(() => Close(this));

        public GroupViewModel(IGraphService graphService)
        {
            _graphService = graphService;
            MediaFiles = new ObservableCollection<DriveItemModel>();
            DocumentFiles = new ObservableCollection<DriveItemModel>();
            Conversations = new ObservableCollection<ConversationModel>();
        }

        public async void Init(string data)
        {
            IsLoading = true;
            var group = JsonConvert.DeserializeObject<GroupModel>(data);
            Group = group;

            await Task.WhenAll(UpdateDriveItemsAsync(), UpdateConversationsAsync());
            IsLoading = false;
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
    }
}
