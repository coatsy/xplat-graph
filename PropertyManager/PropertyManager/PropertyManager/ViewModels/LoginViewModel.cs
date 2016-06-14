using System.Windows.Input;
using MvvmCross.Core.ViewModels;
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
        }

        private async void LoginAsync()
        {
            IsLoading = true;
            var d = await _graphService.GetGroupsAsync();
            var p = await _graphService.GetUserGroupsAsync();
        }
    }
}
