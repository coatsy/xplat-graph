using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        public DriveItemModel ExcelFile { get; set; }

        public TableColumnModel[] TableColumns { get; set; }

        private PropertyDataModel _propertyData;

        public PropertyDataModel PropertyData
        {
            get { return _propertyData; }
            set
            {
                _propertyData = value;
                RaisePropertyChanged(() => PropertyData);
            }
        }

        public GroupModel Group { get; set; }

        public ObservableCollection<DriveItemModel> MediaFiles { get; set; }

        public ObservableCollection<DriveItemModel> DocumentFiles { get; set; }

        public ObservableCollection<ConversationModel> Conversations { get; set; }

        public ICommand GoBackCommand => new MvxCommand(() => Close(this));

        public ICommand SaveDetailsCommand => new MvxCommand(SaveDetailsAsync);

        public GroupViewModel(IGraphService graphService)
        {
            _graphService = graphService;
            MediaFiles = new ObservableCollection<DriveItemModel>();
            DocumentFiles = new ObservableCollection<DriveItemModel>();
            Conversations = new ObservableCollection<ConversationModel>();
        }

        public async void Init(string excelFileData, string groupData)
        {
            IsLoading = true;

            // Deserialize Excel file.
            var excelFile = JsonConvert.DeserializeObject<DriveItemModel>(excelFileData);
            ExcelFile = excelFile;

            // Deserialize Group.
            var group = JsonConvert.DeserializeObject<GroupModel>(groupData);
            Group = group;

            // Get table columns.
            TableColumns = await _graphService.GetTableColumnsAsync(excelFile,
                Constants.ExcelPropertyTable);

            // If an entry in the table doesn't exist, create it.
            var rowIndex = GetRowIndex();
            TableRowModel tableRow;
            if (rowIndex > -1)
            {
                tableRow = new TableRowModel();
                tableRow.AddRange(TableColumns.Select(column => column.Values[rowIndex][0]));
            }
            else
            {
                // Create the table row.
                tableRow = await _graphService.AddTableRowAsync(ExcelFile,
                    Constants.ExcelPropertyTable,
                    new PropertyDataModel
                    {
                        Id = Group.Mail
                    });
                for (var i = 0; i < tableRow.Count; i++)
                {
                    TableColumns[i].Values.Add(new List<JToken> {tableRow[i]});
                }
            }
            PropertyData = new PropertyDataModel(tableRow);

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

        private async void SaveDetailsAsync()
        {
            IsLoading = true;

            // Calculate address and create table row.
            var rowIndex = GetRowIndex() + 1;
            var address = $"{Constants.ExcelPropertyTableColumnStart}{rowIndex}:" +
                          $"{Constants.ExcelPropertyTableColumnEnd}{rowIndex}";

            // Update the table row.
            await _graphService.UpdateTableRowAsync(ExcelFile, Constants.ExcelDataSheet,
                address, PropertyData);
            IsLoading = false;
        }

        private int GetRowIndex()
        {
            var idCell = TableColumns[0].Values
                .FirstOrDefault(v => v.Count > 0 &&
                                     v[0].ToString() == Group.Mail);
            return idCell == null ? -1 : TableColumns[0].Values.IndexOf(idCell);
        }
    }
}