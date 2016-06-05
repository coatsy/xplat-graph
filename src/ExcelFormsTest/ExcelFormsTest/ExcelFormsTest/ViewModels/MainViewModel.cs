using ExcelFormsTest.Models;
using ExcelFormsTest.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelFormsTest.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ObservableCollection<FileValue> matchingFiles;
        public ObservableCollection<FileValue> MatchingFiles
        {
            get { matchingFiles = matchingFiles ?? new ObservableCollection<FileValue>(); return matchingFiles; }
        }

        public MainViewModel()
        {
            Title = "Using Microsoft Graph API with Xamarin Forms";
        }

        public AppViewModel AppVM
        {
            get { return (App.Current as App).VM; }
        }

        private CommandBase getExcelFilesCommand;

        public CommandBase GetExcelFilesCommand
        {
            get
            {
                getExcelFilesCommand = getExcelFilesCommand ?? new CommandBase(DoGetExcelFilesCommand);
                return getExcelFilesCommand;
            }
        }

        private async void DoGetExcelFilesCommand()
        {
            var excelFiles = await DataService.GetExcelFiles();

            MatchingFiles.Clear();
            foreach (var file in excelFiles.fileValues.OrderBy(f=>f.name))
            {
                MatchingFiles.Add(file);
            }
        }


    }
}
