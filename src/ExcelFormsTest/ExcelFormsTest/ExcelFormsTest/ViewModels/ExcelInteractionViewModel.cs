using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelFormsTest.ViewModels
{
    public class ExcelInteractionViewModel : ViewModelBase
    {
        private string workbookName;
        public string WorkbookName
        {
            get { return workbookName; }
            set { if (workbookName == value) return; workbookName = value; NotifyPropertyChanged(); }
        }

        private CommandBase refreshExcelDataCommand;

        public CommandBase RefreshExcelDataCommand
        {
            get
            {
                refreshExcelDataCommand = refreshExcelDataCommand ?? new CommandBase(DoRefreshExcelDataCommand);
                return refreshExcelDataCommand;
            }
        }

        private void DoRefreshExcelDataCommand()
        {

        }
    }
}
