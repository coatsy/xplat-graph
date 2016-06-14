using System.Linq;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using Newtonsoft.Json;
using PropertyManager.Services;

namespace PropertyManager.ViewModels
{
    public class LoginViewModel
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

        public ICommand LoginCommand => new MvxCommand(LoginAsync);

        public LoginViewModel(IGraphService graphService)
        {
            _graphService = graphService;
            LoginCommand.Execute(null);
        }

        private async void LoginAsync()
        {
            // Get groups and filter them. We need to make sure
            // the group can be accessed by the user.
            IsLoading = true;
            var allGroups = await _graphService.GetGroupsAsync();
            var userGroups = await _graphService.GetUserGroupsAsync();
            var groups = userGroups.Where(ug =>
                allGroups.Any(ag => ug.Id == ag.Id)).ToArray();

            // Navigate to groups view.
            var data = JsonConvert.SerializeObject(groups);
            ShowViewModel<GroupsViewModel>(new { data });
            IsLoading = false;
        }
    }
}
