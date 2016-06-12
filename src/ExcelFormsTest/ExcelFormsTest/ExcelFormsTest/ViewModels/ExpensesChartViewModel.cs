using ExcelFormsTest.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ExcelFormsTest.ViewModels
{
    public class ExpensesChartViewModel : ViewModelBase
    {

        public ExpensesChartViewModel()
        {
            Title = "Expenses Summary Chart";
            GetChartData();
        }

        private async void GetChartData()
        {
            var isRefreshing = IsRefreshing;
            IsRefreshing = true;
            await DoRefreshChart();
            IsRefreshing = isRefreshing;
        }

        // image handling from Clemens' answer at
        // http://stackoverflow.com/questions/37080258/xamarin-show-image-from-base64-string
        private string chartImageBase64;
        public string ChartImageBase64
        {
            get { return chartImageBase64; }
            set
            {
                chartImageBase64 = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("ChartImage");
            }
        }

        public ImageSource ChartImage
        {
            get
            {
                return string.IsNullOrEmpty(ChartImageBase64) ?
                    null :
                    ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(ChartImageBase64)));
            }
        }

        private CommandBase refreshChartCommand;

        public CommandBase RefreshChartCommand
        {
            get
            {
                refreshChartCommand = refreshChartCommand ?? new CommandBase(DoRefreshChartCommand);
                return refreshChartCommand;
            }
        }

        private async void DoRefreshChartCommand()
        {
            var isRefreshing = IsRefreshing;
            IsRefreshing = true;
            await DoRefreshChart();
            IsRefreshing = isRefreshing;
        }

        private async Task DoRefreshChart()
        {
            ChartImageBase64 = await DataService.GetChartImageAsBase64();
        }
    }
}
