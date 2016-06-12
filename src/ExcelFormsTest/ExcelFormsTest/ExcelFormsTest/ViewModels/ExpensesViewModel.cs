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



        private ObservableCollection<ExpenseViewModel> expenses;
        public ObservableCollection<ExpenseViewModel> Expenses
        {
            get { expenses = expenses ?? new ObservableCollection<ExpenseViewModel>(); return expenses; }
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
            ClearMessage();
            var isRefreshing = IsRefreshing;
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

            IsRefreshing = isRefreshing;
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

        private async void DoAddExpenseCommand()
        {
            ClearMessage();
            ExpenseRow row = new ExpenseRow()
            {
                Vendor = "Vodafone",
                Category = "Connectivity",
                Amount = 128d,
                Id = string.Empty
            };

            var success = await DataService.AddRow(row);

            if (success)
            {
                Expenses.Add(new ExpenseViewModel(row));
            }
            else
            {
                ShowMessage("Error adding expense");
            }
        }
    }
}
