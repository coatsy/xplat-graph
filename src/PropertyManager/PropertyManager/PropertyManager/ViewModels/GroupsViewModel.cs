using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using Newtonsoft.Json;
using PropertyManager.Extensions;
using PropertyManager.Models;
using PropertyManager.Services;

namespace PropertyManager.ViewModels
{
    public class GroupsViewModel : BaseViewModel
    {
        private readonly IConfigService _configService;

        private string _query;

        public string Query
        {
            get { return _query; }
            set
            {
                _query = value;
                RaisePropertyChanged(() => Query);
                FilterGroupsCommand.Execute(null);
            }
        }

        public ObservableCollection<GroupModel> FilteredGroups { get; set; }

        public ICommand GroupClickCommand => new MvxCommand<GroupModel>(group => ShowGroup(group));

        public ICommand FilterGroupsCommand => new MvxCommand(FilterGroups);

        public ICommand AddPropertyCommand => new MvxCommand(AddProperty);

        public GroupsViewModel(IConfigService configService)
        {
            _configService = configService;
            FilteredGroups = new ObservableCollection<GroupModel>();
        }

        public override void Start()
        {
            // Filter groups.
            _query = null;
            FilterGroups();
            base.Start();
        }

        public override void OnResume()
        {
            FilterGroups();
            base.OnResume();
        }

        public void ShowGroup(GroupModel group)
        {
            // Navigate to the group view.
            ShowViewModel<GroupViewModel>(new
            {
                groupData = JsonConvert.SerializeObject(group)
            });
        }

        private void FilterGroups()
        {
            if (string.IsNullOrWhiteSpace(_query))
            {
                FilteredGroups.Clear();
                FilteredGroups.AddRange(_configService.Groups);
            }
            else
            {
                FilteredGroups.Clear();
                FilteredGroups.AddRange(_configService.Groups
                    .Where(g => g.DisplayName.Contains(_query) ||
                                g.Mail.Contains(_query)));
            }
        }

        private void AddProperty()
        {
            // Navigate to the details view.
            ShowViewModel<DetailsViewModel>();
        }
    }
}
