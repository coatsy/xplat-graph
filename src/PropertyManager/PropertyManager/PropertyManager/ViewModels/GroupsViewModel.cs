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
    public class GroupsViewModel
        : MvxViewModel
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
                FiltereGroupsCommand.Execute(null);
            }
        }

        public ObservableCollection<GroupModel> FilteredGroups { get; set; }

        public ICommand GoBackCommand => new MvxCommand(() => Close(this));

        public ICommand FiltereGroupsCommand => new MvxCommand(FilterGroups);

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
    }
}
