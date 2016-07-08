using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using PropertyManager.Models;
using PropertyManager.Services;

namespace PropertyManager.ViewModels
{
    public class DetailsViewModel
        : MvxViewModel
    {
        private readonly IGraphService _graphService;
        private readonly IConfigService _configService;
        private bool _isExisting;

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

        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        public PropertyTableRowModel Details { get; set; }

        public ICommand GoBackCommand => new MvxCommand(() => Close(this));

        public ICommand SaveDetailsCommand => new MvxCommand(SaveDetailsAsync);

        public DetailsViewModel(IGraphService graphService, IConfigService configService)
        {
            _graphService = graphService;
            _configService = configService;
        }

        public void Init(string id)
        {
            // Get details.
            Details = _configService.DataFile.PropertyTable
                .Rows.FirstOrDefault(r => r.Id == id);
            _isExisting = Details != null;

            // Set title.
            Title = (_isExisting ? "Add" : "Edit") + " a property";
            if (Details == null)
            {
                Details = new PropertyTableRowModel();
            }
        }

        public async void SaveDetailsAsync()
        {
            IsLoading = true;

            if (_isExisting)
            {
                // Calculate address (range).
                const int startRow = 2;
                var endRow = 2 + (_configService.DataFile.PropertyTable.Rows.Length - 1);
                var address = $"{Constants.DataFilePropertyTableColumnStart}{startRow}:" +
                              $"{Constants.DataFilePropertyTableColumnEnd}{endRow}";

                // Update the table row.
                await _graphService.UpdateTableRowsAsync(_configService.DataFile.DriveItem,
                    Constants.DataFileDataSheet, address, _configService.DataFile.PropertyTable.Rows
                        .Cast<TableRowModel>().ToArray(), _configService.AppGroup);
            }

            IsLoading = false;
            GoBackCommand.Execute(null);
        }
    }
}