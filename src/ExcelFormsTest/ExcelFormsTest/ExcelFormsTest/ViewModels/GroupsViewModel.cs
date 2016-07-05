using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExcelFormsTest.Services;
using System.Collections.ObjectModel;

namespace ExcelFormsTest.ViewModels
{
    public class GroupsViewModel : ViewModelBase
    {
        public GroupsViewModel()
        {
            Title = "Groups Demo";
        }

        private ObservableCollection<GroupViewModel> groups;
        public ObservableCollection<GroupViewModel> Groups
        {
            get { groups = groups ?? new ObservableCollection<GroupViewModel>(); return groups; }
        }

        private CommandBase getAllGroupsCommand;

        public CommandBase GetAllGroupsCommand
        {
            get
            {
                getAllGroupsCommand = getAllGroupsCommand ?? new CommandBase(DoGetAllGroupsCommand);
                return getAllGroupsCommand;
            }
        }

        private async void DoGetAllGroupsCommand()
        {
            GetAllGroups(GroupQuery.All, "All visible groups");
        }

        private CommandBase getMyGroupsCommand;

        public CommandBase GetMyGroupsCommand
        {
            get
            {
                getMyGroupsCommand = getMyGroupsCommand ?? new CommandBase(DoGetMyGroupsCommand);
                return getMyGroupsCommand;
            }
        }

        private async void DoGetMyGroupsCommand()
        {
            GetAllGroups(GroupQuery.My, "All groups I belong to");
        }

        private async void GetAllGroups(GroupQuery extent, string successMessage = "Groups Fopund")
        {
            var isRefreshing = IsRefreshing;
            IsRefreshing = true;
            Message = string.Empty;

            Groups.Clear();
            GroupList groups = null;
            switch (extent)
            {
                case GroupQuery.All:
                    groups = await DataService.GetAllGroups();
                    break;
                case GroupQuery.My:
                    groups = await DataService.GetMyGroups();
                    break;
                default:
                    break;
            }
            if ((groups?.groups?.Count ?? 0) > 0)
            {
                foreach (var group in groups.groups)
                {
                    Groups.Add(new GroupViewModel(group));
                }
                Message = successMessage;
            }
            else
            {
                Message = "No groups found";
            }

            IsRefreshing = isRefreshing;
        }
    }

    enum GroupQuery
    {
        All,
        My
    }
}
