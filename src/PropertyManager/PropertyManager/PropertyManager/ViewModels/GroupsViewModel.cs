using System.Collections.ObjectModel;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using Newtonsoft.Json;
using PropertyManager.Models;

namespace PropertyManager.ViewModels
{
    public class GroupsViewModel
        : MvxViewModel
    {
        public DriveItemModel ExcelFile { get; set; }

        public ObservableCollection<GroupModel> Groups { get; set; }

        public ICommand GoBackCommand => new MvxCommand(() => Close(this));

        public GroupsViewModel()
        {
            Groups = new ObservableCollection<GroupModel>();
        }

        public void Init(string excelFileData, string groupsData)
        {
            // Deserialize Excel file.
            var excelFile = JsonConvert.DeserializeObject<DriveItemModel>(excelFileData);
            ExcelFile = excelFile;

            // Deserialize groups.
            var groups = JsonConvert.DeserializeObject<GroupModel[]>(groupsData);
            foreach (var group in groups)
            {
                Groups.Add(group);
            }
        }

        public void ShowGroup(GroupModel group)
        {
            // Navigate to groups view.
            var excelFileData = JsonConvert.SerializeObject(ExcelFile);
            var groupData = JsonConvert.SerializeObject(group);
            ShowViewModel<GroupViewModel>(new { excelFileData, groupData });
        }
    }
}
