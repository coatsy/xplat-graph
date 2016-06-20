using System.Collections.ObjectModel;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using Newtonsoft.Json;
using PropertyManager.Models;
using PropertyManager.Services;

namespace PropertyManager.ViewModels
{
    public class GroupsViewModel
        : MvxViewModel
    {
        private readonly IGraphService _graphService;

        public DriveItemModel ExcelFile { get; set; }

        public TableColumnModel[] TableColumns { get; set; }

        public ObservableCollection<GroupModel> Groups { get; set; }

        public ICommand GoBackCommand => new MvxCommand(() => Close(this));

        public GroupsViewModel(IGraphService graphService)
        {
            _graphService = graphService;
            Groups = new ObservableCollection<GroupModel>();
        }

        public async void Init(string excelFileData, string groupsData)
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

            // Get table columns.
            var tableColumns = await _graphService.GetTableColumnsAsync(excelFile,
                Constants.ExcelPropertyTable);
            TableColumns = tableColumns;
        }

        public void ShowGroup(GroupModel group)
        {
            // Navigate to groups view.
            var data = JsonConvert.SerializeObject(group);
            ShowViewModel<GroupViewModel>(new { data });
        }
    }
}
