using ExcelFormsTest.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ExcelFormsTest.ViewModels
{
    public class ExpensesViewModel : ViewModelBase
    {
        public ExpensesViewModel()
        {
            Title = "Expense List";
        }

        private bool isRefreshing;
        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set { if (isRefreshing == value) return; isRefreshing = value; NotifyPropertyChanged(); }
        }


        private ObservableCollection<ExpenseViewModel> expenses;
        public ObservableCollection<ExpenseViewModel> Expenses
        {
            get { expenses = expenses ?? new ObservableCollection<ExpenseViewModel>(); return expenses; }
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


        private CommandBase getExpensesCommand;

        public CommandBase GetExpensesCommand
        {
            get
            {
                getExpensesCommand = getExpensesCommand ?? new CommandBase(DoGetExpensesCommand);
                return getExpensesCommand;
            }
        }

        private async void DoGetExpensesCommand()
        {
            IsRefreshing = true;
            var rows = await DataService.GetRows();
            Expenses.Clear();
            if (rows != null)
            {
                foreach (var row in rows)
                {
                    Expenses.Add(new ExpenseViewModel(row));
                }
            }
            ChartImageBase64 = await DataService.GetChartImageAsBase64();
            IsRefreshing = false;
        }

        private CommandBase deleteExpenseCommand;

        public CommandBase DeleteExpenseCommand
        {
            get
            {
                deleteExpenseCommand = deleteExpenseCommand ?? new CommandBase(DoDeleteExpenseCommand);
                return deleteExpenseCommand;
            }
        }

        private void DoDeleteExpenseCommand()
        {

        }

        private CommandBase addExpenseCommand;

        public CommandBase AddExpenseCommand
        {
            get
            {
                addExpenseCommand = addExpenseCommand ?? new CommandBase(DoAddExpenseCommand);
                return addExpenseCommand;
            }
        }

        private void DoAddExpenseCommand()
        {

        }
    }
}
