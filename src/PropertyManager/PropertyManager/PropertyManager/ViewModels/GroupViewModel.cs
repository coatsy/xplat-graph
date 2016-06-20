using System.Collections.ObjectModel;
using System.Linq;
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

        public ObservableCollection<ConversationModel> Conversations { get; set; }

        public ICommand GoBackCommand => new MvxCommand(() => Close(this));

        public GroupViewModel(IGraphService graphService)
        {
            _graphService = graphService;
            Conversations = new ObservableCollection<ConversationModel>();
        }

        public async void Init(string data)
        {
            IsLoading = true;
            var group = JsonConvert.DeserializeObject<GroupModel>(data);
            Group = group;

            // Get conversations.
            var conversations = await _graphService.GetGroupConversationsAsync(Group);
            foreach (var conversation in conversations.Reverse())
            {
                Conversations.Add(conversation);
            }
            IsLoading = false;
        }
    }
}
