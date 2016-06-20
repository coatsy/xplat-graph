using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using Newtonsoft.Json;
using PropertyManager.Models;
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
            IsLoading = true;

            // If the backing Excel file doesn't exist, create it.
            var driveItems = (await _graphService.GetDriveItemsAsync()).ToList();
            if (!driveItems.Any(f => f.Name.Equals(Constants.ExcelFileName)))
            {
                var excelFile = await CreateExcelDataFileAsync();
                if (excelFile == null)
                {
                    // TODO: Handle error.
                }
                driveItems.Add(excelFile);
            }

            // Get groups and filter them. We need to make sure
            // the group can be accessed by the user.
            var allGroups = await _graphService.GetGroupsAsync();
            var userGroups = await _graphService.GetUserGroupsAsync();
            var groups = userGroups.Where(ug =>
                allGroups.Any(ag => ug.Id == ag.Id)).ToArray();

            // Navigate to groups view.
            var data = JsonConvert.SerializeObject(groups);
            ShowViewModel<GroupsViewModel>(new { data });
            IsLoading = false;
        }

        public async Task<DriveItemModel> CreateExcelDataFileAsync()
        {
            var assembly = typeof(App).GetTypeInfo().Assembly;
            using (var stream = assembly.GetManifestResourceStream(Constants.ExcelFileResourceName))
            {
                var file = await _graphService.CreateDriveItemAsync(Constants.ExcelFileName,
                    stream, Constants.ExcelContentType);
                return file;
            }
        }
    }
}
